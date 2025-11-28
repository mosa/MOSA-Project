namespace System.Reflection;

[Flags]
public enum MethodImportAttributes : short
{
	None = 0,
	ExactSpelling = 1,
	CharSetAnsi = 2,
	CharSetUnicode = 4,
	CharSetAuto = 6,
	CharSetMask = 6,
	BestFitMappingEnable = 0x10,
	BestFitMappingDisable = 0x20,
	BestFitMappingMask = 0x30,
	SetLastError = 0x40,
	CallingConventionWinApi = 0x100,
	CallingConventionCDecl = 0x200,
	CallingConventionStdCall = 0x300,
	CallingConventionThisCall = 0x400,
	CallingConventionFastCall = 0x500,
	CallingConventionMask = 0x700,
	ThrowOnUnmappableCharEnable = 0x1000,
	ThrowOnUnmappableCharDisable = 0x2000,
	ThrowOnUnmappableCharMask = 0x3000
}
