namespace System.Runtime.CompilerServices;

public readonly struct ConfiguredAsyncDisposable
{
	private readonly object _dummy;

	private readonly int _dummyPrimitive;

	public ConfiguredValueTaskAwaitable DisposeAsync()
	{
		throw null;
	}
}
