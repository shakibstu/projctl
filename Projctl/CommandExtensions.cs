namespace Projctl
{
    #region Namespace Imports

    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;

    #endregion


    public static class CommandExtensions
    {
        public static TCommand WithGlobalOption<TCommand>(this TCommand command, Option option) where TCommand : Command
        {
            command.AddGlobalOption(option);

            return command;
        }

        public static Command WithHandler<T1, T2>(this Command command, Action<T1, T2> action)
        {
            command.Handler = CommandHandler.Create(action);

            return command;
        }

        public static Command WithHandler<T1, T2, T3>(this Command command, Action<T1, T2, T3> action)
        {
            command.Handler = CommandHandler.Create(action);

            return command;
        }

        public static Command WithHandler<T1, T2, T3, T4>(this Command command, Action<T1, T2, T3, T4> action)
        {
            command.Handler = CommandHandler.Create(action);

            return command;
        }
    }
}