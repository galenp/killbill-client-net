using System.Collections.Generic;
using KillBill.Client.Net.Infrastructure;
using RestSharp;

namespace KillBill.Client.Net
{
    public interface IKbHttpClient
    {
        IRestResponse Get(string uri, MultiMap<string> options);
        T Get<T>(string uri, MultiMap<string> options) where T : new();
        T GetWithUrl<T>(string uri, MultiMap<string> options) where T : new();

        IRestResponse Post(string uri, object body, MultiMap<string> options);
        T Post<T>(string uri, object body, MultiMap<string> options) where T : new();
        T Post<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow,bool followLocation) where T : new();

        IRestResponse Put(string uri, object body, MultiMap<string> options);
        T Put<T>(string uri, object body, MultiMap<string> options) where T : new();
        T Put<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow, bool followLocation) where T : new();
    }
}
