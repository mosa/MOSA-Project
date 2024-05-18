namespace System;

public class Random
{
	public static Random Shared
	{
		get
		{
			throw null;
		}
	}

	public Random()
	{
	}

	public Random(int Seed)
	{
	}

	public T[] GetItems<T>(ReadOnlySpan<T> choices, int length)
	{
		throw null;
	}

	public void GetItems<T>(ReadOnlySpan<T> choices, Span<T> destination)
	{
	}

	public T[] GetItems<T>(T[] choices, int length)
	{
		throw null;
	}

	public virtual int Next()
	{
		throw null;
	}

	public virtual int Next(int maxValue)
	{
		throw null;
	}

	public virtual int Next(int minValue, int maxValue)
	{
		throw null;
	}

	public virtual void NextBytes(byte[] buffer)
	{
	}

	public virtual void NextBytes(Span<byte> buffer)
	{
	}

	public virtual double NextDouble()
	{
		throw null;
	}

	public virtual long NextInt64()
	{
		throw null;
	}

	public virtual long NextInt64(long maxValue)
	{
		throw null;
	}

	public virtual long NextInt64(long minValue, long maxValue)
	{
		throw null;
	}

	public virtual float NextSingle()
	{
		throw null;
	}

	protected virtual double Sample()
	{
		throw null;
	}

	public void Shuffle<T>(Span<T> values)
	{
	}

	public void Shuffle<T>(T[] values)
	{
	}
}
