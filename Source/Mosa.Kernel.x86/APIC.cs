// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86
{
	public static class APIC
	{
		public const uint IA32_APIC_BASE_MSR = 0x1B;
		public const uint IA32_APIC_BASE_MSR_BSP = 0x100; // Processor is a BSP
		public const uint IA32_APIC_BASE_MSR_ENABLE = 0x800;

		public const uint EOI = 0xB0;

		public const uint CPUID_FEAT_EDX_APIC = 1 << 9;

		public static Pointer LocalApic, IOApic;

		public static void Setup(Pointer localApic, Pointer ioApic)
		{
			LocalApic = localApic;
			IOApic = ioApic;

			if (!MSR.HasMSR())
				return;

			// Hardware enable the Local APIC if it wasn't enabled
			SetAPICBase(GetAPICBase());

			// Set the Spurious Interrupt Vector Register bit 8 to start receiving interrupts
			WriteRegister(0xF0, ReadRegister(0xF0) | IA32_APIC_BASE_MSR_BSP);
		}

		public static void SetAPICBase(Pointer apic)
		{
			uint edx = 0;
			uint eax = (uint)(((uint)apic & 0xfffff0000) | IA32_APIC_BASE_MSR_ENABLE);

			/*if (PAE)
				edx = (apic >> 32) & 0x0f;*/

			MSR.SetMSR(IA32_APIC_BASE_MSR, edx << 32 | (ulong)eax);
		}

		public static Pointer GetAPICBase()
		{
			ulong msr = MSR.GetMSR(IA32_APIC_BASE_MSR);

			uint eax = (uint)msr;
			uint edx = (uint)(msr >> 32);

			/*if (PAE)
				return (eax & 0xfffff000) | ((edx & 0x0f) << 32);
			else
				*/
			return (Pointer)(eax & 0xfffff000);
		}

		public static bool CheckAPIC()
		{
			uint eax = Native.CpuIdEAX(1, 0);
			uint edx = Native.CpuIdEDX(1, 0);

			return (edx & CPUID_FEAT_EDX_APIC) != 0;
		}

		public static uint ReadRegister(uint reg)
		{
			return LocalApic.Load32(reg);
		}

		public static void WriteRegister(uint reg, uint value)
		{
			LocalApic.Store32(reg, value);
		}

		public static void SendEndOfInterrupt(uint irq)
		{
			WriteRegister(irq + EOI, 0);
		}

		// TODO: Volatile pointer?
		public static uint ReadToApic(Pointer ioApic, uint reg)
		{
			ioApic.Store32(0, reg & 0xff);
			return ioApic.Load32(4);
		}

		// TODO: Volatile pointer?
		public static void WriteToApic(Pointer ioApic, uint reg, uint value)
		{
			ioApic.Store32(0, reg & 0xff);
			ioApic.Store32(4, value);
		}
	}
}
