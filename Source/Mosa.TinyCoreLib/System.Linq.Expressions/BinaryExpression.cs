using System.Reflection;

namespace System.Linq.Expressions;

public class BinaryExpression : Expression
{
	public override bool CanReduce
	{
		get
		{
			throw null;
		}
	}

	public LambdaExpression? Conversion
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

	public Expression Left
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

	public Expression Right
	{
		get
		{
			throw null;
		}
	}

	internal BinaryExpression()
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

	public BinaryExpression Update(Expression left, LambdaExpression? conversion, Expression right)
	{
		throw null;
	}
}
