namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySchemaClass : IDisposable
{
	public ActiveDirectorySchemaClassCollection AuxiliaryClasses
	{
		get
		{
			throw null;
		}
	}

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

	public ActiveDirectorySecurity? DefaultObjectSecurityDescriptor
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

	public ActiveDirectorySchemaPropertyCollection MandatoryProperties
	{
		get
		{
			throw null;
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

	public ActiveDirectorySchemaPropertyCollection OptionalProperties
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyActiveDirectorySchemaClassCollection PossibleInferiors
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectorySchemaClassCollection PossibleSuperiors
	{
		get
		{
			throw null;
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

	public ActiveDirectorySchemaClass? SubClassOf
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SchemaClassType Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ActiveDirectorySchemaClass(DirectoryContext context, string ldapDisplayName)
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public static ActiveDirectorySchemaClass FindByName(DirectoryContext context, string ldapDisplayName)
	{
		throw null;
	}

	public ReadOnlyActiveDirectorySchemaPropertyCollection GetAllProperties()
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
