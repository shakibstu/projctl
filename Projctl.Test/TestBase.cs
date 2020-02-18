namespace Projctl.Test
{
    #region Namespace Imports

    using System;
    using System.CommandLine.IO;
    using System.CommandLine.Parsing;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Build.Locator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion


    [TestClass]
    public class TestBase
    {
        protected const string TestProjectA = "TestSolution\\TestProjectA\\TestProjectA.csproj";
        protected const string TestProjectAClass1Path = "TestSolution\\TestProjectA\\Class1.cs";
        protected const string TestProjectAClass2Path = "TestSolution\\TestProjectA\\Class2.cs";
        protected const string TestSlnPath = "TestSolution\\Test.sln";

        private TestConsole _console;
        private Parser _parser;

        protected string Error => _console.Error.ToString();
        protected string[] Out => _console.Out.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Directory.SetCurrentDirectory("TestCodebase");

            MSBuildLocator.RegisterDefaults();
        }

        [TestInitialize]
        public void Init()
        {
            _console = new TestConsole();
            _parser = new Parser(Program.BuildCli());
        }

        protected async Task InvokeAsync(string commandLine)
        {
            await _parser.InvokeAsync(commandLine, _console);
        }
    }
}