namespace System.Configuration;

public abstract class ConfigurationValidatorBase
{
	public virtual bool CanValidate(Type type)
	{
		throw null;
	}

	public abstract void Validate(object value);
}
