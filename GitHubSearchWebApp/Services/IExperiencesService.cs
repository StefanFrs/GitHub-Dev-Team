using GitHubSearchWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Services
{
    interface IExperiencesService
    {
        IEnumerable<Experience> Get(string githubLoginDeveloper);

        ISet<string> GetProgrammingLanguages(string githubLoginDeveloper);
    }
}
