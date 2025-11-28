namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class EnumDataTypeAttribute : DataTypeAttribute
{
	public Type EnumType
	{
		get
		{
			throw null;
		}
	}

	public EnumDataTypeAttribute(Type enumType)
		: base(DataType.Custom)
	{
	}

	public override bool IsValid(object? value)
	{
		throw null;
	}
}
