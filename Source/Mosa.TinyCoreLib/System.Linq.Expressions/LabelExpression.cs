namespace System.Linq.Expressions;

public sealed class LabelExpression : Expression
{
	public Expression? DefaultValue
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

	internal LabelExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public LabelExpression Update(LabelTarget target, Expression? defaultValue)
	{
		throw null;
	}
}
