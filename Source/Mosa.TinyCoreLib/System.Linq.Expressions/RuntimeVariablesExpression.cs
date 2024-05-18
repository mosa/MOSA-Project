using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions;

public sealed class RuntimeVariablesExpression : Expression
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

	public ReadOnlyCollection<ParameterExpression> Variables
	{
		get
		{
			throw null;
		}
	}

	internal RuntimeVariablesExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public RuntimeVariablesExpression Update(IEnumerable<ParameterExpression> variables)
	{
		throw null;
	}
}
