using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.Threading;

public class EventWaitHandle : WaitHandle
{
	public EventWaitHandle(bool initialState, EventResetMode mode)
	{
	}

	public EventWaitHandle(bool initialState, EventResetMode mode, string? name)
	{
	}

	public EventWaitHandle(bool initialState, EventResetMode mode, string? name, out bool createdNew)
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static EventWaitHandle OpenExisting(string name)
	{
		throw null;
	}

	public bool Reset()
	{
		throw null;
	}

	public bool Set()
	{
		throw null;
	}

	[SupportedOSPlatform("windows")]
	public static bool TryOpenExisting(string name, [NotNullWhen(true)] out EventWaitHandle? result)
	{
		throw null;
	}
}
