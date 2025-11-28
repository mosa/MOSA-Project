namespace System.Formats.Tar;

public sealed class GnuTarEntry : PosixTarEntry
{
	public DateTimeOffset AccessTime
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DateTimeOffset ChangeTime
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public GnuTarEntry(TarEntry other)
	{
	}

	public GnuTarEntry(TarEntryType entryType, string entryName)
	{
	}
}
