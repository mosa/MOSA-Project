namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class LongValidatorAttribute : ConfigurationValidatorAttribute
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

	public long MaxValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public long MinValue
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
