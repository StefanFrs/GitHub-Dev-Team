using GitHubSearchWebApp.Data;
using GitHubSearchWebApp.Models;
using GitHubSearchWebApp.Repo;
using GitHubSearchWebApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GitHubSearchWebApp.Tests
{
    public class GitApiTest
    {
        private IDevelopersRepository developersRepo;
        DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        private GitHubApiService gitHubApiService;

        [Fact]
        public void GetProjectsByDeveloperByLanguageTest()
        {
            //Assume
            gitHubApiService = new GitHubApiService("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");
            InitiateDbRepository();

            //Act
            IList<Project> codeSize = (IList<Project>)gitHubApiService.GetProjectsByDeveloperByLanguage("ionutroth", "HTML");

            //Assert
            Assert.Equal(295, codeSize[0].CodeSize);
        }

        [Fact]
        public void GetProgrammingLanguagesTest()
        {
            //Assume
            gitHubApiService = new GitHubApiService("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");
            InitiateDbRepository();

            //Act
            HashSet<ProgrammingLanguages> languagesHashSet = (HashSet<ProgrammingLanguages>)gitHubApiService.GetProgrammingLanguagesByDeveloper("ionutroth");
            List<ProgrammingLanguages> languagesList = languagesHashSet.ToList();
            
            //Assert
            Assert.Equal("CSharp", languagesList[0].ToString());
        }

        private void InitiateDbRepository()
        {
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnetserver;Trusted_Connection=True;MultipleActiveResultSets=true");
            ApplicationDbContext context = new ApplicationDbContext(builder.Options);
            developersRepo = new DbDevelopersRepository(context);
        }
    }
}
