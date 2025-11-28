using System.Collections.Generic;

namespace System.Formats.Tar;

public sealed class PaxTarEntry : PosixTarEntry
{
	public IReadOnlyDictionary<string, string> ExtendedAttributes
	{
		get
		{
			throw null;
		}
	}

	public PaxTarEntry(TarEntry other)
	{
	}

	public PaxTarEntry(TarEntryType entryType, string entryName)
	{
	}

	public PaxTarEntry(TarEntryType entryType, string entryName, IEnumerable<KeyValuePair<string, string>> extendedAttributes)
	{
	}
}
