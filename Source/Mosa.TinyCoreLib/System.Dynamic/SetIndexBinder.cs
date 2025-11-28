namespace System.Dynamic;

public abstract class SetIndexBinder : DynamicMetaObjectBinder
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

	protected SetIndexBinder(CallInfo callInfo)
	{
	}

	public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
	{
		throw null;
	}

	public DynamicMetaObject FallbackSetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value)
	{
		throw null;
	}

	public abstract DynamicMetaObject FallbackSetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value, DynamicMetaObject? errorSuggestion);
}
