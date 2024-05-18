using System.ComponentModel;

namespace System.Configuration;

public sealed class ConfigurationProperty
{
	public TypeConverter Converter
	{
		get
		{
			throw null;
		}
	}

	public object DefaultValue
	{
		get
		{
			throw null;
		}
	}

	public string Description
	{
		get
		{
			throw null;
		}
	}

	public bool IsAssemblyStringTransformationRequired
	{
		get
		{
			throw null;
		}
	}

	public bool IsDefaultCollection
	{
		get
		{
			throw null;
		}
	}

	public bool IsKey
	{
		get
		{
			throw null;
		}
	}

	public bool IsRequired
	{
		get
		{
			throw null;
		}
	}

	public bool IsTypeStringTransformationRequired
	{
		get
		{
			throw null;
		}
	}

	public bool IsVersionCheckRequired
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

	public Type Type
	{
		get
		{
			throw null;
		}
	}

	public ConfigurationValidatorBase Validator
	{
		get
		{
			throw null;
		}
	}

	public ConfigurationProperty(string name, Type type)
	{
	}

	public ConfigurationProperty(string name, Type type, object defaultValue)
	{
	}

	public ConfigurationProperty(string name, Type type, object defaultValue, TypeConverter typeConverter, ConfigurationValidatorBase validator, ConfigurationPropertyOptions options)
	{
	}

	public ConfigurationProperty(string name, Type type, object defaultValue, TypeConverter typeConverter, ConfigurationValidatorBase validator, ConfigurationPropertyOptions options, string description)
	{
	}

	public ConfigurationProperty(string name, Type type, object defaultValue, ConfigurationPropertyOptions options)
	{
	}
}
