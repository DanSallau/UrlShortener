using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Domain.ClassHelpers
{
    public class Security
    {
        public string Encrypt(string clearText)
        {
            var bytes = Encoding.UTF8.GetBytes(clearText);
            var base64 = Convert.ToBase64String(bytes);

            return base64;
        }
        public string Decrypt(string cipherText)
        {
            var base64 = cipherText;
            var data = Convert.FromBase64String(base64);

            string str = Encoding.UTF8.GetString(data);

            return str;
        }
    }
}
