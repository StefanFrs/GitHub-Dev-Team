using GitHubSearchWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Services
{
    interface IExperiencesService
    {
        IEnumerable<Project> GetProjectsByDeveloperByLanguage(string githubLoginDeveloper, string programmingLanguage);

        ISet<ProgrammingLanguages> GetProgrammingLanguagesByDeveloper(string githubLoginDeveloper);

        long GetCodeSizeByDeveloperByLanguage(string githubLoginDeveloper, string programmingLanguage);
    }
}
