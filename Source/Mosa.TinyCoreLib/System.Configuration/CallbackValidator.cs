namespace System.Configuration;

public sealed class CallbackValidator : ConfigurationValidatorBase
{
	public CallbackValidator(Type type, ValidatorCallback callback)
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
