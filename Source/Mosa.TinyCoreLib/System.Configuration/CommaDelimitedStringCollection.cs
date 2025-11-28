using System.Collections.Specialized;

namespace System.Configuration;

public sealed class CommaDelimitedStringCollection : StringCollection
{
	public bool IsModified
	{
		get
		{
			throw null;
		}
	}

	public new bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public new string this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public new void Add(string value)
	{
	}

	public new void AddRange(string[] range)
	{
	}

	public new void Clear()
	{
	}

	public CommaDelimitedStringCollection Clone()
	{
		throw null;
	}

	public new void Insert(int index, string value)
	{
	}

	public new void Remove(string value)
	{
	}

	public void SetReadOnly()
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
