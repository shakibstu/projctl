namespace Projctl
{
    #region Namespace Imports

    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.IO;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Build.Locator;

    using Projctl.Core;

    #endregion


    internal class Program
    {
        public static RootCommand BuildCli() =>
            new RootCommand
            {
                new Command("get-projects")
                {
                    new Argument<string[]>("projects"),
                    new Option("--containing-files")
                    {
                        Argument = new Argument<string[]> { Arity = new ArgumentArity(0, int.MaxValue) }
                    }.WithAlias("-f"),
                    new Option("--project-item-types")
                    {
                        Argument = new Argument<string[]> { Arity = new ArgumentArity(0, int.MaxValue) }
                    }.WithAlias("-t")
                }.WithHandler<IConsole, string[], string[], string[]>(GetProjects),
                new Command("get-project-references")
                {
                    new Argument<string[]>("projects"),
                    new Option<bool>("--recursive").WithAlias("-r"),
                    new Option<bool>("--include-root-projects")
                }.WithHandler<IConsole, string[], bool, bool>(GetProjectReferences),
                new Command("create-solution-filter")
                {
                    new Argument<string[]>("projects"),
                    new Option<FileInfo>("--solution").ExistingOnly(),
                    new Option<string>("--destination")
                }.WithHandler<IConsole, string[], FileInfo, string>(CreateSolutionFilter)
            };

        private static void CreateSolutionFilter(IConsole console, string[] projects, FileInfo solution, string destination)
        {
            var codebase = new Codebase(new ProjectFactory());

            if (string.IsNullOrEmpty(destination))
            {
                var solutionFilter = codebase.CreateSolutionFilter(solution, projects);
                console.Out.Write(solutionFilter);
            }
            else
            {
                codebase.CreateSolutionFilter(destination, solution, projects);
            }
        }

        private static Codebase GetCodebase(params string[] projects)
        {
            var codebase = new Codebase(new ProjectFactory());

            codebase.Discover(projects);

            return codebase;
        }

        private static void GetProjectReferences(IConsole console, string[] projects, bool recursive, bool includeRootProjects)
        {
            var codebase = GetCodebase(projects);

            var projectReferences = codebase.GetProjectReferences(recursive, includeRootProjects);

            WriteProjects(console, codebase, projectReferences);
        }

        private static void GetProjects(IConsole console, string[] projects, string[] containingFiles, string[] projectItemTypes)
        {
            var codebase = GetCodebase(projects);

            var projectsContainingFiles = codebase.GetProjectsContainingFiles(containingFiles, projectItemTypes);

            WriteProjects(console, codebase, projectsContainingFiles);
        }

        private static async Task<int> Main(string[] args)
        {
            MSBuildLocator.RegisterDefaults();

            return await BuildCli().InvokeAsync(args);
        }

        private static void WriteProjects(IConsole console, Codebase codebase, IEnumerable<IProject> projects)
        {
            foreach (var project in projects)
            {
                var relativePath = Path.GetRelativePath(codebase.FullPath, project.FullPath);
                console.Out.WriteLine(relativePath);
            }
        }
    }
}