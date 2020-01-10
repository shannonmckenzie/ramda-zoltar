using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RamdaZoltar.Api.Models.GitHub;

namespace RamdaZoltar.Api.Data
{
    public interface IPullRequestRepository
    {
        Task<List<PullRequest>> Get(Repo repo, string state);
    }
    
    public class PullRequestRepository : GitHubRepository, IPullRequestRepository
    {
        private static readonly Dictionary<string, List<PullRequest>> _pullRequestsByRepo = new Dictionary<string, List<PullRequest>>();

        public PullRequestRepository(IConfiguration configuration) : base(configuration)
        {
            
        }

        public async Task<List<PullRequest>> Get(Repo repo, string state)
        {
            var hasFetchedPullRequests = _pullRequestsByRepo.TryGetValue(repo.Name, out var repoPullRequests);

            if (!hasFetchedPullRequests)
            {
                repoPullRequests = new List<PullRequest>();

                var requestUri = new Uri($"{repo.PullsUrl}?state=all");
            
                while (requestUri != null)
                {
                    var response = await Client.GetAsync(requestUri);
                    var content = await response.Content.ReadAsStringAsync();
                    var pullRequests = JsonConvert.DeserializeObject<List<PullRequest>>(content, SerializerSettings);
            
                    repoPullRequests.AddRange(pullRequests);

                    requestUri = GetNextUri(response.Headers);
                }
                
                _pullRequestsByRepo.Add(repo.Name, repoPullRequests);
            }

            if (state != "all")
            {
                return repoPullRequests.Where(pr => pr.State == state).ToList();
            }

            return repoPullRequests;
        }
    }
}