namespace Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.Repositories;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.PhantomJS;

    public class Parser
    {
        public static string status = string.Empty;

        private List<Building> buildingList;
        private string url = "https://www.avito.ru/voronezh/kvartiry/prodam/vtorichka/kirpichnyy_dom?district=150&f=549_5698-5699-5700";
        
        public void Parse()
        {
            var buildingRepository = new BuildingRepository();
            var advertRepository = new AdvertRepository();

            buildingList = buildingRepository.GetAll();
            List<Advert> adverts = new List<Advert>();

            using (IWebDriver driver = new PhantomJSDriver())
            //using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                List<AdvertElement> advertElements = GetAdvertElements(driver);
                status = $"Getting adverts finished. Count:{advertElements.Count}";
                
                var index = 0;
                foreach (var advertElement in advertElements)
                {
                    index++;
                    status = $"Parsing adverts... {index}/{advertElements.Count}";
                    var advert = GetAdvert(advertElement, driver);
                    adverts.Add(advert);
                }
            }
            status = "Updatind database...";
            advertRepository.AddOrUpdateRange(adverts);
            status = "Done!";
        }

        private List<AdvertElement> GetAdvertElements(IWebDriver driver)
        {
            List<AdvertElement> result = new List<AdvertElement>();
            driver.Navigate().GoToUrl(url);

            IList<IWebElement> pageElements = driver.FindElement(By.ClassName("pagination-pages")).FindElements(By.CssSelector("a.pagination-page"));
            Dictionary<int, string> pageUrlList = GetPageUrls(pageElements);

            foreach (var pageUrl in pageUrlList)
            {
                status = $"Getting adverts... page {pageUrl.Key}/{pageUrlList.Count}";

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
            if (buildingList.Any(addr => elementAddr.Contains(addr.ShortStreet) && elementAddr.Contains(addr.No)))
            {
                return buildingList.First(addr => elementAddr.Contains(addr.ShortStreet) && elementAddr.Contains(addr.No)).Id;
            }
            return null;
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
                           Description = driver.FindElement(By.Id("desc_text")).FindElement(By.TagName("p")).Text,
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
    }
}
