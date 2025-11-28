namespace System.DirectoryServices.ActiveDirectory;

public class Domain : ActiveDirectoryPartition
{
	public DomainCollection Children
	{
		get
		{
			throw null;
		}
	}

	public DomainControllerCollection DomainControllers
	{
		get
		{
			throw null;
		}
	}

	public DomainMode DomainMode
	{
		get
		{
			throw null;
		}
	}

	public int DomainModeLevel
	{
		get
		{
			throw null;
		}
	}

	public Forest Forest
	{
		get
		{
			throw null;
		}
	}

	public DomainController InfrastructureRoleOwner
	{
		get
		{
			throw null;
		}
	}

	public Domain? Parent
	{
		get
		{
			throw null;
		}
	}

	public DomainController PdcRoleOwner
	{
		get
		{
			throw null;
		}
	}

	public DomainController RidRoleOwner
	{
		get
		{
			throw null;
		}
	}

	internal Domain()
	{
	}

	public void CreateLocalSideOfTrustRelationship(string targetDomainName, TrustDirection direction, string trustPassword)
	{
	}

	public void CreateTrustRelationship(Domain targetDomain, TrustDirection direction)
	{
	}

	public void DeleteLocalSideOfTrustRelationship(string targetDomainName)
	{
	}

	public void DeleteTrustRelationship(Domain targetDomain)
	{
	}

	public DomainControllerCollection FindAllDiscoverableDomainControllers()
	{
		throw null;
	}

	public DomainControllerCollection FindAllDiscoverableDomainControllers(string siteName)
	{
		throw null;
	}

	public DomainControllerCollection FindAllDomainControllers()
	{
		throw null;
	}

	public DomainControllerCollection FindAllDomainControllers(string siteName)
	{
		throw null;
	}

	public DomainController FindDomainController()
	{
		throw null;
	}

	public DomainController FindDomainController(LocatorOptions flag)
	{
		throw null;
	}

	public DomainController FindDomainController(string siteName)
	{
		throw null;
	}

	public DomainController FindDomainController(string siteName, LocatorOptions flag)
	{
		throw null;
	}

	public TrustRelationshipInformationCollection GetAllTrustRelationships()
	{
		throw null;
	}

	public static Domain GetComputerDomain()
	{
		throw null;
	}

	public static Domain GetCurrentDomain()
	{
		throw null;
	}

	public override DirectoryEntry GetDirectoryEntry()
	{
		throw null;
	}

	public static Domain GetDomain(DirectoryContext context)
	{
		throw null;
	}

	public bool GetSelectiveAuthenticationStatus(string targetDomainName)
	{
		throw null;
	}

	public bool GetSidFilteringStatus(string targetDomainName)
	{
		throw null;
	}

	public TrustRelationshipInformation GetTrustRelationship(string targetDomainName)
	{
		throw null;
	}

	public void RaiseDomainFunctionality(DomainMode domainMode)
	{
	}

	public void RaiseDomainFunctionalityLevel(int domainMode)
	{
	}

	public void RepairTrustRelationship(Domain targetDomain)
	{
	}

	public void SetSelectiveAuthenticationStatus(string targetDomainName, bool enable)
	{
	}

	public void SetSidFilteringStatus(string targetDomainName, bool enable)
	{
	}

	public void UpdateLocalSideOfTrustRelationship(string targetDomainName, TrustDirection newTrustDirection, string newTrustPassword)
	{
	}

	public void UpdateLocalSideOfTrustRelationship(string targetDomainName, string newTrustPassword)
	{
	}

	public void UpdateTrustRelationship(Domain targetDomain, TrustDirection newTrustDirection)
	{
	}

	public void VerifyOutboundTrustRelationship(string targetDomainName)
	{
	}

	public void VerifyTrustRelationship(Domain targetDomain, TrustDirection direction)
	{
	}
}
