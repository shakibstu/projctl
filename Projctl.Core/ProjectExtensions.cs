namespace Projctl.Core
{
    #region Namespace Imports

    using System.Collections.Generic;

    using JetBrains.Annotations;

    using Microsoft.Build.Evaluation;

    #endregion


    internal static class ProjectExtensions
    {
        private const string _projectReferenceItemType = "ProjectReference";

        public static ICollection<ProjectItem> GetProjectReferences([NotNull] this Microsoft.Build.Evaluation.Project project) =>
            project.GetItemsIgnoringCondition(_projectReferenceItemType);
    }
}