// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Services;

namespace Mosa.DeviceSystem.HardwareAbstraction;

/// <summary>
/// An interface used for controlling ACPI in Mosa.DeviceSystem. It is most notably used by the <see cref="PCService"/> in order to shut
/// down and reboot the system.
/// </summary>
public interface IACPI
{
	IOPortWrite ResetAddress { get; set; }

	IOPortWrite PM1aControlBlock { get; set; }  // FIXME - Should not expose I/O ports

	IOPortWrite PM1bControlBlock { get; set; }  // FIXME - Should not expose I/O ports

	short SLP_TYPa { get; set; }

	short SLP_TYPb { get; set; }

	short SLP_EN { get; set; }

	byte ResetValue { get; set; }

	byte[] ProcessorIDs { get; set; }

	int ProcessorCount { get; set; }

	uint IOApicAddress { get; set; }

	uint LocalApicAddress { get; set; }
}
