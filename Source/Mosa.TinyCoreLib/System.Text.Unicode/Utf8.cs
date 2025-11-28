using System.Buffers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Text.Unicode;

public static class Utf8
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[InterpolatedStringHandler]
	public ref struct TryWriteInterpolatedStringHandler
	{
		private readonly object _dummy;

		private readonly int _dummyPrimitive;

		public TryWriteInterpolatedStringHandler(int literalLength, int formattedCount, Span<byte> destination, out bool shouldAppend)
		{
			throw null;
		}

		public TryWriteInterpolatedStringHandler(int literalLength, int formattedCount, Span<byte> destination, IFormatProvider? provider, out bool shouldAppend)
		{
			throw null;
		}

		public bool AppendLiteral(string value)
		{
			throw null;
		}

		public bool AppendFormatted(scoped ReadOnlySpan<char> value)
		{
			throw null;
		}

		public bool AppendFormatted(scoped ReadOnlySpan<char> value, int alignment = 0, string? format = null)
		{
			throw null;
		}

		public bool AppendFormatted(scoped ReadOnlySpan<byte> utf8Value)
		{
			throw null;
		}

		public bool AppendFormatted(scoped ReadOnlySpan<byte> utf8Value, int alignment = 0, string? format = null)
		{
			throw null;
		}

		public bool AppendFormatted<T>(T value)
		{
			throw null;
		}

		public bool AppendFormatted<T>(T value, string? format)
		{
			throw null;
		}

		public bool AppendFormatted<T>(T value, int alignment)
		{
			throw null;
		}

		public bool AppendFormatted<T>(T value, int alignment, string? format)
		{
			throw null;
		}

		public bool AppendFormatted(object? value, int alignment = 0, string? format = null)
		{
			throw null;
		}

		public bool AppendFormatted(string? value)
		{
			throw null;
		}

		public bool AppendFormatted(string? value, int alignment = 0, string? format = null)
		{
			throw null;
		}
	}

	public static OperationStatus FromUtf16(ReadOnlySpan<char> source, Span<byte> destination, out int charsRead, out int bytesWritten, bool replaceInvalidSequences = true, bool isFinalBlock = true)
	{
		throw null;
	}

	public static OperationStatus ToUtf16(ReadOnlySpan<byte> source, Span<char> destination, out int bytesRead, out int charsWritten, bool replaceInvalidSequences = true, bool isFinalBlock = true)
	{
		throw null;
	}

	public static bool TryWrite(Span<byte> destination, [InterpolatedStringHandlerArgument("destination")] ref TryWriteInterpolatedStringHandler handler, out int bytesWritten)
	{
		throw null;
	}

	public static bool TryWrite(Span<byte> destination, IFormatProvider? provider, [InterpolatedStringHandlerArgument(new string[] { "destination", "provider" })] ref TryWriteInterpolatedStringHandler handler, out int bytesWritten)
	{
		throw null;
	}

	public static bool IsValid(ReadOnlySpan<byte> value)
	{
		throw null;
	}
}
