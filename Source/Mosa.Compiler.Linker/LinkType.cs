﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Specifies the type of link to perform by the linker.
	/// </summary>
	[Flags]
	public enum LinkType
	{
		/// <summary>
		/// Specifies the kind of link to perform.
		/// </summary>
		KindMask = 0xF0,

		/// <summary>
		/// The link destination receives a relative address.
		/// </summary>
		RelativeOffset = 0x80,

		/// <summary>
		/// The link destination receives the absolute address.
		/// </summary>
		AbsoluteAddress = 0x40,

		/// <summary>
		/// Mask to retrieve the size of the address to store.
		/// </summary>
		SizeMask = 0x0F,

		/// <summary>
		/// An platform dependent (native) 8-bit offset link.
		/// </summary>
		NativeI1 = 0x01,

		/// <summary>
		/// A platform dependent (native) 16-bit offset link.
		/// </summary>
		NativeI2 = 0x02,

		/// <summary>
		/// A platform dependent (native) 32-bit offset link.
		/// </summary>
		NativeI4 = 0x04,

		/// <summary>
		/// A platform dependent (native) 64-bit offset link.
		/// </summary>
		NativeI8 = 0x08,
	}
}