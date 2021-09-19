using GitHubSearchWebApp.Models;
using GitHubSearchWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebHooks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GitHubSearchWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubUpdatesController : ControllerBase
    {
        private readonly IHubContext<UpdatesHub> hubContext;

        public GithubUpdatesController(IHubContext<UpdatesHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        // GET: api/<GithubUpdatesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [GitHubWebHook]
        public IActionResult GitHub(string id, JObject data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        // POST api/<GithubUpdatesController>
        [GitHubWebHook, HttpPost]
        public void Post([FromBody] string value)
        {
            var gitPush = new GitPush
            {
                User = "Updated Repo User",
                Repository = "Updated Repo",
                Size = 256,
                PushedAt = DateTime.Now
            };
            hubContext.Clients.All.SendAsync("RepositoryUpdate", gitPush);
        }

    }
}
