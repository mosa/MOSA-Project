using System.Collections;

namespace System.Data;

public sealed class DataRowCollection : InternalDataCollectionBase
{
	public override int Count
	{
		get
		{
			throw null;
		}
	}

	public DataRow this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal DataRowCollection()
	{
	}

	public void Add(DataRow row)
	{
	}

	public DataRow Add(params object?[] values)
	{
		throw null;
	}

	public void Clear()
	{
	}

	public bool Contains(object? key)
	{
		throw null;
	}

	public bool Contains(object?[] keys)
	{
		throw null;
	}

	public override void CopyTo(Array ar, int index)
	{
	}

	public void CopyTo(DataRow[] array, int index)
	{
	}

	public DataRow? Find(object? key)
	{
		throw null;
	}

	public DataRow? Find(object?[] keys)
	{
		throw null;
	}

	public override IEnumerator GetEnumerator()
	{
		throw null;
	}

	public int IndexOf(DataRow? row)
	{
		throw null;
	}

	public void InsertAt(DataRow row, int pos)
	{
	}

	public void Remove(DataRow row)
	{
	}

	public void RemoveAt(int index)
	{
	}
}
