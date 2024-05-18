using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Dynamic;

public abstract class BindingRestrictions
{
	public static readonly BindingRestrictions Empty;

	internal BindingRestrictions()
	{
	}

	public static BindingRestrictions Combine(IList<DynamicMetaObject>? contributingObjects)
	{
		throw null;
	}

	public static BindingRestrictions GetExpressionRestriction(Expression expression)
	{
		throw null;
	}

	public static BindingRestrictions GetInstanceRestriction(Expression expression, object? instance)
	{
		throw null;
	}

	public static BindingRestrictions GetTypeRestriction(Expression expression, Type type)
	{
		throw null;
	}

	public BindingRestrictions Merge(BindingRestrictions restrictions)
	{
		throw null;
	}

	public Expression ToExpression()
	{
		throw null;
	}
}
