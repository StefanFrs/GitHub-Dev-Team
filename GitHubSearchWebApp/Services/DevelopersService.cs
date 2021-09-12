using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Services
{
    public class DevelopersService: IDevelopersService
    {
        private String serverContent;

        /// <summary>Initializes a new instance of the <see cref="ExperiencesService" /> class.</summary>
        public DevelopersService()
        {
            serverContent = "";
        }

        private void GetUser(string githubLoginDeveloper)
        {
            var client = new RestClient();
            client.Timeout = -1;
            var request = FormRequest(githubLoginDeveloper);
            IRestResponse response = client.Execute(request);

            serverContent = response.Content;
        }
        private static IRestRequest FormRequest(string githubLoginDeveloper)
        {
            return new RestRequest("https://api.github.com/users/{user}", Method.GET)
                                            .AddHeader("Authorization", "Bearer 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")
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
