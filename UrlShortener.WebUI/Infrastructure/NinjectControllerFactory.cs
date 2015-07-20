using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using System.Web;
using UrlShortener.Domain.Abstract;
using UrlShortener.Domain.Entities;
using Moq;
using UrlShortener.Domain.Concrete;

namespace UrlShortener.WebUI.Infrastructure
{
    public class NinjectControllerFactory:DefaultControllerFactory
    {
        private IKernel ninjectkernel;

        public NinjectControllerFactory()
        {
            //create object of the ninject kernel
            ninjectkernel = new StandardKernel();
            //We then add our bindings that explain how our controllers call
            //should be handled
            AddBindings();
        }
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            //Each page request comes here first, the controller name is then return as requested
            return controllerType == null
                ? null
                : (IController)ninjectkernel.Get(controllerType);
        }

        private void AddBindings()
        {
            /*
            Mock<IUrlsRepository> mock = new Mock<IUrlsRepository>();
            mock.Setup(m => m.Urls).Returns(new List<Url>{
            new Url{ OriginalUrl = "www.facebook.com", UrlCode="fcb", PostedDate = DateTime.Today.Date },
            new Url{ OriginalUrl = "www.twitter.com", UrlCode="twt", PostedDate = DateTime.Today.Date.AddDays(-2) },
            new Url{ OriginalUrl = "www.gmail.com", UrlCode="gml", PostedDate = DateTime.Today.Date.AddDays(-1) }
            }.AsQueryable());

            ninjectkernel.Bind<IUrlsRepository>().ToConstant(mock.Object);
            */

            //Here we inject our Interface in to the DBcontext
            //In other word, we bind our DB with our Model class here
            ninjectkernel.Bind<IUrlsRepository>().To<EFUrlRepository>();
        }
    }
}