using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq.Expressions;

[RequiresDynamicCode("Creating arrays at runtime requires dynamic code generation.")]
public class NewArrayExpression : Expression
{
	public ReadOnlyCollection<Expression> Expressions
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

	internal NewArrayExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public NewArrayExpression Update(IEnumerable<Expression> expressions)
	{
		throw null;
	}
}
