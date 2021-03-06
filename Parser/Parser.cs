﻿namespace Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using Microsoft.AspNet.SignalR.Client;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.PhantomJS;

    public class Parser
    {
        public static string Status { get; private set; } = "Ready to start";
        private List<Building> buildings;
        private List<Advert> dbAdverts;
        private readonly string url;

        public Parser(string url)
        {
            this.url = url;
        }

        public void Parse(string hubUrl, string picturesPath, string absolutePicturesPath)
        {
            var hubConnection = new HubConnection(hubUrl);
            var hubProxy = hubConnection.CreateHubProxy("foxyHub");
            hubConnection.Start().Wait();

            if (Status == "Ready to start" || Status == "Done!")
            {
                try
                {
                    Status = "Start parsing...";
                    hubProxy.Invoke("PushStatus", Status);

                    var buildingRepository = new BuildingRepository();
                    buildings = buildingRepository.GetAll();
                    var advertRepository = new AdvertRepository();
                    dbAdverts = advertRepository.GetAll();

                    List<Advert> parsedAdverts = new List<Advert>();

                    using (IWebDriver driver = new PhantomJSDriver())
                    //using (IWebDriver driver = new FirefoxDriver())
                    {
                        driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                        List<AdvertElement> advertElements = GetAdvertElements(driver, hubProxy);
                        Status = $"Getting adverts finished. Count:{advertElements.Count}";
                        hubProxy.Invoke("PushStatus", Status);

                        var index = 0;
                        foreach (var advertElement in advertElements)
                        {
                            index++;
                            Status = $"Parsing adverts... {index}/{advertElements.Count}";
                            hubProxy.Invoke("PushStatus", Status);
                            var advert = GetAdvert(advertElement, driver);
                            parsedAdverts.Add(advert);
                        }
                    }

                    var filter = new ParserFilter(dbAdverts);
                    var newAdverts = filter.GetNewAdverts(parsedAdverts);
                    var changedAdverts = filter.GetChangedAdverts(parsedAdverts);
                    var outdatedAdverts = filter.GetOutdatedAdverts(parsedAdverts);

                    Status = "Updatind database...";
                    hubProxy.Invoke("PushStatus", Status);
                    advertRepository.AddRange(newAdverts);
                    advertRepository.AddRange(changedAdverts);
                    advertRepository.EditRange(outdatedAdverts);


                    var parserImageCacher = new ParserImageCacher();
                    parserImageCacher.CacheImages(hubProxy, hubUrl, picturesPath, absolutePicturesPath);
                    
                    Status = "Done!";
                    hubProxy.Invoke("PushStatus", Status);
                }
                catch (Exception ex)
                {
                    Status = "Error! " + ex.GetType();
                    hubProxy.Invoke("PushStatus", Status);
                    throw;
                }
            }
        }

        private List<AdvertElement> GetAdvertElements(IWebDriver driver, IHubProxy hubProxy)
        {
            List<AdvertElement> result = new List<AdvertElement>();
            driver.Navigate().GoToUrl(url);

            IList<IWebElement> pageElements = driver.FindElement(By.ClassName("pagination-pages")).FindElements(By.CssSelector("a.pagination-page"));
            Dictionary<int, string> pageUrlList = GetPageUrls(pageElements);

            foreach (var pageUrl in pageUrlList)
            {
                Status = $"Getting adverts... page {pageUrl.Key}/{pageUrlList.Count}";
                hubProxy.Invoke("PushStatus", Status);

                IList<IWebElement> advertWebElements = GetAdvertWebElements(driver, pageUrl.Value);
                List<AdvertElement> advertElements = GetCorrectAdvertElements(advertWebElements);
                result.AddRange(advertElements);
            }
            return result;
        }

        private Dictionary<int, string> GetPageUrls(IList<IWebElement> pageElements)
        {
            Dictionary<int, string> rezult = new Dictionary<int, string> { { 1, url } };
            foreach (IWebElement element in pageElements)
            {
                if (!rezult.ContainsValue(element.GetAttribute("href")))
                {
                    rezult.Add(int.Parse(element.Text), element.GetAttribute("href"));
                }
            }
            return rezult;
        }

        private IList<IWebElement> GetAdvertWebElements(IWebDriver driver, string url)
        {
            if (driver.Url != url)
            {
                driver.Navigate().GoToUrl(url);
            }
            return driver.FindElements(By.ClassName("item"));
        }

        private List<AdvertElement> GetCorrectAdvertElements(IList<IWebElement> advertElements)
        {
            List<AdvertElement> rezult = new List<AdvertElement>();

            foreach (IWebElement element in advertElements)
            {
                var buildingId = GetBuildingId(element.FindElement(By.ClassName("description")).FindElement(By.ClassName("address")).Text);
                if (buildingId != null)
                {
                    rezult.Add(new AdvertElement { BuildingId = (int)buildingId,
                                                   WebElement = element,
                                                   AdvertUrl = element.FindElement(By.ClassName("description")).FindElement(By.ClassName("item-description-title")).FindElement(By.ClassName("item-description-title-link")).GetAttribute("href")
                    });
                }
            }
            return rezult;
        }

        private int? GetBuildingId(string elementAddr)
        {
            var ids = this.buildings.Where(building => elementAddr.ToLower().Contains(building.ShortStreet.ToLower()) && building.AlternativeBuildingNumbers.Any(no => elementAddr.Contains(no.No))).Select(building => building.Id).ToList();

            if (ids.Count == 0)
            {
                return null;
            }
            if (ids.Count == 1)
            {
                return ids.First();
            }

            return buildings.Where(x => ids.Contains(x.Id)).OrderByDescending(x => x.No).First().Id;
        }

        private Advert GetAdvert(AdvertElement advertElement, IWebDriver driver)
        {
            driver.Navigate().GoToUrl(advertElement.AdvertUrl);
            driver.FindElement(By.ClassName("js-phone-show__link")).Click();
            
            var data = driver.FindElements(By.ClassName("item-params"))[1].Text.Replace("-к квартира ", " ").Replace(" м² на ", " ").Replace(" этаже ", " ").Replace("-этажного ", " ").Replace("кирпичного дома", "").Split(' ');

            var advert = new Advert
                       {
                           Url = advertElement.AdvertUrl,
                           BuildingId = advertElement.BuildingId,
                           No = int.Parse(driver.FindElement(By.Id("item_id")).Text),
                           PlacementDate = ParseDate(driver.FindElement(By.ClassName("item-subtitle")).Text),
                           Price = int.Parse(driver.FindElement(By.ClassName("p_i_price")).FindElement(By.TagName("span")).Text.Replace(" руб.", "").Replace(" ", "")),
                           RoomCount = int.Parse(data[0]),
                           Space = double.Parse(data[1].Replace('.', ',')),
                           Floor = int.Parse(data[2]),
                           TotalFloor = int.Parse(data[3]),
                           Description = ParseDescription(driver),
                           SellerName = driver.FindElement(By.Id("seller")).FindElement(By.TagName("strong")).Text,
                           SellerPhone = driver.FindElement(By.ClassName("description__phone-img")).GetAttribute("src"),
                           IsSellerAgency = driver.FindElement(By.ClassName("description_seller")).Text.Trim() == "Агентство"
            };

            List<AdvertImage> images;
            try
            {
                images = driver.FindElement(By.ClassName("gallery-list")).FindElements(By.ClassName("gallery-item")).Select(x => new AdvertImage
                {
                    Url = x.FindElement(By.ClassName("gallery-link")).GetAttribute("href")
                }).ToList();
            }
            catch (Exception ex)
            {
                images = new List<AdvertImage>();
            }
            
            advert.AdvertImages = images;
            return advert;
        }

        private DateTime ParseDate(string elementText)
        {
            DateTime result;
            var date =
                elementText.Replace(". Редактировать, закрыть, поднять объявление", "")
                    .Replace("Размещено ", "")
                    .Replace(" в ", "/")
                    .Split('/');

            switch (date[0])
            {
                case "вчера":
                    result = DateTime.Now.AddDays(-1).Date;
                    break;
                case "сегодня":
                    result = DateTime.Now.Date;
                    break;
                default:
                    result = DateTime.Parse(date[0]);
                    break;
            }

            var hours = int.Parse(date[1].Split(':')[0]);
            var minutes = int.Parse(date[1].Split(':')[1]);

            return result.AddHours(hours).AddMinutes(minutes);
        }

        private string ParseDescription(IWebDriver driver)
        {
            if (IsElementPresent(By.Id("desc_text"), driver))
            {
                return driver.FindElement(By.Id("desc_text")).FindElement(By.TagName("p")).Text;
            }
            if (IsElementPresent(By.XPath(".//*[@id='item']/div[4]/div[1]/div[2]/div[2]/div[3]/div[2]/div[2]/div"), driver))
            {
                return driver.FindElement(By.XPath(".//*[@id='item']/div[4]/div[1]/div[2]/div[2]/div[3]/div[2]/div[2]/div")).Text;
            }
            return string.Empty;
        }

        private bool IsElementPresent(By by, IWebDriver driver)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
