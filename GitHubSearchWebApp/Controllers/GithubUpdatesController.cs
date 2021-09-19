using GitHubSearchWebApp.Models;
using GitHubSearchWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebHooks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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

        /// <summary>
        /// Returns if the service is on.
        /// </summary>
        /// <returns>Status of the service.</returns>
        [HttpGet]
        public IActionResult Get(string id, string e, JObject data)
        {
            var healthCheck = new HealthCheckResult(HealthStatus.Healthy);

            return Ok(healthCheck);
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
        [HttpPost]
        public void Post([FromBody] System.Text.Json.JsonElement data)
        {
            var gitPush = new GitPush
            {
                User = data.GetProperty("repository").GetProperty("owner").GetProperty("login").GetString(),
                Repository = data.GetProperty("repository").GetProperty("name").GetString(),
                Size = data.GetProperty("size").GetInt64(),
                PushedAt = data.GetProperty("size").GetDateTime().ToString("dd MMM yyyy HH:mm")
            };
            hubContext.Clients.All.SendAsync("RepositoryUpdate", gitPush);
        }

    }
}
