using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions;

public sealed class SwitchExpression : Expression
{
	public ReadOnlyCollection<SwitchCase> Cases
	{
		get
		{
			throw null;
		}
	}

	public MethodInfo? Comparison
	{
		get
		{
			throw null;
		}
	}

	public Expression? DefaultBody
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

	public Expression SwitchValue
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

	internal SwitchExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public SwitchExpression Update(Expression switchValue, IEnumerable<SwitchCase>? cases, Expression? defaultBody)
	{
		throw null;
	}
}
