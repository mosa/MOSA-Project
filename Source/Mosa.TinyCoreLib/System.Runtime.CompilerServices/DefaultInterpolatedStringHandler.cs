namespace System.Runtime.CompilerServices;

[InterpolatedStringHandler]
public ref struct DefaultInterpolatedStringHandler
{
	private object _dummy;

	private int _dummyPrimitive;

	public DefaultInterpolatedStringHandler(int literalLength, int formattedCount)
	{
		throw null;
	}

	public DefaultInterpolatedStringHandler(int literalLength, int formattedCount, IFormatProvider? provider)
	{
		throw null;
	}

	public DefaultInterpolatedStringHandler(int literalLength, int formattedCount, IFormatProvider? provider, Span<char> initialBuffer)
	{
		throw null;
	}

	public void AppendFormatted(object? value, int alignment = 0, string? format = null)
	{
	}

	public void AppendFormatted(scoped ReadOnlySpan<char> value)
	{
	}

	public void AppendFormatted(scoped ReadOnlySpan<char> value, int alignment = 0, string? format = null)
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

	public override string ToString()
	{
		throw null;
	}

	public string ToStringAndClear()
	{
		throw null;
	}
}
