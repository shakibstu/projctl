namespace Projctl
{
    #region Namespace Imports

    using System.CommandLine;

    #endregion


    public static class OptionExtensions
    {
        public static Option WithAlias(this Option option, string alias)
        {
            option.AddAlias(alias);

            return option;
        }
    }
}