using GitHubSearchWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GitHubSearchWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperiencesController : ControllerBase
    {
        private IExperiencesService experiencesService;

        public ExperiencesController()
        {
            experiencesService = new ExperiencesService();
        }

        // GET: api/<ExperiencesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ExperiencesController>/5
        [HttpGet("{githubLoginDeveloper}")]
        public ISet<string> Get(string githubLoginDeveloper)
        {
            return experiencesService.GetProgrammingLanguages(githubLoginDeveloper);
        }

        // POST api/<ExperiencesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ExperiencesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ExperiencesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
