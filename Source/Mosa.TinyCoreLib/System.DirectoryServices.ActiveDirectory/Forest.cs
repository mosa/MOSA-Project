namespace System.DirectoryServices.ActiveDirectory;

public class Forest : IDisposable
{
	public ApplicationPartitionCollection ApplicationPartitions
	{
		get
		{
			throw null;
		}
	}

	public DomainCollection Domains
	{
		get
		{
			throw null;
		}
	}

	public ForestMode ForestMode
	{
		get
		{
			throw null;
		}
	}

	public int ForestModeLevel
	{
		get
		{
			throw null;
		}
	}

	public GlobalCatalogCollection GlobalCatalogs
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

	public DomainController NamingRoleOwner
	{
		get
		{
			throw null;
		}
	}

	public Domain RootDomain
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectorySchema Schema
	{
		get
		{
			throw null;
		}
	}

	public DomainController SchemaRoleOwner
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlySiteCollection Sites
	{
		get
		{
			throw null;
		}
	}

	internal Forest()
	{
	}

	public void CreateLocalSideOfTrustRelationship(string targetForestName, TrustDirection direction, string trustPassword)
	{
	}

	public void CreateTrustRelationship(Forest targetForest, TrustDirection direction)
	{
	}

	public void DeleteLocalSideOfTrustRelationship(string targetForestName)
	{
	}

	public void DeleteTrustRelationship(Forest targetForest)
	{
	}

	public void Dispose()
	{
	}

	protected void Dispose(bool disposing)
	{
	}

	public GlobalCatalogCollection FindAllDiscoverableGlobalCatalogs()
	{
		throw null;
	}

	public GlobalCatalogCollection FindAllDiscoverableGlobalCatalogs(string siteName)
	{
		throw null;
	}

	public GlobalCatalogCollection FindAllGlobalCatalogs()
	{
		throw null;
	}

	public GlobalCatalogCollection FindAllGlobalCatalogs(string siteName)
	{
		throw null;
	}

	public GlobalCatalog FindGlobalCatalog()
	{
		throw null;
	}

	public GlobalCatalog FindGlobalCatalog(LocatorOptions flag)
	{
		throw null;
	}

	public GlobalCatalog FindGlobalCatalog(string siteName)
	{
		throw null;
	}

	public GlobalCatalog FindGlobalCatalog(string siteName, LocatorOptions flag)
	{
		throw null;
	}

	public TrustRelationshipInformationCollection GetAllTrustRelationships()
	{
		throw null;
	}

	public static Forest GetCurrentForest()
	{
		throw null;
	}

	public static Forest GetForest(DirectoryContext context)
	{
		throw null;
	}

	public bool GetSelectiveAuthenticationStatus(string targetForestName)
	{
		throw null;
	}

	public bool GetSidFilteringStatus(string targetForestName)
	{
		throw null;
	}

	public ForestTrustRelationshipInformation GetTrustRelationship(string targetForestName)
	{
		throw null;
	}

	public void RaiseForestFunctionality(ForestMode forestMode)
	{
	}

	public void RaiseForestFunctionalityLevel(int forestMode)
	{
	}

	public void RepairTrustRelationship(Forest targetForest)
	{
	}

	public void SetSelectiveAuthenticationStatus(string targetForestName, bool enable)
	{
	}

	public void SetSidFilteringStatus(string targetForestName, bool enable)
	{
	}

	public override string ToString()
	{
		throw null;
	}

	public void UpdateLocalSideOfTrustRelationship(string targetForestName, TrustDirection newTrustDirection, string newTrustPassword)
	{
	}

	public void UpdateLocalSideOfTrustRelationship(string targetForestName, string newTrustPassword)
	{
	}

	public void UpdateTrustRelationship(Forest targetForest, TrustDirection newTrustDirection)
	{
	}

	public void VerifyOutboundTrustRelationship(string targetForestName)
	{
	}

	public void VerifyTrustRelationship(Forest targetForest, TrustDirection direction)
	{
	}
}
