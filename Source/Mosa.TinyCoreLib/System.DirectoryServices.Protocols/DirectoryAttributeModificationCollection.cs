using System.Collections;

namespace System.DirectoryServices.Protocols;

public class DirectoryAttributeModificationCollection : CollectionBase
{
	public DirectoryAttributeModification this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int Add(DirectoryAttributeModification attribute)
	{
		throw null;
	}

	public void AddRange(DirectoryAttributeModificationCollection attributeCollection)
	{
	}

	public void AddRange(DirectoryAttributeModification[] attributes)
	{
	}

	public bool Contains(DirectoryAttributeModification value)
	{
		throw null;
	}

	public void CopyTo(DirectoryAttributeModification[] array, int index)
	{
	}

	public int IndexOf(DirectoryAttributeModification value)
	{
		throw null;
	}

	public void Insert(int index, DirectoryAttributeModification value)
	{
	}

	protected override void OnValidate(object value)
	{
	}

	public void Remove(DirectoryAttributeModification value)
	{
	}
}
