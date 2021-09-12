using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Services
{
    interface IDevelopersService
    {
        string GetDeveloperAvatarURL(string githubLoginDeveloper);
    }
}
