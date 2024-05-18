namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class LCIDConversionAttribute : Attribute
{
	public int Value
	{
		get
		{
			throw null;
		}
	}

	public LCIDConversionAttribute(int lcid)
	{
	}
}
