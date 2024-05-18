namespace System.ComponentModel.Composition;

public sealed class ExportLifetimeContext<T> : IDisposable
{
	public T Value
	{
		get
		{
			throw null;
		}
	}

	public ExportLifetimeContext(T value, Action disposeAction)
	{
	}

	public void Dispose()
	{
	}
}
