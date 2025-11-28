using System.Collections;

namespace System.DirectoryServices;

public class DirectoryEntries : IEnumerable
{
	public SchemaNameCollection SchemaFilter
	{
		get
		{
			throw null;
		}
	}

	internal DirectoryEntries()
	{
	}

	public DirectoryEntry Add(string name, string schemaClassName)
	{
		throw null;
	}

	public DirectoryEntry Find(string name)
	{
		throw null;
	}

	public DirectoryEntry Find(string name, string? schemaClassName)
	{
		throw null;
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public void Remove(DirectoryEntry entry)
	{
	}
}
