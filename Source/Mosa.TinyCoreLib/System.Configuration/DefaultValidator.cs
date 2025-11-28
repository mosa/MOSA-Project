namespace System.Configuration;

public sealed class DefaultValidator : ConfigurationValidatorBase
{
	public override bool CanValidate(Type type)
	{
		throw null;
	}

	public override void Validate(object value)
	{
	}
}
