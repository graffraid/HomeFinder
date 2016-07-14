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
            List<Building> buildingList = buildingRepository.GetAll();

            using (IWebDriver driver = new PhantomJSDriver())
            //using (IWebDriver driver = new FirefoxDriver())
            {
                List<string> urlList = GetAdvertUrls(driver, buildingList);

                Console.WriteLine("\r====== Getting adverts finished. Count:{0} ======", urlList.Count);
                Console.ReadKey(true);
            }
        }

        private static List<string> GetAdvertUrls(IWebDriver driver, List<Building> buildingList)
        {
            List<string> result = new List<string>();
            driver.Navigate().GoToUrl(url);

            IList<IWebElement> pageElements = driver.FindElement(By.ClassName("pagination-pages")).FindElements(By.CssSelector("a.pagination-page"));
            Dictionary<int, string> pageUrlList = GetPageUrls(pageElements);

            foreach (var pageUrl in pageUrlList)
            {
                Console.Write("\rGetting adverts... page {0}/{1}", pageUrl.Key, pageUrlList.Count);

                IList<IWebElement> advertElements = ParsePage(driver, pageUrl.Value);
                IList<IWebElement> filteredAdvertElements = FilterAdvertElements(advertElements, buildingList);
                List<string> urlList = GetAdvertUrlsToList(filteredAdvertElements);
                result.AddRange(urlList);
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

        private static IList<IWebElement> FilterAdvertElements(IList<IWebElement> advertElements, List<Building> buildingList)
        {
            List<IWebElement> rezult = new List<IWebElement>();

            foreach (IWebElement element in advertElements)
            {
                if (IsAdvertElementSatisfy(element.FindElement(By.ClassName("description")).FindElement(By.ClassName("address")).Text, buildingList))
                {
                    rezult.Add(element);
                }
            }
            return rezult;
        }

        private static List<string> GetAdvertUrlsToList(IList<IWebElement> advertElements)
        {
            List<string> result = new List<string>();
            foreach (IWebElement element in advertElements)
            {
                result.Add(element.FindElement(By.ClassName("description")).FindElement(By.ClassName("item-description-title")).FindElement(By.ClassName("item-description-title-link")).GetAttribute("href"));
            }
            return result;
        }

        private static Dictionary<int, string> GetPageUrls(IList<IWebElement> pageElements)
        {
            Dictionary<int, string> rezult = new Dictionary<int, string> { { 1, url } };
            foreach (IWebElement element in pageElements)
            {
                if (!rezult.ContainsValue(element.GetAttribute("href")))
                {
                    rezult.Add(Int32.Parse(element.Text), element.GetAttribute("href"));
                }
            }
            return rezult;
        }

        private static bool IsAdvertElementSatisfy(string elementAddr, List<Building> buildingList)
        {
            return buildingList.Any(addr => elementAddr.Contains(addr.ShortStreet) && elementAddr.Contains(addr.No));
        }
    }
}
