using System.Diagnostics.CodeAnalysis;

namespace System.Globalization;

public class CultureInfo : ICloneable, IFormatProvider
{
	public virtual Calendar Calendar
	{
		get
		{
			throw null;
		}
	}

	public virtual CompareInfo CompareInfo
	{
		get
		{
			throw null;
		}
	}

	public CultureTypes CultureTypes
	{
		get
		{
			throw null;
		}
	}

	public static CultureInfo CurrentCulture
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static CultureInfo CurrentUICulture
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual DateTimeFormatInfo DateTimeFormat
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static CultureInfo? DefaultThreadCurrentCulture
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static CultureInfo? DefaultThreadCurrentUICulture
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string DisplayName
	{
		get
		{
			throw null;
		}
	}

	public virtual string EnglishName
	{
		get
		{
			throw null;
		}
	}

	public string IetfLanguageTag
	{
		get
		{
			throw null;
		}
	}

	public static CultureInfo InstalledUICulture
	{
		get
		{
			throw null;
		}
	}

	public static CultureInfo InvariantCulture
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsNeutralCulture
	{
		get
		{
			throw null;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public virtual int KeyboardLayoutId
	{
		get
		{
			throw null;
		}
	}

	public virtual int LCID
	{
		get
		{
			throw null;
		}
	}

	public virtual string Name
	{
		get
		{
			throw null;
		}
	}

	public virtual string NativeName
	{
		get
		{
			throw null;
		}
	}

	public virtual NumberFormatInfo NumberFormat
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual Calendar[] OptionalCalendars
	{
		get
		{
			throw null;
		}
	}

	public virtual CultureInfo Parent
	{
		get
		{
			throw null;
		}
	}

	public virtual TextInfo TextInfo
	{
		get
		{
			throw null;
		}
	}

	public virtual string ThreeLetterISOLanguageName
	{
		get
		{
			throw null;
		}
	}

	public virtual string ThreeLetterWindowsLanguageName
	{
		get
		{
			throw null;
		}
	}

	public virtual string TwoLetterISOLanguageName
	{
		get
		{
			throw null;
		}
	}

	public bool UseUserOverride
	{
		get
		{
			throw null;
		}
	}

	public CultureInfo(int culture)
	{
	}

	public CultureInfo(int culture, bool useUserOverride)
	{
	}

	public CultureInfo(string name)
	{
	}

	public CultureInfo(string name, bool useUserOverride)
	{
	}

	public void ClearCachedData()
	{
	}

	public virtual object Clone()
	{
		throw null;
	}

	public static CultureInfo CreateSpecificCulture(string name)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public CultureInfo GetConsoleFallbackUICulture()
	{
		throw null;
	}

	public static CultureInfo GetCultureInfo(int culture)
	{
		throw null;
	}

	public static CultureInfo GetCultureInfo(string name)
	{
		throw null;
	}

	public static CultureInfo GetCultureInfo(string name, bool predefinedOnly)
	{
		throw null;
	}

	public static CultureInfo GetCultureInfo(string name, string altName)
	{
		throw null;
	}

	public static CultureInfo GetCultureInfoByIetfLanguageTag(string name)
	{
		throw null;
	}

	public static CultureInfo[] GetCultures(CultureTypes types)
	{
		throw null;
	}

	public virtual object? GetFormat(Type? formatType)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static CultureInfo ReadOnly(CultureInfo ci)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
