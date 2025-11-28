namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class DataTypeAttribute : ValidationAttribute
{
	public string? CustomDataType
	{
		get
		{
			throw null;
		}
	}

	public DataType DataType
	{
		get
		{
			throw null;
		}
	}

	public DisplayFormatAttribute? DisplayFormat
	{
		get
		{
			throw null;
		}
		protected set
		{
		}
	}

	public DataTypeAttribute(DataType dataType)
	{
	}

	public DataTypeAttribute(string customDataType)
	{
	}

	public virtual string GetDataTypeName()
	{
		throw null;
	}

	public override bool IsValid(object? value)
	{
		throw null;
	}
}
