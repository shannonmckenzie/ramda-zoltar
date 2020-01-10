using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RamdaZoltar.Api.Data
{
    public abstract class GitHubRepository
    {
        private HttpClient _client;

        protected HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    InitializeHttpClient();
                }

                return _client;
            }
        }
        
        protected static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        protected const string GitHubApiBase = "https://api.github.com";
        protected const string OrganizationName = "ramda";
        private readonly string _authToken;

        public GitHubRepository(IConfiguration configuration)
        {
            _authToken = configuration.GetValue<string>("Token");
        }

        protected Uri GetNextUri(HttpResponseHeaders responseHeaders)
        {
            var hasLinkHeader = responseHeaders.TryGetValues("Link", out var linkHeaderValues);
                
            if (!hasLinkHeader)
            {
                return null;
            }

            foreach (var linkHeaderValue in linkHeaderValues)
            {
                var links = linkHeaderValue.Split(',');
                foreach (var link in links)
                {
                    if (link.Contains("rel=\"next\""))
                    {
                        var firstBracketIndex = link.IndexOf('<');
                        var lastBracketIndex = link.LastIndexOf('>');
                        var nextLink = link.Substring(firstBracketIndex + 1, lastBracketIndex - firstBracketIndex - 1);
                    
                        return new Uri(nextLink);
                    }
                }
            }

            return null;
        }

        private void InitializeHttpClient()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Host = "api.github.com";
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("banana", "1.0"));
            _client.DefaultRequestHeaders.Add("Authorization", $"token {_authToken}");
        }
    }
}