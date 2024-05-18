using System.IO;

namespace System.CodeDom.Compiler;

public interface ICodeParser
{
	CodeCompileUnit Parse(TextReader codeStream);
}
