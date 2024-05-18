namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public class ConfigurationValidatorAttribute : Attribute
{
	public virtual ConfigurationValidatorBase ValidatorInstance
	{
		get
		{
			throw null;
		}
	}

	public Type ValidatorType
	{
		get
		{
			throw null;
		}
	}

	protected ConfigurationValidatorAttribute()
	{
	}

	public ConfigurationValidatorAttribute(Type validator)
	{
	}
}
