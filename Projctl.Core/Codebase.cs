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

        public Codebase(IProjectFactory projectFactory)
        {
            _projectFactory = projectFactory;
            FullPath = Directory.GetCurrentDirectory();
        }

        public string FullPath { get; private set; }

        public void Discover(string[] projects)
        {
            var filesGlob = GetFilesGlob(projects);

            var files = Directory.EnumerateFileSystemEntries(FullPath, "*.*", SearchOption.AllDirectories).Where(f => filesGlob.IsMatch(f));

            foreach (var file in files)
            {
                _projectFactory.Load(file);
            }
        }

        public IEnumerable<IProject> GetProjectReferences(bool recursive)
        {
            var projects = _projectFactory.Projects.SelectMany(p => p.GetReferencedProjects(recursive)).Distinct().ToList();

            return projects;
        }

        public IEnumerable<IProject> GetProjectsContainingFiles(string[] containingFiles, string[] projectItemTypes = null)
        {
            var filesGlob = GetFilesGlob(containingFiles);
            return _projectFactory.Projects.Where(p => p.ContainsFiles(filesGlob, projectItemTypes));
        }

        private CompositeGlob GetFilesGlob(string[] searchPatterns)
        {
            if (searchPatterns == null || searchPatterns.Length == 0)
            {
                searchPatterns = new[] { "*.*proj", "*.sln" };
            }

            return new CompositeGlob(searchPatterns.Select(s => MSBuildGlob.Parse(FullPath, s)));
        }
    }
}