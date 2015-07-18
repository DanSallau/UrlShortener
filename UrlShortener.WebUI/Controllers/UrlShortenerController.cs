using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrlShortener.Domain.Abstract;
using UrlShortener.WebUI.Models;
using UrlShortener.Domain.Entities;
using System.Net;

namespace UrlShortener.WebUI.Controllers
{
    public class UrlShortenerController : Controller
    {
        //
        // GET: /UrlShortener/
        private IUrlsRepository repository;

        public UrlShortenerController(IUrlsRepository repo)
        {
            repository = repo;
        }
        public ActionResult Index()
        {
            var ip = GetVisitorIpAddress();
            

            var model =new UrlShortenerModel()
            { 
                url = new Url() ,
                urlList =repository.Urls.Where(x=>x.IpAddress.Contains(ip)).OrderByDescending(x=>x.PostedDate) 
            };

            return View(model);
        }
        //Get Visitor IP address method
        private string GetVisitorIpAddress()
        {
            string stringIpAddress;
            stringIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (stringIpAddress == null) //may be the HTTP_X_FORWARDED_FOR is null
                stringIpAddress = Request.ServerVariables["REMOTE_ADDR"]; //we can use REMOTE_ADDR
            else if (stringIpAddress == null)
                stringIpAddress = GetLanIPAddress();

            return stringIpAddress;
        }

        //Get Lan Connected IP address method
        private string GetLanIPAddress()
        {
            //Get the Host Name
            string stringHostName = Dns.GetHostName();
            //Get The Ip Host Entry
            IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
            //Get The Ip Address From The Ip Host Entry Address List
            System.Net.IPAddress[] arrIpAddress = ipHostEntries.AddressList;
            return arrIpAddress[arrIpAddress.Length - 1].ToString();
        }
    }
}
