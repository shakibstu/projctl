namespace Projctl.Core
{
    #region Namespace Imports

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.Build.Evaluation;

    #endregion


    public class ProjectFactory : IProjectFactory
    {
        private readonly Dictionary<string, IProject> _projectsByFileName = new Dictionary<string, IProject>();
        private readonly Dictionary<string, ISolution> _solutionsByFileName = new Dictionary<string, ISolution>();
        private ProjectCollection _projectCollection;

        public ProjectFactory() =>
            _projectCollection = new ProjectCollection(ToolsetDefinitionLocations.Local | ToolsetDefinitionLocations.Registry);

        public IEnumerable<IProject> Projects => _projectsByFileName.Values.ToList();
        public IEnumerable<ISolution> Solutions => _solutionsByFileName.Values.ToList();

        public bool Load(string fileName)
        {
            if (fileName.EndsWith(".sln", StringComparison.OrdinalIgnoreCase))
            {
                return LoadSolution(fileName) != null;
            }

            if (Path.GetExtension(fileName).EndsWith("proj", StringComparison.OrdinalIgnoreCase))
            {
                return LoadProject(fileName) != null;
            }

            return false;
        }

        public IProject LoadProject(string projectFile)
        {
            projectFile = Path.GetFullPath(projectFile);

            IProject project;

            if (_projectsByFileName.TryGetValue(projectFile, out project))
            {
                return project;
            }

            if (!File.Exists(projectFile))
            {
                return null;
            }

            try
            {
                var msbuildProject = new Microsoft.Build.Evaluation.Project(
                    projectFile,
                    _projectCollection.GlobalProperties,
                    _projectCollection.DefaultToolsVersion,
                    _projectCollection,
                    ProjectLoadSettings.IgnoreEmptyImports
                    | ProjectLoadSettings.IgnoreInvalidImports
                    | ProjectLoadSettings.IgnoreMissingImports);

                project = new Project(this, msbuildProject);
            }
            catch
            {
                project = new UnavailableProject(projectFile);
            }

            _projectsByFileName.Add(projectFile, project);

            return project;
        }


        public ISolution LoadSolution(string solutionFile)
        {
            solutionFile = Path.GetFullPath(solutionFile);

            ISolution solution;

            if (_solutionsByFileName.TryGetValue(solutionFile, out solution))
            {
                return solution;
            }

            if (!File.Exists(solutionFile))
            {
                return null;
            }

            solution = new Solution(solutionFile, this);
            _solutionsByFileName.Add(solutionFile, solution);

            solution.GetProjects();

            return solution;
        }
    }
}