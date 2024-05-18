namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class UrlAttribute : DataTypeAttribute
{
	public UrlAttribute()
		: base(DataType.Custom)
	{
	}

	public override bool IsValid(object? value)
	{
		throw null;
	}
}
