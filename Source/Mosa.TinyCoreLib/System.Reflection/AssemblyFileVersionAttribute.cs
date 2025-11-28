namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyFileVersionAttribute(string version) : Attribute
{
	public string Version { get; } = version ?? throw new ArgumentNullException(nameof(version));
}
