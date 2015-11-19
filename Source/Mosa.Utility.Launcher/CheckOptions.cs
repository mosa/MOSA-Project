// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.Launcher
{
	public static class CheckOptions
	{
		public static string Verify(Options options)
		{
			if (options.Emulator == EmulatorType.Qemu && options.ImageFormat == ImageFormat.VDI)
			{
				return "QEMU does not support the VDI image format";
			}

			if (options.Emulator == EmulatorType.Bochs && options.ImageFormat == ImageFormat.VDI)
			{
				return "Boches does not support the VDI image format";
			}

			if (options.Emulator == EmulatorType.Bochs && options.ImageFormat == ImageFormat.VMDK)
			{
				return "Boches does not support the VMDK image format";
			}

			if (options.Emulator == EmulatorType.VMware && options.ImageFormat == ImageFormat.IMG)
			{
				return "VMware does not support the IMG image format";
			}

			if (options.Emulator == EmulatorType.VMware && options.ImageFormat == ImageFormat.VDI)
			{
				return "VMware does not support the VHD image format";
			}

			if (options.ImageFormat != ImageFormat.ISO && options.BootLoaderType == BootLoaderType.Grub)
			{
				return "Grub boot loader not support with virtual disk formats";
			}

			if (options.ImageFormat != ImageFormat.ISO)
			{
				return "Image format and boot loader combination not supported";
			}

			if (options.PlatformType == PlatformType.NotSpecified)
			{
				return "Platform not supported";
			}

			return null;
		}
	}
}
