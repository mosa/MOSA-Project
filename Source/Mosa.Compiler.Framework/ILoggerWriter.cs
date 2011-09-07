
namespace Mosa.Compiler.Framework
{
    public interface ILoggerWriter
    {
        void WriteLine(string s);
        void Indent();
        void Unindent();
    }
}
