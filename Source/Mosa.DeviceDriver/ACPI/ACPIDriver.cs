// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.Runtime;

namespace Mosa.DeviceDriver.ACPI;

// Portions of this code are from Cosmos

//https://wiki.osdev.org/ACPI
//https://wiki.osdev.org/MADT

/// <summary>
/// A basic implementation of ACPI. It implements the <see cref="IACPI"/> interface.
/// </summary>
public class ACPIDriver : BaseDeviceDriver, IACPI
{
	private FADT fadt;
	private RSDT rsdt;
	private XSDT xsdt;
	private MADT madt;

	private IOPortWrite smiCommandPort;

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

		// TODO: Find the multiboot service
		// Multiboot.V2.RSDPv1

		var rsdp = Pointer.Zero; // HAL.GetRSDP();
		if (rsdp.IsNull)
			return;

		var version2 = true; // HAL.IsACPIVersion2();
		if (version2)
		{
			var descriptor20 = new RSDPDescriptor20(rsdp);
			xsdt = new XSDT(HAL.GetPhysicalMemory(descriptor20.XsdtAddress, 0xFFFF).Address);
		}
		else
		{
			var descriptor = new RSDPDescriptor(rsdp);
			rsdt = new RSDT(HAL.GetPhysicalMemory(descriptor.RsdtAddress, 0xFFFF).Address);
		}

		fadt = new FADT(HAL.GetPhysicalMemory(FindBySignature("FACP"), 0xFFFF).Address);
		madt = new MADT(HAL.GetPhysicalMemory(FindBySignature("APIC"), 0xFFFF).Address);

		if (fadt.Pointer.IsNull)
			return;

		if (!madt.Pointer.IsNull)
		{
			ProcessorIDs = new byte[256];
			LocalApicAddress = madt.LocalApicAddress;

			var ptr = madt.Pointer;
			var ptr2 = ptr + madt.ACPISDTHeader.Length;

			for (ptr += 0x2C; ptr < ptr2;)
			{
				var entry = new MADTEntry(ptr);

				switch (entry.Type)
				{
					case 0: // Processor Local APIC
						var plan = new ProcessorLocalAPICEntry(ptr);

						if ((plan.Flags & 1) != 0)
							ProcessorIDs[ProcessorCount++] = plan.ApicID;

						break;

					case 1: // I/O APIC
						var ipe = new IOAPICEntry(ptr);
						IOApicAddress = ipe.ApicAddress;
						break;

					case 5: // 64-bit LAPIC
						var llpe = new LongLocalAPICEntry(ptr);
						LocalApicAddress = llpe.ApicAddress;
						break;
				}

				ptr += entry.Length;
			}
		}

		var dsdt = new Pointer(fadt.Dsdt);
		if (dsdt.Load32() != 0x54445344) return; //DSDT

		var s5addr = dsdt + ACPISDTHeader.Offset.Size;
		var dsdtLength = (int)dsdt.Load32() + 1 - ACPISDTHeader.Offset.Size;

		for (var k = 0; k < dsdtLength; k++)
		{
			if (s5addr.Load32() == 0x5f35535f)
				break; //_S5_

			s5addr++;
		}

		if ((dsdtLength <= 0) || (((s5addr - 1).Load8() != 0x08 && ((s5addr - 2).Load8() != 0x08 || (s5addr - 1).Load8() != '\\'))
								  || (s5addr + 4).Load8() != 0x12)) return;
		s5addr += 5;
		s5addr += ((s5addr.Load32() & 0xC0) >> 6) + 2;

		if (s5addr.Load8() == 0x0A) s5addr++;

		SLP_TYPa = (short)(s5addr.Load16() << 10);
		s5addr++;

		if (s5addr.Load8() == 0x0A) s5addr++;

		SLP_TYPb = (short)(s5addr.Load16() << 10);
		SLP_EN = 1 << 13;

		smiCommandPort = new IOPortWrite((ushort)fadt.SMI_CommandPort);

		var has64BitPtr = false;

		if (version2)
		{
			ResetAddress = new IOPortWrite((ushort)fadt.ResetReg.Address);
			ResetValue = fadt.ResetValue;

			if (Pointer.Size == 8) // 64-bit
			{
				has64BitPtr = true;

				PM1aControlBlock = new IOPortWrite((ushort)fadt.X_PM1aControlBlock.Address);
				if (fadt.X_PM1bControlBlock.Address != 0) PM1bControlBlock = new IOPortWrite((ushort)fadt.X_PM1bControlBlock.Address);
			}
		}

		if (has64BitPtr) return;

		PM1aControlBlock = new IOPortWrite((ushort)fadt.PM1aControlBlock);
		if (fadt.PM1bControlBlock != 0) PM1bControlBlock = new IOPortWrite((ushort)fadt.PM1bControlBlock);
	}

	/// <summary>
	/// Probes this instance.
	/// </summary>
	public override void Probe() => Device.Status = DeviceStatus.Offline; // Future: Should be "Available"

	/// <summary>
	/// Starts this hardware device.
	/// </summary>
	public override void Start()
	{
		smiCommandPort.Write8(fadt.AcpiEnable);
		HAL.Sleep(3000);
		Device.Status = DeviceStatus.Online;
	}

	/// <summary>
	/// Stops this hardware device.
	/// </summary>
	public override void Stop()
	{
		smiCommandPort.Write8(fadt.AcpiDisable);
		HAL.Sleep(3000);
		Device.Status = DeviceStatus.Offline;
	}

	private Pointer FindBySignature(string signature)
	{
		var hasXSDT = !xsdt.Pointer.IsNull;
		var value = CalculateSignatureValue(signature);

		for (var i = 0U; i < (hasXSDT ? 16 : 8); i++)
		{
			// On some systems or VM software (e.g. VirtualBox), we have to map the pointer, or else it will crash.
			// See: https://github.com/msareedjr/MOSA-MikeOS/commit/6867064fedae707280083ba4d9ff12d468a6dce0
			var h = new ACPISDTHeader(HAL.GetPhysicalMemory(hasXSDT ? xsdt.GetPointerToOtherSDT(i) : rsdt.GetPointerToOtherSDT(i), 0xFFF).Address);

			if (!h.Pointer.IsNull && h.Signature == value)
				return h.Pointer;
		}

		// No SDT found (by signature)
		return Pointer.Zero;
	}

	private static int CalculateSignatureValue(string signature) =>
		((byte)signature[0] & 0xFF) | (((byte)signature[1] & 0xFF) << 8) | (((byte)signature[2] & 0xFF) << 16) | (((byte)signature[3] & 0xFF) << 24);
}
