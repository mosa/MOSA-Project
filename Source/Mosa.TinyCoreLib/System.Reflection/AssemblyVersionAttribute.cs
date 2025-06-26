namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyVersionAttribute(string version) : Attribute
{
	public string Version { get; } = version;
}
