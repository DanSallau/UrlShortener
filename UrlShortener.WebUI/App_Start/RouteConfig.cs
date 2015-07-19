using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UrlShortener.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "RedirectCtrl", // Route name
               "{urlcode}", // URL with parameters
               new { action = "ly", controller = "UrlShortener" }, // parameter defaults 
               new[] { "UrlShortener.WebUI.Controllers" } // controller namespaces
               );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "UrlShortener", action = "Index", id = UrlParameter.Optional }
            );

           
        }
    }
}