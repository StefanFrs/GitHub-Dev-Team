using GitHubSearchWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Services
{
    public interface IGitHubApiService
    {
        public IEnumerable<Project> GetProjectsByDeveloperByLanguage(string githubLoginDeveloper, string programmingLanguage);

        public ISet<ProgrammingLanguages> GetProgrammingLanguagesByDeveloper(string githubLoginDeveloper);

        public IEnumerable<string> ConvertServerContentToProgrammingLanguages();

        public IEnumerable<Project> ConvertServerContentToProjectsByLanguage(string language);

        public long GetCodeSizeByDeveloperByLanguage(string githubLoginDeveloper, string programmingLanguage);

        string GetDeveloperAvatarURL(string githubLoginDeveloper);
    }
}
