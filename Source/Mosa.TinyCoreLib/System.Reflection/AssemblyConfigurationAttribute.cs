namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyConfigurationAttribute : Attribute
{
	public string Configuration
	{
		get
		{
			throw null;
		}
	}

	public AssemblyConfigurationAttribute(string configuration)
	{
	}
}
