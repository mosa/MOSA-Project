namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyCultureAttribute(string culture) : Attribute
{
	public string Culture { get; } = culture;
}
