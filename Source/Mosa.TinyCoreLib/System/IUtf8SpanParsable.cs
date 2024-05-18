using System.Diagnostics.CodeAnalysis;

namespace System;

public interface IUtf8SpanParsable<TSelf> where TSelf : IUtf8SpanParsable<TSelf>?
{
	static abstract TSelf Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider);

	static abstract bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result);
}
