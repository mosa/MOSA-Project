namespace System.Dynamic;

public abstract class InvokeBinder : DynamicMetaObjectBinder
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

	protected InvokeBinder(CallInfo callInfo)
	{
	}

	public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
	{
		throw null;
	}

	public DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args)
	{
		throw null;
	}

	public abstract DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject? errorSuggestion);
}
