namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class FileExtensionsAttribute : DataTypeAttribute
{
	public string Extensions
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public FileExtensionsAttribute()
		: base(DataType.Custom)
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
