using GitHubSearchWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Repositories
{
    public interface IDevelopersRepository
    {
        public Developer GetById(int id);
    }
}
