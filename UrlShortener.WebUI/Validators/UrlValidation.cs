using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using UrlShortener.WebUI.Models;
using UrlShortener.Domain.Concrete;

namespace UrlShortener.WebUI.Validators
{
    public class UrlValidation: AbstractValidator<UrlShortenerModel> 
    {
        Uri uriResult;
        private EFDbContext context;
        public UrlValidation()
        {
            context = new EFDbContext();
            RuleFor(x => x.strUrl).NotEmpty().WithMessage("Url cannot be empty")
                .Must((url, e) => isValidUrl(url.strUrl)).WithMessage("Please provide a valid url")
                .Must((url,e) => notExist(url.strUrl)).WithMessage("Record Exist, See the first list");

        }
        private bool isValidUrl(string url)
        {
            bool success = false;

            success = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                      && (   uriResult.Scheme == Uri.UriSchemeHttp 
                  || uriResult.Scheme == Uri.UriSchemeHttps );

            return success;
        }
        private bool notExist(string url)
        {
            bool exist = context.Urls.Any(x => x.OriginalUrl == url);

            return (exist == false);
        }
    }
}