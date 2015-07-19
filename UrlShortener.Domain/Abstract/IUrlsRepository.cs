using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Abstract
{
    public interface IUrlsRepository
    {
        IQueryable<Url> Urls { get; }

        bool AddUrl(Url url);

    }
}
