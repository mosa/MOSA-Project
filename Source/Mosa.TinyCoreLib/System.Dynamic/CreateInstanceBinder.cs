namespace System.Dynamic;

public abstract class CreateInstanceBinder : DynamicMetaObjectBinder
{
	public CallInfo CallInfo
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

	protected CreateInstanceBinder(CallInfo callInfo)
	{
	}

	public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
	{
		throw null;
	}

	public DynamicMetaObject FallbackCreateInstance(DynamicMetaObject target, DynamicMetaObject[] args)
	{
		throw null;
	}

	public abstract DynamicMetaObject FallbackCreateInstance(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject? errorSuggestion);
}
