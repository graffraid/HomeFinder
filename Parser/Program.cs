namespace Parser
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.PhantomJS;

    class Program
    {
        static void Main()
        {
            using (IWebDriver driver = new PhantomJSDriver())
            {
                driver.Navigate().GoToUrl("http://docs.seleniumhq.org/docs/03_webdriver.jsp#introducing-webdriver");

                Console.ReadKey(true);
            }
        }
    }
}
