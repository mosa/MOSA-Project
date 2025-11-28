namespace System.Dynamic;

public abstract class SetMemberBinder : DynamicMetaObjectBinder
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

	protected SetMemberBinder(string name, bool ignoreCase)
	{
	}

	public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
	{
		throw null;
	}

	public DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value)
	{
		throw null;
	}

	public abstract DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject? errorSuggestion);
}
