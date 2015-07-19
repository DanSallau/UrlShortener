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
            //Validate the Form using FluentValidation
            context = new EFDbContext();
            RuleFor(x => x.strUrl).NotEmpty().WithMessage("Url cannot be empty")
                .Must((url, e) => isValidUrl(url.strUrl)).WithMessage("Please provide a valid url")
                .Must((url,e) => notExist(url.strUrl)).WithMessage("Record Exist, See the first list");

        }
        public bool isValidUrl(string url)
        {
            //Check whether The Url is valid Url
            bool success = false;

            success = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                      && (   uriResult.Scheme == Uri.UriSchemeHttp 
                  || uriResult.Scheme == Uri.UriSchemeHttps );

            return success;
        }
        public bool notExist(string url)
        {
            //Check if The Url Exist . This is to stop replication of Urls.
            bool exist = context.Urls.Any(x => x.OriginalUrl == url);

            return (exist == false);
        }
    }
}