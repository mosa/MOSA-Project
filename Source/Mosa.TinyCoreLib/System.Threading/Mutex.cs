using System.Diagnostics.CodeAnalysis;

namespace System.Threading;

public sealed class Mutex : WaitHandle
{
	public Mutex()
	{
	}

	public Mutex(bool initiallyOwned)
	{
	}

	public Mutex(bool initiallyOwned, string? name)
	{
	}

	public Mutex(bool initiallyOwned, string? name, out bool createdNew)
	{
		throw null;
	}

	public static Mutex OpenExisting(string name)
	{
		throw null;
	}

	public void ReleaseMutex()
	{
	}

	public static bool TryOpenExisting(string name, [NotNullWhen(true)] out Mutex? result)
	{
		throw null;
	}
}
