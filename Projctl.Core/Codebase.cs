namespace Projctl.Core
{
    #region Namespace Imports

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.Build.Globbing;

    #endregion


    public class Codebase
    {
        private readonly IProjectFactory _projectFactory;
        private List<IProject> _projects = new List<IProject>();
        private List<ISolution> _solutions = new List<ISolution>();

        public Codebase(IProjectFactory projectFactory)
        {
            _projectFactory = projectFactory;
            FullPath = Directory.GetCurrentDirectory();
        }

        public string FullPath { get; private set; }

        public IEnumerable<IProject> GetProjectReferences(string[] forProjects, bool recursive)
        {
            var filesGlob = GetFilesGlob(forProjects);

            var projects = _projects.Where(p => filesGlob.IsMatch(p.FullPath))
                .SelectMany(p => p.GetReferencedProjects(recursive))
                .Distinct()
                .ToList();

            return projects;
        }

        public IEnumerable<IProject> GetProjectsContainingFiles(string[] containingFiles, string[] projectItemTypes = null)
        {
            var filesGlob = GetFilesGlob(containingFiles);
            return _projects.Where(p => p.ContainsFiles(filesGlob, projectItemTypes));
        }

        public void LoadFolder(string path = null)
        {
            if (!string.IsNullOrEmpty(path))
            {
                FullPath = Path.GetFullPath(path);
            }

            // TODO: support all project types
            var projectFiles = Directory.EnumerateFileSystemEntries(FullPath, "*.csproj", SearchOption.AllDirectories);

            _projects.AddRange(projectFiles.Select(projectFile => _projectFactory.Load(projectFile)));
        }

        public void LoadSolution(string solutionFilePath)
        {
            var solution = new Solution(solutionFilePath, _projectFactory);
            _solutions.Add(solution);

            var projects = solution.GetProjects(true);

            _projects.AddRange(projects.Except(_projects));
        }

        private CompositeGlob GetFilesGlob(string[] searchPatterns) =>
            new CompositeGlob(searchPatterns.Select(s => MSBuildGlob.Parse(FullPath, s)));
    }
}