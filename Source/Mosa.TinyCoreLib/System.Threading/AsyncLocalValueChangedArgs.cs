namespace System.Threading;

public readonly struct AsyncLocalValueChangedArgs<T>
{
	private readonly T _PreviousValue_k__BackingField;

	private readonly T _CurrentValue_k__BackingField;

	private readonly int _dummyPrimitive;

	public T? CurrentValue
	{
		get
		{
			throw null;
		}
	}

	public T? PreviousValue
	{
		get
		{
			throw null;
		}
	}

	public bool ThreadContextChanged
	{
		get
		{
			throw null;
		}
	}
}
