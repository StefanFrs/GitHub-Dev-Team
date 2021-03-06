using GitHubSearchWebApp.Models;
using GitHubSearchWebApp.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Repo
{
    public class GitHubApiService : IGitHubApiService
    {
        private readonly IConfiguration Configuration;
        private String serverContent;
        private string token;
        List<Project> projectsByLanguage;

        /// <summary>Initializes a new instance of the <see cref="GitHubApiService" /> class.</summary>
        public GitHubApiService(IConfiguration configuration)
        {
            Configuration = configuration;
            serverContent = "";
            SetTokenString();
            projectsByLanguage = new List<Project>();
        }

        public GitHubApiService(string tokenGit)
        {
            serverContent = "";
            SetTokenGitHub(tokenGit);
            projectsByLanguage = new List<Project>();
        }

        private void SetTokenString()
        {
            token = Environment.GetEnvironmentVariable("GITHUB_API_TOKEN");
            if (token == null)
            {
                token = Configuration["GitHubApiAccesToken"];
            }
        }

        private void SetTokenGitHub(string tokenGit)
        {
            token = tokenGit;
        }

        /// <summary>Gets the experience by language.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public IEnumerable<Project> GetProjectsByDeveloperByLanguage(string githubLoginDeveloper, string programmingLanguage)
        {
            GetReposByUser(githubLoginDeveloper);
            return ConvertServerContentToProjectsByLanguage(programmingLanguage);
        }

        /// <summary>Gets the programming languages.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public ISet<ProgrammingLanguages> GetProgrammingLanguagesByDeveloper(string githubLoginDeveloper)
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
            var request = FormRequest(githubLoginDeveloper);
            IRestResponse response = client.Execute(request);

            serverContent = response.Content;
        }

        private IRestRequest FormRequest(string githubLoginDeveloper)
        {
            return new RestRequest("https://api.github.com/users/{user}/repos", Method.GET)
                                            .AddHeader("Authorization", $"Bearer {token}")
                                            .AddUrlSegment("user", githubLoginDeveloper)
                                            .AddParameter("type", "all");

        }

        /// <summary>Converts the server content to programming languages.</summary>
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

            projectsByLanguage = GetProjects(language, developerRepositories, numberOfRepositories);

            return projectsByLanguage.ToArray();

        }

        private static List<Project> GetProjects(string language, JArray developerRepositories, int numberOfRepositories)
        {
            List<Project> projectsByLanguage = new List<Project>();
            for (int index = 1; index <= numberOfRepositories; index++)
            {
                var languageRepository = GetLanguage(index, developerRepositories);
                JToken currentRepository = developerRepositories[index - 1];
                if (languageRepository != null && languageRepository == language)
                {
                    projectsByLanguage.Add(new Project
                    {
                        Name = currentRepository.Value<string>("full_name"),
                        URL = currentRepository.Value<string>("html_url"),
                        CodeSize = currentRepository.Value<int>("size"),
                    });
                }
            }

            return projectsByLanguage;
        }

        private static string GetLanguage(int index, JArray developerRepositories)
        {
            string language = developerRepositories[index - 1].Value<string>("language");
            if (language != null)
            {
                language = language.Replace(" ", "").Replace("+", "Plus").Replace("#", "Sharp").Replace("-", "");
            }
            return language;
        }

        /// <summary>Gets the code size of experience by developer and by language.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <param name="programmingLanguage">The programming language.</param>
        /// <returns>The code size as long.<br /></returns>
        public long GetCodeSizeByDeveloperByLanguage(string githubLoginDeveloper, string programmingLanguage)
        {
            return projectsByLanguage.ToArray().Sum(p => p.CodeSize);
        }

        private void GetUser(string githubLoginDeveloper)
        {
            var client = new RestClient();
            client.Timeout = -1;
            var request = FormRequestUser(githubLoginDeveloper);
            IRestResponse response = client.Execute(request);

            serverContent = response.Content;
        }
        private IRestRequest FormRequestUser(string githubLoginDeveloper)
        {
            return new RestRequest("https://api.github.com/users/{user}", Method.GET)
                                            .AddHeader("Authorization", $"Bearer {token}")
                                            .AddUrlSegment("user", githubLoginDeveloper);
        }


        /// <summary>Gets the developer avatar URL.</summary>
        /// <param name="githubLoginDeveloper">The github login developer.</param>
        /// <returns>
        ///   <para>String.</para>
        /// </returns>
        public string GetDeveloperAvatarURL(string githubLoginDeveloper)
        {
            GetUser(githubLoginDeveloper);
            return ConvertServerContentToAvatarURL();
        }

        public string ConvertServerContentToAvatarURL()
        {
            var gitHubUser = JObject.Parse(serverContent);
            return gitHubUser.Value<string>("avatar_url");
        }
    }
}
