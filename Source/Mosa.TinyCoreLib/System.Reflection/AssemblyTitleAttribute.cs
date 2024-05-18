namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyTitleAttribute : Attribute
{
	public string Title
	{
		get
		{
			throw null;
		}
	}

	public AssemblyTitleAttribute(string title)
	{
	}
}
