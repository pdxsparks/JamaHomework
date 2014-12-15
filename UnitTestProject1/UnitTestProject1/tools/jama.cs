using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace framework
{
    class jama
    {
        seleniumTestBase selwrapper = new seleniumTestBase();
        IWebDriver driver;
 
        public void login()
        {
            driver = selwrapper.startBrowser(driver);
            driver.Navigate().GoToUrl("https://testip.jamaland.com/login.req");

            selwrapper.inputText(driver, By.CssSelector("#j_username"), "asparks");
            selwrapper.inputText(driver, By.CssSelector("#j_password"), "A1surfer");
            
            driver.FindElement(By.CssSelector("#loginButton")).Click();
        }

        public void addNewItem_feature()
        {
            //I would normally never use this in this context however for some reason the wait.Until works in debug only 
            //mode with this context menu only... I would need more time to understand this behaviour.
            Thread.Sleep(6000);

            IWebElement menu = driver.FindElement(By.CssSelector(".x-tree-node"),30);
            
            Actions action = new Actions(driver);
            action.ContextClick(menu);
            action.Perform();

            mouseToContextMenuItemByText("Add");
            mouseToContextMenuItemByText("New Item");
            mouseToContextMenuItemByText("Feature");

            action.SendKeys(Keys.Enter);
            action.Perform();
        }

        public void createFeature_priorityHigh_noNotify()
        {
            selwrapper.inputText(driver, By.Name("name"), "Project Homework");

            choosePriority_NewFeature("Medium");

            toggleNotifyCheckBox_NewFeature();

            saveAndCloseNewFeature();
        }

        #region helpers
        public void saveAndCloseNewFeature()
        {
            Thread.Sleep(1000);

            IList<IWebElement> buttons = driver.FindElements(By.CssSelector("button"));

            foreach (IWebElement b in buttons)
            {
                if (b.Text == "Save and Close")
                {
                    b.Click();
                    break;
                }
            }
        }

        public void toggleNotifyCheckBox_NewFeature()
        {
            IList<IWebElement> checkboxes = driver.FindElements(By.CssSelector(".x-form-check-wrap"));

            foreach (IWebElement a in checkboxes)
            {
                if (a.FindElement(By.CssSelector("label")).Text == "Notify")
                {
                    if (a.FindElement(By.CssSelector("input")).Selected)
                    {
                        a.FindElement(By.CssSelector("input")).Click();
                        break;
                    }
                }
            }
        }
        
        public void choosePriority_NewFeature(string priority)
        {
            driver.FindElement(By.CssSelector("input[name=\"priority\"]")).Click();

            IList<IWebElement> options = driver.FindElements(By.CssSelector(".x-combo-list-item"));
            foreach (IWebElement o in options) 
            {
                if (o.Text == priority)
                {
                    o.Click();
                    break;
                }
            }
        }

        public void mouseToContextMenuItemByText(string text)
        {
            IList<IWebElement> contextMenu = driver.FindElements(By.CssSelector(".x-menu-item-text"));
            foreach (IWebElement item in contextMenu)
            {
                if (item.Text == text)
                {
                    Actions action = new Actions(driver);
                    action.MoveToElement(item);
                    action.Perform();
                    Thread.Sleep(800);
                    break;
                }
            }
        }

        public bool verifyText(string text, By by)
        {
            IWebElement elemToTest = driver.FindElement(by,30);

            if (elemToTest.Text == text)
            { return true; }
            else
            { return false; }
        }

        public bool verifyClass(By by)
        {
            IWebElement elemToTest = driver.FindElement(by);

            if (elemToTest.Displayed)
            { return true; }
            else { return false; }
        }


        public void teardown() { driver.Quit(); }
        #endregion
    }
}
