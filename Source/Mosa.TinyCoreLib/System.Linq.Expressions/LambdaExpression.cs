using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions;

public abstract class LambdaExpression : Expression
{
	public Expression Body
	{
		get
		{
			throw null;
		}
	}

	public string? Name
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

	public ReadOnlyCollection<ParameterExpression> Parameters
	{
		get
		{
			throw null;
		}
	}

	public Type ReturnType
	{
		get
		{
			throw null;
		}
	}

	public bool TailCall
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

	internal LambdaExpression()
	{
	}

	public Delegate Compile()
	{
		throw null;
	}

	public Delegate Compile(bool preferInterpretation)
	{
		throw null;
	}

	public Delegate Compile(DebugInfoGenerator debugInfoGenerator)
	{
		throw null;
	}
}
