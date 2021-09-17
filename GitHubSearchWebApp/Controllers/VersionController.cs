using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GitHubSearchWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public VersionController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(GetVersionString());
        }

        private string GetVersionString()
        {
            var version = Environment.GetEnvironmentVariable("HEROKU_RELEASE_VERSION");
            if (version != null)
            {
                return ConvertVersionString(version);
            }
            return Configuration["Version"];
        }

        private string ConvertVersionString(string version)
        {
            return version;
        }
    }
}

