using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Abstract;
using UrlShortener.Domain.ClassHelpers;

namespace UrlShortener.Domain.Concrete
{
    public class EFUrlRepository : IUrlsRepository
    {
        public EFDbContext context = new EFDbContext();
        private Security security = new Security();
        public IQueryable<Url> Urls
        {
            get { return context.Urls; }
        }

        public bool AddUrl(Url url)
        {
            if(url.UrlId == 0)
            {
                context.Urls.Add(url);
                context.SaveChanges();
                url.UrlCode = security.Encrypt(url.UrlId.ToString());
                context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
