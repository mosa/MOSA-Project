using Mosa.DeviceSystem;
using Mosa.Runtime;
using System.Runtime.InteropServices;

namespace Mosa.DeviceDriver.ISA
{
	// Portions of this code are from Cosmos
	// This code only really works on x86, we need to access other addresses to make it work on x86-64

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
		public fixed uint PointerToOtherSDT[8]; // This is problematic, we need a way to statically initialize this array's size with h.Length and stuff

		/*public void Init()
		{
			fixed (uint* ptr = new uint[(int)((h.Length - sizeof(ACPISDTHeader)) / 4)])
				PointerToOtherSDT = ptr;
		}*/
	}

	public unsafe class ACPI : BaseDeviceDriver
	{
		private RSDPDescriptor* Descriptor;
		private FADT* FADT;
		private RSDT* RSDT;

		private short SLP_TYPa;
		private short SLP_TYPb;
		private short SLP_EN;

		private BaseIOPortWrite SMI_CommandPort;
		private BaseIOPortWrite PM1aControlBlock;
		private BaseIOPortWrite PM1bControlBlock;

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

			RSDT = (RSDT*)HAL.GetPhysicalMemory((Pointer)Descriptor->RsdtAddress, 0xFFFF).Address;

			// Won't work, we need to initialize array the same time we cast to RSDT*
			//RSDT->Init();

			FADT = (FADT*)HAL.GetPhysicalMemory((Pointer)FindBySignature(RSDT, "FACP"), 0xFFFF).Address;
			if (FADT == null)
				return;

			Pointer dsdt = (Pointer)FADT->Dsdt;

			if (dsdt.Load32() == 0x54445344) //DSDT
			{
				Pointer S5Addr = dsdt + sizeof(ACPISDTHeader);
				int dsdtLength = (int)dsdt.Load32() + 1 - sizeof(ACPISDTHeader);

				for (int k = 0; k < dsdtLength; k++)
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
						PM1aControlBlock = HAL.GetWriteIOPort((ushort)FADT->PM1aControlBlock);
						PM1bControlBlock = HAL.GetWriteIOPort((ushort)FADT->PM1bControlBlock);
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

		public void Shutdown()
		{
			PM1aControlBlock.Write16((ushort)(SLP_TYPa | SLP_EN));
			if (FADT->PM1bControlBlock != 0)
				PM1bControlBlock.Write16((ushort)(SLP_TYPb | SLP_EN));

			HAL.Pause();
			HAL.Abort("ACPI shutdown failed!");
		}

		private void* FindBySignature(void* RootSDT, string signature)
		{
			RSDT* rsdt = (RSDT*)RootSDT;

			//int entries = (int)((rsdt->h.Length - sizeof(ACPISDTHeader)) / 4);
			int entries = 8;
			for (int i = 0; i < entries; i++)
			{
				ACPISDTHeader* h = (ACPISDTHeader*)rsdt->PointerToOtherSDT[i];

				if (h != null && ToStringFromCharPointer(4, h->Signature) == signature)
					return h;
			}

			// No SDT found (by signature)
			return null;
		}

		private string ToStringFromCharPointer(int length, sbyte* pointer)
		{
			string str = string.Empty;

			for (int i = 0; i < length; i++)
				str += (char)pointer[i];

			return str;
		}
	}
}
