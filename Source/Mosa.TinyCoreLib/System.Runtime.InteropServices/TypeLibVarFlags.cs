namespace System.Runtime.InteropServices;

[Flags]
public enum TypeLibVarFlags
{
	FReadOnly = 1,
	FSource = 2,
	FBindable = 4,
	FRequestEdit = 8,
	FDisplayBind = 0x10,
	FDefaultBind = 0x20,
	FHidden = 0x40,
	FRestricted = 0x80,
	FDefaultCollelem = 0x100,
	FUiDefault = 0x200,
	FNonBrowsable = 0x400,
	FReplaceable = 0x800,
	FImmediateBind = 0x1000
}
