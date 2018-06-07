// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Static class of helpful memory functions
	/// </summary>
	public static class MemoryBlock
	{
		/// <summary>
		/// Clears the specified memory area.
		/// </summary>
		/// <param name="destination">The destination address.</param>
		/// <param name="length">The length of bytes to clear.</param>
		public static void Clear(uint destination, uint length)
		{
			Internal.MemoryClear(new IntPtr(destination), length);
		}

		/// <summary>
		/// Copies memory from the source to the destination.
		/// </summary>
		/// <param name="source">The source address.</param>
		/// <param name="destination">The destination address.</param>
		/// <param name="length">The length of bytes to copy.</param>
		public static void Copy(uint source, uint destination, uint length)
		{
			Internal.MemoryCopy(new IntPtr(destination), new IntPtr(source), length);
		}

		/// <summary>
		/// Sets the specified memory area to the specified byte value.
		/// </summary>
		/// <param name="destination">The destination address.</param>
		/// <param name="value">The byte value.</param>
		/// <param name="length">The length of bytes to set.</param>
		public static void Set(uint destination, byte value, uint length)
		{
			Internal.MemorySet(new IntPtr(destination), value, length);
		}
	}
}
