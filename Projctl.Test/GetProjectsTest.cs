namespace Projctl.Test
{
    #region Namespace Imports

    using System.Threading.Tasks;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion


    [TestClass]
    public class GetProjectsTest : TestBase
    {
        private const string _testProjectA = "TestSolution\\TestProjectA\\TestProjectA.csproj";
        private const string _testProjectAClass1Path = "TestSolution\\TestProjectA\\Class1.cs";
        private const string _testProjectAClass2Path = "TestSolution\\TestProjectA\\Class2.cs";
        private const string _testSlnPath = "TestSolution\\Test.sln";

        [TestMethod]
        public async Task ShouldFindOneProjectContainingCompileProjectItemInSolution()
        {
            await InvokeAsync(
                $"get-projects --solution {_testSlnPath} --containing-files {_testProjectAClass1Path}\n{_testProjectAClass2Path} --project-item-types Compile");

            Out.Should().HaveCount(1).And.Contain(_testProjectA);
        }

        [TestMethod]
        public async Task ShouldFindOneProjectContainingFileInFolder()
        {
            await InvokeAsync($"get-projects --containing-files {_testProjectAClass1Path}\n{_testProjectAClass2Path}");

            Out.Should().HaveCount(1).And.Contain(_testProjectA);
        }

        [TestMethod]
        public async Task ShouldFindOneProjectContainingFileInSolution()
        {
            await InvokeAsync(
                $"get-projects --solution {_testSlnPath} --containing-files {_testProjectAClass1Path}\n{_testProjectAClass2Path}");

            Out.Should().HaveCount(1).And.Contain(_testProjectA);
        }
    }
}