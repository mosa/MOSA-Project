namespace System;

public interface IUtf8SpanFormattable
{
	bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider);
}
