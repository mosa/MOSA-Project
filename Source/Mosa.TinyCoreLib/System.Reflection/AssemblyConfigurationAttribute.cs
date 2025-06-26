namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyConfigurationAttribute(string configuration) : Attribute
{
	public string Configuration { get; } = configuration;
}
