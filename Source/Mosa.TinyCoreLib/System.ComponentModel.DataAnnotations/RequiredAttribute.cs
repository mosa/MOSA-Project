namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class RequiredAttribute : ValidationAttribute
{
	public bool AllowEmptyStrings
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override bool IsValid(object? value)
	{
		throw null;
	}
}
