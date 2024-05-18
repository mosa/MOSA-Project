using System.Collections;

namespace System.DirectoryServices.Protocols;

public class DirectoryAttribute : CollectionBase
{
	public object this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DirectoryAttribute()
	{
	}

	public DirectoryAttribute(string name, byte[] value)
	{
	}

	public DirectoryAttribute(string name, params object[] values)
	{
	}

	public DirectoryAttribute(string name, string value)
	{
	}

	public DirectoryAttribute(string name, Uri value)
	{
	}

	public int Add(byte[] value)
	{
		throw null;
	}

	public int Add(string value)
	{
		throw null;
	}

	public int Add(Uri value)
	{
		throw null;
	}

	public void AddRange(object[] values)
	{
	}

	public bool Contains(object value)
	{
		throw null;
	}

	public void CopyTo(object[] array, int index)
	{
	}

	public object[] GetValues(Type valuesType)
	{
		throw null;
	}

	public int IndexOf(object value)
	{
		throw null;
	}

	public void Insert(int index, byte[] value)
	{
	}

	public void Insert(int index, string value)
	{
	}

	public void Insert(int index, Uri value)
	{
	}

	protected override void OnValidate(object value)
	{
	}

	public void Remove(object value)
	{
	}
}
