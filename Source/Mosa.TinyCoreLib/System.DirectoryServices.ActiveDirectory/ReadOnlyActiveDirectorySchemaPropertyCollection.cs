using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ReadOnlyActiveDirectorySchemaPropertyCollection : ReadOnlyCollectionBase
{
	public ActiveDirectorySchemaProperty this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal ReadOnlyActiveDirectorySchemaPropertyCollection()
	{
	}

	public bool Contains(ActiveDirectorySchemaProperty schemaProperty)
	{
		throw null;
	}

	public void CopyTo(ActiveDirectorySchemaProperty[] properties, int index)
	{
	}

	public int IndexOf(ActiveDirectorySchemaProperty schemaProperty)
	{
		throw null;
	}
}
