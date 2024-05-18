namespace System.Configuration;

public sealed class SubclassTypeValidator : ConfigurationValidatorBase
{
	public SubclassTypeValidator(Type baseClass)
	{
	}

	public override bool CanValidate(Type type)
	{
		throw null;
	}

	public override void Validate(object value)
	{
	}
}
