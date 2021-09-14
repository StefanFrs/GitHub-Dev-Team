using GitHubSearchWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Repo
{
    public interface IExperiencesRepository
    {
        IEnumerable<Project> GetProjectsByDeveloperAndLanguage(string githubLoginDeveloper, string programmingLanguage);

        Dictionary<string, string> GetStatisticsByLanguage(string programmingLanguage);

        Dictionary<string, long> GetCodeSizes();

        void Update(string githubLoginDeveloper, string programmingLanguage, string description);

        void Add(Experience experience);

    }
}
