namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
public sealed class GuidAttribute : Attribute
{
	public string Value
	{
		get
		{
			throw null;
		}
	}

	public GuidAttribute(string guid)
	{
	}
}
