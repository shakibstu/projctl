namespace Projctl.Core
{
    public interface IProjectFactory
    {
        IProject Load(string projectFile);
    }
}