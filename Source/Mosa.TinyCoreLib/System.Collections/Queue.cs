namespace System.Collections;

public class Queue : ICollection, IEnumerable, ICloneable
{
	public virtual int Count
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public virtual object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public Queue()
	{
	}

	public Queue(ICollection col)
	{
	}

	public Queue(int capacity)
	{
	}

	public Queue(int capacity, float growFactor)
	{
	}

	public virtual void Clear()
	{
	}

	public virtual object Clone()
	{
		throw null;
	}

	public virtual bool Contains(object? obj)
	{
		throw null;
	}

	public virtual void CopyTo(Array array, int index)
	{
	}

	public virtual object? Dequeue()
	{
		throw null;
	}

	public virtual void Enqueue(object? obj)
	{
	}

	public virtual IEnumerator GetEnumerator()
	{
		throw null;
	}

	public virtual object? Peek()
	{
		throw null;
	}

	public static Queue Synchronized(Queue queue)
	{
		throw null;
	}

	public virtual object?[] ToArray()
	{
		throw null;
	}

	public virtual void TrimToSize()
	{
	}
}
