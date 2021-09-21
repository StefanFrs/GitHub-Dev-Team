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
                    dev1.FullName = "Ionut";
                    dev1.GitLogin = "ionutroth";
                    dev1.Email = "rothionut@mail.com";
                    dev2.Id = 2;
                    dev2.FullName = "sofia";
                    dev2.GitLogin = "SofiaHritcu";
                    dev2.Email = "sofia@mail.com";
                    applicationDbContext.Add(dev1);
                    applicationDbContext.Add(dev2);
                    applicationDbContext.SaveChanges();
                }
            }
        }
    }
}