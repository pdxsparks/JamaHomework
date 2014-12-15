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
 
        //login to jama test server
        public void login()
        {
            //setup driver for tests
            driver = selwrapper.startBrowser(driver);
            
            driver.Navigate().GoToUrl("https://testip.jamaland.com/login.req");

            //login with test user text boxes dont always clear as expected so method clears and enteres expected text
            selwrapper.inputText(driver, By.CssSelector("#j_username"), "asparks");
            selwrapper.inputText(driver, By.CssSelector("#j_password"), "A1surfer");
            
            driver.FindElement(By.CssSelector("#loginButton")).Click();
        }

        //this will always open a new feature I image this is used a lot so it is set as a reusable method. 
        public void addNewItem_feature()
        {
            //I would normally never use this in this context however for some reason the wait.Until works in debug only 
            //mode with this context menu only... I would need more time to understand this behaviour.
            Thread.Sleep(6000);

            //get the menu element
            IWebElement menu = driver.FindElement(By.CssSelector(".x-tree-node"),30);
            
            //right click the menu
            Actions action = new Actions(driver);
            action.ContextClick(menu);
            action.Perform();

            //context menu move mouse I assumed this gets used a lot so I created a method
            mouseToContextMenuItemByText("Add");
            mouseToContextMenuItemByText("New Item");
            mouseToContextMenuItemByText("Feature");

            //send enter key once on the correct context menu item
            action.SendKeys(Keys.Enter);
            action.Perform();
        }

        //we need to create a new feather with specific test instructions. the next step would be to paramaratize this 
        //method so that you can reuse it and choose what ever specific optioins for the new feature 
        public void createFeature_priorityHigh_noNotify()
        {
            //set feature name
            selwrapper.inputText(driver, By.Name("name"), "Project Homework");

            //choose a priority
            choosePriority_NewFeature("Medium");

            //uncheck the checkbox (checks if it is already unchecked)
            UncheckCheckBox_NewFeature();

            saveAndCloseNewFeature();
        }

        #region helpers
        public void saveAndCloseNewFeature()
        {
            //Again I would never user thread sleep like this. In this case I would override findelements with a webdriverwait
            //like I did with findelement, I just ran out of time. 
            Thread.Sleep(1000);

            //get all the buttons because the buttons dont have a good class or id to select
            IList<IWebElement> buttons = driver.FindElements(By.CssSelector("button"));

            //find the button we need and click it
            foreach (IWebElement b in buttons)
            {
                if (b.Text == "Save and Close")
                {
                    b.Click();
                    break;
                }
            }
        }

        public void UncheckCheckBox_NewFeature()
        {
            //find all the checkboxes because they have a generic class
            IList<IWebElement> checkboxes = driver.FindElements(By.CssSelector(".x-form-check-wrap"));

            //find the check box we are looking for
            foreach (IWebElement a in checkboxes)
            {
                //find element will only look with in the branch of the element so a. accesses only the labels in the 
                //specific iwebelement if it is not the notify checkbox we move on
                if (a.FindElement(By.CssSelector("label")).Text == "Notify")
                {
                    //now that we found the right one we need to check if the box is checked as it appears the users last
                    //choice is saved. 
                    if (a.FindElement(By.CssSelector("input")).Selected)
                    {
                        //if the checkbox was checked, click to uncheck it
                        a.FindElement(By.CssSelector("input")).Click();
                        break;
                    }
                }
            }
        }
        
        public void choosePriority_NewFeature(string priority)
        {
            //priority drop down has a nice cssselector
            driver.FindElement(By.CssSelector("input[name=\"priority\"]")).Click();

            //but we need to know all the options it contains 
            IList<IWebElement> options = driver.FindElements(By.CssSelector(".x-combo-list-item"));
            foreach (IWebElement o in options) 
            {
                //if priority matches the requested priority click it
                if (o.Text == priority)
                {
                    o.Click();
                    break;
                }
            }
        }

        public void mouseToContextMenuItemByText(string text)
        {
            //used to dynamically mouseover the specified context menu item by getting all the items and looping, I can 
            //avoid fragile xpaths and easily maintain this if the text changes which is less likely to change than an xpath
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
            //we spend a lot of time verifying text is what we wanted, so I like to create methods for this. Usually these 
            //type of methods get their own helper class, but in the name of time I wrote them here
            IWebElement elemToTest = driver.FindElement(by,30);

            if (elemToTest.Text == text)
            { return true; }
            else
            { return false; }
        }

        public bool verifyClass(By by)
        {
            //same as the text verify but to check classes are added to elements when the user changes things as expected
            IWebElement elemToTest = driver.FindElement(by);

            if (elemToTest.Displayed)
            { return true; }
            else { return false; }
        }

        //kill the driver
        public void teardown() { driver.Quit(); }
        #endregion
    }
}
