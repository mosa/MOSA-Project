
using System;

namespace Mosa.Compiler.Metadata
{

	[Flags]
	public enum FieldAttributes : ushort
	{
		FieldAccessMask = 0x0007,
		CompilerControlled = 0x0000,
		Private = 0x0001,
		FamANDAssem = 0x0002,
		Assembly = 0x0003,
		Family = 0x0004,
		FamORAssem = 0x0005,
		Public = 0x0006,
		Static = 0x0010,
		InitOnly = 0x0020,
		Literal = 0x0040,
		NotSerialized = 0x0080,
		SpecialName = 0x0200,
		PInvokeImpl = 0x2000,
		RTSpecialName = 0x0400,
		HasFieldMarshal = 0x1000,
		HasDefault = 0x8000,
		HasFieldRVA = 0x0100
	}
}
