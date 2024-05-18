namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class PositiveTimeSpanValidatorAttribute : ConfigurationValidatorAttribute
{
	public override ConfigurationValidatorBase ValidatorInstance
	{
		get
		{
			throw null;
		}
	}
}
