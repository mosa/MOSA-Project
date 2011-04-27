using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    public interface ILoggerWriter
    {
        void WriteLine(string s);
        void Indent();
        void Unindent();
    }
}
