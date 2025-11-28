namespace System.Dynamic;

public abstract class GetMemberBinder : DynamicMetaObjectBinder
{
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

	protected GetMemberBinder(string name, bool ignoreCase)
	{
	}

	public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[]? args)
	{
		throw null;
	}

	public DynamicMetaObject FallbackGetMember(DynamicMetaObject target)
	{
		throw null;
	}

	public abstract DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject? errorSuggestion);
}
