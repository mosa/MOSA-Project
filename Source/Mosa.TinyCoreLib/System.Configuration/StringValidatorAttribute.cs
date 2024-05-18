namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class StringValidatorAttribute : ConfigurationValidatorAttribute
{
	public string InvalidCharacters
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int MaxLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int MinLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override ConfigurationValidatorBase ValidatorInstance
	{
		get
		{
			throw null;
		}
	}
}
