using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using UrlShortener.WebUI.Models;

namespace UrlShortener.WebUI.Validators
{
    public class UrlValidation: AbstractValidator<UrlShortenerModel> 
    {
        Uri uriResult;
        public UrlValidation()
        {
            RuleFor(x => x.strUrl).NotEmpty().WithMessage("Url cannot be empty")
                .Must((url, e) => isValidUrl(url.strUrl)).WithMessage("Please provide a valid url");

        }
        private bool isValidUrl(string url)
        {
            bool success = false;

            success = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                      && (   uriResult.Scheme == Uri.UriSchemeHttp 
                  || uriResult.Scheme == Uri.UriSchemeHttps );

            return success;
        }
    }
}