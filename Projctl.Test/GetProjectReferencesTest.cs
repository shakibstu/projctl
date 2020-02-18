namespace Projctl.Test
{
    #region Namespace Imports

    using System.Threading.Tasks;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion


    [TestClass]
    public class GetProjectReferencesTest : TestBase
    {
        [DataTestMethod]
        [DataRow(ProjectA,                                                               ProjectB, ProjectC)]
        [DataRow(ProjectA + " --recursive",                                              ProjectB, ProjectC, ProjectD)]
        [DataRow(ProjectA + " --recursive --include-root-projects",                      ProjectA, ProjectB, ProjectC, ProjectD)]
        [DataRow(TestSolution,                                                           ProjectB, ProjectC, ProjectD)]
        [DataRow("**\\*.csproj",                                                         ProjectB, ProjectC, ProjectD)]
        [DataRow(ProjectB,                                                               ProjectD)]
        [DataRow(ProjectB + " --recursive",                                              ProjectD)]
        [DataRow(ProjectB + " --recursive --include-root-projects",                      ProjectB, ProjectD)]
        [DataRow("TestSolution\\**\\*B.csproj",                                          ProjectD)]
        [DataRow("TestSolution\\**\\*B.csproj" + " --recursive",                         ProjectD)]
        [DataRow("TestSolution\\**\\*B.csproj" + " --recursive --include-root-projects", ProjectB, ProjectD)]
        public async Task ShouldGetProjectReferences(string projectGlob, params string[] results)
        {
            await InvokeAsync($"get-project-references {projectGlob}");

            Out.Should().HaveCount(results.Length).And.Contain(results);
        }
    }
}