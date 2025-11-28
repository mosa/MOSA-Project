namespace System.Configuration;

public class LongValidator : ConfigurationValidatorBase
{
	public LongValidator(long minValue, long maxValue)
	{
	}

	public LongValidator(long minValue, long maxValue, bool rangeIsExclusive)
	{
	}

	public LongValidator(long minValue, long maxValue, bool rangeIsExclusive, long resolution)
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
