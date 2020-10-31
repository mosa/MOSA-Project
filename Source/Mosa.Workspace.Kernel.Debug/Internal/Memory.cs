// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;

namespace Mosa.Workspace.Kernel.Internal
{
	public class Memory
	{
		public uint BlockSize = 4096;

		private readonly Dictionary<ulong, uint[]> MemoryBlocks = new Dictionary<ulong, uint[]>();

		#region Private

		private ushort InternalRead16(ulong address)
		{
			var offset = address % 4;

			uint a = InternalRead32Ex(address - offset);

			if (offset == 0)
			{
				return (ushort)((a & 0xFFFF0000) >> 16);
			}
			else if (offset == 1)
			{
				return (ushort)((a & 0x00FFFF00) >> 8);
			}
			else if (offset == 2)
			{
				return (ushort)(a & 0x0000FFFF);
			}
			else if (offset == 3)
			{
				uint b = InternalRead32Ex(address + 1);

				return (ushort)(((a & 0x000000FF) << 8) | ((b & 0xFF000000) >> 24));
			}

			throw new InvalidProgramException();
		}

		private uint InternalRead32(ulong address)
		{
			var offset = address % 4;

			if (offset == 0)
			{
				return InternalRead32Ex(address);
			}
			else if (offset == 1)
			{
				uint a = InternalRead32Ex(address - 1);
				uint b = InternalRead32Ex(address + 3);

				return ((a & 0x00FFFFFF) << 8) | ((b & 0xFF000000) >> 24);
			}
			else if (offset == 2)
			{
				uint a = InternalRead32Ex(address - 2);
				uint b = InternalRead32Ex(address + 2);

				return ((a & 0x0000FFFF) << 16) | ((b & 0xFFFF0000) >> 16);
			}
			else if (offset == 3)
			{
				uint a = InternalRead32Ex(address - 3);
				uint b = InternalRead32Ex(address + 1);

				return ((a & 0x000000FF) << 24) | ((b & 0xFFFFFF00) >> 8);
			}

			throw new InvalidProgramException();
		}

		private uint InternalRead32Ex(ulong address)
		{
			System.Diagnostics.Debug.Assert(address % 4 == 0);

			ulong index = address / (BlockSize * 4);

			if (!MemoryBlocks.TryGetValue(index, out uint[] block))
			{
				block = new uint[BlockSize];
				MemoryBlocks.Add(index, block);

				return 0;
			}

			uint offset = (uint)((address / 4) % BlockSize);

			return block[offset];
		}

		private byte InternalRead8(ulong address)
		{
			uint offset = (uint)(address % 4);

			uint value = InternalRead32(address - offset);

			int shift = (3 - (int)offset) * 8;

			return (byte)((value >> shift) & 0xFF);
		}

		private void InternalWrite16(ulong address, ushort value)
		{
			uint offset = (uint)(address % 4);

			if (offset == 0)
			{
				InternalWrite32Ex(address, ((uint)value) << 16, 0xFFFF0000);
			}
			else if (offset == 1)
			{
				InternalWrite32Ex(address - 1, ((uint)value) << 8, 0x00FFFF00);
			}
			else if (offset == 2)
			{
				InternalWrite32Ex(address - 2, value, 0x0000FFFF);
			}
			else if (offset == 3)
			{
				InternalWrite32Ex(address - 3, ((uint)value >> 8), 0x000000FF);
				InternalWrite32Ex(address + 1, ((uint)value << 24), 0xFF000000);
			}
		}

		private void InternalWrite32(ulong address, uint value)
		{
			uint offset = (uint)(address % 4);

			if (offset == 0)
			{
				InternalWrite32Ex(address, value, 0xFFFFFFFF);
			}
			else if (offset == 1)
			{
				InternalWrite32Ex(address - 1, value >> 8, 0x00FFFFFF);
				InternalWrite32Ex(address + 3, value << 24, 0xFF000000);
			}
			else if (offset == 2)
			{
				InternalWrite32Ex(address - 2, value >> 16, 0x0000FFFF);
				InternalWrite32Ex(address + 2, value << 16, 0xFFFF0000);
			}
			else if (offset == 3)
			{
				InternalWrite32Ex(address - 3, value >> 24, 0x000000FF);
				InternalWrite32Ex(address + 1, value << 8, 0xFFFFFF00);
			}
		}

		private void InternalWrite32Ex(ulong address, uint value, uint mask)
		{
			System.Diagnostics.Debug.Assert(address % 4 == 0);

			ulong index = address / (BlockSize * 4);

			if (!MemoryBlocks.TryGetValue(index, out uint[] block))
			{
				block = new uint[BlockSize];
				MemoryBlocks.Add(index, block);

				if (value == 0)
					return;
			}

			uint offset = (uint)((address / 4) % BlockSize);

			uint newvalue = (block[offset] & ~mask) | (value & mask);

			block[offset] = newvalue;
		}

		#endregion Private

		public void Clear()
		{
			MemoryBlocks.Clear();
		}

		public byte Read8(ulong address)
		{
			byte value = InternalRead8(address);

			return value;
		}

		public ushort Read16(ulong address)
		{
			ushort value = InternalRead16(address);

			value = Endian.Swap(value);

			return value;
		}

		public uint Read32(ulong address)
		{
			uint value = InternalRead32(address);

			value = Endian.Swap(value);

			return value;
		}

		public ulong Read64(ulong address)
		{
			uint low = InternalRead32(address);
			uint high = InternalRead32(address + 0x4);
			ulong val = high | ((ulong)low << 32);

			var value = val;

			value = Endian.Swap(value);

			return value;
		}

		public void Write8(ulong address, byte value)
		{
			uint offset = (uint)(address % 4);

			int shift = (3 - (int)offset) * 8;

			InternalWrite32Ex(address - offset, ((uint)value) << shift, (uint)0xFF << shift);
		}

		public void Write16(ulong address, ushort value)
		{
			value = Endian.Swap(value);

			InternalWrite16(address, value);
		}

		public void Write32(ulong address, uint value)
		{
			value = Endian.Swap(value);

			InternalWrite32(address, value);
		}

		public void Write64(ulong address, ulong value)
		{
			value = Endian.Swap(value);

			uint low = (uint)(value >> 32);
			uint high = (uint)value;

			InternalWrite32(address, low);
			InternalWrite32(address + 0x4, high);
		}
	}
}
