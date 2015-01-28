/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.Internal.x86;

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
		/// <param name="start">The start.</param>
		/// <param name="bytes">The bytes.</param>
		public static void Clear(uint start, uint bytes)
		{
			if (bytes % 4 == 0)
			{
				Clear4(start, bytes);
				return;
			}

			for (uint at = start; at < (start + bytes); at++)
				Native.Set8(at, 0);
		}

		public static void Clear4(uint start, uint bytes)
		{
			for (uint at = start; at < (start + bytes); at = at + 4)
				Native.Set32(at, 0);
		}

		public static void Copy(uint source, uint destination, uint length)
		{
			for (uint i = 0; i < length; i++)
				Native.Set8(destination + i, Native.Get8(source + i));  //TODO: Optimize with Set32
		}
	}
}