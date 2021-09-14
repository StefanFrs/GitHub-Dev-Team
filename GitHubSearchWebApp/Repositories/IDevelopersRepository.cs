using GitHubSearchWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Repositories
{
    public interface IDevelopersRepository
    {
        public IEnumerable<Developer> GetAll();
        public Developer GetById(int id);

        public Developer GetByDeveloperId(int? id);

        public Developer GetByGithubLogin(string githubLoginDeveloper);

        public int GetRepoCountByDeveloper(int developerId);

        public long GetCodeSizeByDeveloperIdAndLanguage(int developerId, string language);

        public bool DeveloperExists(int id);

        public void Add(Developer developer);

        public void Update(Developer developer);

        public void Delete(int id);
    }
}
