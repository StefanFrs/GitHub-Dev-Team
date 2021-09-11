using GitHubSearchWebApp.Models;
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
        [HttpGet("{githubLoginDeveloper}/{programmingLanguage}/")]
        public IEnumerable<Project> Get(string githubLoginDeveloper, string programmingLanguage)
        {
            return experiencesService.GetProjectsByLanguage(githubLoginDeveloper, programmingLanguage);
        }

        // GET api/<ExperiencesController>/5
        /// <summary>Gets the programming languages of specified github login developer.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("programmingLanguages/{githubLoginDeveloper}")]
        public IEnumerable<string> Get(string githubLoginDeveloper)
        {
            ISet<ProgrammingLanguages> programmingLanguages = experiencesService.GetProgrammingLanguages(githubLoginDeveloper);
            return programmingLanguages.ToList().Select( l => l.ToString());
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
