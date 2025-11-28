namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class ComCompatibleVersionAttribute : Attribute
{
	public int BuildNumber
	{
		get
		{
			throw null;
		}
	}

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

	public int RevisionNumber
	{
		get
		{
			throw null;
		}
	}

	public ComCompatibleVersionAttribute(int major, int minor, int build, int revision)
	{
	}
}
