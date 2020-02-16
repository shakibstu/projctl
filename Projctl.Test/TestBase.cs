namespace Projctl.Test
{
    #region Namespace Imports

    using Microsoft.Build.Locator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion


    [TestClass]
    public class TestBase
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            MSBuildLocator.RegisterDefaults();
        }
    }
}