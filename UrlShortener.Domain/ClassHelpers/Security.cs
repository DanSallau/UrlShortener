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
            //Encode text into 64bit 
            var bytes = Encoding.UTF8.GetBytes(clearText);
            var base64 = Convert.ToBase64String(bytes);

            return base64;
        }
        public string Decrypt(string cipherText)
        {
            var base64 = cipherText;

            //Decode 64bit to a characters 
            var data = Convert.FromBase64String(base64);

            //And finally Encode the resulting text into a string/unicode
            //equivalent using the UTF8 character encoder.
            string str = Encoding.UTF8.GetString(data);

            return str;
        }
    }
}
