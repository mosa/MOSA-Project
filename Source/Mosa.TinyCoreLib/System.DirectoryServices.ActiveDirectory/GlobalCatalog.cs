namespace System.DirectoryServices.ActiveDirectory;

public class GlobalCatalog : DomainController
{
	internal GlobalCatalog()
	{
	}

	public DomainController DisableGlobalCatalog()
	{
		throw null;
	}

	public override GlobalCatalog EnableGlobalCatalog()
	{
		throw null;
	}

	public new static GlobalCatalogCollection FindAll(DirectoryContext context)
	{
		throw null;
	}

	public new static GlobalCatalogCollection FindAll(DirectoryContext context, string siteName)
	{
		throw null;
	}

	public ReadOnlyActiveDirectorySchemaPropertyCollection FindAllProperties()
	{
		throw null;
	}

	public new static GlobalCatalog FindOne(DirectoryContext context)
	{
		throw null;
	}

	public new static GlobalCatalog FindOne(DirectoryContext context, LocatorOptions flag)
	{
		throw null;
	}

	public new static GlobalCatalog FindOne(DirectoryContext context, string siteName)
	{
		throw null;
	}

	public new static GlobalCatalog FindOne(DirectoryContext context, string siteName, LocatorOptions flag)
	{
		throw null;
	}

	public override DirectorySearcher GetDirectorySearcher()
	{
		throw null;
	}

	public static GlobalCatalog GetGlobalCatalog(DirectoryContext context)
	{
		throw null;
	}

	public override bool IsGlobalCatalog()
	{
		throw null;
	}
}
