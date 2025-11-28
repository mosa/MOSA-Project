namespace System.Runtime.Serialization;

public readonly struct SerializationEntry
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public Type ObjectType
	{
		get
		{
			throw null;
		}
	}

	public object? Value
	{
		get
		{
			throw null;
		}
	}
}
