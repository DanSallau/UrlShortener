using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UrlShortener.Domain.Entities;

namespace UrlShortener.WebUI.Models
{
    public class UrlShortenerModel
    {
        public Url url { get; set; }
        public IEnumerable<Url> urlList { get; set; }

    }
}