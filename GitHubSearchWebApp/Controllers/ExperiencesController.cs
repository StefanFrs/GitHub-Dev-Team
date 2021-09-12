﻿using GitHubSearchWebApp.Data;
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
    [Route("api/[controller]")]
    [ApiController]
    public class ExperiencesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IExperiencesService experiencesService;
        private DevelopersController developersController;

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
        [HttpGet("{githubLoginDeveloper}/{programmingLanguage}/")]
        public IEnumerable<Project> Get(string githubLoginDeveloper, string programmingLanguage)
        {
            return experiencesService.GetProjectsByDeveloperByLanguage(githubLoginDeveloper, programmingLanguage);
        }

        // GET api/<ExperiencesController>/programmingLanguages/user
        /// <summary>Gets the programming languages of specified github login developer.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("programmingLanguages/{githubLoginDeveloper}")]
        public ISet<ProgrammingLanguages> Get(string githubLoginDeveloper)
        {
            return experiencesService.GetProgrammingLanguagesByDeveloper(githubLoginDeveloper);
        }

        // POST api/<ExperiencesController>
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
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Experience value)
        {
        }

        // DELETE api/<ExperiencesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
