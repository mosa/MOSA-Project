namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySchemaProperty : IDisposable
{
	public string? CommonName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Description
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsDefunct
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsInAnr
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsIndexed
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsIndexedOverContainer
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsInGlobalCatalog
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsOnTombstonedObject
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsSingleValued
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsTupleIndexed
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ActiveDirectorySchemaProperty? Link
	{
		get
		{
			throw null;
		}
	}

	public int? LinkId
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
	}

	public string? Oid
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int? RangeLower
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int? RangeUpper
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Guid SchemaGuid
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ActiveDirectorySyntax Syntax
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ActiveDirectorySchemaProperty(DirectoryContext context, string ldapDisplayName)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public static ActiveDirectorySchemaProperty FindByName(DirectoryContext context, string ldapDisplayName)
	{
		throw null;
	}

	public DirectoryEntry GetDirectoryEntry()
	{
		throw null;
	}

	public void Save()
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
