using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions;

public sealed class MemberInitExpression : Expression
{
	public ReadOnlyCollection<MemberBinding> Bindings
	{
		get
		{
			throw null;
		}
	}

	public override bool CanReduce
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

	internal MemberInitExpression()
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

	public MemberInitExpression Update(NewExpression newExpression, IEnumerable<MemberBinding> bindings)
	{
		throw null;
	}
}
