namespace Projctl.Core
{
    #region Namespace Imports

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.Build.Evaluation;
    using Microsoft.Build.Globbing;

    #endregion


    public class Project : IProject
    {
        private readonly Microsoft.Build.Evaluation.Project _project;
        private readonly IProjectFactory _projectFactory;

        public Project(IProjectFactory projectFactory, Microsoft.Build.Evaluation.Project msbuildProject)
            : this(projectFactory, msbuildProject.FullPath) =>
            _project = msbuildProject;

        public Project(IProjectFactory projectFactory, string fullPath)
        {
            FullPath = fullPath;
            _projectFactory = projectFactory;
            Name = Path.GetFileNameWithoutExtension(FullPath);
        }

        public string DirectoryPath => _project?.DirectoryPath ?? Path.GetDirectoryName(FullPath);
        public string FullPath { get; }
        public bool IsDirty => _project?.IsDirty ?? false;
        public bool IsSupported => _project != null;
        public string Name { get; }

        public bool ContainsFiles(CompositeGlob files)
        {
            var project = GetProject();

            if (project.Items.Any(i => files.IsMatch(i.GetFullPath())))
            {
                return true;
            }

            var propertyValue = project.GetPropertyValue("TargetFramework");

            if (string.IsNullOrEmpty(propertyValue) || !propertyValue.StartsWith("netcoreapp", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Hack! There is no easy way to make MSBuild resolve project items, so we fetch them from disk
            var projectFiles = Directory.EnumerateFiles(DirectoryPath, "*", SearchOption.AllDirectories).ToList();
            return projectFiles.Any(f => files.IsMatch(f));
        }

        public ICollection<ProjectItem> GetItems(string itemType) => GetProject().GetItems(itemType);

        public ProjectProperty GetProperty(string name) => GetProject().GetProperty(name);

        private Microsoft.Build.Evaluation.Project GetProject()
        {
            if (_project == null)
            {
                throw new InvalidOperationException("The project could not be loaded.");
            }

            return _project;
        }
    }
}