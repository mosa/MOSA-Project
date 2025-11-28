namespace System.Runtime.CompilerServices;

public class CallSite
{
	public CallSiteBinder? Binder
	{
		get
		{
			throw null;
		}
	}

	internal CallSite()
	{
	}

	public static CallSite Create(Type delegateType, CallSiteBinder binder)
	{
		throw null;
	}
}
public class CallSite<T> : CallSite where T : class
{
	public T Target;

	public T Update
	{
		get
		{
			throw null;
		}
	}

	internal CallSite()
	{
	}

	public static CallSite<T> Create(CallSiteBinder binder)
	{
		throw null;
	}
}
