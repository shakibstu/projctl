namespace Projctl.Core
{
    #region Namespace Imports

    using Microsoft.Build.Globbing;

    #endregion


    public interface IProject
    {
        string DirectoryPath { get; }
        string FullPath { get; }
        bool IsDirty { get; }
        bool IsSupported { get; }
        string Name { get; }
        bool ContainsFiles(CompositeGlob files);
    }
}