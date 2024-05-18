namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyTrademarkAttribute : Attribute
{
	public string Trademark
	{
		get
		{
			throw null;
		}
	}

	public AssemblyTrademarkAttribute(string trademark)
	{
	}
}
