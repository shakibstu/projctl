namespace Projctl.Core
{
    #region Namespace Imports

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Build.Construction;

    #endregion


    internal static class SolutionFileExtensions
    {
        public static string GetFullPath(this SolutionFile solution) =>
            solution.GetType().GetProperty("FullPath", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(solution) as string;

        public static IEnumerable<ProjectInSolution> GetMsBuildProjects(this SolutionFile solution) =>
            solution.ProjectsInOrder.Where(p => p.ProjectType != SolutionProjectType.SolutionFolder);
    }
}