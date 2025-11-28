namespace System.Threading;

public sealed class PreAllocatedOverlapped : IDisposable
{
	[CLSCompliant(false)]
	public PreAllocatedOverlapped(IOCompletionCallback callback, object? state, object? pinData)
	{
	}

	public void Dispose()
	{
	}

	~PreAllocatedOverlapped()
	{
	}

	[CLSCompliant(false)]
	public static PreAllocatedOverlapped UnsafeCreate(IOCompletionCallback callback, object? state, object? pinData)
	{
		throw null;
	}
}
