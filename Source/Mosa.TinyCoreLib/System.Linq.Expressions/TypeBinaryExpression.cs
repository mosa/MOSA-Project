namespace System.Linq.Expressions;

public sealed class TypeBinaryExpression : Expression
{
	public Expression Expression
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

	public sealed override Type Type
	{
		get
		{
			throw null;
		}
	}

	public Type TypeOperand
	{
		get
		{
			throw null;
		}
	}

	internal TypeBinaryExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public TypeBinaryExpression Update(Expression expression)
	{
		throw null;
	}
}
