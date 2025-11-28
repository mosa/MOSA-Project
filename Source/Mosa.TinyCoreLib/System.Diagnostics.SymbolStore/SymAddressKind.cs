namespace System.Diagnostics.SymbolStore;

public enum SymAddressKind
{
	ILOffset = 1,
	NativeRVA,
	NativeRegister,
	NativeRegisterRelative,
	NativeOffset,
	NativeRegisterRegister,
	NativeRegisterStack,
	NativeStackRegister,
	BitField,
	NativeSectionOffset
}
