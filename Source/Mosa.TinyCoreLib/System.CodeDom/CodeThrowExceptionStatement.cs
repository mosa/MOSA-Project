namespace System.CodeDom;

public class CodeThrowExceptionStatement : CodeStatement
{
	public CodeExpression ToThrow
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeThrowExceptionStatement()
	{
	}

	public CodeThrowExceptionStatement(CodeExpression toThrow)
	{
	}
}
