namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyInformationalVersionAttribute : Attribute
{
	public string InformationalVersion
	{
		get
		{
			throw null;
		}
	}

	public AssemblyInformationalVersionAttribute(string informationalVersion)
	{
	}
}
