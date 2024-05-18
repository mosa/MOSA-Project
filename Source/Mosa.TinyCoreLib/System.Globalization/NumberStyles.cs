namespace System.Globalization;

[Flags]
public enum NumberStyles
{
	None = 0,
	AllowLeadingWhite = 1,
	AllowTrailingWhite = 2,
	AllowLeadingSign = 4,
	Integer = 7,
	AllowTrailingSign = 8,
	AllowParentheses = 0x10,
	AllowDecimalPoint = 0x20,
	AllowThousands = 0x40,
	Number = 0x6F,
	AllowExponent = 0x80,
	Float = 0xA7,
	AllowCurrencySymbol = 0x100,
	Currency = 0x17F,
	Any = 0x1FF,
	AllowHexSpecifier = 0x200,
	HexNumber = 0x203,
	AllowBinarySpecifier = 0x400,
	BinaryNumber = 0x403
}
