namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ConfigurationPropertyAttribute : Attribute
{
	public object DefaultValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsDefaultCollection
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsKey
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsRequired
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

	public ConfigurationPropertyOptions Options
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ConfigurationPropertyAttribute(string name)
	{
	}
}
