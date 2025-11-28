namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyCompanyAttribute(string company) : Attribute
{
	public string Company { get; } = company;
}
