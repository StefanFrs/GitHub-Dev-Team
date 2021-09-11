using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevsWebApp.Models
{
    public class Developer
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string GitLogin { get; set; }

        public string Email { get; set; }
    }
}
