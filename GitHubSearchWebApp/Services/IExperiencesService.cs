using GitHubSearchWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Services
{
    interface IExperiencesService
    {
        IEnumerable<Project> GetProjectsByLanguage(string githubLoginDeveloper, string programmingLanguage);

        ISet<ProgrammingLanguages> GetProgrammingLanguages(string githubLoginDeveloper);
    }
}
