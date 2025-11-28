using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectoryReplicationMetadata : DictionaryBase
{
	public ReadOnlyStringCollection AttributeNames
	{
		get
		{
			throw null;
		}
	}

	public AttributeMetadata? this[string name]
	{
		get
		{
			throw null;
		}
	}

	public AttributeMetadataCollection Values
	{
		get
		{
			throw null;
		}
	}

	internal ActiveDirectoryReplicationMetadata()
	{
	}

	public bool Contains(string attributeName)
	{
		throw null;
	}

	public void CopyTo(AttributeMetadata[] array, int index)
	{
	}
}
