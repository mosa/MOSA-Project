using System.ComponentModel;

namespace System.Runtime.InteropServices;

public enum UnmanagedType
{
	Bool = 2,
	I1 = 3,
	U1 = 4,
	I2 = 5,
	U2 = 6,
	I4 = 7,
	U4 = 8,
	I8 = 9,
	U8 = 10,
	R4 = 11,
	R8 = 12,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Marshalling as Currency may be unavailable in future releases.")]
	Currency = 15,
	BStr = 19,
	LPStr = 20,
	LPWStr = 21,
	LPTStr = 22,
	ByValTStr = 23,
	IUnknown = 25,
	[EditorBrowsable(EditorBrowsableState.Never)]
	IDispatch = 26,
	[EditorBrowsable(EditorBrowsableState.Never)]
	Struct = 27,
	Interface = 28,
	[EditorBrowsable(EditorBrowsableState.Never)]
	SafeArray = 29,
	ByValArray = 30,
	SysInt = 31,
	SysUInt = 32,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Marshalling as VBByRefString may be unavailable in future releases.")]
	VBByRefStr = 34,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Marshalling as AnsiBStr may be unavailable in future releases.")]
	AnsiBStr = 35,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Marshalling as TBstr may be unavailable in future releases.")]
	TBStr = 36,
	[EditorBrowsable(EditorBrowsableState.Never)]
	VariantBool = 37,
	FunctionPtr = 38,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Marshalling arbitrary types may be unavailable in future releases. Specify the type you wish to marshal as.")]
	AsAny = 40,
	LPArray = 42,
	LPStruct = 43,
	CustomMarshaler = 44,
	Error = 45,
	IInspectable = 46,
	HString = 47,
	LPUTF8Str = 48
}
