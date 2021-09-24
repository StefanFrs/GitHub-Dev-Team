using GitHubSearchWebApp.Data;
using GitHubSearchWebApp.Models;
using GitHubSearchWebApp.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Repo 
{
    
    public class DbExperiencesRepository : IExperiencesRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IGitHubApiService gitHubApi;

        public DbExperiencesRepository(ApplicationDbContext context, IGitHubApiService gitHubApi)
        {
            this.context = context;
            this.gitHubApi = gitHubApi;
        }
        public IEnumerable<Project> GetProjectsByDeveloperAndLanguage(string githubLoginDeveloper, string programmingLanguage)
        {
            var developer =  context.Developer.Include(d => d.Experiences).FirstOrDefault(d => d.GitLogin == githubLoginDeveloper);
            ProgrammingLanguages language = (ProgrammingLanguages)Enum.Parse(typeof(ProgrammingLanguages), programmingLanguage);

            var experienceInProgrammingLanguage =  context.Experience.Include(e => e.Projects).FirstOrDefault(e => e.ProgrammingLanguage == language && e.DeveloperId == developer.Id);

            return experienceInProgrammingLanguage.Projects;
        }

        public void Add(Experience experience)
        {
            context.Add(experience);
            context.SaveChanges();
        }
        

        public void Update(string githubLoginDeveloper, string programmingLanguage, string description)
        {
            Experience experienceToUpdate =  GetExperienceToUpdate(githubLoginDeveloper, programmingLanguage, description);
            context.Update(experienceToUpdate);
            context.SaveChanges();
           
        }
        private Experience GetExperienceToUpdate(string githubLoginDeveloper, string programmingLanguage, string description)
        {
            var developer =  context.Developer.Include(d => d.Experiences).FirstOrDefault(d => d.GitLogin == githubLoginDeveloper);
            ProgrammingLanguages language = (ProgrammingLanguages)Enum.Parse(typeof(ProgrammingLanguages), programmingLanguage);
            var experienceToUpdate =  context.Experience.Include(e => e.Projects).FirstOrDefault(e => e.ProgrammingLanguage == language && e.DeveloperId == developer.Id);
            experienceToUpdate.Description += "\n" + description;
            return experienceToUpdate;
        }

        public Dictionary<string, string> GetStatisticsByLanguage(string programmingLanguage)
        {
            Dictionary<string, string> developersStatistics = new Dictionary<string, string>();
            ProgrammingLanguages language = (ProgrammingLanguages)Enum.Parse(typeof(ProgrammingLanguages), programmingLanguage);

            var experiencesDvelopers = context.Developer
                            .Join(
                                context.Experience.Include(e => e.Projects),
                                developer => developer.Id,
                                experience => experience.DeveloperId,
                                (developer, experience) => new
                                {
                                    ProgrammingLanguage = experience.ProgrammingLanguage,
                                    FullName = developer.FullName,
                                    CodeSize = experience.CodeSize,
                                    ProjectsCount = experience.Projects.Count,
                                }
                            )
                            .ToList();

            foreach (var ed in experiencesDvelopers)
            {
                if (ed.ProgrammingLanguage == language)
                {
                    developersStatistics.Add(ed.FullName, ed.CodeSize + " " + ed.ProjectsCount);
                }
            }

            return developersStatistics;
        }

        public Dictionary<string, long> GetCodeSizes()
        {
            Dictionary<string, long> codeSizesByLanguage = new Dictionary<string, long>();
            var developers = context.Developer.Include(d => d.Experiences);
            foreach (var developer in developers)
            {
                foreach (var experience in developer.Experiences)
                {
                    context.Experience.Include(e => e.Projects);
                    string language = experience.ProgrammingLanguage.ToString();
                    if (codeSizesByLanguage.ContainsKey(language))
                    {
                        codeSizesByLanguage[language] += Convert.ToInt64(experience.CodeSize);
                    }
                    else
                    {
                        codeSizesByLanguage.Add(language, Convert.ToInt64(experience.CodeSize));
                    }
                }
            }
            return codeSizesByLanguage;
        }
    }
}
