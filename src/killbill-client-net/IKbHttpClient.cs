using System.Collections.Generic;
using KillBill.Client.Net.Infrastructure;
using RestSharp;

namespace KillBill.Client.Net
{
    public interface IKbHttpClient
    {
        IRestResponse Get(string uri, MultiMap<string> options);
        T Get<T>(string uri, MultiMap<string> options) where T : class;
        T GetWithUrl<T>(string uri, MultiMap<string> options) where T : class;

        IRestResponse Post(string uri, object body, MultiMap<string> options);
        T Post<T>(string uri, object body, MultiMap<string> options) where T : class;
        T Post<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow,bool followLocation) where T : class;
        T PostAndFollow<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow ) where T : class;

        IRestResponse Put(string uri, object body, MultiMap<string> options);
        T Put<T>(string uri, object body, MultiMap<string> options) where T : class;
        T Put<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow, bool followLocation) where T : class;        
        T PutAndFollow<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow) where T : class;

        IRestResponse Delete(string uri, MultiMap<string> options);
        IRestResponse Delete(string uri, object body, MultiMap<string> options);
        T Delete<T>(string uri, object body, MultiMap<string> options) where T : class;
        T Delete<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow, bool followLocation) where T : class;
        T DeleteAndFollow<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow) where T : class;
    }
}
