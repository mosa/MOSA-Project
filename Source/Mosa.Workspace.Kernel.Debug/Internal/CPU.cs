// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Workspace.Kernel.Internal
{
	public static class CPU
	{
		private static readonly Memory Memory = new Memory();

		public static int PageSize = 4096;
		public static int Size = 32;

		public static bool Is32Bit { get { return Size == 32; } }
		public static bool Is64Bit { get { return Size == 64; } }

		public static bool IsVirtualMemoryConfigured { get; set; } = false;

		public static bool IsIntel { get; set; } = true;

		public static ulong IntelCR3Register { get; set; } = 0;

		private static ulong TranslateToPhysical(ulong address)
		{
			if (!IsVirtualMemoryConfigured)
				return address;

			if (IsIntel && Is32Bit)
			{
				uint pd = Memory.Read32(IntelCR3Register + ((address >> 22) * 4));
				uint pt = Memory.Read32((pd & 0xFFFFF000) + ((address >> 12 & 0x03FF) * 4));

				if ((pt & 0x1) == 0)
				{
					// page not present
					throw new PageFaultException(address);
				}

				return (address & 0xFFF) | (pt & 0xFFFFF000);
			}

			return address;
		}

		public static ulong Read64(ulong address)
		{
			return Memory.Read64(TranslateToPhysical(address));
		}

		public static uint Read32(ulong address)
		{
			return Memory.Read32(TranslateToPhysical(address));
		}

		public static ushort Read16(ulong address)
		{
			return Memory.Read16(TranslateToPhysical(address));
		}

		public static byte Read8(ulong address)
		{
			return Memory.Read8(TranslateToPhysical(address));
		}

		public static void Write64(ulong address, ulong value)
		{
			Memory.Write64(TranslateToPhysical(address), value);
		}

		public static void Write32(ulong address, uint value)
		{
			Memory.Write32(TranslateToPhysical(address), value);
		}

		public static void Write16(ulong address, ushort value)
		{
			Memory.Write16(TranslateToPhysical(address), value);
		}

		public static void Write8(ulong address, byte value)
		{
			Memory.Write8(TranslateToPhysical(address), value);
		}
	}
}
