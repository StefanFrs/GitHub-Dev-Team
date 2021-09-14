using GitHubSearchWebApp.Data;
using GitHubSearchWebApp.Models;
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
        private readonly ApplicationDbContext _context;
        private IExperiencesService experiencesService;
        private DevelopersController developersController;

        /// <summary>Initializes a new instance of the <see cref="ExperiencesController" /> class.</summary>
        /// <param name="context">The context.</param>
        public ExperiencesController(ApplicationDbContext context)
        {
            _context = context;
            experiencesService = new ExperiencesService();
        }

        // GET: api/<ExperiencesController>/user/language
        /// <summary>Gets the projects of specified github login developer by language.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <param name="programmingLanguage">The programming language.</param>
        /// <returns>Enumerable of projects.<br /></returns>
        [HttpGet("{githubLoginDeveloper}/{programmingLanguage}")]
        public async Task<IActionResult> Get(string githubLoginDeveloper, string programmingLanguage)
        {
            var developer = await _context.Developer.Include(d => d.Experiences).FirstOrDefaultAsync(d => d.GitLogin == githubLoginDeveloper);
            ProgrammingLanguages language = (ProgrammingLanguages)Enum.Parse(typeof(ProgrammingLanguages), programmingLanguage);

            var experienceInProgrammingLanguage = await _context.Experience.Include(e => e.Projects).FirstOrDefaultAsync(e => e.ProgrammingLanguage == language && e.DeveloperId == developer.Id);

            return Ok(experienceInProgrammingLanguage.Projects);
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
            return experiencesService.GetProgrammingLanguagesByDeveloper(githubLoginDeveloper).Select(l => l.ToString());
        }

        /// <summary>Gets the get programming languages as set.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>
        ///   <para>ISet&lt;ProgrammingLanguages&gt;<br /></para>
        /// </returns>
        [NonAction]
        public ISet<ProgrammingLanguages> GetGetProgrammingLanguagesAsSet(string githubLoginDeveloper)
        {
            return experiencesService.GetProgrammingLanguagesByDeveloper(githubLoginDeveloper);
        }

        /// <summary>Gets the languages of the team.</summary>
        /// <returns>Http Response.</returns>
        [HttpGet("statistics/languages"), ActionName("GetLanguages")]
        public async Task<IActionResult> GetLanguages()
        {
            Dictionary<string, long> codeSizesByLanguage = new Dictionary<string, long>();
            var developers = _context.Developer.Include(d => d.Experiences);
            foreach (var developer in developers)
            {
                foreach (var experience in developer.Experiences)
                {
                    await _context.Experience.Include(e => e.Projects).FirstOrDefaultAsync(e => e.Id == experience.Id);
                    string language = experience.ProgrammingLanguage.ToString();
                    if ( codeSizesByLanguage.ContainsKey(language))
                    {
                        codeSizesByLanguage[language] += Convert.ToInt64(experience.CodeSize);
                    } else
                    {
                        codeSizesByLanguage.Add(language, Convert.ToInt64(experience.CodeSize));
                    }
                }
            }
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
            Dictionary<string, string> developersStatistics = new Dictionary<string, string>();
            var developers = _context.Developer.Include(d => d.Experiences);
            ProgrammingLanguages programmingLanguage = (ProgrammingLanguages)Enum.Parse(typeof(ProgrammingLanguages), language);
            foreach (var developer in developers)
            {
                var experience = await _context.Experience.Include(e => e.Projects).FirstOrDefaultAsync(e => e.ProgrammingLanguage == programmingLanguage && e.DeveloperId == developer.Id);
                if ( experience != null)
                {
                    developersStatistics.Add(developer.FullName, experience.CodeSize + " " + experience.Projects.Count);
                }
            }
            return Ok(developersStatistics);
        }


        // POST api/<ExperiencesController>
        /// <summary>Addsexperience.</summary>
        /// <param name="experience">The experience.</param>
        /// <returns>Http Response.<br /></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Experience experience)
        {
            await SetExperienceCodeSizeAndProjects(experience);
            _context.Add(experience);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task SetExperienceCodeSizeAndProjects(Experience experience)
        {
            Developer developer = await _context.Developer.FirstOrDefaultAsync(m => m.Id == experience.DeveloperId);
            IEnumerable<Project> projects = experiencesService.GetProjectsByDeveloperByLanguage(developer.GitLogin, experience.ProgrammingLanguage.ToString());
            long codeSize = experiencesService.GetCodeSizeByDeveloperByLanguage(developer.GitLogin, experience.ProgrammingLanguage.ToString());
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
        public async Task<IActionResult> Put(string githubLoginDeveloper, string programmingLanguage, string description)
        {
            Experience experienceToUpdate = await GetExperienceToUpdate(githubLoginDeveloper, programmingLanguage, description);
            _context.Update(experienceToUpdate);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task<Experience> GetExperienceToUpdate(string githubLoginDeveloper, string programmingLanguage, string description)
        {
            var developer = await _context.Developer.Include(d => d.Experiences).FirstOrDefaultAsync(d => d.GitLogin == githubLoginDeveloper);
            ProgrammingLanguages language = (ProgrammingLanguages)Enum.Parse(typeof(ProgrammingLanguages), programmingLanguage);
            var experienceToUpdate = await _context.Experience.Include(e => e.Projects).FirstOrDefaultAsync(e => e.ProgrammingLanguage == language && e.DeveloperId == developer.Id);
            experienceToUpdate.Description += "\n" + description;
            return experienceToUpdate;
        }
    }
}