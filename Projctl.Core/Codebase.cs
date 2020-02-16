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
        private string _path;
        private List<IProject> _projects = new List<IProject>();

        public Codebase(IProjectFactory projectFactory) => _projectFactory = projectFactory;

        public IEnumerable<IProject> GetProjectsContainingFiles(string[] containingFiles)
        {
            var filesGlob = new CompositeGlob(containingFiles.Select(s => MSBuildGlob.Parse(_path, s)));
            return _projects.Where(p => p.IsSupported && p.ContainsFiles(filesGlob));
        }

        public void Load(string path = null)
        {
            _path = path;

            if (string.IsNullOrEmpty(_path))
            {
                _path = Directory.GetCurrentDirectory();
            }

            var projectFiles = Directory.EnumerateFileSystemEntries(_path, "*.csproj", SearchOption.AllDirectories);

            foreach (var projectFile in projectFiles)
            {
                var project = _projectFactory.Load(projectFile);

                _projects.Add(project);
            }
        }
    }
}