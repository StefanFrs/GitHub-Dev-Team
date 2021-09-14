using GitHubSearchWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Repo
{
    interface IExperiencesRepository
    {
        IEnumerable<Project> Get(string githubLoginDeveloper, string programmingLanguage);

        void Put(string githubLoginDeveloper, string programmingLanguage, string description);

        void Post(Experience experience);


    }
}
