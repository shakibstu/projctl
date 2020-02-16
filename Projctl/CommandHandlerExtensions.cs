namespace Projctl
{
    #region Namespace Imports

    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;

    #endregion


    public static class CommandHandlerExtensions
    {
        public static Command WithHandler<T1, T2>(this Command command, Action<T1, T2> action)
        {
            command.Handler = CommandHandler.Create(action);

            return command;
        }
    }
}