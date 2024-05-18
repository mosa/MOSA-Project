namespace System.Linq.Expressions;

public sealed class CatchBlock
{
	public Expression Body
	{
		get
		{
			throw null;
		}
	}

	public Expression? Filter
	{
		get
		{
			throw null;
		}
	}

	public Type Test
	{
		get
		{
			throw null;
		}
	}

	public ParameterExpression? Variable
	{
		get
		{
			throw null;
		}
	}

	internal CatchBlock()
	{
	}

	public override string ToString()
	{
		throw null;
	}

	public CatchBlock Update(ParameterExpression? variable, Expression? filter, Expression body)
	{
		throw null;
	}
}
