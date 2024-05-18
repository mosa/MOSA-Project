namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyFileVersionAttribute : Attribute
{
	public string Version
	{
		get
		{
			throw null;
		}
	}

	public AssemblyFileVersionAttribute(string version)
	{
	}
}
