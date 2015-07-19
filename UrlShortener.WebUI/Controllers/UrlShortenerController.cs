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
        [HttpGet]
        public ActionResult Index()
        {
            var ip = GetVisitorIpAddress();
            var model = new UrlShortenerModel()
            {
                strUrl = null,
                urlList = repository.Urls.Where(x => x.IpAddress.Contains(ip)).OrderByDescending(x => x.PostedDate)
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult ShortenURl(UrlShortenerModel model)
        {
            
            var ip = GetVisitorIpAddress();
            bool succes = false;
            var UrlLink = new Url { OriginalUrl = model.strUrl};

            if (ModelState.IsValid)
            {
                UrlLink.IpAddress = ip;
                UrlLink.PostedDate = DateTime.Now;
                succes = repository.AddUrl(UrlLink);
                if (succes)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Fail", "Adding Url failed");
                    model = new UrlShortenerModel()
                    {
                        strUrl = UrlLink.OriginalUrl,
                        urlList = repository.Urls.Where(x => x.IpAddress.Equals(ip)).OrderByDescending(x => x.PostedDate)
                    };
                }
            }
            else
            {
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        if (error.ErrorMessage.Contains("Record Exist, See the first list"))
                        {
                            model = new UrlShortenerModel()
                            {
                                strUrl = UrlLink.OriginalUrl,
                                urlList = repository.Urls.Where(x => x.IpAddress.Equals(ip)).OrderByDescending(x => x.OriginalUrl == model.strUrl)
                            };

                            return View("Index", model);
                        }

                    }
                }
                model = new UrlShortenerModel()
                {
                    strUrl = UrlLink.OriginalUrl,
                    urlList = repository.Urls.Where(x => x.IpAddress.Equals(ip)).OrderByDescending(x => x.PostedDate)
                };

            }

            return View("Index",model);
        }
        public ActionResult ly(string urlcode)
        {
            string link = repository.Urls.FirstOrDefault(x => x.UrlCode == urlcode).OriginalUrl;

            return Redirect(link);

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
