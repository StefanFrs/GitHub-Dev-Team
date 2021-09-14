using GitHubSearchWebApp.Data;
using GitHubSearchWebApp.Models;
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
        private readonly GitHubApi gitHubApi;

        public DbExperiencesRepository(ApplicationDbContext context, GitHubApi gitHubApi)
        {
            this.context = context;
            this.gitHubApi = gitHubApi;
        }
        public IEnumerable<Project> Get(string githubLoginDeveloper, string programmingLanguage)
        {
            var developer =  context.Developer.Include(d => d.Experiences).FirstOrDefault(d => d.GitLogin == githubLoginDeveloper);
            ProgrammingLanguages language = (ProgrammingLanguages)Enum.Parse(typeof(ProgrammingLanguages), programmingLanguage);

            var experienceInProgrammingLanguage =  context.Experience.Include(e => e.Projects).FirstOrDefault(e => e.ProgrammingLanguage == language && e.DeveloperId == developer.Id);

            return experienceInProgrammingLanguage.Projects;
        }

        public void Post(Experience experience)
        {
            SetExperienceCodeSizeAndProjects(experience);
            context.Add(experience);
            context.SaveChangesAsync();
           
        }
        private void SetExperienceCodeSizeAndProjects(Experience experience)
        {
            Developer developer = context.Developer.FirstOrDefault(m => m.Id == experience.DeveloperId);
            IEnumerable<Project> projects = gitHubApi.GetProjectsByDeveloperByLanguage(developer.GitLogin, experience.ProgrammingLanguage.ToString());
            long codeSize = gitHubApi.GetCodeSizeByDeveloperByLanguage(developer.GitLogin, experience.ProgrammingLanguage.ToString());
            experience.CodeSize = codeSize.ToString();
            experience.Projects = projects.ToList();
        }

        public void Put(string githubLoginDeveloper, string programmingLanguage, string description)
        {
            Experience experienceToUpdate =  GetExperienceToUpdate(githubLoginDeveloper, programmingLanguage, description);
            context.Update(experienceToUpdate);
            context.SaveChangesAsync();
           
        }
        private Experience GetExperienceToUpdate(string githubLoginDeveloper, string programmingLanguage, string description)
        {
            var developer =  context.Developer.Include(d => d.Experiences).FirstOrDefault(d => d.GitLogin == githubLoginDeveloper);
            ProgrammingLanguages language = (ProgrammingLanguages)Enum.Parse(typeof(ProgrammingLanguages), programmingLanguage);
            var experienceToUpdate =  context.Experience.Include(e => e.Projects).FirstOrDefault(e => e.ProgrammingLanguage == language && e.DeveloperId == developer.Id);
            experienceToUpdate.Description += "\n" + description;
            return experienceToUpdate;
        }
    }
}
