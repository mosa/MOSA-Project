namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyCopyrightAttribute(string copyright) : Attribute
{
	public string Copyright { get; } = copyright;
}
