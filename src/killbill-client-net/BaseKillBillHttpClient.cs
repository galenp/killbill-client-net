using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using KillBill.Client.Net.Infrastructure;
using KillBill.Client.Net.JSON;
using KillBill.Client.Net.Model;
using RestSharp;

namespace KillBill.Client.Net
{
    public abstract class BaseKillBillHttpClient : IKbHttpClient
    {
        public abstract IAuthenticator ApiAuthentication();
        private static MultiMap<string> DEFAULT_EMPTY_QUERY = new MultiMap<string>();

        // GET
        //-------------------------------------------------------------------------------
        public IRestResponse Get(string uri, MultiMap<string> options)
        {
            return SendRequest(Method.GET, uri, null, options);
        }

        public T Get<T>(string uri, MultiMap<string> options) where T : new()
        {
            return GetWithUrl<T>(uri, options);
        }

        public T GetWithUrl<T>(string uri, MultiMap<string> options) where T : new()
        {
            return SendRequestAndMaybeFollowLocation<T>(Method.GET, uri, options);
        }


        // POST
        //-------------------------------------------------------------------------------
        // - untyped without follow
        public IRestResponse Post(string uri, object body, MultiMap<string> options)
        {
            return SendRequest(Method.POST, uri, body, options);
        }

        // - typed without follow
        public T Post<T>(string uri, object body, MultiMap<string> options)  where T : new()
        {
            return Post<T>(uri, body, options, DEFAULT_EMPTY_QUERY, false);
        }

        // - typed with follow
        public T Post<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow, bool followLocation) where T : new()
        {
            return SendRequestAndMaybeFollowLocation<T>(Method.POST, uri, body, options, optionsForFollow, followLocation);
        }

        public T PostAndFollow<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow) where T : new() 
        {
            return SendRequestAndMaybeFollowLocation<T>(Method.POST, uri, body, options, optionsForFollow, true);
        }

        // PUT
        //-------------------------------------------------------------------------------
        // - untyped without follow
        public IRestResponse Put(string uri, object body, MultiMap<string> options)
        {
            return SendRequest(Method.PUT, uri, body, options);
        }

        // - typed without follow
        public T Put<T>(string uri, object body, MultiMap<string> options) where T : new()
        {
            return Put<T>(uri, body, options, DEFAULT_EMPTY_QUERY, false);
        }

        // - typed with follow
        public T Put<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow, bool followLocation) where T : new()
        {
            return SendRequestAndMaybeFollowLocation<T>(Method.PUT, uri, body, options, optionsForFollow, followLocation);
        }

        public T PutAndFollow<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow) where T : new()
        {
            return SendRequestAndMaybeFollowLocation<T>(Method.POST, uri, body, options, optionsForFollow, true);
        }

        // COIMMON
        //-------------------------------------------------------------------------------
        private T SendRequestAndMaybeFollowLocation<T>(Method method, string uri, MultiMap<string> options) where T : new()
        {
            return SendRequestAndMaybeFollowLocation<T>(method, uri, null, options, DEFAULT_EMPTY_QUERY, false);
        }


        private IRestResponse SendRequest(Method method, string uri, object body, MultiMap<string> options)
        {
            var request = BuildRequestWithHeaderAndQuery(method, uri, options);

            if (method != Method.GET || method != Method.HEAD)
            {
                if (body != null)
                {
                    try { request.AddBody(body); }
                    catch (Exception ex) { throw new KillBillClientException("Error serializing object for API request.", ex); }
                }
            }

            return ExecuteRequest(request);
        }

        private T SendRequestAndMaybeFollowLocation<T>(Method method, string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow, bool followLocation) where T : new()
        {
            
            var request = BuildRequestWithHeaderAndQuery(method, uri, options);

            if (method != Method.GET || method != Method.HEAD) {
                if (body != null)
                {
                    try
                    {
                        request.AddBody(body);
                    }
                    catch (Exception ex)
                    {
                        throw new KillBillClientException("Error serializing object for API request.", ex);
                    }
                }
            }

            if (followLocation)
            {
                var responseToFollow = ExecuteRequest(request);
                
                if (responseToFollow == null)
                    return default(T);

                var locationHeader = responseToFollow.Headers.SingleOrDefault(h => h.Type == ParameterType.HttpHeader && h.Name == "Location");
                if (locationHeader == null || locationHeader.Value == null || locationHeader.Value.ToString() == string.Empty)
                    return default(T);

                return GetWithUrl<T>(locationHeader.Value.ToString(), optionsForFollow);
            }

            var response = ExecuteRequest<T>(request);

            return response.Data;
        }

        

        private IRestRequest BuildRequestWithHeaderAndQuery(Method method, string uri, MultiMap<string> options)
        {
            var request = new RestRequest(uri, method)
            {
                RequestFormat = DataFormat.Json
            };

            if (options == null)
                options = new MultiMap<string>();

            request.JsonSerializer = new RestSharpJsonNetSerializer();

            var createdBy = GetUniqueValue(options, KbConfig.AUDIT_OPTION_CREATED_BY);
            var reason = GetUniqueValue(options, KbConfig.AUDIT_OPTION_REASON);
            var comment = GetUniqueValue(options, KbConfig.AUDIT_OPTION_COMMENT);
            var apiKey = KbConfig.ApiKey;
            var apiSecret = KbConfig.ApiSecret;

            options.RemoveAll(KbConfig.AUDIT_OPTION_CREATED_BY);
            options.RemoveAll(KbConfig.AUDIT_OPTION_REASON);
            options.RemoveAll(KbConfig.AUDIT_OPTION_COMMENT);
            options.RemoveAll(KbConfig.TENANT_OPTION_API_KEY);
            options.RemoveAll(KbConfig.TENANT_OPTION_API_SECRET);

            // Multi Tenancy Headers
            request.AddHeader("X-Killbill-ApiSecret", apiSecret);
            request.AddHeader("X-Killbill-ApiKey", apiKey);

            if (createdBy != null)
                request.AddHeader("X-Killbill-CreatedBy", createdBy);

            if (comment != null)
                request.AddHeader("X-Killbill-Comment", comment);

            if (reason != null)
                request.AddHeader("X-Killbill-Reason", reason);

           
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("Accept", "application/json, text/html");

            foreach (var key in options.Keys)
                request.AddParameter(key, options[key].FirstOrDefault(), ParameterType.QueryString);

            return request;
        }
       

        /// <summary>
        /// Sends the request and deserialized the response to type T
        /// </summary>
        /// <typeparam name="T">RESPONSE type to deserialize.</typeparam>
        /// <param name="request">Request</param>
        /// <returns>Deserialized T from response.</returns>
        private IRestResponse<T> ExecuteRequest<T>(IRestRequest request) where T : new()
        {
            var baseUri = request.Resource.StartsWith("http") ? "" : KbConfig.ServerUrl;
            var client = CreateClient(baseUri);
            var response = client.Execute<T>(request);

            T defaultObject;

            if (!CheckResponse(response, out defaultObject))
                response.Data = defaultObject;

            return response;
        }


        /// <summary>
        /// Sends the request and returns the response.
        /// </summary>
        /// <param name="request">API request</param>
        /// <returns>API response</returns>
        private IRestResponse ExecuteRequest(IRestRequest request)
        {
            var baseUri = request.Resource.StartsWith("http") ? "" : KbConfig.ServerUrl;
            var client = CreateClient(baseUri);            
            var response = client.Execute(request);

            object defaultObject;
            CheckResponse(response, out defaultObject);

            return response;
        }


        private bool CheckResponse<T>(IRestResponse response, out T defaultObject) where T : new()
        {
            if (response == null)
                throw new KillBillClientException("Error calling KillBill: no response");

            if (response.ErrorException != null)
            {
                const string message = "An unexpected error occurred connecting with KillBill: ";
                var exception = new KillBillClientException(message, response.ErrorException);
                throw exception;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.NoContent)
            {
                // Return empty list for KillBillObjects instead of null for convenience
                if (typeof (T).IsAssignableFrom(typeof (KillBillObjects<>)))
                {
                    defaultObject = Activator.CreateInstance<T>();
                    return false;
                }
                    
            }

            defaultObject = default(T);
            return true;
        }

        private RestClient CreateClient(string baseUri)
        {
            return new RestClient(baseUri)
            {
                Authenticator = ApiAuthentication(),
                Proxy = new WebProxy("http://localhost:8888")
            };
        }


        #region private helpers

        private string GetUniqueValue(MultiMap<string> options, string key)
        {
            if (options == null || !options.Keys.Contains(key))
                return null;

            var values = options[key];
            
            if (values == null || values.Count == 0)
                return null;
            
            return values.First();
        }
        #endregion
    }
}