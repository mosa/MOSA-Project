namespace System.CodeDom;

public class CodeVariableDeclarationStatement : CodeStatement
{
	public CodeExpression InitExpression
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeTypeReference Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeVariableDeclarationStatement()
	{
	}

	public CodeVariableDeclarationStatement(CodeTypeReference type, string name)
	{
	}

	public CodeVariableDeclarationStatement(CodeTypeReference type, string name, CodeExpression initExpression)
	{
	}

	public CodeVariableDeclarationStatement(string type, string name)
	{
	}

	public CodeVariableDeclarationStatement(string type, string name, CodeExpression initExpression)
	{
	}

	public CodeVariableDeclarationStatement(Type type, string name)
	{
	}

	public CodeVariableDeclarationStatement(Type type, string name, CodeExpression initExpression)
	{
	}
}
