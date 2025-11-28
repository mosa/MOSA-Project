using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions;

public sealed class InvocationExpression : Expression, IArgumentProvider
{
	public ReadOnlyCollection<Expression> Arguments
	{
		get
		{
			throw null;
		}
	}

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

	int IArgumentProvider.ArgumentCount
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

	internal InvocationExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	Expression IArgumentProvider.GetArgument(int index)
	{
		throw null;
	}

	public InvocationExpression Update(Expression expression, IEnumerable<Expression>? arguments)
	{
		throw null;
	}
}
