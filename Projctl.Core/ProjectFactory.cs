namespace Projctl.Core
{
    #region Namespace Imports

    using System.Collections.Generic;
    using System.IO;

    using Microsoft.Build.Evaluation;

    #endregion


    public class ProjectFactory : IProjectFactory
    {
        private readonly Dictionary<string, IProject> _projectsByFileName = new Dictionary<string, IProject>();
        private ProjectCollection _projectCollection;

        public ProjectFactory() =>
            _projectCollection = new ProjectCollection(ToolsetDefinitionLocations.Local | ToolsetDefinitionLocations.Registry);


        public IProject Load(string projectFile)
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
    }
}