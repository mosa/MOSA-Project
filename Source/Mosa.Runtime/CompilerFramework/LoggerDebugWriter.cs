using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
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
