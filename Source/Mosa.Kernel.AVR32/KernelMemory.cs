/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Internal.Plug;

namespace Mosa.Kernel.AVR32
{
	/// <summary>
	/// Kernel Memory Allocator - This is a pure HACK!
	/// </summary>
	public class KernelMemory
	{
		static private uint _heap = 0;
		static private uint _size = 0;
		static private uint _used = 0;

		[PlugMethod("Mosa.Internal.Runtime.AllocateMemory")]
		static public uint AllocateMemory(uint size)
		{
			if ((_heap == 0) || (size > (_size - _used)))
			{
				// Go allocate memory

				_size = 1024 * 1024 * 64; // 64Mb
				_heap = 0; // TODO: x86.ProcessManager.AllocateMemory(0, _size);
				_used = 0;
			}

			uint at = _heap + _used;
			_used = _used + size;
			return at;
		}

	}
}
