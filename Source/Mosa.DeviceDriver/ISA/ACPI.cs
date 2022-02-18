// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Runtime;
using System.Runtime.InteropServices;

namespace Mosa.DeviceDriver.ISA
{
	// Portions of this code are from Cosmos

	//https://wiki.osdev.org/ACPI
	//https://wiki.osdev.org/MADT

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct RSDPDescriptor
	{
		public fixed sbyte Signature[8];
		public byte Checksum;
		public fixed sbyte OEMID[6];
		public byte Revision;
		public uint RsdtAddress;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct RSDPDescriptor20
	{
		public RSDPDescriptor FirstPart;

		public uint Length;
		public ulong XsdtAddress;
		public byte ExtendedChecksum;
		public fixed byte Reserved[3];
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct ACPISDTHeader
	{
		public fixed sbyte Signature[4];
		public uint Length;
		public byte Revision;
		public byte Checksum;
		public fixed sbyte OEMID[6];
		public fixed sbyte OEMTableID[8];
		public uint OEMRevision;
		public uint CreatorID;
		public uint CreatorRevision;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct FADT
	{
		public ACPISDTHeader h;
		public uint FirmwareCtrl;
		public uint Dsdt;

		// Field used in ACPI 1.0; no longer in use, for compatibility only
		public byte Reserved;

		public byte PreferredPowerManagementProfile;
		public ushort SCI_Interrupt;
		public uint SMI_CommandPort;
		public byte AcpiEnable;
		public byte AcpiDisable;
		public byte S4BIOS_REQ;
		public byte PSTATE_Control;
		public uint PM1aEventBlock;
		public uint PM1bEventBlock;
		public uint PM1aControlBlock;
		public uint PM1bControlBlock;
		public uint PM2ControlBlock;
		public uint PMTimerBlock;
		public uint GPE0Block;
		public uint GPE1Block;
		public byte PM1EventLength;
		public byte PM1ControlLength;
		public byte PM2ControlLength;
		public byte PMTimerLength;
		public byte GPE0Length;
		public byte GPE1Length;
		public byte GPE1Base;
		public byte CStateControl;
		public ushort WorstC2Latency;
		public ushort WorstC3Latency;
		public ushort FlushSize;
		public ushort FlushStride;
		public byte DutyOffset;
		public byte DutyWidth;
		public byte DayAlarm;
		public byte MonthAlarm;
		public byte Century;

		// Reserved in ACPI 1.0; used since ACPI 2.0+
		public ushort BootArchitectureFlags;

		public byte Reserved2;
		public uint Flags;

		// 12 byte structure; see below for details
		public GenericAddressStructure ResetReg;

		public byte ResetValue;
		public fixed byte Reserved3[3];

		// 64bit pointers - Available on ACPI 2.0+
		public ulong X_FirmwareControl;

		public ulong X_Dsdt;

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
		public byte AddressSpace;
		public byte BitWidth;
		public byte BitOffset;
		public byte AccessSize;
		public ulong Address;
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

		public uint LocalApicAddress;
		public uint Flags;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct MADTEntry
	{
		public byte Type;
		public byte Length;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct ProcessorLocalAPICEntry
	{
		public MADTEntry Entry;

		public byte AcpiProcessorID;
		public byte ApicID;

		public uint Flags;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct IOAPICEntry
	{
		public MADTEntry Entry;

		public byte ApicID;
		public byte Reserved;

		public uint ApicAddress;
		public uint GlobalSystemInterruptBase;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct LongLocalAPICEntry
	{
		public MADTEntry Entry;

		public sbyte Reserved;

		public ulong ApicAddress;
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
			} else */RSDT = (RSDT*)HAL.GetPhysicalMemory((Pointer)Descriptor->RsdtAddress, 0xFFFF).Address;

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
				var h = (ACPISDTHeader*)(xsdt ? XSDT->PointerToOtherSDT[i] : RSDT->PointerToOtherSDT[i]);

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
}
