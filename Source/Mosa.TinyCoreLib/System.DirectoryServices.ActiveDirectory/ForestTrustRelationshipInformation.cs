using System.Collections.Specialized;

namespace System.DirectoryServices.ActiveDirectory;

public class ForestTrustRelationshipInformation : TrustRelationshipInformation
{
	public StringCollection ExcludedTopLevelNames
	{
		get
		{
			throw null;
		}
	}

	public TopLevelNameCollection TopLevelNames
	{
		get
		{
			throw null;
		}
	}

	public ForestTrustDomainInfoCollection TrustedDomainInformation
	{
		get
		{
			throw null;
		}
	}

	internal ForestTrustRelationshipInformation()
	{
	}

	public void Save()
	{
	}
}
