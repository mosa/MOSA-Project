namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public sealed class TargetFrameworkAttribute : Attribute
{
	public string? FrameworkDisplayName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string FrameworkName
	{
		get
		{
			throw null;
		}
	}

	public TargetFrameworkAttribute(string frameworkName)
	{
	}
}
