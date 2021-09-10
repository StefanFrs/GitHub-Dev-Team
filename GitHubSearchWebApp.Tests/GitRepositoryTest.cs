using GitHubSearchWebApp.Controllers;
using GitHubSearchWebApp.Models;
using System.IO;
using Xunit;

namespace GitHubSearchWebApp.Tests
{
    public class GitRepositoryTest
    {
        [Fact]
        public void ConvertResponseToToGitRepositoriesNameTest()
        {
            //Asume
            string content = LoadJsonFromResourceJson();
            var controller = new GitRepositoryController();

            // Act
            var output = controller.ConvertServerContentToGitRepositories(content);

            // Assert
            var gitName = ((GitRepository[])output)[0];
            Assert.Equal("jnewland/gsa-prototype", gitName.Name);
        }
    
        [Fact]
        public void ConvertResponseToToGitRepositoriesHtmlUrlTest()
        {
            //Asume
            string content = LoadJsonFromResourceJson();
            var controller = new GitRepositoryController();

            // Act
            var output = controller.ConvertServerContentToGitRepositories(content);

            // Assert
            var gitHtmlUrl = ((GitRepository[])output)[0];
            Assert.Equal("https://github.com/jnewland/gsa-prototype", gitHtmlUrl.HtmlUrl);
        }
        [Fact]
        public void ConvertResponseToToGitRepositoriesOwnerTest()
        {
            //Asume
            string content = LoadJsonFromResourceJson();
            var controller = new GitRepositoryController();
            // Act
            var output = controller.ConvertServerContentToGitRepositories(content);
            // Assert
            var gitOwner = ((GitRepository[])output)[0];
            Assert.Equal("jnewland", gitOwner.Username);
            Assert.Equal("https://avatars.githubusercontent.com/u/47?v=4", gitOwner.AvatarUrl);
        }
        [Fact]
        public void ConvertResponseToToGitRepositoriesIdTest()
        {
            //Asume
            string content = LoadJsonFromResourceJson();
            var controller = new GitRepositoryController();

            // Act
            var output = controller.ConvertServerContentToGitRepositories(content);

            // Assert
            var gitId = ((GitRepository[])output)[0];
            Assert.Equal(1, gitId.Id);
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
