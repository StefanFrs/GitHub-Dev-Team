using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        /// <summary>Gets or sets the projects.</summary>
        /// <value>The projects.</value>
        public ICollection<Project> Projects { get; set; }

        /// <summary>Gets or sets the programming language.</summary>
        /// <value>The programming language.</value>
        public ProgrammingLanguages ProgrammingLanguage { get; set; }

        /// <summary>Gets or sets the developer identifier.</summary>
        /// <value>The developer identifier.</value>
        public int DeveloperId { get; set; }
    }

    /// <summary>
    ///   <para>Programming Languages.<br /></para>
    /// </summary>
    public enum ProgrammingLanguages { 
        [Description("Java")] Java = 1,
        [Description("C++")] CPlusPlus = 2,
        [Description("C")] C = 3,
        [Description("C#")] CSharp = 4,
        [Description("JavaScript")] JavaScript = 5,
        [Description("CSS")] CSS = 6,
        [Description("Type Script")] TypeScript = 7,
        [Description("Vue")] Vue = 8,
        [Description("Jupyter Notebook")] JupyterNotebook = 9,
        [Description("Python")] Python = 10,
        [Description("Go")] Go = 11,
        [Description("Ruby")] Ruby = 12,
        [Description("PHP")] PHP = 13,
        [Description("Scala")] Scala = 14,
        [Description("Shell")] Shell = 15,
        [Description("Kotlin")] Kotlin = 16,
        [Description("Swift")] Swift = 17,
        [Description("Perl")] Perl = 18,
        [Description("Objective-C")] ObjectiveC = 19,
        [Description("WebAssembly")] WebAssembly = 20
    }


}
