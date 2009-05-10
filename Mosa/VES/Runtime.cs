/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

namespace Mosa.VES
{
	using System;

	/// <summary>
	/// Color
	/// </summary>
	public static class Runtime
	{
		/// <summary>
		/// Allocates memory.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		internal static uint AllocateMemory(uint size)
		{
			// TODO: Allocated Memory
			// MemoryManager.Allocate(size);
			return 0;
		}

		internal static Object AllocateObject (/* ??? */)
		{
			// TODO
			// uint location = AllocateMemory(size);
			return null;
		}

		/// <summary>
		/// Frees the object.
		/// </summary>
		/// <param name="o">The object.</param>
		public static void FreeObject (object o)
		{
			// TODO: De-allocated Memory
			// MemoryManager.Free (GetObjectPointer (o));
		}
	}
}
