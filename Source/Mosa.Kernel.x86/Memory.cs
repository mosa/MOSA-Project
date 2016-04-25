// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

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
			MemoryClear(destination, length);
		}

		/// <summary>
		/// Copies memory from the source to the destination.
		/// </summary>
		/// <param name="source">The source address.</param>
		/// <param name="destination">The destination address.</param>
		/// <param name="length">The length of bytes to copy.</param>
		public static void Copy(uint source, uint destination, uint length)
		{
			MemoryCopy(destination, source, length);
		}

		/// <summary>
		/// Sets the specified memory area to the specified byte value.
		/// </summary>
		/// <param name="destination">The destination address.</param>
		/// <param name="value">The byte value.</param>
		/// <param name="length">The length of bytes to set.</param>
		public static void Set(uint destination, byte value, uint length)
		{
			MemorySet(destination, value, length);
		}

		private unsafe static void MemoryClear(uint destination, uint count)
		{
			Internal.MemoryClear((void*)destination, count);
		}

		private unsafe static void MemoryCopy(uint destination, uint source, uint count)
		{
			Internal.MemoryCopy((void*)destination, (void*)source, count);
		}

		private unsafe static void MemorySet(uint destination, byte value, uint count)
		{
			Internal.MemorySet((void*)destination, value, count);
		}
	}
}
