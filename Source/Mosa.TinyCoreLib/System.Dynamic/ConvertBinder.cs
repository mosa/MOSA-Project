namespace System.Dynamic;

public abstract class ConvertBinder : DynamicMetaObjectBinder
{
	public bool Explicit
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

	public Type Type
	{
		get
		{
			throw null;
		}
	}

	protected ConvertBinder(Type type, bool @explicit)
	{
	}

	public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[]? args)
	{
		throw null;
	}

	public DynamicMetaObject FallbackConvert(DynamicMetaObject target)
	{
		throw null;
	}

	public abstract DynamicMetaObject FallbackConvert(DynamicMetaObject target, DynamicMetaObject? errorSuggestion);
}
