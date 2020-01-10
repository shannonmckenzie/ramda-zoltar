using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RamdaZoltar.Api.Services;

namespace RamdaZoltar.Api.Controllers
{
    [ApiController]
    [Route("pullrequests")]
    public class PullRequestsController : ControllerBase
    {
        private readonly IOrganizationPullRequestService _organizationPullRequestService;
        
        public PullRequestsController(IOrganizationPullRequestService organizationPullRequestService)
        {
            _organizationPullRequestService = organizationPullRequestService;
        }
        
        [HttpGet]
        public async Task<ActionResult> Get(string state = "open")
        {
            var pullRequests = await _organizationPullRequestService.GetAll(state);
            
            return Ok(pullRequests);
        }
    }
}