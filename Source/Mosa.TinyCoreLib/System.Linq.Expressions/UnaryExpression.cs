using System.Reflection;

namespace System.Linq.Expressions;

public sealed class UnaryExpression : Expression
{
	public override bool CanReduce
	{
		get
		{
			throw null;
		}
	}

	public bool IsLifted
	{
		get
		{
			throw null;
		}
	}

	public bool IsLiftedToNull
	{
		get
		{
			throw null;
		}
	}

	public MethodInfo? Method
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

	public Expression Operand
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

	internal UnaryExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public override Expression Reduce()
	{
		throw null;
	}

	public UnaryExpression Update(Expression operand)
	{
		throw null;
	}
}
