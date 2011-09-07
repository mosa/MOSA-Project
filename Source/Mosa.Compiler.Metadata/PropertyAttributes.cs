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
	public enum PropertyAttributes : ushort
	{
		None = 0x0000,
		SpecialName = 0x0200,	// Property is special
		RTSpecialName = 0x0400,	// Runtime(metadata internal APIs) should check name encoding
		HasDefault = 0x1000,	// Property has default
		Unused = 0xe9ff	 // Reserved: shall be zero in a conforming implementation
	}
}
