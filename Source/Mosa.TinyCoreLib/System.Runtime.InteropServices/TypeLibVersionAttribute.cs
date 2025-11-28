namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class TypeLibVersionAttribute : Attribute
{
	public int MajorVersion
	{
		get
		{
			throw null;
		}
	}

	public int MinorVersion
	{
		get
		{
			throw null;
		}
	}

	public TypeLibVersionAttribute(int major, int minor)
	{
	}
}
