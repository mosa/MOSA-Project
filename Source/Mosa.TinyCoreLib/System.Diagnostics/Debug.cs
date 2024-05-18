using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Diagnostics;

public static class Debug
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[InterpolatedStringHandler]
	public struct AssertInterpolatedStringHandler
	{
		private object _dummy;

		private int _dummyPrimitive;

		public AssertInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, out bool shouldAppend)
		{
			throw null;
		}

		public void AppendFormatted(object? value, int alignment = 0, string? format = null)
		{
		}

		public void AppendFormatted(ReadOnlySpan<char> value)
		{
		}

		public void AppendFormatted(ReadOnlySpan<char> value, int alignment = 0, string? format = null)
		{
		}

		public void AppendFormatted(string? value)
		{
		}

		public void AppendFormatted(string? value, int alignment = 0, string? format = null)
		{
		}

		public void AppendFormatted<T>(T value)
		{
		}

		public void AppendFormatted<T>(T value, int alignment)
		{
		}

		public void AppendFormatted<T>(T value, int alignment, string? format)
		{
		}

		public void AppendFormatted<T>(T value, string? format)
		{
		}

		public void AppendLiteral(string value)
		{
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[InterpolatedStringHandler]
	public struct WriteIfInterpolatedStringHandler
	{
		private object _dummy;

		private int _dummyPrimitive;

		public WriteIfInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, out bool shouldAppend)
		{
			throw null;
		}

		public void AppendFormatted(object? value, int alignment = 0, string? format = null)
		{
		}

		public void AppendFormatted(ReadOnlySpan<char> value)
		{
		}

		public void AppendFormatted(ReadOnlySpan<char> value, int alignment = 0, string? format = null)
		{
		}

		public void AppendFormatted(string? value)
		{
		}

		public void AppendFormatted(string? value, int alignment = 0, string? format = null)
		{
		}

		public void AppendFormatted<T>(T value)
		{
		}

		public void AppendFormatted<T>(T value, int alignment)
		{
		}

		public void AppendFormatted<T>(T value, int alignment, string? format)
		{
		}

		public void AppendFormatted<T>(T value, string? format)
		{
		}

		public void AppendLiteral(string value)
		{
		}
	}

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

	public static DebugProvider SetProvider(DebugProvider provider)
	{
		throw null;
	}

	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition)
	{
	}

	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, [InterpolatedStringHandlerArgument("condition")] ref AssertInterpolatedStringHandler message)
	{
	}

	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, [InterpolatedStringHandlerArgument("condition")] ref AssertInterpolatedStringHandler message, [InterpolatedStringHandlerArgument("condition")] ref AssertInterpolatedStringHandler detailMessage)
	{
	}

	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, string? message)
	{
	}

	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, string? message, string? detailMessage)
	{
	}

	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, string? message, [StringSyntax("CompositeFormat")] string detailMessageFormat, params object?[] args)
	{
	}

	[Conditional("DEBUG")]
	public static void Close()
	{
	}

	[DoesNotReturn]
	[Conditional("DEBUG")]
	public static void Fail(string? message)
	{
		throw null;
	}

	[DoesNotReturn]
	[Conditional("DEBUG")]
	public static void Fail(string? message, string? detailMessage)
	{
		throw null;
	}

	[Conditional("DEBUG")]
	public static void Flush()
	{
	}

	[Conditional("DEBUG")]
	public static void Indent()
	{
	}

	[Conditional("DEBUG")]
	public static void Print(string? message)
	{
	}

	[Conditional("DEBUG")]
	public static void Print([StringSyntax("CompositeFormat")] string format, params object?[] args)
	{
	}

	[Conditional("DEBUG")]
	public static void Unindent()
	{
	}

	[Conditional("DEBUG")]
	public static void Write(object? value)
	{
	}

	[Conditional("DEBUG")]
	public static void Write(object? value, string? category)
	{
	}

	[Conditional("DEBUG")]
	public static void Write(string? message)
	{
	}

	[Conditional("DEBUG")]
	public static void Write(string? message, string? category)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteIf(bool condition, [InterpolatedStringHandlerArgument("condition")] ref WriteIfInterpolatedStringHandler message)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteIf(bool condition, [InterpolatedStringHandlerArgument("condition")] ref WriteIfInterpolatedStringHandler message, string? category)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteIf(bool condition, object? value)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteIf(bool condition, object? value, string? category)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteIf(bool condition, string? message)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteIf(bool condition, string? message, string? category)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteLine(object? value)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteLine(object? value, string? category)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteLine(string? message)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteLine([StringSyntax("CompositeFormat")] string format, params object?[] args)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteLine(string? message, string? category)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteLineIf(bool condition, [InterpolatedStringHandlerArgument("condition")] ref WriteIfInterpolatedStringHandler message)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteLineIf(bool condition, [InterpolatedStringHandlerArgument("condition")] ref WriteIfInterpolatedStringHandler message, string? category)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteLineIf(bool condition, object? value)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteLineIf(bool condition, object? value, string? category)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteLineIf(bool condition, string? message)
	{
	}

	[Conditional("DEBUG")]
	public static void WriteLineIf(bool condition, string? message, string? category)
	{
	}
}
