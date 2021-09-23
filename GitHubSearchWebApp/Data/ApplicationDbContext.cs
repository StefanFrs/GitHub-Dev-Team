using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DevsWebApp.Models;
using GitHubSearchWebApp.Models;

namespace GitHubSearchWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GitHubSearchWebApp.Models.Developer> Developer { get; set; }

        public DbSet<GitHubSearchWebApp.Models.Experience> Experience { get; set; }

        public DbSet<GitHubSearchWebApp.Models.Project> Project { get; set; }
    }
}
