namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyDescriptionAttribute : Attribute
{
	public string Description
	{
		get
		{
			throw null;
		}
	}

	public AssemblyDescriptionAttribute(string description)
	{
	}
}
