namespace System.DirectoryServices.ActiveDirectory;

public class ActiveDirectorySchema : ActiveDirectoryPartition
{
	public DirectoryServer SchemaRoleOwner
	{
		get
		{
			throw null;
		}
	}

	internal ActiveDirectorySchema()
	{
	}

	protected override void Dispose(bool disposing)
	{
	}

	public ReadOnlyActiveDirectorySchemaClassCollection FindAllClasses()
	{
		throw null;
	}

	public ReadOnlyActiveDirectorySchemaClassCollection FindAllClasses(SchemaClassType type)
	{
		throw null;
	}

	public ReadOnlyActiveDirectorySchemaClassCollection FindAllDefunctClasses()
	{
		throw null;
	}

	public ReadOnlyActiveDirectorySchemaPropertyCollection FindAllDefunctProperties()
	{
		throw null;
	}

	public ReadOnlyActiveDirectorySchemaPropertyCollection FindAllProperties()
	{
		throw null;
	}

	public ReadOnlyActiveDirectorySchemaPropertyCollection FindAllProperties(PropertyTypes type)
	{
		throw null;
	}

	public ActiveDirectorySchemaClass FindClass(string ldapDisplayName)
	{
		throw null;
	}

	public ActiveDirectorySchemaClass FindDefunctClass(string commonName)
	{
		throw null;
	}

	public ActiveDirectorySchemaProperty FindDefunctProperty(string commonName)
	{
		throw null;
	}

	public ActiveDirectorySchemaProperty FindProperty(string ldapDisplayName)
	{
		throw null;
	}

	public static ActiveDirectorySchema GetCurrentSchema()
	{
		throw null;
	}

	public override DirectoryEntry GetDirectoryEntry()
	{
		throw null;
	}

	public static ActiveDirectorySchema GetSchema(DirectoryContext context)
	{
		throw null;
	}

	public void RefreshSchema()
	{
	}
}
