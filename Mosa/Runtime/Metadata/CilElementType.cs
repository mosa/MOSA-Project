/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata {
	public enum CilElementType {
		/// <summary>
		/// Marks the end of the list
		/// </summary>
		End = 0x00,

		Void = 0x01,
		Boolean = 0x02,
		Char = 0x03,
		I1 = 0x04,
		U1 = 0x05,
		I2 = 0x06,
		U2 = 0x07,
		I4 = 0x08,
		U4 = 0x09,
		I8 = 0x0A,
		U8 = 0x0B,
		R4 = 0x0C,
		R8 = 0x0D,
		String = 0x0E,

		Ptr = 0x0F,
		ByRef = 0x10,
		ValueType = 0x11,
		Class = 0x12,
		Var = 0x13,
		Array = 0x14, // FIXME: Decode the representation
		GenericInst = 0x15,
		TypedByRef = 0x16,
		
		I = 0x18,
		U = 0x19,

		FunctionPtr = 0x1B,
		Object = 0x1C,
		SZArray = 0x1D,
		MVar = 0x1E,
		Required = 0x1F,
		Optional = 0x20,
		Internal = 0x21,
		
		Modifier = 0x40,
		Sentinel = 0x41,
		Pinned = 0x45,

		Type = 0x50,
		BoxedObject = 0x51,
		Reserved = 0x52,
		Field = 0x53,
		Property = 0x54,
		Enum = 0x55
	}
}
