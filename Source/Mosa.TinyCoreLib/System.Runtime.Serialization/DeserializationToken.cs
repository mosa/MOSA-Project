namespace System.Runtime.Serialization;

public readonly struct DeserializationToken : IDisposable
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	internal DeserializationToken(object tracker)
	{
		_dummy = null;
		_dummyPrimitive = 0;
	}

	public void Dispose()
	{
	}
}
