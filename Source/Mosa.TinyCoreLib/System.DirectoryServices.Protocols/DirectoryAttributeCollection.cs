using System.Collections;

namespace System.DirectoryServices.Protocols;

public class DirectoryAttributeCollection : CollectionBase
{
	public DirectoryAttribute this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Add(DirectoryAttribute attribute)
	{
		throw null;
	}

	public void AddRange(DirectoryAttributeCollection attributeCollection)
	{
	}

	public void AddRange(DirectoryAttribute[] attributes)
	{
	}

	public bool Contains(DirectoryAttribute value)
	{
		throw null;
	}

	public void CopyTo(DirectoryAttribute[] array, int index)
	{
	}

	public int IndexOf(DirectoryAttribute value)
	{
		throw null;
	}

	public void Insert(int index, DirectoryAttribute value)
	{
	}

	protected override void OnValidate(object value)
	{
	}

	public void Remove(DirectoryAttribute value)
	{
	}
}
