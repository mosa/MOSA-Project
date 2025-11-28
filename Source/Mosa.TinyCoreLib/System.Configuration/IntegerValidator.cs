namespace System.Configuration;

public class IntegerValidator : ConfigurationValidatorBase
{
	public IntegerValidator(int minValue, int maxValue)
	{
	}

	public IntegerValidator(int minValue, int maxValue, bool rangeIsExclusive)
	{
	}

	public IntegerValidator(int minValue, int maxValue, bool rangeIsExclusive, int resolution)
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
