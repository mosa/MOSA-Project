namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
public sealed class PrimaryInteropAssemblyAttribute : Attribute
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

	public PrimaryInteropAssemblyAttribute(int major, int minor)
	{
	}
}
