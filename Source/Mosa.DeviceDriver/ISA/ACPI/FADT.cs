﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct FADT
{
	private readonly Pointer Entry;

	public readonly Pointer Pointer => Entry;

	public readonly uint Size = Offset.Size;

	internal static class Offset
	{
		public const int h = 0;
		public const int FirmwareCtrl = h + ACPISDTHeader.Offset.Size;
		public const int Dsdt = FirmwareCtrl + 4;
		public const int Reserved = Dsdt + 4;
		public const int PreferredPowerManagementProfile = Reserved + 1;
		public const int SCI_Interrupt = PreferredPowerManagementProfile + 1;
		public const int SMI_CommandPort = SCI_Interrupt + 2;
		public const int AcpiEnable = SMI_CommandPort + 4;
		public const int AcpiDisable = AcpiEnable + 1;
		public const int S4BIOS_REQ = AcpiDisable + 1;
		public const int PSTATE_Control = S4BIOS_REQ + 1;
		public const int PM1aEventBlock = PSTATE_Control + 1;
		public const int PM1bEventBlock = PM1aEventBlock + 4;
		public const int PM1aControlBlock = PM1bEventBlock + 4;
		public const int PM1bControlBlock = PM1aControlBlock + 4;
		public const int PM2ControlBlock = PM1bControlBlock + 4;
		public const int PMTimerBlock = PM2ControlBlock + 4;
		public const int GPE0Block = PMTimerBlock + 4;
		public const int GPE1Block = GPE0Block + 4;
		public const int PM1EventLength = GPE1Block + 4;
		public const int PM1ControlLength = PM1EventLength + 1;
		public const int PM2ControlLength = PM1ControlLength + 1;
		public const int PMTimerLength = PM2ControlLength + 1;
		public const int GPE0Length = PMTimerLength + 1;
		public const int GPE1Length = GPE0Length + 1;
		public const int GPE1Base = GPE1Length + 1;
		public const int CStateControl = GPE1Base + 1;
		public const int WorstC2Latency = CStateControl + 1;
		public const int WorstC3Latency = WorstC2Latency + 2;
		public const int FlushSize = WorstC3Latency + 2;
		public const int FlushStride = FlushSize + 2;
		public const int DutyOffset = FlushStride + 2;
		public const int DutyWidth = DutyOffset + 1;
		public const int DayAlarm = DutyWidth + 1;
		public const int MonthAlarm = DayAlarm + 1;
		public const int Century = MonthAlarm + 1;
		public const int BootArchitectureFlags = Century + 1;
		public const int Reserved2 = BootArchitectureFlags + 2;
		public const int Flags = Reserved2 + 1;
		public const int ResetReg = Flags + 4;
		public const int ResetValue = ResetReg + GenericAddressStructure.Offset.Size;
		public const int Reserved3 = ResetValue + 1;
		public const int X_FirmwareControl = Reserved3 + 3;
		public const int X_Dsdt = X_FirmwareControl + 8;
		public const int X_PM1aEventBlock = X_Dsdt + 8;
		public const int X_PM1bEventBlock = X_PM1aEventBlock + GenericAddressStructure.Offset.Size;
		public const int X_PM1aControlBlock = X_PM1bEventBlock + GenericAddressStructure.Offset.Size;
		public const int X_PM1bControlBlock = X_PM1aControlBlock + GenericAddressStructure.Offset.Size;
		public const int X_PM2ControlBlock = X_PM1bControlBlock + GenericAddressStructure.Offset.Size;
		public const int X_PMTimerBlock = X_PM2ControlBlock + GenericAddressStructure.Offset.Size;
		public const int X_GPE0Block = X_PMTimerBlock + GenericAddressStructure.Offset.Size;
		public const int X_GPE1Block = X_GPE0Block + GenericAddressStructure.Offset.Size;
		public const int Size = X_GPE1Block + GenericAddressStructure.Offset.Size;
	}

	public FADT(Pointer entry) => Entry = entry;

	public readonly ACPISDTHeader ACPISDTHeader => new ACPISDTHeader(Entry + Offset.h);

	public GenericAddressStructure ResetReg => new GenericAddressStructure(Entry + Offset.ResetReg);

	public GenericAddressStructure X_PM1aControlBlock => new GenericAddressStructure(Entry + Offset.X_PM1aControlBlock);

	public GenericAddressStructure X_PM1bControlBlock => new GenericAddressStructure(Entry + Offset.X_PM1bControlBlock);

	public readonly uint FirmwareCtrl => Entry.Load32(Offset.FirmwareCtrl);

	public readonly uint Dsdt => Entry.Load32(Offset.Dsdt);

	public readonly byte Reserved => Entry.Load8(Offset.Reserved);

	public readonly byte PreferredPowerManagementProfile => Entry.Load8(Offset.PreferredPowerManagementProfile);

	public readonly ushort SCI_Interrupt => Entry.Load16(Offset.SCI_Interrupt);

	public readonly uint SMI_CommandPort => Entry.Load32(Offset.SMI_CommandPort);

	public readonly byte AcpiEnable => Entry.Load8(Offset.AcpiEnable);

	public readonly byte AcpiDisable => Entry.Load8(Offset.AcpiDisable);

	public readonly byte S4BIOS_REQ => Entry.Load8(Offset.S4BIOS_REQ);

	public readonly byte PSTATE_Control => Entry.Load8(Offset.PSTATE_Control);

	public readonly uint PM1aEventBlock => Entry.Load32(Offset.PM1aEventBlock);

	public readonly uint PM1bEventBlock => Entry.Load32(Offset.PM1bEventBlock);

	public readonly uint PM1aControlBlock => Entry.Load32(Offset.PM1aControlBlock);

	public readonly uint PM1bControlBlock => Entry.Load32(Offset.PM1bControlBlock);

	public readonly uint PM2ControlBlock => Entry.Load32(Offset.PM2ControlBlock);

	public readonly uint PMTimerBlock => Entry.Load32(Offset.PMTimerBlock);

	public readonly uint GPE0Block => Entry.Load32(Offset.GPE0Block);

	public readonly uint GPE1Block => Entry.Load32(Offset.GPE1Block);

	public readonly byte PM1EventLength => Entry.Load8(Offset.PM1EventLength);

	public readonly byte PM1ControlLength => Entry.Load8(Offset.PM1ControlLength);

	public readonly byte PM2ControlLength => Entry.Load8(Offset.PM2ControlLength);

	public readonly byte PMTimerLength => Entry.Load8(Offset.PMTimerLength);

	public readonly byte GPE0Length => Entry.Load8(Offset.GPE0Length);

	public readonly byte GPE1Length => Entry.Load8(Offset.GPE1Length);

	public readonly byte GPE1Base => Entry.Load8(Offset.GPE1Base);

	public readonly byte CStateControl => Entry.Load8(Offset.CStateControl);

	public readonly ushort WorstC2Latency => Entry.Load16(Offset.WorstC2Latency);

	public readonly ushort WorstC3Latency => Entry.Load16(Offset.WorstC3Latency);

	public readonly ushort FlushSize => Entry.Load16(Offset.FlushSize);

	public readonly ushort FlushStride => Entry.Load16(Offset.FlushStride);

	public readonly byte DutyOffset => Entry.Load8(Offset.DutyOffset);

	public readonly byte DutyWidth => Entry.Load8(Offset.DutyWidth);

	public readonly byte DayAlarm => Entry.Load8(Offset.DayAlarm);

	public readonly byte MonthAlarm => Entry.Load8(Offset.MonthAlarm);

	public readonly byte Century => Entry.Load8(Offset.Century);

	public readonly ushort BootArchitectureFlags => Entry.Load16(Offset.BootArchitectureFlags);

	public readonly byte Reserved2 => Entry.Load8(Offset.Reserved2);

	public readonly uint Flags => Entry.Load32(Offset.Flags);

	public readonly byte ResetValue => Entry.Load8(Offset.ResetValue);

	public readonly ulong X_FirmwareControl => Entry.Load64(Offset.X_FirmwareControl);

	public readonly ulong X_Dsdt => Entry.Load64(Offset.X_Dsdt);
}