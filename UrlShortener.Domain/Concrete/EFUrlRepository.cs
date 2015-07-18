using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Abstract;

namespace UrlShortener.Domain.Concrete
{
    public class EFUrlRepository : IUrlsRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Url> Urls
        {
            get { return context.Urls; }
        }
    }
}
