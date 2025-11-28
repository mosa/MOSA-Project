using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions;

public sealed class ListInitExpression : Expression
{
	public override bool CanReduce
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<ElementInit> Initializers
	{
		get
		{
			throw null;
		}
	}

	public NewExpression NewExpression
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

	internal ListInitExpression()
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

	public ListInitExpression Update(NewExpression newExpression, IEnumerable<ElementInit> initializers)
	{
		throw null;
	}
}
