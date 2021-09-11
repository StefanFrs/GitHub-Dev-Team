using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Models
{
    public class Experience
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string CodeSize { get; set; }

        public int DeveloperId { get; set; }
    }
}
