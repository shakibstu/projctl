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
                $"get-projects {TestSlnPath} --containing-files {ProjectAClass1Path}\n{ProjectAClass2Path} --project-item-types Compile");

            Out.Should().HaveCount(1).And.Contain(ProjectA);
        }

        [TestMethod]
        public async Task ShouldFindOneProjectContainingFileInFolder()
        {
            await InvokeAsync($"get-projects --containing-files {ProjectAClass1Path}\n{ProjectAClass2Path}");

            Out.Should().HaveCount(1).And.Contain(ProjectA);
        }

        [TestMethod]
        public async Task ShouldFindOneProjectContainingFileInSolution()
        {
            await InvokeAsync($"get-projects {TestSlnPath} --containing-files {ProjectAClass1Path}\n{ProjectAClass2Path}");

            Out.Should().HaveCount(1).And.Contain(ProjectA);
        }
    }
}