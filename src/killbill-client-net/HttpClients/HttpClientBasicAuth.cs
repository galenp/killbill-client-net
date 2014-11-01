using System.Configuration;
using RestSharp;

namespace KillBill.Client.Net.HttpClients
{
    public class HttpClientBasicAuth : BaseKillBillHttpClient
    {
        public override IAuthenticator ApiAuthentication()
        {
            return new HttpBasicAuthenticator(ConfigurationManager.AppSettings["kb.http.user"], ConfigurationManager.AppSettings["kb.http.password"]);
        }
    }
}