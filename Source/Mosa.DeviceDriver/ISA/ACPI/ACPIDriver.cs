// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Runtime;

// Portions of this code are from Cosmos

//https://wiki.osdev.org/ACPI
//https://wiki.osdev.org/MADT

namespace Mosa.DeviceDriver.ISA.ACPI;

public unsafe class ACPIDriver : BaseDeviceDriver, IACPI
{
	private FADT FADT;
	private RSDT RSDT;
	private XSDT XSDT;
	private MADT MADT;

	private IOPortWrite SMI_CommandPort;

	public short SLP_TYPa { get; set; }

	public short SLP_TYPb { get; set; }

	public short SLP_EN { get; set; }

	public IOPortWrite ResetAddress { get; set; }

	public IOPortWrite PM1aControlBlock { get; set; }

	public IOPortWrite PM1bControlBlock { get; set; }

	public byte ResetValue { get; set; }

	public byte[] ProcessorIDs { get; set; }

	public int ProcessorCount { get; set; }

	public uint IOApicAddress { get; set; }

	public uint LocalApicAddress { get; set; }

	public override void Initialize()
	{
		Device.Name = "ACPI";

		var rsdp = HAL.GetRSDP();
		var version2 = HAL.IsACPIVersion2();

		if (version2)
		{
			var descriptor20 = new RSDPDescriptor20(rsdp);
			XSDT = new XSDT(HAL.GetPhysicalMemory(descriptor20.XsdtAddress, 0xFFFF).Address);
		}
		else
		{
			var descriptor = new RSDPDescriptor(rsdp);
			RSDT = new RSDT(HAL.GetPhysicalMemory(descriptor.RsdtAddress, 0xFFFF).Address);
		}

		FADT = new FADT(HAL.GetPhysicalMemory(FindBySignature("FACP"), 0xFFFF).Address);
		MADT = new MADT(HAL.GetPhysicalMemory(FindBySignature("APIC"), 0xFFFF).Address);

		if (FADT.Pointer.IsNull) return;

		if (!MADT.Pointer.IsNull)
		{
			ProcessorIDs = new byte[256];
			LocalApicAddress = MADT.LocalApicAddress;

			var ptr = MADT.Pointer;
			var ptr2 = ptr + MADT.ACPISDTHeader.Length;

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
						LocalApicAddress = llpe->ApicAddress;
						break;
				}

				ptr += entry->Length;
			}
		}

		var dsdt = new Pointer(FADT.Dsdt);

		if (dsdt.Load32() == 0x54445344) //DSDT
		{
			var S5Addr = dsdt + ACPISDTHeader.Offset.Size;
			var dsdtLength = (int)dsdt.Load32() + 1 - ACPISDTHeader.Offset.Size;

			for (var k = 0; k < dsdtLength; k++)
			{
				if (S5Addr.Load32() == 0x5f35535f) break; //_S5_

				S5Addr++;
			}

			if ((dsdtLength > 0)
				&& (((S5Addr - 1).Load8() == 0x08 || (S5Addr - 2).Load8() == 0x08 && (S5Addr - 1).Load8() == '\\') && (S5Addr + 4).Load8() == 0x12))
			{
				S5Addr += 5;
				S5Addr += ((S5Addr.Load32() & 0xC0) >> 6) + 2;

				if (S5Addr.Load8() == 0x0A) S5Addr++;

				SLP_TYPa = (short)(S5Addr.Load16() << 10);
				S5Addr++;

				if (S5Addr.Load8() == 0x0A) S5Addr++;

				SLP_TYPb = (short)(S5Addr.Load16() << 10);
				SLP_EN = 1 << 13;

				SMI_CommandPort = new IOPortWrite((ushort)FADT.SMI_CommandPort);

				var has64BitPtr = false;

				if (version2)
				{
					ResetAddress = new IOPortWrite((ushort)FADT.ResetReg.Address);
					ResetValue = FADT.ResetValue;

					if (Pointer.Size == 8) // 64-bit
					{
						has64BitPtr = true;

						PM1aControlBlock = new IOPortWrite((ushort)FADT.X_PM1aControlBlock.Address);
						if (FADT.X_PM1bControlBlock.Address != 0) PM1bControlBlock = new IOPortWrite((ushort)FADT.X_PM1bControlBlock.Address);
					}
				}

				if (!has64BitPtr)
				{
					PM1aControlBlock = new IOPortWrite((ushort)FADT.PM1aControlBlock);
					if (FADT.PM1bControlBlock != 0) PM1bControlBlock = new IOPortWrite((ushort)FADT.PM1bControlBlock);
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
		SMI_CommandPort.Write8(FADT.AcpiEnable);
		HAL.Sleep(3000);
		Device.Status = DeviceStatus.Online;
	}

	/// <summary>
	/// Stops this hardware device.
	/// </summary>
	public override void Stop()
	{
		SMI_CommandPort.Write8(FADT.AcpiDisable);
		HAL.Sleep(3000);
		Device.Status = DeviceStatus.Offline;
	}

	private Pointer FindBySignature(string signature)
	{
		var xsdt = !XSDT.Pointer.IsNull;
		var value = CalculateSignatureValue(signature);

		for (var i = 0U; i < (xsdt ? 16 : 8); i++)
		{
			// On some systems or VM software (e.g. VirtualBox), we have to map the pointer, or else it will crash.
			// See: https://github.com/msareedjr/MOSA-MikeOS/commit/6867064fedae707280083ba4d9ff12d468a6dce0
			var h = new ACPISDTHeader(HAL.GetPhysicalMemory(xsdt ? XSDT.GetPointerToOtherSDT(i) : RSDT.GetPointerToOtherSDT(i), 0xFFF).Address);

			if (!h.Pointer.IsNull && h.Signature == value) return h.Pointer;
		}

		// No SDT found (by signature)
		return Pointer.Zero;
	}

	private static int CalculateSignatureValue(string signature)
		=> ((byte)signature[0] & 0xFF) | (((byte)signature[1] & 0xFF) << 8) | (((byte)signature[2] & 0xFF) << 16) | (((byte)signature[3] & 0xFF) << 24);
}
