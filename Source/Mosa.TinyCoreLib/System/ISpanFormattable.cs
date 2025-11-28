namespace System;

public interface ISpanFormattable : IFormattable
{
	bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider);
}
