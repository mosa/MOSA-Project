namespace System.Globalization;

[Flags]
public enum CompareOptions
{
	None = 0,
	IgnoreCase = 1,
	IgnoreNonSpace = 2,
	IgnoreSymbols = 4,
	IgnoreKanaType = 8,
	IgnoreWidth = 0x10,
	OrdinalIgnoreCase = 0x10000000,
	StringSort = 0x20000000,
	Ordinal = 0x40000000
}
