// Copyright (c) MOSA Project. Licensed under the New BSD License.
namespace Mosa.DeviceSystem.PCI;

/// <summary>
/// Provides the main commands to use in PCI registers, like the command register.
/// </summary>
public struct PCICommand
{
	public const ushort IOSpaceEnable = 0x1; // Enable response in I/O space
	public const ushort MemorySpaceEnable = 0x2; //  Enable response in memory space
	public const ushort BusMasterFunctionEnable = 0x4; //  Enable bus mastering
	public const ushort SpecialCycleEnable = 0x8; //  Enable response to special cycles
	public const ushort MemoryWriteandInvalidateEnable = 0x10; //  Use memory write and invalidate

	//public const ushort VGA_Pallete = 0x20; //  Enable palette snooping
	//public const ushort Parity = 0x40; //  Enable parity checking
	//public const ushort Wait = 0x80; //  Enable address/data stepping
	//public const ushort SERR = 0x100; //  Enable SERR
	//public const ushort Fast_Back = 0x200; //  Enable back-to-back writes
}
