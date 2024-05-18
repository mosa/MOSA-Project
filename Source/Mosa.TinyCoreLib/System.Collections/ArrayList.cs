using System.Diagnostics.CodeAnalysis;

namespace System.Collections;

public class ArrayList : ICollection, IEnumerable, IList, ICloneable
{
	public virtual int Capacity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual int Count
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsFixedSize
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsReadOnly
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

	public virtual object? this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public ArrayList()
	{
	}

	public ArrayList(ICollection c)
	{
	}

	public ArrayList(int capacity)
	{
	}

	public static ArrayList Adapter(IList list)
	{
		throw null;
	}

	public virtual int Add(object? value)
	{
		throw null;
	}

	public virtual void AddRange(ICollection c)
	{
	}

	public virtual int BinarySearch(int index, int count, object? value, IComparer? comparer)
	{
		throw null;
	}

	public virtual int BinarySearch(object? value)
	{
		throw null;
	}

	public virtual int BinarySearch(object? value, IComparer? comparer)
	{
		throw null;
	}

	public virtual void Clear()
	{
	}

	public virtual object Clone()
	{
		throw null;
	}

	public virtual bool Contains(object? item)
	{
		throw null;
	}

	public virtual void CopyTo(Array array)
	{
	}

	public virtual void CopyTo(Array array, int arrayIndex)
	{
	}

	public virtual void CopyTo(int index, Array array, int arrayIndex, int count)
	{
	}

	public static ArrayList FixedSize(ArrayList list)
	{
		throw null;
	}

	public static IList FixedSize(IList list)
	{
		throw null;
	}

	public virtual IEnumerator GetEnumerator()
	{
		throw null;
	}

	public virtual IEnumerator GetEnumerator(int index, int count)
	{
		throw null;
	}

	public virtual ArrayList GetRange(int index, int count)
	{
		throw null;
	}

	public virtual int IndexOf(object? value)
	{
		throw null;
	}

	public virtual int IndexOf(object? value, int startIndex)
	{
		throw null;
	}

	public virtual int IndexOf(object? value, int startIndex, int count)
	{
		throw null;
	}

	public virtual void Insert(int index, object? value)
	{
	}

	public virtual void InsertRange(int index, ICollection c)
	{
	}

	public virtual int LastIndexOf(object? value)
	{
		throw null;
	}

	public virtual int LastIndexOf(object? value, int startIndex)
	{
		throw null;
	}

	public virtual int LastIndexOf(object? value, int startIndex, int count)
	{
		throw null;
	}

	public static ArrayList ReadOnly(ArrayList list)
	{
		throw null;
	}

	public static IList ReadOnly(IList list)
	{
		throw null;
	}

	public virtual void Remove(object? obj)
	{
	}

	public virtual void RemoveAt(int index)
	{
	}

	public virtual void RemoveRange(int index, int count)
	{
	}

	public static ArrayList Repeat(object? value, int count)
	{
		throw null;
	}

	public virtual void Reverse()
	{
	}

	public virtual void Reverse(int index, int count)
	{
	}

	public virtual void SetRange(int index, ICollection c)
	{
	}

	public virtual void Sort()
	{
	}

	public virtual void Sort(IComparer? comparer)
	{
	}

	public virtual void Sort(int index, int count, IComparer? comparer)
	{
	}

	public static ArrayList Synchronized(ArrayList list)
	{
		throw null;
	}

	public static IList Synchronized(IList list)
	{
		throw null;
	}

	public virtual object?[] ToArray()
	{
		throw null;
	}

	[RequiresDynamicCode("The code for an array of the specified type might not be available.")]
	public virtual Array ToArray(Type type)
	{
		throw null;
	}

	public virtual void TrimToSize()
	{
	}
}
