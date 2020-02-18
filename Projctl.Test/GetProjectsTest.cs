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
        [TestMethod]
        public async Task ShouldFindOneProjectContainingCompileProjectItemInSolution()
        {
            await InvokeAsync(
                $"get-projects --solution {TestSlnPath} --containing-files {TestProjectAClass1Path}\n{TestProjectAClass2Path} --project-item-types Compile");

            Out.Should().HaveCount(1).And.Contain(TestProjectA);
        }

        [TestMethod]
        public async Task ShouldFindOneProjectContainingFileInFolder()
        {
            await InvokeAsync($"get-projects --containing-files {TestProjectAClass1Path}\n{TestProjectAClass2Path}");

            Out.Should().HaveCount(1).And.Contain(TestProjectA);
        }

        [TestMethod]
        public async Task ShouldFindOneProjectContainingFileInSolution()
        {
            await InvokeAsync(
                $"get-projects --solution {TestSlnPath} --containing-files {TestProjectAClass1Path}\n{TestProjectAClass2Path}");

            Out.Should().HaveCount(1).And.Contain(TestProjectA);
        }
    }
}