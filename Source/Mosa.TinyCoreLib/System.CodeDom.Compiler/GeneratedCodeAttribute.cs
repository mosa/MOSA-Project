namespace System.CodeDom.Compiler;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
public sealed class GeneratedCodeAttribute : Attribute
{
	public string? Tool
	{
		get
		{
			throw null;
		}
	}

	public string? Version
	{
		get
		{
			throw null;
		}
	}

	public GeneratedCodeAttribute(string? tool, string? version)
	{
	}
}
