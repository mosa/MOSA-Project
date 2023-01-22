// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System;
using Mosa.Runtime;

namespace Mosa.Kernel.x86
{
	public static class IOAPIC
	{
		public static Pointer Pointer;

		public static bool IsInitialized;

		public static void Setup(Pointer ptr)
		{
			Pointer = ptr;

			// TODO: SetEntry() and disable all entries

			IsInitialized = true;
		}

		public static void WriteRegister(byte reg, uint val)
		{
			Pointer.Store32(0, reg);
			Pointer.Store32(0x10, val);
		}

		public static uint ReadRegister(byte reg)
		{
			Pointer.Store32(0, reg);
			return Pointer.Load32(0x10);
		}

		public static uint ReadToApic(Pointer ioApic, uint reg)
		{
			if (!IsInitialized)
				throw new InvalidOperationException("Trying to read from APIC while being uninitialized.");

			ioApic.Store32(0, reg & 0xff);
			return ioApic.Load32(4);
		}

		public static void WriteToApic(Pointer ioApic, uint reg, uint value)
		{
			if (!IsInitialized)
				throw new InvalidOperationException("Trying to write to APIC while being uninitialized.");

			ioApic.Store32(0, reg & 0xff);
			ioApic.Store32(4, value);
		}
	}
}
