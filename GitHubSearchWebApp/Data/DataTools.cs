using GitHubSearchWebApp.Controllers;
using GitHubSearchWebApp.Models;
using GitHubSearchWebApp.Repo;
using GitHubSearchWebApp.Repositories;
using GitHubSearchWebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Data
{
    public static class DataTools
    {
        /// <summary>Seeds the data.</summary>
        /// <param name="application">The application.</param>
        public static void SeedData(this IApplicationBuilder application)
        {
            using (var serviceScope = application.ApplicationServices.CreateScope())
            {
                var applicationDbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                var developersRepository = serviceScope.ServiceProvider.GetService<IDevelopersRepository>();
                var experiencesRepository = serviceScope.ServiceProvider.GetService<IExperiencesRepository>();
                var githubApiServices = serviceScope.ServiceProvider.GetService<IGitHubApiService>();

                var developersController = new DevelopersController(githubApiServices, experiencesRepository, developersRepository);

                if (applicationDbContext.Developer.Any())
                {
                    Console.WriteLine("Developers already exist!");
                }
                else
                {
                    Console.WriteLine("Populating DB with developers.");
                    PopulateDbWithDevelopersAsync(developersRepository, developersController);
                }
            }
        }

        private static void PopulateDbWithDevelopersAsync(IDevelopersRepository developersRepository, DevelopersController developersController)
        {
            Console.WriteLine("Adding develpers with experience.");

            AddDeveloper("Stefan", "StefanFrs", "stefan@principal33.com", developersRepository, developersController);
            AddDeveloper("Ionut", "ionutroth", "ionut@principal33.com", developersRepository, developersController);
            AddDeveloper("George", "Geo3777", "george@principal33.com", developersRepository, developersController);
            AddDeveloper("Sofia", "SofiaHritcu", "sofia@principal33.com", developersRepository, developersController);

        }

        private static async void AddDeveloper( string fullname, string githubLogin, string email, IDevelopersRepository developersRepository, DevelopersController developersController)
        {
            var developer = new Developer
            {
                FullName = fullname,
                GitLogin = githubLogin,
                Email = email,
            };
            developersRepository.Add(developer);
            await developersController.AddDeveloperExperience(developer);
        }
    }
}
