using System.Linq.Expressions;

namespace System.Dynamic;

public abstract class BinaryOperationBinder : DynamicMetaObjectBinder
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

	protected BinaryOperationBinder(ExpressionType operation)
	{
	}

	public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
	{
		throw null;
	}

	public DynamicMetaObject FallbackBinaryOperation(DynamicMetaObject target, DynamicMetaObject arg)
	{
		throw null;
	}

	public abstract DynamicMetaObject FallbackBinaryOperation(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject? errorSuggestion);
}
