namespace Projctl
{
    #region Namespace Imports

    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Linq;
    using System.Threading.Tasks;

    using Projctl.Core;

    #endregion


    internal class Program
    {
        public static RootCommand BuildCli()
        {
            Microsoft.Build.Locator.MSBuildLocator.RegisterDefaults();

            var rootCommand = new RootCommand();
            var getProjectsCommand = new Command("get-projects") { Handler = CommandHandler.Create<string[]>(GetProjects) };

            getProjectsCommand.Add(new Option("--containing-files")
            {
                Argument = new Argument<string[]> { Arity = new ArgumentArity(0, int.MaxValue) }
            });

            getProjectsCommand.Add(
                new Option("--with-dependencies") { Argument = new Argument<string[]> { Arity = new ArgumentArity(0, int.MaxValue) } });

            rootCommand.Add(getProjectsCommand);
            return rootCommand;
        }

        private static void GetProjects(string[] containingFiles)
        {
            var codebase = new Codebase(new ProjectFactory());

            codebase.Load();

            var projects = codebase.GetProjectsContainingFiles(containingFiles).ToList();

            foreach (var project in projects)
            {
                Console.WriteLine(project.FullPath);
            }
        }

        private static async Task<int> Main(string[] args) => await BuildCli().InvokeAsync(args);
    }
}