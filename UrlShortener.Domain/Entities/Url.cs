using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Domain.Entities
{
    public class Url
    {
        public int UrlId { get; set; }
        public string OriginalUrl { get; set; }
        public string UrlCode { get; set; }
        public DateTime PostedDate { get; set; }
        public string IpAddress { get; set; }
    }
}
