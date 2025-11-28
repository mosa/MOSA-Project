using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions;

public class BlockExpression : Expression
{
	public ReadOnlyCollection<Expression> Expressions
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

	public Expression Result
	{
		get
		{
			throw null;
		}
	}

	public override Type Type
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

	internal BlockExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public BlockExpression Update(IEnumerable<ParameterExpression>? variables, IEnumerable<Expression> expressions)
	{
		throw null;
	}
}
