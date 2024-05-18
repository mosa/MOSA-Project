namespace System.Reflection.Metadata;

public enum SignatureCallingConvention : byte
{
	Default = 0,
	CDecl = 1,
	StdCall = 2,
	ThisCall = 3,
	FastCall = 4,
	VarArgs = 5,
	Unmanaged = 9
}
