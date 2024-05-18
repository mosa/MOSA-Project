namespace System.Linq.Expressions;

public sealed class LoopExpression : Expression
{
	public Expression Body
	{
		get
		{
			throw null;
		}
	}

	public LabelTarget? BreakLabel
	{
		get
		{
			throw null;
		}
	}

	public LabelTarget? ContinueLabel
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

	internal LoopExpression()
	{
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		throw null;
	}

	public LoopExpression Update(LabelTarget? breakLabel, LabelTarget? continueLabel, Expression body)
	{
		throw null;
	}
}
