namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
[CLSCompliant(false)]
public class AllowedValuesAttribute : ValidationAttribute
{
	public object?[] Values
	{
		get
		{
			throw null;
		}
	}

	public AllowedValuesAttribute(params object?[] values)
	{
	}

	public override bool IsValid(object? value)
	{
		throw null;
	}
}
