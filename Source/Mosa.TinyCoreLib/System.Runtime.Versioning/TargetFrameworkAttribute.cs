namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public sealed class TargetFrameworkAttribute(string frameworkName) : Attribute
{
	public string? FrameworkDisplayName { get; set; }

	public string FrameworkName { get; } = frameworkName;
}
