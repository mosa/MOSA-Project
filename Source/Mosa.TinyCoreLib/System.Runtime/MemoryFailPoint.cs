using System.Runtime.ConstrainedExecution;

namespace System.Runtime;

public sealed class MemoryFailPoint : CriticalFinalizerObject, IDisposable
{
	public MemoryFailPoint(int sizeInMegabytes)
	{
	}

	public void Dispose()
	{
	}

	~MemoryFailPoint()
	{
	}
}
