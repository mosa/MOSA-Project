using System.IO;

namespace System.CodeDom.Compiler;

public abstract class CodeParser : ICodeParser
{
	public abstract CodeCompileUnit Parse(TextReader codeStream);
}
