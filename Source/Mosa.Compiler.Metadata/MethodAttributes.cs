/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

namespace Mosa.Runtime.Metadata
{
	[Flags]
	public enum MethodAttributes : ushort
	{
		MemberAccessMask = 0x0007,
		CompilerControlled = 0x0000,
		Private = 0x0001,
		FamANDAssem = 0x0002,
		Assembly = 0x0003,
		Family = 0x0004,
		FamORAssem = 0x0005,
		Public = 0x0006,

		Static = 0x0010,
		Final = 0x0020,
		Virtual = 0x0040,
		HideBySig = 0x0080,

		VtableLayoutMask = 0x0100,
		ReuseSlot = 0x0000,
		NewSlot = 0x0100,

		CheckAccessOnOverride = 0x0200,
		Abstract = 0x0400,
		SpecialName = 0x0800,

		PInvokeImpl = 0x2000,
		UnmanagedExport = 0x0008,

		RTSpecialName = 0x1000,
		HasSecurity = 0x4000,
		RequireSecObject = 0x8000
	}
}
