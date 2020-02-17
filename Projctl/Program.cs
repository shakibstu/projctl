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
                    new Option("--containing-files")
                    {
                        Argument = new Argument<string[]> { Arity = new ArgumentArity(0, int.MaxValue) }
                    }.WithAlias("-f"),
                    new Option("--project-item-types")
                    {
                        Argument = new Argument<string[]> { Arity = new ArgumentArity(0, int.MaxValue) }
                    }.WithAlias("-t")
                }.WithHandler<IConsole, FileInfo, string[], string[]>(GetProjects),
                new Command("get-project-references")
                {
                    new Argument<string[]>("for-projects"), new Option<bool>("--recursive").WithAlias("-r")
                }.WithHandler<IConsole, FileInfo, string[], bool>(GetProjectReferences)
            }.WithGlobalOption(new Option("--solution") { Argument = new Argument<FileInfo>().ExistingOnly() }.WithAlias("-s"));

        private static Codebase GetCodebase(FileInfo solution)
        {
            var codebase = new Codebase(new ProjectFactory());

            if (solution != null)
            {
                codebase.LoadSolution(solution.FullName);
            }
            else
            {
                codebase.LoadFolder();
            }

            return codebase;
        }

        private static void GetProjectReferences(IConsole console, FileInfo solution, string[] forProjects, bool recursive)
        {
            var codebase = GetCodebase(solution);

            var projects = codebase.GetProjectReferences(forProjects, recursive);

            WriteProjects(console, codebase, projects);
        }

        private static void GetProjects(IConsole console, FileInfo solution, string[] containingFiles, string[] projectItemTypes)
        {
            var codebase = GetCodebase(solution);

            var projects = codebase.GetProjectsContainingFiles(containingFiles, projectItemTypes);

            WriteProjects(console, codebase, projects);
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