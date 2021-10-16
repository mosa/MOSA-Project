// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to ACPI
	/// </summary>
	public interface IACPI
	{
		BaseIOPortWrite ResetAddress { get; set; }
		BaseIOPortWrite PM1aControlBlock { get; set; }
		BaseIOPortWrite PM1bControlBlock { get; set; }

		short SLP_TYPa { get; set; }
		short SLP_TYPb { get; set; }
		short SLP_EN { get; set; }

		byte ResetValue { get; set; }
	}
}
