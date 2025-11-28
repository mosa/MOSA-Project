namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class TimeSpanValidatorAttribute : ConfigurationValidatorAttribute
{
	public const string TimeSpanMaxValue = "10675199.02:48:05.4775807";

	public const string TimeSpanMinValue = "-10675199.02:48:05.4775808";

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

	public TimeSpan MaxValue
	{
		get
		{
			throw null;
		}
	}

	public string MaxValueString
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public TimeSpan MinValue
	{
		get
		{
			throw null;
		}
	}

	public string MinValueString
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
