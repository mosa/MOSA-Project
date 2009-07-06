/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Kernel.Memory.X86
{

	/// <summary>
	/// Static class of helpful port io functions
	/// </summary>
	public static class Port
	{
		/// <summary>
		/// Wraps the x86 in instruction to read from an 8-bit port.
		/// </summary>
		[IntrinsicAttribute]
		public static unsafe byte In8(byte* address) { return 0; }

		/// <summary>
		/// Wraps the x86 in instruction to read from a 16-bit port.
		/// </summary>
		[IntrinsicAttribute]
		public static unsafe ushort In16(ushort* address) { return 0; }

		/// <summary>
		/// Wraps the x86 in instruction to read from a 32-bit port.
		/// </summary>
		[IntrinsicAttribute]
		public static unsafe uint In32(uint* address) { return 0; }

		/// <summary>
		/// Wraps the x86 out instruction to write to an 8-bit port.
		/// </summary>
		[IntrinsicAttribute]
		public static unsafe void Out8(byte* address, byte value) { }

		/// <summary>
		/// Wraps the x86 out instruction to write to a 16-bit port.
		/// </summary>
		[IntrinsicAttribute]
		public static unsafe void Out16(ushort* address, ushort value) { }

		/// <summary>
		/// Wraps the x86 out instruction to write to a 32-bit port.
		/// </summary>
		[IntrinsicAttribute]
		public static unsafe void Out32(uint* address, uint value) { }

	}
}
