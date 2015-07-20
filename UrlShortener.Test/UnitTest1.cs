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
using System.Data.Entity;
using UrlShortener.Domain.Concrete;

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
            //Test if our url validator is working 
            Assert.IsTrue(val.isValidUrl("https://fluentvalidation.codeplex.com/wikipage?title=Testing"));
            Assert.IsFalse(val.isValidUrl("notvalidUrl"));
        }
        [TestMethod]
        public void Test_Create_With_Validation_Error()
        {
            //  Arrange and mock our Repository with ourfake data
             Mock<IUrlsRepository> mock = new Mock<IUrlsRepository>();
            mock.Setup(u => u.Urls).Returns(new Url[] { 
                new Url{ UrlId = 0, UrlCode = "TYUR", OriginalUrl="https://fluentvalidation.com", PostedDate = DateTime.Now},
                new Url{ UrlId = 1, UrlCode = "TwUR", OriginalUrl="https://facebook.com", PostedDate = DateTime.Now},
                new Url{ UrlId = 2, UrlCode = "TkUR", OriginalUrl="https://youtube.com/", PostedDate = DateTime.Now}
            }.AsQueryable());

            UrlShortenerController.ip = "127.0.0.1";
            UrlShortenerController controller= new UrlShortenerController(mock.Object);
             
            controller.ModelState.AddModelError("NoteText", "NoteText cannot be null");
            UrlShortenerModel model = new UrlShortenerModel();
            
            // Act
            ActionResult result = controller.ShortenURl(model);

            // Assert , when we encounter an error, we redirect back to the same
            // Shorten url controller method which is of ActionResult type.
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }
        [TestMethod]
        public void Test_Create_Without_Validation_Error()
        {
            // Arrange and mock our Repository with ourfake data
            Mock<IUrlsRepository> mock = new Mock<IUrlsRepository>();
            mock.Setup(u => u.Urls).Returns(new Url[] { 
                new Url{ UrlId = 0, UrlCode = "TYUR", OriginalUrl="https://fluentvalidation.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 1, UrlCode = "TwUR", OriginalUrl="https://facebook.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 2, UrlCode = "TkUR", OriginalUrl="https://youtube.com/", IpAddress="127.0.0.1", PostedDate = DateTime.Now}
            }.AsQueryable());

            UrlShortenerController.ip = "127.0.0.1"; // Set Ip 
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
            // Arrange - Fill our model with data first.

            UrlShortenerModel rs = new UrlShortenerModel(){
                 strUrl = "string",
                 urlList = new Url[]{
                       new Url{ UrlId = 0, UrlCode = "TYUR", OriginalUrl="https://fluentvalidation.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                       new Url{ UrlId = 1, UrlCode = "TwUR", OriginalUrl="https://facebook.com", IpAddress="127.0.0.2", PostedDate = DateTime.Now},
                       new Url{ UrlId = 2, UrlCode = "TkUR", OriginalUrl="https://youtube.com/", IpAddress="127.0.0.1", PostedDate = DateTime.Now}
          
                 }.AsQueryable()
            };
            
            //Assert , to determine if your model is ready

            Assert.AreEqual(rs.urlList.Count(), 3);
            Assert.AreEqual(rs.urlList.Count(x=>x.IpAddress==rs.urlList.ToList()[1].IpAddress), 1);
            Assert.AreEqual("TkUR", rs.urlList.ToList()[2].UrlCode);
            Assert.AreEqual("TwUR", rs.urlList.ToList()[1].UrlCode);
            Assert.AreEqual("TYUR", rs.urlList.ToList()[0].UrlCode);

        }
        [TestMethod]
        public void CreateUrl_saves_a_url_via_context()
        {
            // IF our create and save url is adding to the db

            //Mock our Model
            var mockSet = new Mock<IDbSet<Url>>();

            var mockContext = new Mock<EFDbContext>();

            //Bind our model to our Db context
            mockContext.Setup(m => m.Urls).Returns(mockSet.Object);

            var repository = new EFUrlRepository();
            Url url = new Url() { UrlCode = "TYUyR", OriginalUrl = "https://fluentvalidation.com", IpAddress = "127.0.0.1", PostedDate = DateTime.Now };
            repository.context = mockContext.Object;

            //Try adding new record
            repository.AddUrl(url);

            //We add once in the AddUrl method
            mockSet.Verify(m => m.Add(It.IsAny<Url>()), Times.Once());

            //We save changes twice in our Method
            mockContext.Verify(m => m.SaveChanges(), Times.AtMost(2));
        }
        [TestMethod]
        public void Should_know_if_Url_Exist_or_Not()
        {
            //Cheate an array of our Model class
            var arr = new Url[]
            {
                new Url{ UrlId = 0, UrlCode = "TYUR", OriginalUrl="https://fluentvalidation.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 1, UrlCode = "TwUR", OriginalUrl="https://www.facebook.com", IpAddress="127.0.0.1", PostedDate = DateTime.Now},
                new Url{ UrlId = 2, UrlCode = "TkUR", OriginalUrl="https://www.youtube.com/", IpAddress="127.0.0.1", PostedDate = DateTime.Now}
            }.AsQueryable();

            //mock and create the fake of our Url
            var mockSet = new Mock<IDbSet<Url>>();
            mockSet.As<IQueryable<Url>>().Setup(m => m.Provider).Returns(arr.Provider);
            mockSet.As<IQueryable<Url>>().Setup(m => m.Expression).Returns(arr.Expression);
            mockSet.As<IQueryable<Url>>().Setup(m => m.ElementType).Returns(arr.ElementType);
            mockSet.As<IQueryable<Url>>().Setup(m => m.GetEnumerator()).Returns(arr.GetEnumerator()); 
 
            var fakeContext = new Mock<EFDbContext>();
            fakeContext.SetupGet(ctx => ctx.Urls).Returns(mockSet.Object);

            var validator = new  UrlValidation();
            validator.context = (EFDbContext)fakeContext.Object;

            //the url below exist in our record above, therefore notExist shall be false.
            Assert.IsFalse(validator.notExist("https://www.youtube.com/"));

            //the url below does not exist in our record above and hence notExist shall return true
            Assert.IsTrue(validator.notExist("https://www.facebooker.com/"));

        }
      
    }
}
