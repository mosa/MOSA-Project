namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event, Inherited = false)]
public sealed class DispIdAttribute : Attribute
{
	public int Value
	{
		get
		{
			throw null;
		}
	}

	public DispIdAttribute(int dispId)
	{
	}
}
