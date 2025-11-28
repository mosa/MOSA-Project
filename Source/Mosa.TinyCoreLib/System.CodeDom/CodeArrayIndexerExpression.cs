namespace System.CodeDom;

public class CodeArrayIndexerExpression : CodeExpression
{
	public CodeExpressionCollection Indices
	{
		get
		{
			throw null;
		}
	}

	public CodeExpression TargetObject
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeArrayIndexerExpression()
	{
	}

	public CodeArrayIndexerExpression(CodeExpression targetObject, params CodeExpression[] indices)
	{
	}
}
