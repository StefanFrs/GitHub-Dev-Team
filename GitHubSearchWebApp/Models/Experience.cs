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
        [Description("C++")] CPlusPlus ,
        [Description("C")] C,
        [Description("C#")] CSharp,
        [Description("JavaScript")] JavaScript,
        [Description("CSS")] CSS,
        [Description("Type Script")] TypeScript,
        [Description("Vue")] Vue,
        [Description("Jupyter Notebook")] JupyterNotebook,
        [Description("Python")] Python,
        [Description("Go")] Go,
        [Description("Ruby")] Ruby,
        [Description("PHP")] PHP,
        [Description("Scala")] Scala,
        [Description("Shell")] Shell,
        [Description("Kotlin")] Kotlin,
        [Description("Swift")] Swift,
        [Description("Perl")] Perl,
        [Description("Objective-C")] ObjectiveC,
        [Description("WebAssembly")] WebAssembly,
        [Description("HTML")] HTML,
        [Description("Dart")] Dart,
        [Description("Dockerfile")] Dockerfile,
        [Description("Haskell")] Haskell,
        [Description("Starlark")] Starlark,
    }


}
