using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
    class LoggerDebugWriter : ILoggerWriter
    {
        public void WriteLine(string s)
        {
            Debug.Write(s);
        }
        public void Indent()
        {
            Debug.Indent();
        }
        public void Unindent()
        {
            Debug.Unindent();
        }
    }
}
