// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.Configuration;

namespace Mosa.Utility.Launcher;

public static class CheckOptions
{
	public static string Verify(MosaSettings settings)
	{
		settings.NormalizeSettings();

		if (settings.Platform != "x86" && settings.Platform != "x64")
		{
			return $"Platform not supported: {settings.Platform}";
		}

		switch (settings.Emulator)
		{
			case "qemu" when settings.ImageFolder == "vdi":
				return "QEMU does not support the VDI image format";

			case "bochs" when settings.ImageFolder == "vdi":
				return "Bochs does not support the VDI image format";

			case "bochs" when settings.ImageFolder == "vmdk":
				return "Bochs does not support the VMDK image format";

			case "vmware" when settings.ImageFolder == "img":
				return "VMware does not support the IMG image format";

			case "vmware" when settings.ImageFolder == "vdi":
				return "VMware does not support the VHD image format";

			case "virtualbox" when settings.ImageFolder == "img":
				return "VirtualBox does not support the IMG file format";
		}

		return null;
	}
}
