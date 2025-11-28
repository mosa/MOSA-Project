namespace System.Configuration;

public class TimeSpanValidator : ConfigurationValidatorBase
{
	public TimeSpanValidator(TimeSpan minValue, TimeSpan maxValue)
	{
	}

	public TimeSpanValidator(TimeSpan minValue, TimeSpan maxValue, bool rangeIsExclusive)
	{
	}

	public TimeSpanValidator(TimeSpan minValue, TimeSpan maxValue, bool rangeIsExclusive, long resolutionInSeconds)
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
