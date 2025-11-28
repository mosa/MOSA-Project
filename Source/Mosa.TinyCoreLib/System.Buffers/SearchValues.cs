namespace System.Buffers;

public class SearchValues<T> where T : IEquatable<T>?
{
	internal SearchValues()
	{
	}

	public bool Contains(T value)
	{
		throw null;
	}
}
public static class SearchValues
{
	public static SearchValues<byte> Create(ReadOnlySpan<byte> values)
	{
		throw null;
	}

	public static SearchValues<char> Create(ReadOnlySpan<char> values)
	{
		throw null;
	}
}
