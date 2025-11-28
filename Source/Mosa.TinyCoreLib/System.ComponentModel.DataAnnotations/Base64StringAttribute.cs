namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class Base64StringAttribute : ValidationAttribute
{
	public override bool IsValid(object? value)
	{
		throw null;
	}
}
