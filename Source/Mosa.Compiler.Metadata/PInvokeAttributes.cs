/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

namespace Mosa.Compiler.Metadata
{
	[Flags]
	public enum PInvokeAttributes : ushort
	{
		NoMangle = 0x0001,
		CharSetMask = 0x0006,
		CharSetNotSpec = 0x0000,
		CharSetAnsi = 0x0002,
		CharSetUnicode = 0x0004,
		CharSetAuto = 0x0006,

		SupportsLastError = 0x0040,

		CallConvMask = 0x0700,
		CallConvWinapi = 0x0100,
		CallConvCdecl = 0x0200,
		CallConvStdCall = 0x0300,
		CallConvThiscall = 0x0400,
		CallConvFastcall = 0x0500,

		BestFitMask = 0x0030,
		BestFitEnabled = 0x0010,
		BestFitDisabled = 0x0020,

		ThrowOnUnmappableCharMask = 0x3000,
		ThrowOnUnmappableCharEnabled = 0x1000,
		ThrowOnUnmappableCharDisabled = 0x2000,
	}
}