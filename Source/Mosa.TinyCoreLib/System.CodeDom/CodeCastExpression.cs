namespace System.CodeDom;

public class CodeCastExpression : CodeExpression
{
	public CodeExpression Expression
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeTypeReference TargetType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeCastExpression()
	{
	}

	public CodeCastExpression(CodeTypeReference targetType, CodeExpression expression)
	{
	}

	public CodeCastExpression(string targetType, CodeExpression expression)
	{
	}

	public CodeCastExpression(Type targetType, CodeExpression expression)
	{
	}
}
