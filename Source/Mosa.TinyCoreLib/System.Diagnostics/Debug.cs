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
		public AssertInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, out bool shouldAppend)
			=> throw new NotImplementedException();

		public void AppendFormatted(object? value, int alignment = 0, string? format = null)
			=> throw new NotImplementedException();

		public void AppendFormatted(ReadOnlySpan<char> value) => throw new NotImplementedException();

		public void AppendFormatted(ReadOnlySpan<char> value, int alignment = 0, string? format = null)
			=> throw new NotImplementedException();

		public void AppendFormatted(string? value) => throw new NotImplementedException();

		public void AppendFormatted(string? value, int alignment = 0, string? format = null)
			=> throw new NotImplementedException();

		public void AppendFormatted<T>(T value) => throw new NotImplementedException();

		public void AppendFormatted<T>(T value, int alignment) => throw new NotImplementedException();

		public void AppendFormatted<T>(T value, int alignment, string? format) => throw new NotImplementedException();

		public void AppendFormatted<T>(T value, string? format) => throw new NotImplementedException();

		public void AppendLiteral(string value) => throw new NotImplementedException();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[InterpolatedStringHandler]
	public struct WriteIfInterpolatedStringHandler
	{
		public WriteIfInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, out bool shouldAppend)
			=> throw new NotImplementedException();

		public void AppendFormatted(object? value, int alignment = 0, string? format = null)
			=> throw new NotImplementedException();

		public void AppendFormatted(ReadOnlySpan<char> value) => throw new NotImplementedException();

		public void AppendFormatted(ReadOnlySpan<char> value, int alignment = 0, string? format = null)
			=> throw new NotImplementedException();

		public void AppendFormatted(string? value) => throw new NotImplementedException();

		public void AppendFormatted(string? value, int alignment = 0, string? format = null)
			=> throw new NotImplementedException();

		public void AppendFormatted<T>(T value) => throw new NotImplementedException();

		public void AppendFormatted<T>(T value, int alignment) => throw new NotImplementedException();

		public void AppendFormatted<T>(T value, int alignment, string? format) => throw new NotImplementedException();

		public void AppendFormatted<T>(T value, string? format) => throw new NotImplementedException();

		public void AppendLiteral(string value) => throw new NotImplementedException();
	}

	public static bool AutoFlush { get; set; }

	public static int IndentLevel { get; set; }

	public static int IndentSize { get; set; } = 2;

	public static DebugProvider SetProvider(DebugProvider provider) => throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition)
		=> Assert(condition, null, null);

	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, [InterpolatedStringHandlerArgument("condition")] ref AssertInterpolatedStringHandler message)
		=> throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, [InterpolatedStringHandlerArgument("condition")] ref AssertInterpolatedStringHandler message, [InterpolatedStringHandlerArgument("condition")] ref AssertInterpolatedStringHandler detailMessage)
		=> throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, string? message)
		=> Assert(condition, message, null);

	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, string? message, string? detailMessage)
	{
		if (!condition)
			Fail(message, detailMessage);
	}

	[Conditional("DEBUG")]
	public static void Assert([DoesNotReturnIf(false)] bool condition, string? message, [StringSyntax("CompositeFormat")] string detailMessageFormat, params object?[] args)
		=> throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void Close() => throw new NotImplementedException();

	[DoesNotReturn]
	[Conditional("DEBUG")]
	public static void Fail(string? message) => Fail(message, null);

	[DoesNotReturn]
	[Conditional("DEBUG")]
	public static void Fail(string? message, string? detailMessage)
		=> Environment.FailFast(string.Format(
			Internal.Impl.Debug.FailText,
			message ?? Internal.Impl.Debug.NoMessage,
			detailMessage ?? Internal.Impl.Debug.NoDetails));

	[Conditional("DEBUG")]
	public static void Flush() => throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void Indent() => IndentLevel++;

	[Conditional("DEBUG")]
	public static void Print(string? message) => Write(message);

	[Conditional("DEBUG")]
	public static void Print([StringSyntax("CompositeFormat")] string format, params object?[] args)
		=> throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void Unindent()
		=> IndentLevel--;

	[Conditional("DEBUG")]
	public static void Write(object? value) => Write(value, string.Empty);

	[Conditional("DEBUG")]
	public static void Write(object? value, string? category)
		=> Write(string.Format(Internal.Impl.Debug.WriteText,
			category ?? Internal.Impl.Debug.NoCategory,
			value ?? Internal.Impl.Debug.NoValue));

	[Conditional("DEBUG")]
	public static void Write(string? message) => Write(message, "");

	[Conditional("DEBUG")]
	public static void Write(string? message, string? category)
		=> Write(string.Format(Internal.Impl.Debug.WriteText,
			category ?? Internal.Impl.Debug.NoCategory,
			message ?? Internal.Impl.Debug.NoMessage));

	[Conditional("DEBUG")]
	public static void WriteIf(bool condition, [InterpolatedStringHandlerArgument("condition")] ref WriteIfInterpolatedStringHandler message)
		=> throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void WriteIf(bool condition, [InterpolatedStringHandlerArgument("condition")] ref WriteIfInterpolatedStringHandler message, string? category)
		=> throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void WriteIf(bool condition, object? value)
	{
		if (condition)
			Write(value, null);
	}

	[Conditional("DEBUG")]
	public static void WriteIf(bool condition, object? value, string? category)
	{
		if (condition)
			Write(value, category);
	}

	[Conditional("DEBUG")]
	public static void WriteIf(bool condition, string? message)
	{
		if (condition)
			Write(message, null);
	}

	[Conditional("DEBUG")]
	public static void WriteIf(bool condition, string? message, string? category)
	{
		if (condition)
			Write(message, category);
	}

	[Conditional("DEBUG")]
	public static void WriteLine(object? value) => throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void WriteLine(object? value, string? category)
		=> WriteLine(Internal.Impl.Debug.WriteText,
			category ?? Internal.Impl.Debug.NoCategory,
			value ?? Internal.Impl.Debug.NoValue);

	[Conditional("DEBUG")]
	public static void WriteLine(string? message) => throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void WriteLine([StringSyntax("CompositeFormat")] string format, params object?[] args)
		=> throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void WriteLine(string? message, string? category)
		=> WriteLine(Internal.Impl.Debug.WriteText,
			category ?? Internal.Impl.Debug.NoCategory,
			message ?? Internal.Impl.Debug.NoMessage);

	[Conditional("DEBUG")]
	public static void WriteLineIf(bool condition, [InterpolatedStringHandlerArgument("condition")] ref WriteIfInterpolatedStringHandler message) =>
		throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void WriteLineIf(bool condition, [InterpolatedStringHandlerArgument("condition")] ref WriteIfInterpolatedStringHandler message, string? category)
		=> throw new NotImplementedException();

	[Conditional("DEBUG")]
	public static void WriteLineIf(bool condition, object? value)
	{
		if (condition)
			WriteLine(value);
	}

	[Conditional("DEBUG")]
	public static void WriteLineIf(bool condition, object? value, string? category)
	{
		if (condition)
			WriteLine(value, category);
	}

	[Conditional("DEBUG")]
	public static void WriteLineIf(bool condition, string? message)
	{
		if (condition)
			WriteLine(message);
	}

	[Conditional("DEBUG")]
	public static void WriteLineIf(bool condition, string? message, string? category)
	{
		if (condition)
			WriteLine(message, category);
	}
}
