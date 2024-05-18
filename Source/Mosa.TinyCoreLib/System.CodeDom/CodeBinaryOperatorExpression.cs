namespace System.CodeDom;

public class CodeBinaryOperatorExpression : CodeExpression
{
	public CodeExpression Left
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeBinaryOperatorType Operator
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeExpression Right
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeBinaryOperatorExpression()
	{
	}

	public CodeBinaryOperatorExpression(CodeExpression left, CodeBinaryOperatorType op, CodeExpression right)
	{
	}
}
