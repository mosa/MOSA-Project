using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.Threading;

public sealed class Semaphore : WaitHandle
{
	public Semaphore(int initialCount, int maximumCount)
	{
	}

	public Semaphore(int initialCount, int maximumCount, string? name)
	{
	}

	public Semaphore(int initialCount, int maximumCount, string? name, out bool createdNew)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static Semaphore OpenExisting(string name)
	{
		throw null;
	}

	public int Release()
	{
		throw null;
	}

	public int Release(int releaseCount)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static bool TryOpenExisting(string name, [NotNullWhen(true)] out Semaphore? result)
	{
		throw null;
	}
}
