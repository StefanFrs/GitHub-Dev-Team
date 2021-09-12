using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubSearchWebApp.Models
{
    /// <summary>
    ///  Developer.<br />
    /// </summary>
    public class Developer
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the full name.</summary>
        /// <value>The full name.</value>
        public string FullName { get; set; }

        /// <summary>Gets or sets the git login.</summary>
        /// <value>The git login.</value>
        public string GitLogin { get; set; }

        /// <summary>Gets or sets the email.</summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>Gets or sets the avatar URL.</summary>
        /// <value>The avatar URL.</value>
        public string AvatarURL { get; set; }

        /// <summary>Gets or sets the experiences.</summary>
        /// <value>The experiences.</value>
        public ICollection<Experience> Experiences { get; set; }
    }
}
