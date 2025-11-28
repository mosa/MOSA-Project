namespace System.DirectoryServices.ActiveDirectory;

public class ConfigurationSet
{
	public AdamInstanceCollection AdamInstances
	{
		get
		{
			throw null;
		}
	}

	public ApplicationPartitionCollection ApplicationPartitions
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

	public AdamInstance NamingRoleOwner
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

	public AdamInstance SchemaRoleOwner
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

	internal ConfigurationSet()
	{
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public AdamInstance FindAdamInstance()
	{
		throw null;
	}

	public AdamInstance FindAdamInstance(string partitionName)
	{
		throw null;
	}

	public AdamInstance FindAdamInstance(string? partitionName, string siteName)
	{
		throw null;
	}

	public AdamInstanceCollection FindAllAdamInstances()
	{
		throw null;
	}

	public AdamInstanceCollection FindAllAdamInstances(string? partitionName)
	{
		throw null;
	}

	public AdamInstanceCollection FindAllAdamInstances(string? partitionName, string? siteName)
	{
		throw null;
	}

	public static ConfigurationSet GetConfigurationSet(DirectoryContext context)
	{
		throw null;
	}

	public DirectoryEntry GetDirectoryEntry()
	{
		throw null;
	}

	public ReplicationSecurityLevel GetSecurityLevel()
	{
		throw null;
	}

	public void SetSecurityLevel(ReplicationSecurityLevel securityLevel)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
