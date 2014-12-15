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
        //this is an override for findElement that allows me to wait until and element is visible, with some pages 
        //the page load timer of selenium will give the all clear WAY before the elements are actually drawn
        // so we need a way to repoll the driver to see, the repoll rate on webdriver wait is 500 milliseconds, but can be changed
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
        //basic setup and control over the selenium driver
        public IWebDriver sdriver;

        public IWebDriver startBrowser(IWebDriver sdriver)
        {
            //check to see if a driver already exsists
            if (sdriver == null)
            {
                //I choose chrome as apparently firefox does not like the way mstest closes it down, again I would need to 
                //look more into that, however I would not choose to use mstest anyway as Nunit and Junit are much more capable
                sdriver = new ChromeDriver();
                    //new  FirefoxDriver();
                //selenium does not always like to move to an element off screen, reduce the likely hood of issues with maximize
                sdriver.Manage().Window.Maximize();
                return sdriver;
            }
            else { return sdriver; }
        }

        public void teardown()
        {
           sdriver.Quit();
        }

        public void inputText(IWebDriver driver, By by, string text)
        {
            //a quick way to use selenium to properly input text 
            IWebElement element = driver.FindElement(by,30);

            element.Clear();
            element.SendKeys(text);
        }
       
    }
}