namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
public sealed class RequiresAssemblyFilesAttribute : Attribute
{
	public string? Message
	{
		get
		{
			throw null;
		}
	}

	public string? Url
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RequiresAssemblyFilesAttribute()
	{
	}

	public RequiresAssemblyFilesAttribute(string message)
	{
	}
}
