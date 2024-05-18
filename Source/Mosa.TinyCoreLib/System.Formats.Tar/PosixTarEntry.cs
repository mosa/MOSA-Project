namespace System.Formats.Tar;

public abstract class PosixTarEntry : TarEntry
{
	public int DeviceMajor
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int DeviceMinor
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string GroupName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string UserName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal PosixTarEntry()
	{
	}
}
