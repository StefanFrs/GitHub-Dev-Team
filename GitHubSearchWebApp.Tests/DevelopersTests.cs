using GitHubSearchWebApp.Controllers;
using GitHubSearchWebApp.Data;
using GitHubSearchWebApp.Models;
using GitHubSearchWebApp.Repo;
using GitHubSearchWebApp.Repositories;
using GitHubSearchWebApp.Services;
using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Web.Helpers;
using Xunit;

namespace DevelopersTests.Tests
{
    public class DevelopersTests
    {

        private IDevelopersRepository developersRepo;
        DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        private GitHubApiService gitHubApiService;

        [Fact]
        public void ShouldAddDevloperToDbTest()
        {
            //Assume
            InitiateDbRepository();

            //Act
            developersRepo.Add(new Developer
            {
                FullName = "Test.Developer.FullName.1",
                AvatarURL = "https://avatars.githubusercontent.com/u/87963655?v=4",
                GitLogin = "Test.Developer.GitLogin.1",
                Email = "Test.Developer.Email.1",
                Experiences = new List<Experience>()
            });

            IEnumerable<Developer> developers = developersRepo.GetAll();

            bool justAddedFound = false;

            Developer found = new Developer();
            foreach (var d in developers)
            {
                if (d.FullName == "Test.Developer.FullName.1" && d.GitLogin == "Test.Developer.GitLogin.1" && d.Email == "Test.Developer.Email.1")
                {
                    found = d;
                    justAddedFound = true;
                }
            }

            // Assert
            Assert.True(justAddedFound);
            Assert.Equal("Test.Developer.FullName.1", found.FullName);
            Assert.Equal("Test.Developer.Email.1", found.Email);
            Assert.Equal("Test.Developer.GitLogin.1", found.GitLogin);
        }

        [Fact]
        public void ShouldUpdateDeveloperTest()
        {
            //Assume
            InitiateDbRepository();

            //Act
            IEnumerable<Developer> developers = developersRepo.GetAll();

            Developer found = new Developer();
            foreach (var d in developers)
            {
                if (d.FullName == "Test.Developer.FullName.1" && d.GitLogin == "Test.Developer.GitLogin.1" && d.Email == "Test.Developer.Email.1")
                {
                    found = d;
                }
            }

            found.FullName = "test_FullName";
            found.GitLogin = "test_gitLogin";
            found.Email = "test@email.com";
            developersRepo.Update(found);
            var afterUpdate = developersRepo.GetById(found.Id);

            //Assert
            Assert.Equal("test_FullName", afterUpdate.FullName);
            Assert.Equal("test@email.com", afterUpdate.Email);
            Assert.Equal("test_gitLogin", afterUpdate.GitLogin);

            developersRepo.Delete(found.Id);
        }

        [Fact]
        public void ShouldDeleteDeveloperTests()
        {
            //Assume
            InitiateDbRepository();

            //Act
            IEnumerable<Developer> developers = developersRepo.GetAll();

            bool existingDeveloper = false;
            foreach (var d in developers)
            {
                if (d.FullName == "test_FullName" && d.GitLogin == "test_gitLogin" && d.Email == "test@email.com")
                {
                    existingDeveloper = true;
                }
            }

            //Assert
            Assert.False(existingDeveloper);
        }

        private void InitiateDbRepository()
        {
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnetserver;Trusted_Connection=True;MultipleActiveResultSets=true");
            ApplicationDbContext context = new ApplicationDbContext(builder.Options);
            developersRepo = new DbDevelopersRepository(context);
        }
    }
}
