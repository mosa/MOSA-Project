using System.Globalization;

namespace System.Collections;

[Obsolete("CaseInsensitiveHashCodeProvider has been deprecated. Use StringComparer instead.")]
public class CaseInsensitiveHashCodeProvider : IHashCodeProvider
{
	public static CaseInsensitiveHashCodeProvider Default
	{
		get
		{
			throw null;
		}
	}

	public static CaseInsensitiveHashCodeProvider DefaultInvariant
	{
		get
		{
			throw null;
		}
	}

	public CaseInsensitiveHashCodeProvider()
	{
	}

	public CaseInsensitiveHashCodeProvider(CultureInfo culture)
	{
	}

	public int GetHashCode(object obj)
	{
		throw null;
	}
}
