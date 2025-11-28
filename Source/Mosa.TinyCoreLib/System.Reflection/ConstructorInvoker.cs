namespace System.Reflection;

public sealed class ConstructorInvoker
{
	internal ConstructorInvoker()
	{
	}

	public object Invoke(Span<object?> arguments)
	{
		throw null;
	}

	public object Invoke()
	{
		throw null;
	}

	public object Invoke(object? arg1)
	{
		throw null;
	}

	public object Invoke(object? arg1, object? arg2)
	{
		throw null;
	}

	public object Invoke(object? arg1, object? arg2, object? arg3)
	{
		throw null;
	}

	public object Invoke(object? arg1, object? arg2, object? arg3, object? arg4)
	{
		throw null;
	}

	public static ConstructorInvoker Create(ConstructorInfo constructor)
	{
		throw null;
	}
}
