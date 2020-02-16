namespace Projctl.Test
{
    #region Namespace Imports

    using System.CommandLine.Parsing;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion


    [TestClass]
    public class GetProjectsTest
    {
        private TestConsole _console;
        private string _currentDirectory;
        private Parser _parser;

        public GetProjectsTest()
        {
            _console = new TestConsole();
            _parser = new Parser(Program.BuildCli());
        }

        [TestInitialize]
        public void Init()
        {
            _currentDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path.Combine(_currentDirectory, "TestSolution"));
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            await _parser.InvokeAsync("get-projects --containing-files 'TestProjectA\\Class1.cs'\n'TestProjectA\\Class2.cs'", _console);
        }
    }
}