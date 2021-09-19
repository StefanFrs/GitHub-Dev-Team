using GitHubSearchWebApp.Controllers;
using GitHubSearchWebApp.Models;
using GitHubSearchWebApp.Repo;
using Microsoft.Extensions.Configuration;
using System.IO;
using Xunit;

namespace GitHubSearchWebApp.Tests
{
    public class GitGubApiServiceTests
    {
        [Fact]
        public void ConvertServerContentToAvatarURLTest()
        {
            //Asume
            string content = LoadJsonFromResourceJson();

            var service = new GitHubApiService("place-your-token-here");

            // Act
            var output = service.ConvertServerContentToAvatarURL();

            // Assert
            string gitAvatar = output;
            Assert.Equal("https://avatars.githubusercontent.com/u/87963537?v=4", gitAvatar);
        }
        private string LoadJsonFromResourceJson()
        {
            var assembly = this.GetType().Assembly;
            var assemblyName = assembly.GetName().Name;
            var resourceName = $"{assemblyName}.DataFromGitRepoApi.json";
            var resourceStream = assembly.GetManifestResourceStream(resourceName);
            using (var tr = new StreamReader(resourceStream))
            {
                return tr.ReadToEnd();
            }
        }

    }
}
