namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class CallbackValidatorAttribute : ConfigurationValidatorAttribute
{
	public string CallbackMethodName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Type Type
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
