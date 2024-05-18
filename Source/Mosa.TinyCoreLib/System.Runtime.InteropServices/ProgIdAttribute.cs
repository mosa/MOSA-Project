namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ProgIdAttribute : Attribute
{
	public string Value
	{
		get
		{
			throw null;
		}
	}

	public ProgIdAttribute(string progId)
	{
	}
}
