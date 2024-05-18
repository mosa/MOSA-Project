namespace System.Buffers;

public abstract class ArrayPool<T>
{
	public static ArrayPool<T> Shared
	{
		get
		{
			throw null;
		}
	}

	public static ArrayPool<T> Create()
	{
		throw null;
	}

	public static ArrayPool<T> Create(int maxArrayLength, int maxArraysPerBucket)
	{
		throw null;
	}

	public abstract T[] Rent(int minimumLength);

	public abstract void Return(T[] array, bool clearArray = false);
}
