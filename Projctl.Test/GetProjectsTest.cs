namespace Projctl.Test
{
    #region Namespace Imports

    using System.CommandLine.Parsing;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion


    [TestClass]
    public class GetProjectsTest : TestBase
    {
        private TestConsole _console;
        private string _currentDirectory;
        private Parser _parser;

        public GetProjectsTest()
        {
            _currentDirectory = Directory.GetCurrentDirectory();
            _console = new TestConsole();
            _parser = new Parser(Program.BuildCli());
        }

        [TestInitialize]
        public void Init() => Directory.SetCurrentDirectory(Path.Combine(_currentDirectory, "TestSolution"));

        [TestMethod]
        public async Task ShouldFindOneProjectInFolder() =>
            await _parser.InvokeAsync("get-projects --containing-files TestProjectA\\Class1.cs\nTestProjectA\\Class2.cs", _console);

        [TestMethod]
        public async Task ShouldFindOneProjectInSolution()
        {
            await _parser.InvokeAsync(
                "get-projects --solution Test.sln --containing-files TestProjectA\\Class1.cs\nTestProjectA\\Class2.cs",
                _console);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            Directory.SetCurrentDirectory(_currentDirectory);
        }
    }
}