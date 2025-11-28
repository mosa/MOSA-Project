namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class SubclassTypeValidatorAttribute : ConfigurationValidatorAttribute
{
	public Type BaseClass
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

	public SubclassTypeValidatorAttribute(Type baseClass)
	{
	}
}
