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
	public enum MethodSemanticsAttributes : ushort
	{
		None = 0x0000,
		Setter = 0x0001,
		Getter = 0x0002,
		Other = 0x0004,
		AddOn = 0x0008,
		RemoveOn = 0x0010,
		Fire = 0x0020
	}
}
