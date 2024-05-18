using System.Collections;

namespace System.DirectoryServices.Protocols;

public class DirectoryControlCollection : CollectionBase
{
	public DirectoryControl this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Add(DirectoryControl control)
	{
		throw null;
	}

	public void AddRange(DirectoryControlCollection controlCollection)
	{
	}

	public void AddRange(DirectoryControl[] controls)
	{
	}

	public bool Contains(DirectoryControl value)
	{
		throw null;
	}

	public void CopyTo(DirectoryControl[] array, int index)
	{
	}

	public int IndexOf(DirectoryControl value)
	{
		throw null;
	}

	public void Insert(int index, DirectoryControl value)
	{
	}

	protected override void OnValidate(object value)
	{
	}

	public void Remove(DirectoryControl value)
	{
	}
}
