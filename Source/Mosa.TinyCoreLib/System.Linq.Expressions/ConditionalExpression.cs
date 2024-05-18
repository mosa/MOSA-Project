namespace System.Linq.Expressions;

public class ConditionalExpression : Expression
{
	public Expression IfFalse
	{
		get
		{
			throw null;
		}
	}

	public Expression IfTrue
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

	public Expression Test
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

	internal ConditionalExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public ConditionalExpression Update(Expression test, Expression ifTrue, Expression ifFalse)
	{
		throw null;
	}
}
