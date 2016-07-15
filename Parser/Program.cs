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

    class Program
    {
        private static string url = "https://www.avito.ru/voronezh/kvartiry/prodam/vtorichka/kirpichnyy_dom?district=150&f=549_5698-5699-5700";
        
        static void Main()
        {
            var buildingRepository = new BuildingRepository();
            var advertRepository = new AdvertRepository();

            List<Building> buildingList = buildingRepository.GetAll();

            using (IWebDriver driver = new PhantomJSDriver())
            //using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                List<AdvertElement> advertElements = GetAdvertElements(driver, buildingList);
                Console.WriteLine("\r====== Getting adverts finished. Count:{0} ======", advertElements.Count);

                foreach (var advertElement in advertElements)
                {
                    Advert advert = GetAdvert(advertElement, driver);
                    advertRepository.Add(advert);
                }

                Console.ReadKey(true);
            }
        }

        private static List<AdvertElement> GetAdvertElements(IWebDriver driver, List<Building> buildingList)
        {
            List<AdvertElement> result = new List<AdvertElement>();
            driver.Navigate().GoToUrl(url);

            IList<IWebElement> pageElements = driver.FindElement(By.ClassName("pagination-pages")).FindElements(By.CssSelector("a.pagination-page"));
            Dictionary<int, string> pageUrlList = GetPageUrls(pageElements);

            foreach (var pageUrl in pageUrlList)
            {
                Console.Write("\rGetting adverts... page {0}/{1}", pageUrl.Key, pageUrlList.Count);

                IList<IWebElement> webElements = ParsePage(driver, pageUrl.Value);
                List<AdvertElement> filteredAdvertElements = FilterAdvertElements(webElements, buildingList);
                result.AddRange(filteredAdvertElements);
            }
            return result;
        }

        private static IList<IWebElement> ParsePage(IWebDriver driver, string url)
        {
            if (driver.Url != url)
            {
                driver.Navigate().GoToUrl(url);
            }
            return driver.FindElements(By.ClassName("item"));
        }

        private static List<AdvertElement> FilterAdvertElements(IList<IWebElement> advertElements, List<Building> buildingList)
        {
            List<AdvertElement> rezult = new List<AdvertElement>();

            foreach (IWebElement element in advertElements)
            {
                var buildingId = IsAdvertElementSatisfy(element.FindElement(By.ClassName("description")).FindElement(By.ClassName("address")).Text, buildingList);
                if (buildingId > 0)
                {
                    rezult.Add(new AdvertElement { BuildingId = buildingId,
                                                   WebElement = element,
                                                   AdvertUrl = element.FindElement(By.ClassName("description")).FindElement(By.ClassName("item-description-title")).FindElement(By.ClassName("item-description-title-link")).GetAttribute("href")
                    });
                }
            }
            return rezult;
        }

        private static Dictionary<int, string> GetPageUrls(IList<IWebElement> pageElements)
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

        private static int IsAdvertElementSatisfy(string elementAddr, List<Building> buildingList)
        {

            if (buildingList.Any(addr => elementAddr.Contains(addr.ShortStreet) && elementAddr.Contains(addr.No)))
            {
                return buildingList.First(addr => elementAddr.Contains(addr.ShortStreet) && elementAddr.Contains(addr.No)).Id;
            }
            return 0;
        }

        private static DateTime ParseDate(string elementText)
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

        public static Advert GetAdvert(AdvertElement advertElement, IWebDriver driver)
        {
            driver.Navigate().GoToUrl(advertElement.AdvertUrl);
            driver.FindElement(By.ClassName("js-phone-show__link")).Click();
            
            var data = driver.FindElements(By.ClassName("item-params"))[1].Text.Replace("-к квартира ", " ").Replace(" м² на ", " ").Replace(" этаже ", " ").Replace("-этажного ", " ").Replace("кирпичного дома", "").Split(' ');

            return new Advert
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
        }
    }

    public class AdvertElement
    {
        public int BuildingId { get; set; }

        public IWebElement WebElement { get; set; }

        public string AdvertUrl { get; set; }
}
}
