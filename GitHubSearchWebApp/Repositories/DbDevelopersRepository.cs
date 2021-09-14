using GitHubSearchWebApp.Data;
using GitHubSearchWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Repositories
{
    public class DbDevelopersRepository : IDevelopersRepository
    {
        private readonly ApplicationDbContext context;

        public DbDevelopersRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Developer GetById(int id)
        {
            return context.Developer.FirstOrDefault(d => d.Id == id);
        }
    }
}
