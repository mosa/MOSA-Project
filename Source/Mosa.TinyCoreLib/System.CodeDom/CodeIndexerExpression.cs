namespace System.CodeDom;

public class CodeIndexerExpression : CodeExpression
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

	public CodeIndexerExpression()
	{
	}

	public CodeIndexerExpression(CodeExpression targetObject, params CodeExpression[] indices)
	{
	}
}
