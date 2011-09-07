
namespace Mosa.Runtime.CompilerFramework
{
    public interface ILoggerWriter
    {
        void WriteLine(string s);
        void Indent();
        void Unindent();
    }
}
