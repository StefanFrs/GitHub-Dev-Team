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

namespace GitHubSearchWebApp.Controllers
{
    /// <summary>
    ///   <br />
    /// </summary>
    public class DevelopersController : Controller
    {
        private readonly ApplicationDbContext _context;
        ExperiencesController experiencesController;
        private IDevelopersService developersService;


        /// <summary>Initializes a new instance of the <see cref="DevelopersController" /> class.</summary>
        /// <param name="context">The context.</param>
        public DevelopersController(ApplicationDbContext context)
        {
            _context = context;
            developersService = new DevelopersService();
            experiencesController = new ExperiencesController(_context);
        }

        /// <summary>Gets all instances of developers.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet("allDevelopers")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Developer.ToListAsync());
        }


        /// <summary>Gets the developer by githubLogin.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>Http Response.<br /></returns>
        [HttpGet("developer/{githubLoginDeveloper}")]
        public async Task<IActionResult> Get(string githubLoginDeveloper)
        {
            var developer = await _context.Developer.Include(d => d.Experiences).FirstOrDefaultAsync(d => d.GitLogin == githubLoginDeveloper);
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
            var developer = await GetDeveloper(developerId);
            int numberOfRepos = await GetNumberOfRepos(developer);
            return Ok(numberOfRepos);
        }

        private async Task<int> GetNumberOfRepos(Developer developer)
        {
            int numberOfRepos = 0;
            foreach (var experience in developer.Experiences)
            {
                var experienceWithProjects = await _context.Experience.Include(e => e.Projects).FirstOrDefaultAsync(e => e.Id == experience.Id);
                numberOfRepos += experienceWithProjects.Projects.Count;
            }

            return numberOfRepos;
        }

        [HttpGet("developer/codeSize/{developerId}/{language}")]
        public async Task<IActionResult> Get(int developerId, string language)
        {
            var developer = await GetDeveloper(developerId);
            ProgrammingLanguages programmingLanguage = (ProgrammingLanguages)Enum.Parse(typeof(ProgrammingLanguages), language);
            return base.Ok(GetCodeSizeByLanguage(developer, programmingLanguage));
        }

        private static long GetCodeSizeByLanguage(Developer developer, ProgrammingLanguages programmingLanguage)
        {
            return developer.Experiences.ToList().FindAll(e => e.ProgrammingLanguage == programmingLanguage).Sum(e => Convert.ToInt64(e.CodeSize));
        }

        private async Task<Developer> GetDeveloper(int developerId)
        {
            return await _context.Developer.Include(d => d.Experiences).FirstOrDefaultAsync(d => d.Id == developerId);
        }

        // GET: Developers
        /// <summary>Indexes this instance.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Developer.ToListAsync());
        }

        // GET: Developers/Details/5
        /// <summary>Detailses the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developer
                .FirstOrDefaultAsync(m => m.Id == id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,GitLogin,Email")] Developer developer)
        {
            if (ModelState.IsValid)
            {
                Developer developerJustAdded = _context.Add(developer).Entity;
                await _context.SaveChangesAsync();
                await AddDeveloperExperience(developer);

                return RedirectToAction(nameof(Index));
            }
            return View(developer);
        }

        private async Task AddDeveloperExperience(Developer developer)
        {
            ISet<ProgrammingLanguages> programmingLanguagesDeveloper = experiencesController.GetGetProgrammingLanguagesAsSet(developer.GitLogin);
            List<Experience> experiences = new List<Experience>();
            foreach (var programmingLanguage in programmingLanguagesDeveloper)
            {
                Experience experience = new Experience();
                experience.Description = "";
                experience.ProgrammingLanguage = programmingLanguage;
                experience.DeveloperId = developer.Id;
                experiences.Add(experience);
                await experiencesController.PostAsync(experience);
            }
            developer.Experiences = experiences;
            developer.AvatarURL = developersService.GetDeveloperAvatarURL(developer.GitLogin);
            _context.Update(developer);
            await _context.SaveChangesAsync();
        }

        // GET: Developers/Edit/5
        /// <summary>Edits the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developer.FindAsync(id);
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
                    _context.Update(developer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeveloperExists(developer.Id))
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var developer = await _context.Developer
                .FirstOrDefaultAsync(m => m.Id == id);
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var developer = await _context.Developer.FindAsync(id);
            _context.Developer.Remove(developer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeveloperExists(int id)
        {
            return _context.Developer.Any(e => e.Id == id);
        }
    }
}
