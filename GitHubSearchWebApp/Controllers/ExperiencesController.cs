using GitHubSearchWebApp.Data;
using GitHubSearchWebApp.Models;
using GitHubSearchWebApp.Repo;
using GitHubSearchWebApp.Repositories;
using GitHubSearchWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GitHubSearchWebApp.Controllers
{
    /// <summary>ExperiencesController .</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExperiencesController : ControllerBase
    {
        private readonly IExperiencesRepository experiencesRepository;
        private readonly IDevelopersRepository developersRepository;
        private readonly IGitHubApiService gitHubApiService;

        /// <summary>Initializes a new instance of the <see cref="ExperiencesController" /> class.</summary>
        /// <param name="experiencesRepository">The experiences repo.</param>
        public ExperiencesController(IGitHubApiService gitHubApiServices , IExperiencesRepository experiencesRepository, IDevelopersRepository developersRepository)
        {
            this.experiencesRepository = experiencesRepository;
            this.developersRepository = developersRepository;
            this.gitHubApiService = gitHubApiServices;        }

        // GET: api/<ExperiencesController>/user/language
        /// <summary>Gets the projects of specified github login developer by language.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <param name="programmingLanguage">The programming language.</param>
        /// <returns>Enumerable of projects.<br /></returns>
        [HttpGet("{githubLoginDeveloper}/{programmingLanguage}")]
        public async Task<IActionResult> Get(string githubLoginDeveloper, string programmingLanguage)
        {
            var experienceInProgrammingLanguage = experiencesRepository.GetProjectsByDeveloperAndLanguage(githubLoginDeveloper, programmingLanguage);
            return Ok(experienceInProgrammingLanguage);
        }

        // GET api/<ExperiencesController>/programmingLanguages/user
        /// <summary>Gets the programming languages of specified github login developer.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("programmingLanguages/{githubLoginDeveloper}")]
        public IEnumerable<string> Get(string githubLoginDeveloper)
        {
            return gitHubApiService.GetProgrammingLanguagesByDeveloper(githubLoginDeveloper).Select(l => l.ToString());
        }

        /// <summary>Gets the get programming languages as set.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>
        ///   <para>ISet&lt;ProgrammingLanguages&gt;<br /></para>
        /// </returns>
        [NonAction]
        public ISet<ProgrammingLanguages> GetProgrammingLanguagesAsSet(string githubLoginDeveloper)
        {
            return gitHubApiService.GetProgrammingLanguagesByDeveloper(githubLoginDeveloper);
        }

        /// <summary>Gets the languages of the team.</summary>
        /// <returns>Http Response.</returns>
        [HttpGet("statistics/languages"), ActionName("GetLanguages")]
        public async Task<IActionResult> GetLanguages()
        {
            Dictionary<string, long> codeSizesByLanguage = experiencesRepository.GetCodeSizes();
            return Ok(codeSizesByLanguage);
        }


        /// <summary>Gets the statistics of the team.</summary>
        /// <param name="language">The language.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("statistics/{language}"), ActionName("GetStatistics")]
        public async Task<IActionResult> GetStatistics(string language)
        {
            try
            {
                Dictionary<string, string> developersStatistics = experiencesRepository.GetStatisticsByLanguage(language);
                return Ok(developersStatistics);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return BadRequest();
        }


        // POST api/<ExperiencesController>
        /// <summary>Addsexperience.</summary>
        /// <param name="experience">The experience.</param>
        /// <returns>Http Response.<br /></returns>
        [HttpPost]
        public IActionResult PostAsync([FromBody] Experience experience)
        {
            SetExperienceCodeSizeAndProjects(experience);
            experiencesRepository.Add(experience);
            return Ok();
        }

        private void SetExperienceCodeSizeAndProjects(Experience experience)
        {
            Developer developer = developersRepository.GetById(experience.DeveloperId);
            IEnumerable<Project> projects = gitHubApiService.GetProjectsByDeveloperByLanguage(developer.GitLogin, experience.ProgrammingLanguage.ToString());
            long codeSize = gitHubApiService.GetCodeSizeByDeveloperByLanguage(developer.GitLogin, experience.ProgrammingLanguage.ToString());
            experience.CodeSize = codeSize.ToString();
            experience.Projects = projects.ToList();
        }

        // PUT api/<ExperiencesController>/5
        /// <summary>Updates experience of specified developer on specified language.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <param name="programmingLanguage">The programming language.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPut("{githubLoginDeveloper}/{programmingLanguage}/{description}")]
        public IActionResult Put(string githubLoginDeveloper, string programmingLanguage, string description)
        {
            experiencesRepository.Update(githubLoginDeveloper, programmingLanguage, description);
            return Ok();
        }
    }
}