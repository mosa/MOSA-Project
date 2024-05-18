namespace System.Globalization;

[Flags]
public enum CultureTypes
{
	NeutralCultures = 1,
	SpecificCultures = 2,
	InstalledWin32Cultures = 4,
	AllCultures = 7,
	UserCustomCulture = 8,
	ReplacementCultures = 0x10,
	[Obsolete("CultureTypes.WindowsOnlyCultures has been deprecated. Use other values in CultureTypes instead.")]
	WindowsOnlyCultures = 0x20,
	[Obsolete("CultureTypes.FrameworkCultures has been deprecated. Use other values in CultureTypes instead.")]
	FrameworkCultures = 0x40
}
