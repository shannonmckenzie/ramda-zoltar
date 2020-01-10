using System.Collections.Generic;
using System.Threading.Tasks;
using RamdaZoltar.Api.Data;
using RamdaZoltar.Api.Models.GitHub;

namespace RamdaZoltar.Api.Services
{
    public interface IOrganizationPullRequestService
    {
        Task<List<PullRequest>> GetAll(string state);
    }
    
    public class OrganizationPullRequestService : IOrganizationPullRequestService
    {
        private readonly IPullRequestRepository _pullRequestRepository;
        private readonly IRepoRepository _repoRepository;
        
        public OrganizationPullRequestService(IRepoRepository repoRepository, IPullRequestRepository pullRequestRepository)
        {
            _repoRepository = repoRepository;
            _pullRequestRepository = pullRequestRepository;
        }
        
        public async Task<List<PullRequest>> GetAll(string state)
        {
            var repositories = await _repoRepository.GetAll();

            var allPullRequests = new List<PullRequest>();

            foreach (var repository in repositories)
            {
                var pullRequests = await _pullRequestRepository.Get(repository, state);

                if (pullRequests != null)
                {
                    allPullRequests.AddRange(pullRequests);
                }
            }

            return allPullRequests;
        }
    }
}