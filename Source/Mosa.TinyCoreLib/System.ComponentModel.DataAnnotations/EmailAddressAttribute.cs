namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class EmailAddressAttribute : DataTypeAttribute
{
	public EmailAddressAttribute()
		: base(DataType.Custom)
	{
	}

	public override bool IsValid(object? value)
	{
		throw null;
	}
}
