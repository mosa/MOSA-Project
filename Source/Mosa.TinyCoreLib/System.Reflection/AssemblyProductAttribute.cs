namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyProductAttribute : Attribute
{
	public string Product
	{
		get
		{
			throw null;
		}
	}

	public AssemblyProductAttribute(string product)
	{
	}
}
