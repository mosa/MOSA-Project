// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;

namespace Mosa.Utility.Launcher
{
	public static class CheckOptions
	{
		public static string Verify(Settings settings)
		{
			var emulator = settings.GetValue("Emulator", string.Empty).ToLower();
			var imageformat = settings.GetValue("Image.Format", string.Empty).ToUpper();
			var bootloader = settings.GetValue("Image.BootLoader", string.Empty).ToLower();
			var platform = settings.GetValue("Compiler.Platform", string.Empty);

			if (emulator == "qemu" && imageformat == "VDI")
			{
				return "QEMU does not support the VDI image format";
			}

			if (emulator == "bochs" && imageformat == "VDI")
			{
				return "Bochs does not support the VDI image format";
			}

			if (emulator == "bochs" && imageformat == "VMDK")
			{
				return "Bochs does not support the VMDK image format";
			}

			if (emulator == "vmware" && imageformat == "IMG")
			{
				return "VMware does not support the IMG image format";
			}

			if (emulator == "vmware" && imageformat == "VDI")
			{
				return "VMware does not support the VHD image format";
			}

			if (bootloader == "grub0.97" && imageformat != "ISO")
			{
				return "Grub boot loader does not support virtual disk formats";
			}

			if (bootloader == "grub2.00" && imageformat != "ISO")
			{
				return "Grub boot loader does not support virtual disk formats";
			}

			if (bootloader == "syslinux6.03" && imageformat != "ISO")
			{
				return "Syslinux boot loader v6.03 does not support virtual disk format";
			}

			if (platform != "x86" && platform != "x64")
			{
				return "Platform not supported";
			}

			return null;
		}
	}
}
