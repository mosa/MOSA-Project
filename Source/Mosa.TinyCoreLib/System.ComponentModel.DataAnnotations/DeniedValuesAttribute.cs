namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
[CLSCompliant(false)]
public class DeniedValuesAttribute : ValidationAttribute
{
	public object?[] Values
	{
		get
		{
			throw null;
		}
	}

	public DeniedValuesAttribute(params object?[] values)
	{
	}

	public override bool IsValid(object? value)
	{
		throw null;
	}
}
