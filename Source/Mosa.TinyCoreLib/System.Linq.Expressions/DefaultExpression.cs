namespace System.Linq.Expressions;

public sealed class DefaultExpression : Expression
{
	public sealed override ExpressionType NodeType
	{
		get
		{
			throw null;
		}
	}

	public sealed override Type Type
	{
		get
		{
			throw null;
		}
	}

	internal DefaultExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}
}
