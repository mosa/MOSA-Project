namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class IntegerValidatorAttribute : ConfigurationValidatorAttribute
{
	public bool ExcludeRange
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int MaxValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int MinValue
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
