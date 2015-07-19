using System;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UrlShortener.WebUI.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using UrlShortener.WebUI.Models;
using Moq;
using UrlShortener.Domain.Abstract;
using UrlShortener.Domain.Entities;
using UrlShortener.WebUI.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace UrlShortener.Test
{
    [TestClass]
    public class UnitTest1
    {
        private UrlValidation val;
        public UnitTest1()
        {
            val = new UrlValidation();
        }
        [TestMethod]
        public void is_Valid_Url()
        {
            Assert.IsTrue(val.isValidUrl("https://fluentvalidation.codeplex.com/wikipage?title=Testing"));
            Assert.IsFalse(val.isValidUrl("notvalidUrl"));
        }
        [TestMethod]
        public void Test_Create_With_Validation_Error()
        {
            // Arrange
             Mock<IUrlsRepository> mock = new Mock<IUrlsRepository>();
            mock.Setup(u => u.Urls).Returns(new Url[] { 
                new Url{ UrlId = 0, UrlCode = "TYUR", OriginalUrl="https://fluentvalidation.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 1, UrlCode = "TwUR", OriginalUrl="https://facebook.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 2, UrlCode = "TkUR", OriginalUrl="https://youtube.com/", IpAddress="127.0.0.1", PostedDate = DateTime.Now}
            }.AsQueryable());

            UrlShortenerController controller= new UrlShortenerController(mock.Object);
            controller.ModelState.AddModelError("NoteText", "NoteText cannot be null");
            UrlShortenerModel model = new UrlShortenerModel();

            // Act
            ActionResult result = controller.ShortenURl(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }
        [TestMethod]
        public void Test_Create_Without_Validation_Error()
        {
            // Arrange
            Mock<IUrlsRepository> mock = new Mock<IUrlsRepository>();
            mock.Setup(u => u.Urls).Returns(new Url[] { 
                new Url{ UrlId = 0, UrlCode = "TYUR", OriginalUrl="https://fluentvalidation.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 1, UrlCode = "TwUR", OriginalUrl="https://facebook.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 2, UrlCode = "TkUR", OriginalUrl="https://youtube.com/", IpAddress="127.0.0.1", PostedDate = DateTime.Now}
            }.AsQueryable());

            UrlShortenerController controller = new UrlShortenerController(mock.Object);
            UrlShortenerModel model = new UrlShortenerModel();

            // Act
            ActionResult result = controller.ShortenURl(model);

            // Assert , if our validation no error, it saves and return redirect to 
            //index which is of ViewResult type.
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Index_Contains_All_Urls_By_IP()
        {
            //Old Not valid for the controller model was changed.
            // Arrange - create the mock repository
            Mock<IUrlsRepository> mock = new Mock<IUrlsRepository>();
            mock.Setup(u => u.Urls).Returns(new Url[] { 
                new Url{ UrlId = 0, UrlCode = "TYUR", OriginalUrl="https://fluentvalidation.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 1, UrlCode = "TwUR", OriginalUrl="https://facebook.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 2, UrlCode = "TkUR", OriginalUrl="https://youtube.com/", IpAddress="127.0.0.1", PostedDate = DateTime.Now}
            }.AsQueryable());

            // Arrange - Index a controller

            UrlShortenerController target = new UrlShortenerController(mock.Object);
            //target.ip = "127.0.0.1"; // Controller modified 
            //Action
            Url[] result = ((IEnumerable<Url>)target.Index().ViewData.Model).ToArray();

            //Assert

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("TKUR", result[2].UrlCode);
            Assert.AreEqual("TwUR", result[1].UrlCode);
            Assert.AreEqual("TYUR", result[0].UrlCode);

        }
        [TestMethod]
        public void Should_know_if_Url_Exist_or_Not()
        {
            /*
            Mock<IUrlsRepository> mock = new Mock<IUrlsRepository>();
            mock.Setup(u => u.Urls).Returns(new Url[] { 
                new Url{ UrlId = 0, UrlCode = "TYUR", OriginalUrl="https://fluentvalidation.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 1, UrlCode = "TwUR", OriginalUrl="https://facebook.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 2, UrlCode = "TkUR", OriginalUrl="https://youtube.com/", IpAddress="127.0.0.1", PostedDate = DateTime.Now}
            }.AsQueryable());
            */
            ArrayList arr = new ArrayList();
            arr.AddRange(new Url[]
            {
                new Url{ UrlId = 0, UrlCode = "TYUR", OriginalUrl="https://fluentvalidation.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 1, UrlCode = "TwUR", OriginalUrl="https://facebook.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 2, UrlCode = "TkUR", OriginalUrl="https://youtube.com/", IpAddress="127.0.0.1", PostedDate = DateTime.Now}
            });
            

            Assert.IsTrue(val.notExist("https://www.youtube.com/"));
            Assert.IsFalse(val.notExist("https://www.facebook.com/"));

        }
      
    }
}
