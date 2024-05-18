namespace System;

[Flags]
public enum GenericUriParserOptions
{
	Default = 0,
	GenericAuthority = 1,
	AllowEmptyAuthority = 2,
	NoUserInfo = 4,
	NoPort = 8,
	NoQuery = 0x10,
	NoFragment = 0x20,
	DontConvertPathBackslashes = 0x40,
	DontCompressPath = 0x80,
	DontUnescapePathDotsAndSlashes = 0x100,
	Idn = 0x200,
	IriParsing = 0x400
}
