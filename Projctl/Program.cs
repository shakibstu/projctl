namespace Projctl
{
    #region Namespace Imports

    using System;
    using System.CommandLine;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Build.Locator;

    using Projctl.Core;

    #endregion


    internal class Program
    {
        public static RootCommand BuildCli()
        {
            MSBuildLocator.RegisterDefaults();

            return new RootCommand
            {
                new Command("get-projects")
                {
                    new Option("--solution") { Argument = new Argument<FileInfo>().ExistingOnly() },
                    new Option("--containing-files")
                    {
                        Argument = new Argument<string[]> { Arity = new ArgumentArity(0, int.MaxValue) }
                    }
                }.WithHandler<FileInfo, string[]>(GetProjects)
            };
        }

        private static void GetProjects(FileInfo solution, string[] containingFiles)
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

            var projects = codebase.GetProjectsContainingFiles(containingFiles).ToList();

            foreach (var project in projects)
            {
                Console.WriteLine(project.FullPath);
            }
        }

        private static async Task<int> Main(string[] args) => await BuildCli().InvokeAsync(args);
    }
}