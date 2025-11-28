namespace System.CodeDom;

public class CodeObjectCreateExpression : CodeExpression
{
	public CodeTypeReference CreateType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeExpressionCollection Parameters
	{
		get
		{
			throw null;
		}
	}

	public CodeObjectCreateExpression()
	{
	}

	public CodeObjectCreateExpression(CodeTypeReference createType, params CodeExpression[] parameters)
	{
	}

	public CodeObjectCreateExpression(string createType, params CodeExpression[] parameters)
	{
	}

	public CodeObjectCreateExpression(Type createType, params CodeExpression[] parameters)
	{
	}
}
