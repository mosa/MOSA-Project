using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions;

public class MethodCallExpression : Expression, IArgumentProvider
{
	public ReadOnlyCollection<Expression> Arguments
	{
		get
		{
			throw null;
		}
	}

	public MethodInfo Method
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

	public Expression? Object
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

	internal MethodCallExpression()
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

	public MethodCallExpression Update(Expression? @object, IEnumerable<Expression>? arguments)
	{
		throw null;
	}
}
