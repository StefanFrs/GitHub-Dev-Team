using GitHubSearchWebApp.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Services
{
    public class ExperiencesService : IExperiencesService
    {
        private String serverContent;

        /// <summary>Initializes a new instance of the <see cref="ExperiencesService" /> class.</summary>
        public ExperiencesService()
        {
            serverContent = "";
        }

        /// <summary>Gets the experience by language.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public IEnumerable<Project> GetProjectsByLanguage(string githubLoginDeveloper, string programmingLanguage)
        {
            GetReposByUser(githubLoginDeveloper);
            return ConvertServerContentToProjectsByLanguage(programmingLanguage);
        }

        /// <summary>Gets the programming languages.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public ISet<ProgrammingLanguages> GetProgrammingLanguages(string githubLoginDeveloper)
        {
            GetReposByUser(githubLoginDeveloper);

            List<ProgrammingLanguages> programmingLanguages = GetNonNullLanguagesList(serverContent).ToList();

            return new HashSet<ProgrammingLanguages>(programmingLanguages);
        }

        private IEnumerable<ProgrammingLanguages> GetNonNullLanguagesList(string content)
        {
            return ConvertServerContentToProgrammingLanguages().ToList().FindAll(l => l != null).Select(l => GetProgrammingLaguageFromString(l));
        }

        private static ProgrammingLanguages GetProgrammingLaguageFromString(string l)
        {
            return (ProgrammingLanguages)Enum.Parse(typeof(ProgrammingLanguages), l.Replace(" ", "").Replace("+", "Plus").Replace("#", "Sharp").Replace("-", ""));
        }

        private void GetReposByUser(string githubLoginDeveloper)
        {
            var client = new RestClient();
            client.Timeout = -1;
            var request =   FormRequest(githubLoginDeveloper);
            IRestResponse response = client.Execute(request);

            serverContent = response.Content;
        }

        private static IRestRequest FormRequest(string githubLoginDeveloper)
        {
            return new RestRequest("https://api.github.com/users/{user}/repos", Method.GET)
                                            .AddUrlSegment("user", githubLoginDeveloper)
                                            .AddParameter("type", "all");
                                            
        }

        /// <summary>Converts the server content to programming languages.</summary>
        /// <param name="contentfromServer">The contentfrom server.</param>
        /// <returns>programming languages enumerable<br /></returns>
        public IEnumerable<string> ConvertServerContentToProgrammingLanguages()
        {
            var developerRepositories = JArray.Parse(serverContent);

            int numberOfRepositories = developerRepositories.Count;
            if (numberOfRepositories == 0)
            {
                return new List<string>();
            }

            
            return Enumerable.Range(1, numberOfRepositories).Select(index =>
            {
                return developerRepositories[index - 1].Value<string>("language");
            })
            .ToArray();
        }

        /// <summary>Converts the server content to projects by language.</summary>
        /// <param name="language">The language.</param>
        /// <returns>
        ///   <para>enumerable of projects<br /></para>
        /// </returns>
        public IEnumerable<Project> ConvertServerContentToProjectsByLanguage(string language)
        {
            var developerRepositories = JArray.Parse(serverContent);
            int numberOfRepositories = developerRepositories.Count;
            if (numberOfRepositories == 0)
            {
                return new List<Project>();
            }
            return Enumerable.Range(1, numberOfRepositories).Select(index =>
            {
                return new Project {
                    Id = index,
                    Name= developerRepositories[index - 1].Value<string>("full_name"),
                    URL= developerRepositories[index-1].Value<string>("html_url"),
                    CodeSize = developerRepositories[index - 1].Value<int>("size"),
                };
            })
            .ToArray();

        }

    }
}
