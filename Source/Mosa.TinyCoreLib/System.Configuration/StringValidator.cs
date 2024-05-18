namespace System.Configuration;

public class StringValidator : ConfigurationValidatorBase
{
	public StringValidator(int minLength)
	{
	}

	public StringValidator(int minLength, int maxLength)
	{
	}

	public StringValidator(int minLength, int maxLength, string invalidCharacters)
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
