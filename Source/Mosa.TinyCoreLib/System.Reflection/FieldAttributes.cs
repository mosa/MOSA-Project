namespace System.Reflection;

[Flags]
public enum FieldAttributes
{
	PrivateScope = 0,
	Private = 1,
	FamANDAssem = 2,
	Assembly = 3,
	Family = 4,
	FamORAssem = 5,
	Public = 6,
	FieldAccessMask = 7,
	Static = 0x10,
	InitOnly = 0x20,
	Literal = 0x40,
	[Obsolete("Formatter-based serialization is obsolete and should not be used.", DiagnosticId = "SYSLIB0050", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	NotSerialized = 0x80,
	HasFieldRVA = 0x100,
	SpecialName = 0x200,
	RTSpecialName = 0x400,
	HasFieldMarshal = 0x1000,
	PinvokeImpl = 0x2000,
	HasDefault = 0x8000,
	ReservedMask = 0x9500
}
