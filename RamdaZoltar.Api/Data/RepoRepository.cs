using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RamdaZoltar.Api.Models.GitHub;

namespace RamdaZoltar.Api.Data
{
    public interface IRepoRepository
    {
        Task<List<Repo>> GetAll();
    }
    
    public class RepoRepository : GitHubRepository, IRepoRepository
    {
        private static List<Repo> _repos;
        
        public RepoRepository(IConfiguration configuration) : base(configuration)
        {
            
        }
        
        public async Task<List<Repo>> GetAll()
        {
            if (_repos == null)
            {
                await PopulateRepositoriesForOrganization();
            }

            return _repos;
        }

        private async Task PopulateRepositoriesForOrganization()
        {
            _repos = new List<Repo>();
            
            var requestUri = new Uri($"{GitHubApiBase}/orgs/{OrganizationName}/repos");
            
            while (requestUri != null)
            {
                var response = await Client.GetAsync(requestUri);
                var content = await response.Content.ReadAsStringAsync();
                var repositories = JsonConvert.DeserializeObject<List<Repo>>(content, SerializerSettings);
            
                _repos.AddRange(repositories);

                requestUri = GetNextUri(response.Headers);
            }
        }
    }
}