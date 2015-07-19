﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UrlShortener.Domain.Entities;
using FluentValidation.Attributes;
using UrlShortener.WebUI.Validators;

namespace UrlShortener.WebUI.Models
{
    [Validator(typeof(UrlValidation))]
    public class UrlShortenerModel
    {
        public string strUrl { get; set; }
        public IEnumerable<Url> urlList { get; set; }

    }
}