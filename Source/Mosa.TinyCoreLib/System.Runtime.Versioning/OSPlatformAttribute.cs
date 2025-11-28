namespace System.Runtime.Versioning;

public abstract class OSPlatformAttribute : Attribute
{
	public string PlatformName
	{
		get
		{
			throw null;
		}
	}

	private protected OSPlatformAttribute(string platformName)
	{
	}
}
