namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class StringLengthAttribute : ValidationAttribute
{
	public int MaximumLength
	{
		get
		{
			throw null;
		}
	}

	public int MinimumLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StringLengthAttribute(int maximumLength)
	{
	}

	public override string FormatErrorMessage(string name)
	{
		throw null;
	}

	public override bool IsValid(object? value)
	{
		throw null;
	}
}
