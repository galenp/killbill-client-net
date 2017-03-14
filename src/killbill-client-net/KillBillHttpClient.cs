using System;
using System.Configuration;
using System.Linq;
using System.Net;
using KillBill.Client.Net.Infrastructure;
using KillBill.Client.Net.JSON;
using KillBill.Client.Net.Model;
using Newtonsoft.Json;
using RestSharp;

namespace KillBill.Client.Net
{
    public class KillBillHttpClient : IKbHttpClient
    {
        // GET
        //-------------------------------------------------------------------------------
        public IRestResponse Get(string uri, RequestOptions requestOptions)
        {
            return SendRequest(Method.GET, uri, null, requestOptions);
        }

        public T Get<T>(string uri, RequestOptions requestOptions) where T : class
        {
            return SendRequestAndMaybeFollowLocation<T>(Method.GET, uri, null, requestOptions);
        }


        #region POST
        // POST
        //-------------------------------------------------------------------------------
        // - untyped without follow
       
        public IRestResponse Post(string uri, object body, RequestOptions requestOptions)
        {
            return SendRequest(Method.POST, uri, body, requestOptions);
        }

        public T Post<T>(string uri, object body, RequestOptions requestOptions) where T : class
        {
            return SendRequestAndMaybeFollowLocation<T>(Method.POST, uri, body, requestOptions);
        }
        #endregion

        #region PUT
        // PUT
        //-------------------------------------------------------------------------------
        // - untyped without follow

        public IRestResponse Put(string uri, object body, RequestOptions requestOptions)
        {
            return SendRequest(Method.PUT, uri, body, requestOptions);
        }

        public T Put<T>(string uri, object body, RequestOptions requestOptions) where T : class
        {
            return SendRequestAndMaybeFollowLocation<T>(Method.PUT, uri, body, requestOptions);
        }
        #endregion

        // DELETE
        //-------------------------------------------------------------------------------
        // - untyped without follow
        public IRestResponse Delete(string uri, RequestOptions requestOptions)
        {
            return SendRequest(Method.DELETE, uri, null, requestOptions);
        }

        public IRestResponse Delete(string uri, object body, RequestOptions requestOptions)
        {
            return SendRequest(Method.DELETE, uri, body, requestOptions);
        }


        // SEND REQUEST
        //-------------------------------------------------------------------------------
        private IRestResponse SendRequest(Method method, string uri, object body, RequestOptions requestOptions)
        {
            var request = BuildRequestWithHeaderAndQuery(method, uri, requestOptions);

            if (method != Method.GET || method != Method.HEAD)
            {
                if (body != null)
                {
                    try { request.AddBody(body); }
                    catch (Exception ex) { throw new KillBillClientException("Error serializing object for API request.", ex); }
                }
            }

            return ExecuteRequest(request, requestOptions);
        }

        // SEND REQUEST MAYBE FOLLOW
        //-------------------------------------------------------------------------------
        private T SendRequestAndMaybeFollowLocation<T>(Method method, string uri, object body, RequestOptions requestOptions) where T : class
        {
            var request = BuildRequestWithHeaderAndQuery(method, uri, requestOptions);

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

            //WHEN FollowLocation == TRUE
            if (requestOptions.FollowLocation.HasValue && requestOptions.FollowLocation.Value)
            {
                var responseToFollow = ExecuteRequest(request, requestOptions);

                var locationHeader = responseToFollow?.Headers.SingleOrDefault(
                    h => h.Type == ParameterType.HttpHeader && h.Name == "Location");

                if (locationHeader?.Value == null || locationHeader.Value.ToString() == string.Empty)
                    return default(T);

                var locationUri = new Uri(locationHeader.Value.ToString());

                var optionsForFollow = RequestOptions.Builder()
                                                            .WithUser(requestOptions.User)
                                                            .WithPassword(requestOptions.Password)
                                                            .WithTenantApiKey(requestOptions.TenantApiKey)
                                                            .WithTenantApiSecret(requestOptions.TenantApiSecret)
                                                            .WithRequestId(requestOptions.RequestId)
                                                            .WithFollowLocation(false)
                                                            .WithQueryParams(requestOptions.QueryParamsForFollow)
                                                            .Build();

                return Get<T>(locationUri.PathAndQuery, optionsForFollow);
            }

            var response = ExecuteRequest(request, requestOptions);

            //If there is no response (204) or if an object cannot be found (404), the code will return null (for single objects) or an empty list (for collections of objects).
            if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
            {
                // Return empty list for KillBillObjects instead of null for convenience
                return typeof(T).IsAssignableFrom(typeof(KillBillObjects<>)) ? Activator.CreateInstance<T>() : default(T);
            }

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
                if (objects == null)
                    return obj;

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

            return obj;
        }


        private IRestRequest BuildRequestWithHeaderAndQuery(Method method, string uri, RequestOptions requestOptions)
        {
            var request = new RestRequest(uri, method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new RestSharpJsonNetSerializer()
            };

            // Multi Tenancy Headers
            request.AddHeader("X-Killbill-ApiKey", requestOptions.TenantApiKey);
            request.AddHeader("X-Killbill-ApiSecret", requestOptions.TenantApiSecret);
            

            if (requestOptions.CreatedBy != null)
                request.AddHeader("X-Killbill-CreatedBy", requestOptions.CreatedBy);

            if (requestOptions.Comment != null)
                request.AddHeader("X-Killbill-Comment", requestOptions.Comment);

            if (requestOptions.Reason != null)
                request.AddHeader("X-Killbill-Reason", requestOptions.Reason);
           
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("Accept", "application/json, text/html");

            foreach (var key in requestOptions.QueryParams.Keys)
                request.AddParameter(key, requestOptions.QueryParams[key].FirstOrDefault(), ParameterType.QueryString);

            return request;
        }
       
        
        private IRestResponse ExecuteRequest(IRestRequest request, RequestOptions requestOptions)
        {
            var baseUri = KbConfig.ServerUrl;
            var client = CreateClient(baseUri, requestOptions);

            if (request.Resource.Contains("http"))
                throw new ArgumentException("Request.Resource should be a relative Uri (/location) and not the full Url (http, domain etc)");
            
            var response = client.Execute(request);

            CheckResponse(response, out object defaultObject);

            return response;
        }


        private void CheckResponse<T>(IRestResponse response, out T defaultObject) where T : class
        {
            if (response == null)
                throw new KillBillClientException("Error calling KillBill: no response");

            if (response.StatusCode == HttpStatusCode.Unauthorized) {
                 throw new KillBillClientException(response.ErrorMessage, new ArgumentException("Unauthorized - did you configure your RBAC and/or tenant credentials?"));
            }

            if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
            {
                // Return empty list for KillBillObjects instead of null for convenience
                if (typeof (T).IsAssignableFrom(typeof (KillBillObjects<>)))
                {
                    defaultObject = Activator.CreateInstance<T>();
                    return;
                }

                defaultObject = default(T);
                return;
            }

            if (response.StatusCode >= HttpStatusCode.BadRequest && response.Content != null)
            {
                var billingException = JsonConvert.DeserializeObject<BillingException>(response.Content, JsonNetSerializationSettings.GetDefault());
                var message = "Error " + response.StatusCode + " from Kill Bill " + billingException.Message;
                throw new KillBillClientException(message, response.StatusCode.ToString(), billingException.Code, billingException.Message);
            }

            if (response.ErrorException != null)
            {
                const string message = "An unexpected error occurred connecting with KillBill: ";
                var exception = new KillBillClientException(message, response.ErrorException);
                throw exception;
            }

            defaultObject = default(T);
        }

        private RestClient CreateClient(string baseUri, RequestOptions requestOptions)
        {
            string proxyUri = null;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["kb.api.proxy"]))
            {
                proxyUri = ConfigurationManager.AppSettings["kb.api.proxy"];
            }

            return new RestClient(baseUri)
            {
                Authenticator = new HttpBasicAuthenticator(requestOptions.User, requestOptions.Password),
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