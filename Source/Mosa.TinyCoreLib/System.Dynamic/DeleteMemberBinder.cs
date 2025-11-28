namespace System.Dynamic;

public abstract class DeleteMemberBinder : DynamicMetaObjectBinder
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

	protected DeleteMemberBinder(string name, bool ignoreCase)
	{
	}

	public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[]? args)
	{
		throw null;
	}

	public DynamicMetaObject FallbackDeleteMember(DynamicMetaObject target)
	{
		throw null;
	}

	public abstract DynamicMetaObject FallbackDeleteMember(DynamicMetaObject target, DynamicMetaObject? errorSuggestion);
}
