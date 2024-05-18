using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions;

public sealed class TryExpression : Expression
{
	public Expression Body
	{
		get
		{
			throw null;
		}
	}

	public Expression? Fault
	{
		get
		{
			throw null;
		}
	}

	public Expression? Finally
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyCollection<CatchBlock> Handlers
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

	internal TryExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public TryExpression Update(Expression body, IEnumerable<CatchBlock>? handlers, Expression? @finally, Expression? fault)
	{
		throw null;
	}
}
