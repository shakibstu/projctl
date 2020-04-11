namespace Projctl
{
    #region Namespace Imports

    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;

    #endregion


    public static class CommandExtensions
    {
        public static Command WithArgument<T>(this Command command, string alias)
        {
            command.AddArgument(new Argument<T>(alias));

            return command;
        }

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

        public static Command WithHandler<T1, T2, T3, T4, T5>(this Command command, Action<T1, T2, T3, T4, T5> action)
        {
            command.Handler = CommandHandler.Create(action);

            return command;
        }

        public static Command WithOption<T>(this Command command, string alias, Action<Option<T>> configureOption = null)
        {
            var option = new Option<T>(alias);

            configureOption?.Invoke(option);

            command.AddOption(option);

            return command;
        }

        public static Command WithProjectFilterArgument(this Command command)
        {
            command.AddArgument(new Argument<string[]>("projects"));

            return command;
        }

        public static Command WithRecursiveOption(this Command command)
        {
            command.AddOption(new Option<bool>("--recursive").WithAlias("-r"));

            return command;
        }
    }
}