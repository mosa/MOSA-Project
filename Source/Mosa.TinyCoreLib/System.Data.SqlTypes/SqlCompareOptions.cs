namespace System.Data.SqlTypes;

[Flags]
public enum SqlCompareOptions
{
	None = 0,
	IgnoreCase = 1,
	IgnoreNonSpace = 2,
	IgnoreKanaType = 8,
	IgnoreWidth = 0x10,
	BinarySort2 = 0x4000,
	BinarySort = 0x8000
}
