namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
public sealed class ComVisibleAttribute : Attribute
{
	public bool Value
	{
		get
		{
			throw null;
		}
	}

	public ComVisibleAttribute(bool visibility)
	{
	}
}
