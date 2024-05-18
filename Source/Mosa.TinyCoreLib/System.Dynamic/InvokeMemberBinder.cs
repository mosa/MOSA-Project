namespace System.Dynamic;

public abstract class InvokeMemberBinder : DynamicMetaObjectBinder
{
	public CallInfo CallInfo
	{
		get
		{
			throw null;
		}
	}

	public bool IgnoreCase
	{
		get
		{
			throw null;
		}
	}

	public string Name
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

	protected InvokeMemberBinder(string name, bool ignoreCase, CallInfo callInfo)
	{
	}

	public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
	{
		throw null;
	}

	public abstract DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject? errorSuggestion);

	public DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args)
	{
		throw null;
	}

	public abstract DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject? errorSuggestion);
}
