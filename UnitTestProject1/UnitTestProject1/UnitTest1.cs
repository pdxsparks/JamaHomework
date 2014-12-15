using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace framework
{
    [TestClass]
    public class UnitTest1
    {
        //seleniumTestBase selwrapper = new seleniumTestBase();
        jama jama = new jama();
        
        [TestMethod]
        public void NewFeatureRequiredfields()
        {
            jama.login();

            jama.addNewItem_feature();
            jama.saveAndCloseNewFeature();

            //test passes if validation message is a match and if class .x-xform-invalid is findable
            Assert.IsTrue(jama.verifyText("You are missing some required fields", By.CssSelector(".j-error-panel")));
            Assert.IsTrue(jama.verifyClass(By.CssSelector(".x-form-invalid")));
        }

        [TestMethod]
        public void CreateNewFeature()
        {
            jama.login();

            jama.addNewItem_feature();
            jama.createFeature_priorityHigh_noNotify();

            //test passes if we are not on the comment and notify page, the project name matches and the priority matches
            Assert.IsFalse(jama.verifyText("Comment and Notify Feature", By.CssSelector(".j-item-header-title")));
            Assert.IsTrue(jama.verifyText("Project Homework", By.CssSelector(".j-item-field-name")));
            Assert.IsTrue(jama.verifyText("Medium", By.CssSelector("div[name=\"priority\"]")));
        }

        // This closes the driver down after the test has finished.
        [TestCleanup]
        public void TearDown()
        {
            jama.teardown();
        }
    }
}
