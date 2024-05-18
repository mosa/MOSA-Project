namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class RegexStringValidatorAttribute : ConfigurationValidatorAttribute
{
	public string Regex
	{
		get
		{
			throw null;
		}
	}

	public override ConfigurationValidatorBase ValidatorInstance
	{
		get
		{
			throw null;
		}
	}

	public RegexStringValidatorAttribute(string regex)
	{
	}
}
