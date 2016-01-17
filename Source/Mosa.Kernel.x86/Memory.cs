// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Static class of helpful memory functions
	/// </summary>
	public static class Memory
	{
		/// <summary>
		/// Clears the specified memory area.
		/// </summary>
		/// <param name="destination">The destination address.</param>
		/// <param name="length">The length of bytes to clear.</param>
		public static void Clear(uint destination, uint length)
		{
			Runtime.x86.Internal.MemoryClear(destination, length);
		}

		/// <summary>
		/// Copies memory from the source to the destination.
		/// </summary>
		/// <param name="source">The source address.</param>
		/// <param name="destination">The destination address.</param>
		/// <param name="length">The length of bytes to copy.</param>
		public static void Copy(uint source, uint destination, uint length)
		{
			Memcpy(destination, source, length);
		}

		/// <summary>
		/// Sets the specified memory area to the specified byte value.
		/// </summary>
		/// <param name="destination">The destination address.</param>
		/// <param name="value">The byte value.</param>
		/// <param name="length">The length of bytes to set.</param>
		public static void Set(uint destination, byte value, uint length)
		{
			Runtime.x86.Internal.MemorySet(destination, value, length);
		}

		private unsafe static void Memcpy(uint destination, uint source, uint count)
		{
			Runtime.x86.Internal.MemoryCopy((void*)destination, (void*)source, count);
		}
	}
}
