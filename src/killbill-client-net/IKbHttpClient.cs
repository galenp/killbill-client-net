using System;
using System.Collections.Generic;
using KillBill.Client.Net.Infrastructure;
using RestSharp;

namespace KillBill.Client.Net
{
    public interface IKbHttpClient
    {

        IRestResponse Get(string uri, RequestOptions requestOptions);
        T Get<T>(string uri, RequestOptions requestOptions) where T : class;


        IRestResponse Post(string uri, object body, RequestOptions requestOptions);
        T Post<T>(string uri, object body, RequestOptions requestOptions) where T : class;

        IRestResponse Put(string uri, object body, RequestOptions requestOptions);
        T Put<T>(string uri, object body, RequestOptions requestOptions) where T : class;

        //T Put<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow, bool followLocation) where T : class;        
        //T PutAndFollow<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow) where T : class;

        IRestResponse Delete(string uri, RequestOptions requestOptions);
        IRestResponse Delete(string uri, object body, RequestOptions requestOptions);

        //IRestResponse Delete(string uri, MultiMap<string> options);
        //IRestResponse Delete(string uri, object body, MultiMap<string> options);
       
    }
}
