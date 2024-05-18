namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class PartMetadataAttribute : Attribute
{
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

	public PartMetadataAttribute(string? name, object? value)
	{
	}
}
