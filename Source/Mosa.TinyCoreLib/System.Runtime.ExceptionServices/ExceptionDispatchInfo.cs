using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.ExceptionServices;

public sealed class ExceptionDispatchInfo
{
	public Exception SourceException
	{
		get
		{
			throw null;
		}
	}

	internal ExceptionDispatchInfo()
	{
	}

	public static ExceptionDispatchInfo Capture(Exception source)
	{
		throw null;
	}

	public static Exception SetCurrentStackTrace(Exception source)
	{
		throw null;
	}

	public static Exception SetRemoteStackTrace(Exception source, string stackTrace)
	{
		throw null;
	}

	[DoesNotReturn]
	public void Throw()
	{
		throw null;
	}

	[DoesNotReturn]
	public static void Throw(Exception source)
	{
		throw null;
	}
}
