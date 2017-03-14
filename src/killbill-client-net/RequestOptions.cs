using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using KillBill.Client.Net.Infrastructure;

namespace KillBill.Client.Net
{
    public class RequestOptions
    {
       public string RequestId { get; }
        public string User { get; }
        public string Password { get; }
        public string CreatedBy { get; }
        public string Reason { get; }
        public string Comment { get; }
        public string TenantApiKey { get; }
        public string TenantApiSecret { get; }
        public ImmutableDictionary<string, string> Headers { get; }
        public MultiMap<string> QueryParams { get; }
        public bool? FollowLocation { get; }
        public MultiMap<string> QueryParamsForFollow { get; }


        public RequestOptions(string requestId, string user, string password, string comment, 
                              string reason, string createdBy, string tenantApiKey, string tenantApiSecret, 
                              ImmutableDictionary<string, string> headers, MultiMap<string> queryParams, bool? followLocation, 
                              MultiMap<string> queryParamsForFollow)
        {
            RequestId = requestId;
            User = user;
            Password = password;
            Comment = comment;
            Reason = reason;
            CreatedBy = createdBy;
            TenantApiKey = tenantApiKey;
            TenantApiSecret = tenantApiSecret;
            FollowLocation = followLocation;
            Headers = headers ?? ImmutableDictionary<string, string>.Empty;
            QueryParams = queryParams ?? new MultiMap<string>();            
            QueryParamsForFollow = queryParamsForFollow ?? new MultiMap<string>();
        }

        public RequestOptionsBuilder Extend()
        {
            var builder = new RequestOptionsBuilder();

            foreach (var hdr in this.Headers) {
                builder.WithHeader(hdr.Key, hdr.Value);
            }

            return builder
                .WithRequestId(this.RequestId)
                .WithUser(User).WithPassword(Password)
                .WithCreatedBy(CreatedBy).WithReason(Reason).WithComment(Comment)
                .WithTenantApiKey(TenantApiKey).WithTenantApiSecret(TenantApiSecret)
                .WithQueryParams(QueryParams)
                .WithFollowLocation(FollowLocation).WithQueryParamsForFollow(QueryParamsForFollow);
        }

        /**
         * Helper method for creating an empty RequestOptions object.
         * @return an empty RequestOptions object.
         */
        public static RequestOptions Empty()
        {
            return new RequestOptionsBuilder().Build();
        }

        /**
         * Helper method for creating a new builder
         * @return a new instance of RequestOptionsBuilder
         */
        public static RequestOptionsBuilder Builder()
        {
            return new RequestOptionsBuilder();
        }

        public bool ShouldFollowLocation() => FollowLocation ?? false;

    }

    public class RequestOptionsBuilder
    {
        private string requestId;
        private string user, password;
        private string createdBy, reason, comment;
        private string tenantApiKey, tenantApiSecret;
        private Dictionary<string, string> headers = new Dictionary<string, string>();
        private MultiMap<string> queryParams = new MultiMap<string>();
        private bool? followLocation;
        private MultiMap<string> queryParamsForFollow = new MultiMap<string>();

        public RequestOptionsBuilder WithRequestId(string requestId)
        {
            this.requestId = requestId;
            return this;
        }

        public RequestOptionsBuilder WithUser(string user)
        {
            this.user = user;
            return this;
        }

        public RequestOptionsBuilder WithPassword(string password )
        {
            this.password = password;
            return this;
        }

        public RequestOptionsBuilder WithCreatedBy(string createdBy)
        {
            this.createdBy = createdBy;
            return this;
        }

        public RequestOptionsBuilder WithReason(string reason)
        {
            this.reason = reason;
            return this;
        }

        public RequestOptionsBuilder WithComment(string comment)
        {
            this.comment = comment;
            return this;
        }

        public RequestOptionsBuilder WithTenantApiKey(string tenantApiKey)
        {
            this.tenantApiKey = tenantApiKey;
            return this;
        }
        public RequestOptionsBuilder WithTenantApiSecret(string tenantApiSecret)
        {
            this.tenantApiSecret = tenantApiSecret;
            return this;
        }

        public RequestOptionsBuilder WithHeader(string header, string value)
        {
            this.headers.Add(header, value);
            return this;
        }

        public RequestOptionsBuilder WithQueryParams(MultiMap<string> queryParams)
        {
            this.queryParams = queryParams;
            return this;
        }

        public RequestOptionsBuilder WithFollowLocation(bool? followLocation)
        {
            this.followLocation = followLocation;
            return this;
        }

        public RequestOptionsBuilder WithQueryParamsForFollow(MultiMap<string> queryParamsForFollow)
        {
            this.queryParamsForFollow = queryParamsForFollow;
            return this;
        }

        public RequestOptions Build()
        {
            return new RequestOptions(requestId, user, password, comment, reason, createdBy, tenantApiKey, tenantApiSecret, headers.ToImmutableDictionary(), queryParams, followLocation, queryParamsForFollow);
        }
    }

}