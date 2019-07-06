// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Workspace.Kernel.Internal.Debug
{
	public static class CPU
	{
		private static readonly Memory Memory = new Memory();

		public static int Size = 32;

		public static bool Is32Bit { get { return Size == 32; } }
		public static bool Is64Bit { get { return Size == 64; } }

		public static ulong Read64(ulong address)
		{
			return Memory.Read64(address);
		}

		public static uint Read32(ulong address)
		{
			return Memory.Read32(address);
		}

		public static ushort Read16(ulong address)
		{
			return Memory.Read16(address);
		}

		public static byte Read8(ulong address)
		{
			return Memory.Read8(address);
		}

		public static void Write64(ulong address, ulong value)
		{
			Memory.Write64(address, value);
		}

		public static void Write32(ulong address, uint value)
		{
			Memory.Write32(address, value);
		}

		public static void Write16(ulong address, ushort value)
		{
			Memory.Write16(address, value);
		}

		public static void Write8(ulong address, byte value)
		{
			Memory.Write8(address, value);
		}
	}
}
