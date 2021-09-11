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
        private JArray developerRepositories;

        /// <summary>Initializes a new instance of the <see cref="ExperiencesService" /> class.</summary>
        public ExperiencesService()
        {
            this.developerRepositories = new JArray();
        }

        public IEnumerable<Experience> Get(string githubLoginDeveloper)
        {
            throw new NotImplementedException();
        }

        /// <summary>Gets the programming languages.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public ISet<ProgrammingLanguages> GetProgrammingLanguages(string githubLoginDeveloper)
        {
            string content = GetReposByUser(githubLoginDeveloper);
            List<ProgrammingLanguages> programmingLanguages = GetNonNullLanguagesList(content).ToList();

            return new HashSet<ProgrammingLanguages>(programmingLanguages);
        }

        private IEnumerable<ProgrammingLanguages> GetNonNullLanguagesList(string content)
        {
            return ConvertServerContentToProgrammingLanguages(content).ToList().FindAll(l => l != null).Select(l => GetProgrammingLaguageFromString(l));
        }

        private static ProgrammingLanguages GetProgrammingLaguageFromString(string l)
        {
            return (ProgrammingLanguages)Enum.Parse(typeof(ProgrammingLanguages), l.Replace(" ", "").Replace("+", "Plus").Replace("#", "Sharp").Replace("-", ""));
        }

        private string GetReposByUser(string githubLoginDeveloper)
        {
            var client = new RestClient();
            client.Timeout = -1;
            var request =   FormRequest(githubLoginDeveloper);
            IRestResponse response = client.Execute(request);

            return response.Content;
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
        public IEnumerable<string> ConvertServerContentToProgrammingLanguages(string contentfromServer)
        {
            developerRepositories = JArray.Parse(contentfromServer);

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

    }
}
