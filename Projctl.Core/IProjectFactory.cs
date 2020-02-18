namespace Projctl.Core
{
    #region Namespace Imports

    using System.Collections.Generic;

    #endregion


    public interface IProjectFactory
    {
        IEnumerable<IProject> Projects { get; }
        IEnumerable<ISolution> Solutions { get; }
        bool Load(string fileName);
        IProject LoadProject(string projectFile);
        ISolution LoadSolution(string solutionFile);
    }
}