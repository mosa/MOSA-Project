namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
public sealed class ExportMetadataAttribute : Attribute
{
	public bool IsMultiple
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public object? Value
	{
		get
		{
			throw null;
		}
	}

	public ExportMetadataAttribute(string? name, object? value)
	{
	}
}
