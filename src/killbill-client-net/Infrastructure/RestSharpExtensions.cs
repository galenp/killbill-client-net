using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace KillBill.Client.Net.Infrastructure
{
    public static class RestSharpExtensions
    {
        public static string GetValue(this IList<Parameter> headers, string key)
        {
            var hdr = headers.FirstOrDefault(x => x.Name.ToString() == key);
            return hdr == null ? null : hdr.Value.ToString();
        }

        public static int ToInt(this string str, int value = 0)
        {
            int.TryParse(str, out value);
            return value;
        }
         
    }
}