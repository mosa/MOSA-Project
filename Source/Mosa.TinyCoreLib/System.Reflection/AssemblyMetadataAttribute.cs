namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
public sealed class AssemblyMetadataAttribute : Attribute
{
	public string Key
	{
		get
		{
			throw null;
		}
	}

	public string? Value
	{
		get
		{
			throw null;
		}
	}

	public AssemblyMetadataAttribute(string key, string? value)
	{
	}
}
