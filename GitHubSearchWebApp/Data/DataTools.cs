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
                    dev1.Id = 11;
                    dev1.FullName = "George";
                    dev1.GitLogin = "Geo3777";
                    dev1.Email = "george@mail.com";
                    var dev2 = new Developer();
                    dev2.Id = 2;
                    dev2.FullName = "Ionut";
                    dev2.GitLogin = "ionutroth";
                    dev2.Email = "ionut@mail.com";
                    applicationDbContext.Add(dev1);
                    applicationDbContext.Add(dev2);
                    applicationDbContext.SaveChanges();
                }
            }
        }
    }
}
