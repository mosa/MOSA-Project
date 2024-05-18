using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;

namespace System.Threading;

public static class EventWaitHandleAcl
{
	public static EventWaitHandle Create(bool initialState, EventResetMode mode, string? name, out bool createdNew, EventWaitHandleSecurity? eventSecurity)
	{
		throw null;
	}

	public static EventWaitHandle OpenExisting(string name, EventWaitHandleRights rights)
	{
		throw null;
	}

	public static bool TryOpenExisting(string name, EventWaitHandleRights rights, [NotNullWhen(true)] out EventWaitHandle? result)
	{
		throw null;
	}
}
