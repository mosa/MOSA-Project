namespace System.Linq.Expressions;

public sealed class GotoExpression : Expression
{
	public GotoExpressionKind Kind
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

	public LabelTarget Target
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

	public Expression? Value
	{
		get
		{
			throw null;
		}
	}

	internal GotoExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public GotoExpression Update(LabelTarget target, Expression? value)
	{
		throw null;
	}
}
