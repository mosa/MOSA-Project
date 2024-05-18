namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyKeyNameAttribute : Attribute
{
	public string KeyName
	{
		get
		{
			throw null;
		}
	}

	public AssemblyKeyNameAttribute(string keyName)
	{
	}
}
