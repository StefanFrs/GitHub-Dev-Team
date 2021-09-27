using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GitHubSearchWebApp.Data;
using DevsWebApp.Models;
using GitHubSearchWebApp.Models;
using GitHubSearchWebApp.Services;
using GitHubSearchWebApp.Repo;
using GitHubSearchWebApp.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace GitHubSearchWebApp.Controllers
{
    /// <summary>
    ///   <br />
    /// </summary>
    [Authorize(Roles = "User")]
    public class DevelopersController : Controller
    {
        private readonly IExperiencesRepository experiencesRepository;
        private readonly IDevelopersRepository developersRepository;
        private readonly IGitHubApiService gitHubApiService;
        ExperiencesController experiencesController;


        /// <summary>Initializes a new instance of the <see cref="DevelopersController" /> class.</summary>
        /// <param name="context">The context.</param>
        public DevelopersController(IGitHubApiService gitHubApiService, IExperiencesRepository experiencesRepository, IDevelopersRepository developersRepository)
        {
            this.experiencesRepository = experiencesRepository;
            this.developersRepository = developersRepository;
            this.gitHubApiService = gitHubApiService;
            experiencesController = new ExperiencesController(gitHubApiService, experiencesRepository, developersRepository);
        }

        /// <summary>Gets all instances of developers.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet("allDevelopers")]
        public async Task<IActionResult> Get()
        {
            return Ok(developersRepository.GetAll());
        }


        /// <summary>Gets the developer by githubLogin.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>Http Response.<br /></returns>
        [HttpGet("developer/{githubLoginDeveloper}")]
        public async Task<IActionResult> Get(string githubLoginDeveloper)
        {
            var developer = developersRepository.GetByGithubLogin(githubLoginDeveloper);
            return Ok(developer);
        }


        /// <summary>Gets the repo count for specified developer identifier.</summary>
        /// <param name="developerId">The developer identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("developer/repoCount/{developerId}")]
        public async Task<IActionResult> Get(int developerId)
        {
            int numberOfRepos = developersRepository.GetRepoCountByDeveloper(developerId);
            return Ok(numberOfRepos);
        }

     

        [HttpGet("developer/codeSize/{developerId}/{language}")]
        public async Task<IActionResult> Get(int developerId, string language)
        {
            return base.Ok(developersRepository.GetCodeSizeByDeveloperIdAndLanguage(developerId, language));
        }

        // GET: Developers
        /// <summary>Indexes this instance.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<IActionResult> Index()
        {
            return View(developersRepository.GetAll());
        }

        // GET: Developers/Details/5
        /// <summary>Detailses the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [Authorize(Roles = "TeamLead")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = developersRepository.GetByDeveloperId(id);
            if (developer == null)
            {
                return NotFound();
            }

            return View(developer);
        }

        // GET: Developers/Create
        /// <summary>Creates this instance.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Developers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>Creates the specified developer.</summary>
        /// <param name="developer">The developer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [Authorize(Roles = "TeamLead")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,GitLogin,Email")] Developer developer)
        {
            if (ModelState.IsValid)
            {
                developersRepository.Add(developer);
                AddDeveloperExperience(developer);

                return RedirectToAction(nameof(Index));
            }
            return View(developer);
        }

        [NonAction]
        public async Task AddDeveloperExperience(Developer developer)
        {
            ISet<ProgrammingLanguages> programmingLanguagesDeveloper = experiencesController.GetProgrammingLanguagesAsSet(developer.GitLogin);
            List<Experience> experiences = new List<Experience>();
            foreach (var programmingLanguage in programmingLanguagesDeveloper)
            {
                Experience experience = new Experience();
                experience.Description = "";
                experience.ProgrammingLanguage = programmingLanguage;
                experience.DeveloperId = developer.Id;
                experiences.Add(experience);
                experiencesController.PostAsync(experience);
            }
            developer.Experiences = experiences;
            developer.AvatarURL = gitHubApiService.GetDeveloperAvatarURL(developer.GitLogin);
            developersRepository.Update(developer);
        }

        // GET: Developers/Edit/5
        /// <summary>Edits the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [Authorize(Roles = "TeamLead")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = developersRepository.GetByDeveloperId(id);
            if (developer == null)
            {
                return NotFound();
            }
            return View(developer);
        }

        // POST: Developers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>Edits the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="developer">The developer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [Authorize(Roles = "TeamLead")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,GitLogin,Email")] Developer developer)
        {
            if (id != developer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    developersRepository.Update(developer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!developersRepository.DeveloperExists(developer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(developer);
        }

        // GET: Developers/Delete/5
        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [Authorize(Roles = "TeamLead")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = developersRepository.GetByDeveloperId(id);
            if (developer == null)
            {
                return NotFound();
            }

            return View(developer);
        }

        // POST: Developers/Delete/5
        /// <summary>Deletes the confirmed.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [Authorize(Roles = "TeamLead")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            developersRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
