namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
public sealed class AutomationProxyAttribute : Attribute
{
	public bool Value
	{
		get
		{
			throw null;
		}
	}

	public AutomationProxyAttribute(bool val)
	{
	}
}
