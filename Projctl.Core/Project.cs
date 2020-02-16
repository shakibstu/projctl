namespace Projctl.Core
{
    #region Namespace Imports

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using JetBrains.Annotations;

    using Microsoft.Build.Evaluation;
    using Microsoft.Build.Globbing;

    #endregion


    public class Project : IProject
    {
        [CanBeNull]
        private readonly Microsoft.Build.Evaluation.Project _project;

        private readonly IProjectFactory _projectFactory;

        [CanBeNull]
        private List<IProject> _referencedProjects;

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

        public bool ContainsFiles(CompositeGlob files) => GetProject().Items.Any(i => files.IsMatch(i.GetFullPath()));

        public IEnumerable<IProject> GetAllReferencedProjects(bool includeUnsupported = false)
        {
            if (includeUnsupported && !IsSupported)
            {
                return Enumerable.Empty<Project>();
            }

            var projects = GetReferencedProjects().Where(p => includeUnsupported || p.IsSupported).ToList();

            return projects.Concat(projects.SelectMany(p => p.GetAllReferencedProjects(includeUnsupported))).Distinct();
        }

        public ICollection<ProjectItem> GetItems(string itemType) => GetProject().GetItems(itemType);

        public ProjectProperty GetProperty(string name) => GetProject().GetProperty(name);

        public IEnumerable<IProject> GetReferencedProjects()
        {
            if (_referencedProjects != null)
            {
                return _referencedProjects;
            }

            _referencedProjects = GetProject()
                .GetProjectReferences()
                .Select(p => _projectFactory.Load(Path.GetFullPath(Path.Combine(DirectoryPath, p.EvaluatedInclude))))
                .Where(p => p != null)
                .Distinct()
                .ToList();

            return _referencedProjects;
        }

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