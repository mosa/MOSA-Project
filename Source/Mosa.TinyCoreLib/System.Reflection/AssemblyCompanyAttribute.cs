namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyCompanyAttribute : Attribute
{
	public string Company
	{
		get
		{
			throw null;
		}
	}

	public AssemblyCompanyAttribute(string company)
	{
	}
}
