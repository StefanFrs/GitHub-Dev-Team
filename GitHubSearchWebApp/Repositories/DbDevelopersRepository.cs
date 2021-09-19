using GitHubSearchWebApp.Data;
using GitHubSearchWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Repositories
{
    public class DbDevelopersRepository : IDevelopersRepository
    {
        private readonly ApplicationDbContext context;

        public DbDevelopersRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public DbDevelopersRepository()
        {
        }

        public IEnumerable<Developer> GetAll()
        {
            return context.Developer.ToList();
        }

        public Developer GetByGithubLogin(string githubLoginDeveloper)
        {
            var developer = context.Developer.Include(d => d.Experiences).FirstOrDefault(d => d.GitLogin == githubLoginDeveloper);
            foreach (var experience in developer.Experiences)
            {
                context.Experience.Include(e => e.Projects).FirstOrDefault(e => e.Id == experience.Id);
            }
            return developer;
        }

        public Developer GetById(int id)
        {
            return context.Developer.FirstOrDefault(d => d.Id == id);
        }

        public int GetRepoCountByDeveloper(int developerId)
        {
            var developer = GetDeveloper(developerId);
            int numberOfRepos = GetNumberOfRepos(developer);
            return numberOfRepos;
        }

        private int GetNumberOfRepos(Developer developer)
        {
            int numberOfRepos = 0;
            foreach (var experience in developer.Experiences)
            {
                var experienceWithProjects =  context.Experience.Include(e => e.Projects).FirstOrDefault(e => e.Id == experience.Id);
                numberOfRepos += experienceWithProjects.Projects.Count;
            }

            return numberOfRepos;
        }

        private Developer GetDeveloper(int developerId)
        {
            return context.Developer.Include(d => d.Experiences).FirstOrDefault(d => d.Id == developerId);
        }

        public long GetCodeSizeByDeveloperIdAndLanguage(int developerId, string language)
        {
            var developer = GetDeveloper(developerId);
            ProgrammingLanguages programmingLanguage = (ProgrammingLanguages)Enum.Parse(typeof(ProgrammingLanguages), language);
            return GetCodeSizeByLanguage(developer, programmingLanguage);
        }

        private static long GetCodeSizeByLanguage(Developer developer, ProgrammingLanguages programmingLanguage)
        {
            return developer.Experiences.ToList().FindAll(e => e.ProgrammingLanguage == programmingLanguage).Sum(e => Convert.ToInt64(e.CodeSize));
        }

        public Developer GetByDeveloperId(int? id)
        {
            return context.Developer.Find(id);
        }

        public void Add(Developer developer)
        {
            Developer developerJustAdded = context.Add(developer).Entity;
            context.SaveChanges();
        }

        public void Update(Developer developer)
        {
            context.Update(developer);
             context.SaveChanges();
        }

        public void Delete(int id)
        {
            var developer = GetByDeveloperId(id);
            context.Developer.Remove(developer);
            context.SaveChanges();
        }

        public bool DeveloperExists(int id)
        {
            return context.Developer.Any(e => e.Id == id);
        }
    }
}
