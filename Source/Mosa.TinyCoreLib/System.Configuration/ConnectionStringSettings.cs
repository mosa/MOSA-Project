namespace System.Configuration;

public sealed class ConnectionStringSettings : ConfigurationElement
{
	public string ConnectionString
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
		set
		{
		}
	}

	protected override ConfigurationPropertyCollection Properties
	{
		get
		{
			throw null;
		}
	}

	public string ProviderName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ConnectionStringSettings()
	{
	}

	public ConnectionStringSettings(string name, string connectionString)
	{
	}

	public ConnectionStringSettings(string name, string connectionString, string providerName)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
