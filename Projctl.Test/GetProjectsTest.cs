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
                $"get-projects {TestSolution} --project-items {ProjectAClass1Path}\n{ProjectAClass2Path} --project-item-types Compile");

            Out.Should().HaveCount(1).And.Contain(ProjectA);
        }

        [DataTestMethod]
        [DataRow(ProjectD, ProjectB, ProjectC)]
        [DataRow(ProjectD + " --recursive", ProjectA, ProjectB, ProjectC)]
        public async Task ShouldFindOneProjectWithReferenceInSolution(string projectGlob, params string[] results)
        {
            await InvokeAsync(
                $"get-projects {TestSolution} --project-items {projectGlob} --project-item-types ProjectReference");

            Out.Should().HaveCount(results.Length).And.Contain(results);
        }

        [TestMethod]
        public async Task ShouldFindOneProjectContainingFileInFolder()
        {
            await InvokeAsync($"get-projects --project-items {ProjectAClass1Path}\n{ProjectAClass2Path}");

            Out.Should().HaveCount(1).And.Contain(ProjectA);
        }

        [TestMethod]
        public async Task ShouldFindOneProjectContainingFileInSolution()
        {
            await InvokeAsync($"get-projects {TestSolution} --project-items {ProjectAClass1Path}\n{ProjectAClass2Path}");

            Out.Should().HaveCount(1).And.Contain(ProjectA);
        }
    }
}