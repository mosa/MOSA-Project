namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class CreditCardAttribute : DataTypeAttribute
{
	public CreditCardAttribute()
		: base(DataType.Custom)
	{
	}

	public override bool IsValid(object? value)
	{
		throw null;
	}
}
