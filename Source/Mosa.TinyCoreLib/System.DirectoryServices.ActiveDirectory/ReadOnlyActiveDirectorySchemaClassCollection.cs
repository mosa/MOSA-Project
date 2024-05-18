using System.Collections;

namespace System.DirectoryServices.ActiveDirectory;

public class ReadOnlyActiveDirectorySchemaClassCollection : ReadOnlyCollectionBase
{
	public ActiveDirectorySchemaClass this[int index]
	{
		get
		{
			throw null;
		}
	}

	internal ReadOnlyActiveDirectorySchemaClassCollection()
	{
	}

	public bool Contains(ActiveDirectorySchemaClass schemaClass)
	{
		throw null;
	}

	public void CopyTo(ActiveDirectorySchemaClass[] classes, int index)
	{
	}

	public int IndexOf(ActiveDirectorySchemaClass schemaClass)
	{
		throw null;
	}
}
