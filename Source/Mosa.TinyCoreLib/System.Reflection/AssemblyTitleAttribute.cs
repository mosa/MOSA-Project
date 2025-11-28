namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyTitleAttribute(string title) : Attribute
{
	public string Title { get; } = title;
}
