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
	public enum ParameterAttributes : ushort
	{
		None = 0x0000,
		In = 0x0001,
		Out = 0x0002,
		Lcid = 0x0004,
		Retval = 0x0008,
		Optional = 0x0010,
		HasDefault = 0x1000,
		HasFieldMarshal = 0x2000,
		Unused = 0xcfe0
	}
}
