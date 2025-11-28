namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
public sealed class RequiresDynamicCodeAttribute : Attribute
{
	public string Message
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

	public RequiresDynamicCodeAttribute(string message)
	{
	}
}
