using System.Collections.Generic;

namespace System.Formats.Tar;

public sealed class PaxGlobalExtendedAttributesTarEntry : PosixTarEntry
{
	public IReadOnlyDictionary<string, string> GlobalExtendedAttributes
	{
		get
		{
			throw null;
		}
	}

	public PaxGlobalExtendedAttributesTarEntry(IEnumerable<KeyValuePair<string, string>> globalExtendedAttributes)
	{
	}
}
