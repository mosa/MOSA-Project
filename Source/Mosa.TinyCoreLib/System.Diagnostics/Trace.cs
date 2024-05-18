using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics;

public sealed class Trace
{
	public static bool AutoFlush
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static CorrelationManager CorrelationManager
	{
		get
		{
			throw null;
		}
	}

	public static int IndentLevel
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static int IndentSize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static TraceListenerCollection Listeners
	{
		get
		{
			throw null;
		}
	}

	public static bool UseGlobalLock
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static event EventHandler? Refreshing
	{
		add
		{
		}
		remove
		{
		}
	}

	internal Trace()
	{
	}

	[Conditional("TRACE")]
	public static void Assert([DoesNotReturnIf(false)] bool condition)
	{
	}

	[Conditional("TRACE")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, string? message)
	{
	}

	[Conditional("TRACE")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, string? message, string? detailMessage)
	{
	}

	[Conditional("TRACE")]
	public static void Close()
	{
	}

	[Conditional("TRACE")]
	[DoesNotReturn]
	public static void Fail(string? message)
	{
		throw null;
	}

	[Conditional("TRACE")]
	[DoesNotReturn]
	public static void Fail(string? message, string? detailMessage)
	{
		throw null;
	}

	[Conditional("TRACE")]
	public static void Flush()
	{
	}

	[Conditional("TRACE")]
	public static void Indent()
	{
	}

	public static void Refresh()
	{
	}

	[Conditional("TRACE")]
	public static void TraceError(string? message)
	{
	}

	[Conditional("TRACE")]
	public static void TraceError([StringSyntax("CompositeFormat")] string format, params object?[]? args)
	{
	}

	[Conditional("TRACE")]
	public static void TraceInformation(string? message)
	{
	}

	[Conditional("TRACE")]
	public static void TraceInformation([StringSyntax("CompositeFormat")] string format, params object?[]? args)
	{
	}

	[Conditional("TRACE")]
	public static void TraceWarning(string? message)
	{
	}

	[Conditional("TRACE")]
	public static void TraceWarning([StringSyntax("CompositeFormat")] string format, params object?[]? args)
	{
	}

	[Conditional("TRACE")]
	public static void Unindent()
	{
	}

	[Conditional("TRACE")]
	public static void Write(object? value)
	{
	}

	[Conditional("TRACE")]
	public static void Write(object? value, string? category)
	{
	}

	[Conditional("TRACE")]
	public static void Write(string? message)
	{
	}

	[Conditional("TRACE")]
	public static void Write(string? message, string? category)
	{
	}

	[Conditional("TRACE")]
	public static void WriteIf(bool condition, object? value)
	{
	}

	[Conditional("TRACE")]
	public static void WriteIf(bool condition, object? value, string? category)
	{
	}

	[Conditional("TRACE")]
	public static void WriteIf(bool condition, string? message)
	{
	}

	[Conditional("TRACE")]
	public static void WriteIf(bool condition, string? message, string? category)
	{
	}

	[Conditional("TRACE")]
	public static void WriteLine(object? value)
	{
	}

	[Conditional("TRACE")]
	public static void WriteLine(object? value, string? category)
	{
	}

	[Conditional("TRACE")]
	public static void WriteLine(string? message)
	{
	}

	[Conditional("TRACE")]
	public static void WriteLine(string? message, string? category)
	{
	}

	[Conditional("TRACE")]
	public static void WriteLineIf(bool condition, object? value)
	{
	}

	[Conditional("TRACE")]
	public static void WriteLineIf(bool condition, object? value, string? category)
	{
	}

	[Conditional("TRACE")]
	public static void WriteLineIf(bool condition, string? message)
	{
	}

	[Conditional("TRACE")]
	public static void WriteLineIf(bool condition, string? message, string? category)
	{
	}
}
