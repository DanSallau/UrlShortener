using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Concrete
{
    public class EFDbContext:DbContext
    {
        public virtual IDbSet<Url> Urls { get; set; }
    }
}
