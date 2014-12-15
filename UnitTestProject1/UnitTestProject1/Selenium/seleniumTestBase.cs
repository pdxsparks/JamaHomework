using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;

namespace framework
{
    public static class webdriverextensions
    {

        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            IWebElement element = wait.Until<IWebElement>((d) => 
                {
                    return d.FindElement(by);
                });
            return element;
        }
    }

    public class seleniumTestBase
    {
        public IWebDriver sdriver;

        public IWebDriver startBrowser(IWebDriver sdriver)
        {
            if (sdriver == null)
            {
                sdriver = new ChromeDriver();
                    //new  FirefoxDriver();
                sdriver.Manage().Window.Maximize();
                return sdriver;
            }
            else { return sdriver; }
        }

        public void teardown()
        {
           IList<string> handles = sdriver.WindowHandles;
           this.sdriver.Quit();
        }

        public void inputText(IWebDriver driver, By by, string text)
        {
            IWebElement element = driver.FindElement(by,30);

            element.Clear();
            element.SendKeys(text);
        }
       
    }
}