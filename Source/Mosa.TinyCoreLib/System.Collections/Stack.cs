namespace System.Collections;

public class Stack : ICollection, IEnumerable, ICloneable
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

	public Stack()
	{
	}

	public Stack(ICollection col)
	{
	}

	public Stack(int initialCapacity)
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

	public virtual IEnumerator GetEnumerator()
	{
		throw null;
	}

	public virtual object? Peek()
	{
		throw null;
	}

	public virtual object? Pop()
	{
		throw null;
	}

	public virtual void Push(object? obj)
	{
	}

	public static Stack Synchronized(Stack stack)
	{
		throw null;
	}

	public virtual object?[] ToArray()
	{
		throw null;
	}
}
