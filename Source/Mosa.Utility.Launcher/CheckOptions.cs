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

		return settings.Emulator switch
		{
			"bochs" when settings.ImageFolder == "vdi" => "Bochs does not support the VDI image format",
			"bochs" when settings.ImageFolder == "vmdk" => "Bochs does not support the VMDK image format",
			"vmware" when settings.ImageFolder == "img" => "VMware does not support the IMG image format",
			"vmware" when settings.ImageFolder == "vdi" => "VMware does not support the VHD image format",
			"virtualbox" when settings.ImageFolder == "img" => "VirtualBox does not support the IMG file format",
			_ => null,
		};
	}
}
