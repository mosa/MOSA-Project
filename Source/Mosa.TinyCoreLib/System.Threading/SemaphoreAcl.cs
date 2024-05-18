using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;

namespace System.Threading;

public static class SemaphoreAcl
{
	public static Semaphore Create(int initialCount, int maximumCount, string? name, out bool createdNew, SemaphoreSecurity? semaphoreSecurity)
	{
		throw null;
	}

	public static Semaphore OpenExisting(string name, SemaphoreRights rights)
	{
		throw null;
	}

	public static bool TryOpenExisting(string name, SemaphoreRights rights, [NotNullWhen(true)] out Semaphore? result)
	{
		throw null;
	}
}
