using System.Linq.Expressions;

namespace System.Dynamic;

public abstract class UnaryOperationBinder : DynamicMetaObjectBinder
{
	public ExpressionType Operation
	{
		get
		{
			throw null;
		}
	}

	public sealed override Type ReturnType
	{
		get
		{
			throw null;
		}
	}

	protected UnaryOperationBinder(ExpressionType operation)
	{
	}

	public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[]? args)
	{
		throw null;
	}

	public DynamicMetaObject FallbackUnaryOperation(DynamicMetaObject target)
	{
		throw null;
	}

	public abstract DynamicMetaObject FallbackUnaryOperation(DynamicMetaObject target, DynamicMetaObject? errorSuggestion);
}
