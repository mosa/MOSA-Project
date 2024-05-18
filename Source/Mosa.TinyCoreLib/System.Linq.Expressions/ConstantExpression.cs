namespace System.Linq.Expressions;

public class ConstantExpression : Expression
{
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

	public object? Value
	{
		get
		{
			throw null;
		}
	}

	internal ConstantExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}
}
