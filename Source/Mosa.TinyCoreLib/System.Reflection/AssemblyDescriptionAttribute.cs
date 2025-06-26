namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyDescriptionAttribute(string description) : Attribute
{
	public string Description { get; } = description;
}
