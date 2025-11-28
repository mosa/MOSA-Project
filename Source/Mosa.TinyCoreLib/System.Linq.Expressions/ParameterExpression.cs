namespace System.Linq.Expressions;

public class ParameterExpression : Expression
{
	public bool IsByRef
	{
		get
		{
			throw null;
		}
	}

	public string? Name
	{
		get
		{
			throw null;
		}
	}

	public sealed override ExpressionType NodeType
	{
		get
		{
			throw null;
		}
	}

	public override Type Type
	{
		get
		{
			throw null;
		}
	}

	internal ParameterExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}
}
