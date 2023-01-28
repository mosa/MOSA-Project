// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;
using Mosa.DeviceSystem;
using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA;
// Portions of this code are from Cosmos

//https://wiki.osdev.org/ACPI
//https://wiki.osdev.org/MADT

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct RSDPDescriptor
{
	public fixed sbyte Signature[8];
	public readonly byte Checksum;
	public fixed sbyte OEMID[6];
	public readonly byte Revision;
	public readonly uint RsdtAddress;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct RSDPDescriptor20
{
	public RSDPDescriptor FirstPart;

	public readonly uint Length;
	public readonly ulong XsdtAddress;
	public readonly byte ExtendedChecksum;
	public fixed byte Reserved[3];
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct ACPISDTHeader
{
	public fixed sbyte Signature[4];
	public readonly uint Length;
	public readonly byte Revision;
	public readonly byte Checksum;
	public fixed sbyte OEMID[6];
	public fixed sbyte OEMTableID[8];
	public readonly uint OEMRevision;
	public readonly uint CreatorID;
	public readonly uint CreatorRevision;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct FADT
{
	public ACPISDTHeader h;
	public readonly uint FirmwareCtrl;
	public readonly uint Dsdt;

	// Field used in ACPI 1.0; no longer in use, for compatibility only
	public readonly byte Reserved;

	public readonly byte PreferredPowerManagementProfile;
	public readonly ushort SCI_Interrupt;
	public readonly uint SMI_CommandPort;
	public readonly byte AcpiEnable;
	public readonly byte AcpiDisable;
	public readonly byte S4BIOS_REQ;
	public readonly byte PSTATE_Control;
	public readonly uint PM1aEventBlock;
	public readonly uint PM1bEventBlock;
	public readonly uint PM1aControlBlock;
	public readonly uint PM1bControlBlock;
	public readonly uint PM2ControlBlock;
	public readonly uint PMTimerBlock;
	public readonly uint GPE0Block;
	public readonly uint GPE1Block;
	public readonly byte PM1EventLength;
	public readonly byte PM1ControlLength;
	public readonly byte PM2ControlLength;
	public readonly byte PMTimerLength;
	public readonly byte GPE0Length;
	public readonly byte GPE1Length;
	public readonly byte GPE1Base;
	public readonly byte CStateControl;
	public readonly ushort WorstC2Latency;
	public readonly ushort WorstC3Latency;
	public readonly ushort FlushSize;
	public readonly ushort FlushStride;
	public readonly byte DutyOffset;
	public readonly byte DutyWidth;
	public readonly byte DayAlarm;
	public readonly byte MonthAlarm;
	public readonly byte Century;

	// Reserved in ACPI 1.0; used since ACPI 2.0+
	public readonly ushort BootArchitectureFlags;

	public readonly byte Reserved2;
	public readonly uint Flags;

	// 12 byte structure; see below for details
	public GenericAddressStructure ResetReg;

	public readonly byte ResetValue;
	public fixed byte Reserved3[3];

	// 64bit pointers - Available on ACPI 2.0+
	public readonly ulong X_FirmwareControl;

	public readonly ulong X_Dsdt;

	public GenericAddressStructure X_PM1aEventBlock;
	public GenericAddressStructure X_PM1bEventBlock;
	public GenericAddressStructure X_PM1aControlBlock;
	public GenericAddressStructure X_PM1bControlBlock;
	public GenericAddressStructure X_PM2ControlBlock;
	public GenericAddressStructure X_PMTimerBlock;
	public GenericAddressStructure X_GPE0Block;
	public GenericAddressStructure X_GPE1Block;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct GenericAddressStructure
{
	public readonly byte AddressSpace;
	public readonly byte BitWidth;
	public readonly byte BitOffset;
	public readonly byte AccessSize;
	public readonly ulong Address;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct RSDT
{
	public ACPISDTHeader h;
	public fixed uint PointerToOtherSDT[8];
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct XSDT
{
	public ACPISDTHeader h;
	public fixed ulong PointerToOtherSDT[16];
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct MADT
{
	public ACPISDTHeader h;

	public readonly uint LocalApicAddress;
	public readonly uint Flags;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct MADTEntry
{
	public readonly byte Type;
	public readonly byte Length;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct ProcessorLocalAPICEntry
{
	public MADTEntry Entry;

	public readonly byte AcpiProcessorID;
	public readonly byte ApicID;

	public readonly uint Flags;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct IOAPICEntry
{
	public MADTEntry Entry;

	public readonly byte ApicID;
	public readonly byte Reserved;

	public readonly uint ApicAddress;
	public readonly uint GlobalSystemInterruptBase;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct LongLocalAPICEntry
{
	public MADTEntry Entry;

	public readonly sbyte Reserved;

	public readonly ulong ApicAddress;
}

public unsafe class ACPI : BaseDeviceDriver, IACPI
{
	private RSDPDescriptor* Descriptor;
	private RSDPDescriptor20* Descriptor20;

	private FADT* FADT;
	private RSDT* RSDT;
	private XSDT* XSDT;
	private MADT* MADT;

	private BaseIOPortWrite SMI_CommandPort;

	public short SLP_TYPa { get; set; }
	public short SLP_TYPb { get; set; }
	public short SLP_EN { get; set; }

	public BaseIOPortWrite ResetAddress { get; set; }
	public BaseIOPortWrite PM1aControlBlock { get; set; }
	public BaseIOPortWrite PM1bControlBlock { get; set; }

	public byte ResetValue { get; set; }

	public byte[] ProcessorIDs { get; set; }
	public int ProcessorCount { get; set; }

	public uint IOApicAddress { get; set; }
	public uint LocalApicAddress { get; set; }

	public override void Initialize()
	{
		Device.Name = "ACPI";

		Pointer rsdpPtr;

		for (uint addr = 0x000E0000; addr <= 0x000FFFFF; addr += 8)
		{
			rsdpPtr = (Pointer)addr;

			if (rsdpPtr.Load64() == 0x2052545020445352) // 'RSD PTR '
				Descriptor = (RSDPDescriptor*)rsdpPtr;
		}

		if (Descriptor == null)
			return;

		/*if (Descriptor->Revision == 2) // ACPI v2.0+
		{
			Descriptor20 = (RSDPDescriptor20*)rsdpPtr;
			XSDT = (XSDT*)HAL.GetPhysicalMemory((Pointer)Descriptor20->XsdtAddress, 0xFFFF).Address;
		} else */
		RSDT = (RSDT*)HAL.GetPhysicalMemory((Pointer)Descriptor->RsdtAddress, 0xFFFF).Address;

		FADT = (FADT*)HAL.GetPhysicalMemory(FindBySignature("FACP"), 0xFFFF).Address;
		MADT = (MADT*)HAL.GetPhysicalMemory(FindBySignature("APIC"), 0xFFFF).Address;

		if (FADT == null)
			return;

		if (MADT != null)
		{
			ProcessorIDs = new byte[256];
			LocalApicAddress = MADT->LocalApicAddress;

			var ptr = (Pointer)MADT;
			var ptr2 = ptr + MADT->h.Length;

			for (ptr += 0x2C; ptr < ptr2;)
			{
				var entry = (MADTEntry*)ptr;

				switch (entry->Type)
				{
					case 0: // Processor Local APIC
						var plan = (ProcessorLocalAPICEntry*)ptr;
						if ((plan->Flags & 1) != 0)
							ProcessorIDs[ProcessorCount++] = plan->ApicID;
						break;

					case 1: // I/O APIC
						var ipe = (IOAPICEntry*)ptr;
						IOApicAddress = ipe->ApicAddress;
						break;

					case 5: // 64-bit LAPIC
						var llpe = (LongLocalAPICEntry*)ptr;
						LocalApicAddress = (uint)llpe->ApicAddress;
						break;
				}

				ptr += entry->Length;
			}
		}

		var dsdt = (Pointer)FADT->Dsdt;

		if (dsdt.Load32() == 0x54445344) //DSDT
		{
			var S5Addr = dsdt + sizeof(ACPISDTHeader);
			var dsdtLength = (int)dsdt.Load32() + 1 - sizeof(ACPISDTHeader);

			for (var k = 0; k < dsdtLength; k++)
			{
				if (S5Addr.Load32() == 0x5f35535f) //_S5_
					break;
				S5Addr++;
			}

			if (dsdtLength > 0)
				if (((S5Addr - 1).Load8() == 0x08 || ((S5Addr - 2).Load8() == 0x08 && (S5Addr - 1).Load8() == '\\')) && (S5Addr + 4).Load8() == 0x12)
				{
					S5Addr += 5;
					S5Addr += ((S5Addr.Load32() & 0xC0) >> 6) + 2;
					if (S5Addr.Load8() == 0x0A)
						S5Addr++;
					SLP_TYPa = (short)(S5Addr.Load16() << 10);
					S5Addr++;
					if (S5Addr.Load8() == 0x0A)
						S5Addr++;
					SLP_TYPb = (short)(S5Addr.Load16() << 10);
					SLP_EN = 1 << 13;

					SMI_CommandPort = HAL.GetWriteIOPort((ushort)FADT->SMI_CommandPort);

					var has64BitPtr = false;

					if (Descriptor->Revision == 2)
					{
						ResetAddress = HAL.GetWriteIOPort((ushort)FADT->ResetReg.Address);
						ResetValue = FADT->ResetValue;

						if (Pointer.Size == 8) // 64-bit
						{
							has64BitPtr = true;

							PM1aControlBlock = HAL.GetWriteIOPort((ushort)FADT->X_PM1aControlBlock.Address);
							if (FADT->X_PM1bControlBlock.Address != 0)
								PM1bControlBlock = HAL.GetWriteIOPort((ushort)FADT->X_PM1bControlBlock.Address);
						}
					}

					if (!has64BitPtr)
					{
						PM1aControlBlock = HAL.GetWriteIOPort((ushort)FADT->PM1aControlBlock);
						if (FADT->PM1bControlBlock != 0)
							PM1bControlBlock = HAL.GetWriteIOPort((ushort)FADT->PM1bControlBlock);
					}
				}
		}
	}

	/// <summary>
	/// Probes this instance.
	/// </summary>
	/// <remarks>
	/// Override for ISA devices, if example
	/// </remarks>
	public override void Probe() => Device.Status = DeviceStatus.Available;

	/// <summary>
	/// Starts this hardware device.
	/// </summary>
	public override void Start()
	{
		SMI_CommandPort.Write8(FADT->AcpiEnable);
		HAL.Sleep(3000);
		Device.Status = DeviceStatus.Online;
	}

	/// <summary>
	/// Stops this hardware device.
	/// </summary>
	public override void Stop()
	{
		SMI_CommandPort.Write8(FADT->AcpiDisable);
		HAL.Sleep(3000);
		Device.Status = DeviceStatus.Offline;
	}

	private Pointer FindBySignature(string signature)
	{
		var xsdt = XSDT != null;

		for (var i = 0; i < (xsdt ? 16 : 8); i++)
		{
			// On some systems or VM software (e.g. VirtualBox), we have to map the pointer, or else it will crash.
			// See: https://github.com/msareedjr/MOSA-MikeOS/commit/6867064fedae707280083ba4d9ff12d468a6dce0
			var h = (ACPISDTHeader*)HAL.GetPhysicalMemory((Pointer)(xsdt ? XSDT->PointerToOtherSDT[i] : RSDT->PointerToOtherSDT[i]), 0xfff).Address;

			if (h != null &&
			    h->Signature[0] == signature[0] &&
			    h->Signature[1] == signature[1] &&
			    h->Signature[2] == signature[2] &&
			    h->Signature[3] == signature[3])
				return (Pointer)h;
		}

		// No SDT found (by signature)
		return Pointer.Zero;
	}
}
