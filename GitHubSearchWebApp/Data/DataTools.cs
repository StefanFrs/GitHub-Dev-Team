using GitHubSearchWebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GitHubSearchWebApp.Data
{
    public class DataTools
    {
        public static void SeedData(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var applicationDbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (applicationDbContext.Developer.Any())
                {
                    Console.WriteLine("The developers are there");
                }
                else
                {
                    Console.WriteLine("No developers");
                    var dev1 = new Developer();
                    var dev2 = new Developer();
                    dev1.Id = 1;
                    dev1.FullName = "Stefan";
                    dev1.GitLogin = "StefanFrs";
                    dev1.Email = "stefan.test@gmail.com";
                    dev2.Id = 2;
                    dev2.FullName = "Ionut";
                    dev2.GitLogin = "ionutroth";
                    dev2.Email = "ionut.test@gmail.com";
                    applicationDbContext.Add(dev1);
                    applicationDbContext.Add(dev2);
                    applicationDbContext.SaveChanges();
                }
            }
        }
    }
}