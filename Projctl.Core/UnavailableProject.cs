namespace Projctl.Core
{
    #region Namespace Imports

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.Build.Globbing;

    #endregion


    public class UnavailableProject : IProject
    {
        public UnavailableProject(string fullPath)
        {
            FullPath = fullPath;
            Name = Path.GetFileNameWithoutExtension(FullPath);
        }

        public string DirectoryPath => Path.GetDirectoryName(FullPath);
        public string FullPath { get; }
        public bool IsDirty => false;
        public string Name { get; }

        public bool ContainsFiles(CompositeGlob files, string[] projectItemTypes = null) => false;
        public IEnumerable<IProject> GetReferencedProjects(bool recursive = false) => Enumerable.Empty<IProject>();
    }
}