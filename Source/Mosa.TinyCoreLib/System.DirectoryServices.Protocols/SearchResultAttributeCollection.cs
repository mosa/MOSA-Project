using System.Collections;

namespace System.DirectoryServices.Protocols;

public class SearchResultAttributeCollection : DictionaryBase
{
	public ICollection AttributeNames
	{
		get
		{
			throw null;
		}
	}

	public DirectoryAttribute this[string attributeName]
	{
		get
		{
			throw null;
		}
	}

	public ICollection Values
	{
		get
		{
			throw null;
		}
	}

	internal SearchResultAttributeCollection()
	{
	}

	public bool Contains(string attributeName)
	{
		throw null;
	}

	public void CopyTo(DirectoryAttribute[] array, int index)
	{
	}
}
