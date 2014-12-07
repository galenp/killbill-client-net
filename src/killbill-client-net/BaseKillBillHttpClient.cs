using System;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using KillBill.Client.Net.Infrastructure;
using KillBill.Client.Net.JSON;
using KillBill.Client.Net.Model;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

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
            return SendRequestAndMaybeFollowLocation<T>(Method.PUT, uri, body, options, optionsForFollow, true);
        }


        // DELETE
        //-------------------------------------------------------------------------------
        // - untyped without follow
        public IRestResponse Delete(string uri, MultiMap<string> options)
        {
            return SendRequest(Method.DELETE, uri, null, options);
        }

        public IRestResponse Delete(string uri, object body, MultiMap<string> options)
        {
            return SendRequest(Method.DELETE, uri, body, options);
        }

        // - typed without follow
        public T Delete<T>(string uri, object body, MultiMap<string> options) where T : new()
        {
            return Delete<T>(uri, body, options, DEFAULT_EMPTY_QUERY, false);
        }

        // - typed with follow
        public T Delete<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow, bool followLocation) where T : new()
        {
            return SendRequestAndMaybeFollowLocation<T>(Method.DELETE, uri, body, options, optionsForFollow, followLocation);
        }

        public T DeleteAndFollow<T>(string uri, object body, MultiMap<string> options, MultiMap<string> optionsForFollow) where T : new()
        {
            return SendRequestAndMaybeFollowLocation<T>(Method.DELETE, uri, body, options, optionsForFollow, true);
        }


        // COMMON
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
               
                var locationHeader = responseToFollow.Headers.SingleOrDefault(
                        h => h.Type == ParameterType.HttpHeader && h.Name == "Location");
                if (locationHeader == null || locationHeader.Value == null ||
                    locationHeader.Value.ToString() == string.Empty)
                    return default(T);

                var locationUri = new Uri(locationHeader.Value.ToString());

                return GetWithUrl<T>(locationUri.PathAndQuery, optionsForFollow);
            }

            var response = ExecuteRequest(request);
            var data = DeserializeResponse<T>(response);
            return data;
        }

        private T DeserializeResponse<T>(IRestResponse response)
        {
            var obj = JsonConvert.DeserializeObject<T>(response.Content, JsonNetSerializationSettings.GetDefault());

            if (obj.GetType().GetInterfaces().Contains(typeof(IKillBillObjects)))
            {
                var objects = obj as IKillBillObjects;

                //Get Pagination meta data from the response headers
                if (objects != null)
                {
                    var paginationCurrentOffset = response.Headers.GetValue(KbConfig.HDR_PAGINATION_CURRENT_OFFSET);
                    if (paginationCurrentOffset != null)
                        objects.PaginationCurrentOffset = paginationCurrentOffset.ToInt();

                    var paginationNextOffset = response.Headers.GetValue(KbConfig.HDR_PAGINATION_NEXT_OFFSET);
                    if (paginationNextOffset != null)
                        objects.PaginationNextOffset = paginationNextOffset.ToInt();

                    var paginationMaxNbRecords = response.Headers.GetValue(KbConfig.HDR_PAGINATION_MAX_NB_RECORDS);
                    if (paginationMaxNbRecords != null)
                        objects.PaginationMaxNbRecords = paginationMaxNbRecords.ToInt();

                    var paginationNextPageUri = response.Headers.GetValue(KbConfig.HDR_PAGINATION_NEXT_PAGE_URI);
                    if (paginationNextPageUri != null)
                        objects.PaginationNextPageUri = paginationNextPageUri;

                    objects.KillBillHttpClient = this;                   
                }
            }

            return obj;
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
        //private IRestResponse<T> ExecuteRequest<T>(IRestRequest request) where T : new()
        //{
        //    var baseUri = request.Resource.StartsWith("http") ? "" : KbConfig.ServerUrl;
        //    var client = CreateClient(baseUri);
        //    var response = client.Execute<T>(request);

        //    T defaultObject;

        //    if (!CheckResponse(response, out defaultObject))
        //        response.Data = defaultObject;

           

        //    return response;
        //}


        /// <summary>
        /// Sends the request and returns the response.
        /// </summary>
        /// <param name="request">API request</param>
        /// <returns>API response</returns>
        private IRestResponse ExecuteRequest(IRestRequest request)
        {
            var baseUri = KbConfig.ServerUrl;
            var client = CreateClient(baseUri);

            if (request.Resource.Contains("http"))
                throw new ArgumentException("Request.Resource should be a relative Uri (/location) and not the full Url (http, domain etc)");
            
            var response = client.Execute(request);

            object defaultObject;
            CheckResponse(response, out defaultObject);

            return response;
        }


        private void CheckResponse<T>(IRestResponse response, out T defaultObject) where T : new()
        {
            if (response == null)
                throw new KillBillClientException("Error calling KillBill: no response");

            if (response.StatusCode == HttpStatusCode.Unauthorized) {
                 throw new KillBillClientException(response.ErrorMessage, new ArgumentException("Unauthorized - did you configure your RBAC and/or tenant credentials?"));
            }

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                // Return empty list for KillBillObjects instead of null for convenience
                if (typeof (T).IsAssignableFrom(typeof (KillBillObjects<>)))
                {
                    defaultObject = Activator.CreateInstance<T>();
                    return;
                }
            }

            if (response.StatusCode >= HttpStatusCode.BadRequest && response.Content != null)
            {
                var billingException = JsonConvert.DeserializeObject<BillingException>(response.Content, JsonNetSerializationSettings.GetDefault());
                var message = "Error " + response.StatusCode + " from Kill Bill" + billingException.Message;
                throw new KillBillClientException(message);
            }

            if (response.ErrorException != null)
            {
                const string message = "An unexpected error occurred connecting with KillBill: ";
                var exception = new KillBillClientException(message, response.ErrorException);
                throw exception;
            }

            defaultObject = default(T);
        }

        private RestClient CreateClient(string baseUri)
        {
            string proxyUri = null;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["kb.api.proxy"]))
            {
                proxyUri = ConfigurationManager.AppSettings["kb.api.proxy"];
            }


            return new RestClient(baseUri)
            {
                Authenticator = ApiAuthentication(),
                Proxy = proxyUri == null ? new WebProxy(proxyUri) : null
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