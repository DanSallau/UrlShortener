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
        public static string ip;
        public UrlShortenerController(IUrlsRepository repo)
        {
            //Inject Dependency.
            repository = repo;

        }
        [HttpGet]
        public ViewResult Index()
        {
            //Default or home controller, a first visit to our site calls this controller.
            ip = ip == null ? GetVisitorIpAddress() : ip;
            var model = new UrlShortenerModel()
            {
                strUrl = null,
                //Select and order our list using Linq
                urlList = repository.Urls.Where(x => x.IpAddress.Contains(ip)).OrderByDescending(x => x.PostedDate)
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult ShortenURl(UrlShortenerModel model)
        {
            //This is a Post method and handles Form submision .
            //Its responsible to collecting the Url submission and shortening.

            bool succes = false;

            ip = ip == null ? GetVisitorIpAddress() : ip;
            var UrlLink = new Url { OriginalUrl = model.strUrl };

            if (ModelState.IsValid)// check if the form validation is correct
            {
                //Set the ip value and postAddress here before we add to the DB
                UrlLink.IpAddress = ip;
                UrlLink.PostedDate = DateTime.Now;
                succes = repository.AddUrl(UrlLink);
                if (succes)
                {
                    // If Add url succeed, return redirect to our Index action method
                    return RedirectToAction("Index");
                }
                else
                {
                    //If action fails, it may be due to db error. Add the erro
                    //and populate the text box with the keyed in text to avoid data loss.
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
                //When Record exist error is encountered, we reshuffle the records as shown below
                //We loop through the errors to determine if record exist error is returned.
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
                            //We Reshuffle the records bringing the existing record top
                            return View("Index", model);
                        }

                    }
                }
                //If record do not exist, perhaps the error might be that the text box is empty.
                // the text provided is not a valid link, so we refer the user back to the index 
                //retaining which ever text is provided 
                model = new UrlShortenerModel()
                {
                    strUrl = UrlLink.OriginalUrl,
                    urlList = repository.Urls.Where(x => x.IpAddress.Equals(ip)).OrderByDescending(x => x.PostedDate)
                };

            }
            // Finally return the user and his data back to the same page
            return View("Index", model);
        }
        public ActionResult ly(string urlcode)
        {
            //The method responsble for redirecting and translating UrlCode to the original url

            string link = repository.Urls.FirstOrDefault(x => x.UrlCode == urlcode).OriginalUrl;

            return Redirect(link);

        }
        //Get Visitor IP address method
        private string GetVisitorIpAddress()
        {
            string stringIpAddress;

            //Incase user is coming from a proxy, we seach the ip in the html header 
            //Through the HTTP_X_FORWARDED_FOR
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
