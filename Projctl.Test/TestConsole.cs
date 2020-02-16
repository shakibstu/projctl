namespace Projctl.Test
{
    #region Namespace Imports

    using System.CommandLine;
    using System.CommandLine.IO;
    using System.IO;
    using System.Text;

    #endregion


    public class TestConsole : IConsole
    {
        public TestConsole()
        {
            Out = new StandardStreamWriter();
            Error = new StandardStreamWriter();
        }

        public IStandardStreamWriter Error { get; protected set; }

        public bool IsErrorRedirected { get; protected set; }

        public bool IsInputRedirected { get; protected set; }

        public bool IsOutputRedirected { get; protected set; }

        public IStandardStreamWriter Out { get; protected set; }


        internal class StandardStreamWriter : TextWriter, IStandardStreamWriter
        {
            private readonly StringBuilder _stringBuilder = new StringBuilder();

            public override Encoding Encoding { get; } = Encoding.Unicode;

            public override string ToString() => _stringBuilder.ToString();

            public override void Write(char value)
            {
                _stringBuilder.Append(value);
            }
        }
    }
}