namespace Projctl.Core
{
    #region Namespace Imports

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using JetBrains.Annotations;

    using Microsoft.Build.Construction;

    #endregion


    public class Solution
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

        public string FileName => Path.GetFileName(FullPath);
        public string FullPath { get; }

        public IEnumerable<IProject> GetAllProjects(bool includeUnsupported = false)
        {
            var projects = GetProjects().Where(p => includeUnsupported || p.IsSupported).ToList();

            return projects.Concat(projects.SelectMany(p => p.GetAllReferencedProjects(includeUnsupported))).Distinct();
        }

        public List<IProject> GetProjects()
        {
            if (_projects != null)
            {
                return _projects;
            }

            _projects = new List<IProject>();

            foreach (var projectInSolution in _file.GetMsBuildProjects())
            {
                var project = _projectFactory.Load(projectInSolution.AbsolutePath);

                if (project == null)
                {
                    continue;
                }

                _projects.Add(project);
            }

            return _projects;
        }
    }


    internal static class SolutionFileExtensions
    {
        public static string GetFullPath(this SolutionFile solution) =>
            solution.GetType().GetProperty("FullPath", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(solution) as string;

        public static IEnumerable<ProjectInSolution> GetMsBuildProjects(this SolutionFile solution) =>
            solution.ProjectsInOrder.Where(p => p.ProjectType != SolutionProjectType.SolutionFolder);
    }
}