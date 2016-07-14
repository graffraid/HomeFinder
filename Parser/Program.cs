﻿namespace Parser
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
        private static List<Building> buildingList;
        private static readonly List<string> FilteredUrlList = new List<string>();
        
        static void Main()
        {
            var buildingRepository = new BuildingRepository();
            buildingList = buildingRepository.GetAll();

            using (IWebDriver driver = new PhantomJSDriver())
            //using (IWebDriver driver = new FirefoxDriver())
            {
                StartParsing(driver);

                Console.WriteLine("====== The End! ======");
                Console.ReadKey(true);
            }
        }

        private static void StartParsing(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(url);

            IList<IWebElement> pageElements = driver.FindElement(By.ClassName("pagination-pages")).FindElements(By.CssSelector("a.pagination-page"));
            Dictionary<int, string> pageUrlList = GetPageUrls(pageElements);

            foreach (var pageUrl in pageUrlList)
            {
                Console.Write("\rParsing... page {0}/{1}", pageUrl.Key, pageUrlList.Count);
                ParsePae(driver, pageUrl.Value);
            }
        }

        private static void ParsePae(IWebDriver driver, string url)
        {
            if (driver.Url != url)
            {
                driver.Navigate().GoToUrl(url);
            }
            IList<IWebElement> advertElements = driver.FindElements(By.ClassName("item"));
            IList<IWebElement> filteredAdvertElements = FilterAdvertElements(advertElements);
            AddAdvertUrlsToList(filteredAdvertElements);
        }

        private static IList<IWebElement> FilterAdvertElements(IList<IWebElement> advertElements)
        {
            List<IWebElement> rezult = new List<IWebElement>();

            foreach (IWebElement element in advertElements)
            {
                if (IsAdvertElementSatisfy(element.FindElement(By.ClassName("description")).FindElement(By.ClassName("address")).Text))
                {
                    rezult.Add(element);
                }
            }
            return rezult;
        }

        private static void AddAdvertUrlsToList(IList<IWebElement> advertElements)
        {
            foreach (IWebElement element in advertElements)
            {
                FilteredUrlList.Add(element.FindElement(By.ClassName("description")).FindElement(By.ClassName("item-description-title")).FindElement(By.ClassName("item-description-title-link")).GetAttribute("href"));
            }
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

        private static bool IsAdvertElementSatisfy(string elementAddr)
        {
            return buildingList.Any(addr => elementAddr.Contains(addr.ShortStreet) && elementAddr.Contains(addr.No));
        }
    }
}
