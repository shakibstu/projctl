namespace Projctl.Core
{
    #region Namespace Imports

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using JetBrains.Annotations;

    using Microsoft.Build.Construction;

    #endregion


    public class Solution : ISolution
    {
        private readonly IProjectFactory _projectFactory;
        private SolutionFile _file;

        [CanBeNull]
        private List<IProject> _projects;

        internal Solution(string fullPath, IProjectFactory projectFactory)
        {
            _file = SolutionFile.Parse(fullPath);
            FullPath = fullPath;
            _projectFactory = projectFactory;
        }

        public string DirectoryPath => Path.GetDirectoryName(FullPath);
        public string FileName => Path.GetFileName(FullPath);
        public string FullPath { get; }

        public IEnumerable<IProject> GetProjects(bool recursive = false)
        {
            var projects = LoadProjects().ToList();

            return !recursive ? projects : projects.Concat(projects.SelectMany(p => p.GetReferencedProjects(true))).Distinct();
        }

        private IEnumerable<IProject> LoadProjects()
        {
            if (_projects != null)
            {
                return _projects;
            }

            _projects = new List<IProject>();

            foreach (var projectInSolution in _file.GetMsBuildProjects())
            {
                var project = _projectFactory.LoadProject(projectInSolution.AbsolutePath);

                if (project == null)
                {
                    continue;
                }

                _projects.Add(project);
            }

            return _projects;
        }
    }
}