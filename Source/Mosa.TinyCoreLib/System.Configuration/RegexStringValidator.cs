namespace System.Configuration;

public class RegexStringValidator : ConfigurationValidatorBase
{
	public RegexStringValidator(string regex)
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
