namespace System.Runtime;

public struct DependentHandle : IDisposable
{
	private object _dummy;

	private int _dummyPrimitive;

	public object? Dependent
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsAllocated
	{
		get
		{
			throw null;
		}
	}

	public object? Target
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public (object? Target, object? Dependent) TargetAndDependent
	{
		get
		{
			throw null;
		}
	}

	public DependentHandle(object? target, object? dependent)
	{
		throw null;
	}

	public void Dispose()
	{
	}
}
