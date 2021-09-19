using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Models
{
    public class GitPush
    {
        public string User { get; set; }

        public string Repository { get; set; }

        public long Size { get; set; }

    }
}
