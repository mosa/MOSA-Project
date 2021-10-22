// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to ACPI
	/// </summary>
	public interface IACPI
	{
		BaseIOPortWrite ResetAddress { get; set; }		// FIXME - Should not expose I/O ports
		BaseIOPortWrite PM1aControlBlock { get; set; }	// FIXME - Should not expose I/O ports
		BaseIOPortWrite PM1bControlBlock { get; set; }	// FIXME - Should not expose I/O ports

		short SLP_TYPa { get; set; }
		short SLP_TYPb { get; set; }
		short SLP_EN { get; set; }

		byte ResetValue { get; set; }

		byte[] ProcessorIDs { get; set; }
		int ProcessorCount { get; set; }

		uint IOApicAddress { get; set; }
		uint LocalApicAddress { get; set; }
	}
}
