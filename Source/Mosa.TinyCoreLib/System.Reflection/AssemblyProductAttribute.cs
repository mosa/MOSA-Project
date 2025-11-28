namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyProductAttribute(string product) : Attribute
{
	public string Product { get; } = product;
}
