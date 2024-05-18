using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;

namespace System.Threading;

public static class MutexAcl
{
	public static Mutex Create(bool initiallyOwned, string? name, out bool createdNew, MutexSecurity? mutexSecurity)
	{
		throw null;
	}

	public static Mutex OpenExisting(string name, MutexRights rights)
	{
		throw null;
	}

	public static bool TryOpenExisting(string name, MutexRights rights, [NotNullWhen(true)] out Mutex? result)
	{
		throw null;
	}
}
