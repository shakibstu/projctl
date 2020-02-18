namespace Projctl.Core
{
    #region Namespace Imports

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Microsoft.Build.Globbing;

    using Newtonsoft.Json;

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

        public void CreateSolutionFilter(string destinationFileName, FileInfo solutionFileInfo, string[] projectsGlob)
        {
            var solutionFilterJson = CreateSolutionFilter(solutionFileInfo, projectsGlob, Path.GetDirectoryName(destinationFileName));

            File.WriteAllText(destinationFileName, solutionFilterJson, Encoding.UTF8);
        }

        public string CreateSolutionFilter(FileInfo solutionFileInfo, string[] projectsGlob, string relativeTo = null)
        {
            var solution = _projectFactory.LoadSolution(solutionFileInfo.FullName);
            var filesGlob = GetFilesGlob(projectsGlob);
            var projects = solution.GetProjects(true).Where(p => filesGlob.IsMatch(p.FullPath));

            projects = GetProjectReferences(projects, true, true);

            var solutionFilterFile = new SolutionFilterFile
            {
                SolutionFilter = new SolutionFilter
                {
                    Path = Path.GetRelativePath(relativeTo ?? FullPath, solution.FullPath),
                    Projects = projects.Select(p => Path.GetRelativePath(solution.DirectoryPath, p.FullPath)).ToArray()
                }
            };

            return JsonConvert.SerializeObject(solutionFilterFile, Formatting.Indented);
        }

        public void Discover(string[] projects)
        {
            var filesGlob = GetFilesGlob(projects);

            var files = Directory.EnumerateFiles(FullPath, "*.*", SearchOption.AllDirectories).Where(f => filesGlob.IsMatch(f));

            foreach (var file in files)
            {
                _projectFactory.Load(file);
            }
        }

        public IEnumerable<IProject> GetProjectReferences(bool recursive, bool includeRootProjects) =>
            GetProjectReferences(_projectFactory.Projects.ToList(), recursive, includeRootProjects);

        public IEnumerable<IProject> GetProjectsContainingFiles(string[] containingFiles, string[] projectItemTypes = null)
        {
            var filesGlob = GetFilesGlob(containingFiles);
            return _projectFactory.Projects.Where(p => p.ContainsFiles(filesGlob, projectItemTypes));
        }

        private static IEnumerable<IProject> GetProjectReferences(
            IEnumerable<IProject> rootProjects,
            bool recursive,
            bool includeRootProjects)
        {
            rootProjects = rootProjects.ToList();

            var referencedProjects = rootProjects.SelectMany(p => p.GetReferencedProjects(recursive));

            if (includeRootProjects)
            {
                referencedProjects = rootProjects.Concat(referencedProjects);
            }

            return referencedProjects.Distinct();
        }

        private CompositeGlob GetFilesGlob(string[] searchPatterns)
        {
            if (searchPatterns == null || searchPatterns.Length == 0)
            {
                searchPatterns = new[] { "**\\*.*proj", "**\\*.sln" };
            }

            return new CompositeGlob(searchPatterns.Select(s => MSBuildGlob.Parse(FullPath, s)));
        }
    }
}