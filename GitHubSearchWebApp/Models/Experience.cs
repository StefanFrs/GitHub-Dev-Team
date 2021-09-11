using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Models
{
    /// <summary>Experience.<br /></summary>
    public class Experience
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>Gets or sets the size of the code.</summary>
        /// <value>The size of the code.</value>
        public string CodeSize { get; set; }

        /// <summary>Gets or sets the developer identifier.</summary>
        /// <value>The developer identifier.</value>
        public int DeveloperId { get; set; }
    }
}
