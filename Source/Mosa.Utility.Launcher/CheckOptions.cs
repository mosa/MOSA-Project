// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.BootImage;

namespace Mosa.Utility.Launcher
{
	public static class CheckOptions
	{
		public static string Verify(LauncherOptions options)
		{
			if (options.Emulator == EmulatorType.Qemu && options.ImageFormat == ImageFormat.VDI)
			{
				return "QEMU does not support the VDI image format";
			}

			if (options.Emulator == EmulatorType.Bochs && options.ImageFormat == ImageFormat.VDI)
			{
				return "Bochs does not support the VDI image format";
			}

			if (options.Emulator == EmulatorType.Bochs && options.ImageFormat == ImageFormat.VMDK)
			{
				return "Bochs does not support the VMDK image format";
			}

			if (options.Emulator == EmulatorType.VMware && options.ImageFormat == ImageFormat.IMG)
			{
				return "VMware does not support the IMG image format";
			}

			if (options.Emulator == EmulatorType.VMware && options.ImageFormat == ImageFormat.VDI)
			{
				return "VMware does not support the VHD image format";
			}

			if (options.BootLoader == BootLoader.Grub_0_97 && options.ImageFormat != ImageFormat.ISO)
			{
				return "Grub boot loader does not support virtual disk formats";
			}

			if (options.BootLoader == BootLoader.Grub_2_00 && options.ImageFormat != ImageFormat.ISO)
			{
				return "Grub boot loader does not support virtual disk formats";
			}

			if (options.BootLoader == BootLoader.Syslinux_6_03 && options.ImageFormat != ImageFormat.ISO)
			{
				return "Syslinux boot loader v6.03 does not support virtual disk format";
			}

			if (options.PlatformType == PlatformType.NotSpecified)
			{
				return "Platform not supported";
			}

			return null;
		}
	}
}
