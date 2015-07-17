using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using System.Web;
using UrlShortener.Domain.Abstract;
using UrlShortener.Domain.Entities;
using Moq;

namespace UrlShortener.WebUI.Infrastructure
{
    public class NinjectControllerFactory:DefaultControllerFactory
    {
        private IKernel ninjectkernel;

        public NinjectControllerFactory()
        {
            ninjectkernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController)ninjectkernel.Get(controllerType);
        }

        private void AddBindings()
        {
            Mock<IUrlsRepository> mock = new Mock<IUrlsRepository>();
            mock.Setup(m => m.Urls).Returns(new List<Url>{
            new Url{ OriginalUrl = "www.facebook.com", UrlCode="fcb", PostedDate = DateTime.Today.Date },
            new Url{ OriginalUrl = "www.twitter.com", UrlCode="twt", PostedDate = DateTime.Today.Date.AddDays(-2) },
            new Url{ OriginalUrl = "www.gmail.com", UrlCode="gml", PostedDate = DateTime.Today.Date.AddDays(-1) }
            }.AsQueryable());

            ninjectkernel.Bind<IUrlsRepository>().ToConstant(mock.Object);
        }
    }
}