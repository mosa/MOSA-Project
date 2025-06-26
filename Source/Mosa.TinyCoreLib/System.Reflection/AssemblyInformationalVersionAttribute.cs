namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyInformationalVersionAttribute(string informationalVersion) : Attribute
{
	public string InformationalVersion { get; } = informationalVersion;
}
