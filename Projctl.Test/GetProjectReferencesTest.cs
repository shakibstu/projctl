namespace Projctl.Test
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class GetProjectReferencesTest : TestBase
    {
        [TestMethod]
        public async Task ShouldFindOneProjectContainingCompileProjectItemInSolution()
        {
            await InvokeAsync($"get-project-references ");

            Out.Should().HaveCount(1).And.Contain(TestProjectA);
        }
    }
}