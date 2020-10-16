// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.BootImage
{
	public enum ImageFormat
	{
		NotSpecified,
		BIN, // Pure Kernel image. Used by Qemu Kernel Direct Boot.
		IMG,
		VHD,
		VDI,
		ISO,
		VMDK
	};
}
