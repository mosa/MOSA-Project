// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.Configuration;

namespace Mosa.Utility.Launcher;

public static class CheckOptions
{
	public static string Verify(MosaSettings settings)
	{
		if (settings.Platform != "x86" && settings.Platform != "x64")
		{
			return $"Platform not supported: {settings.Platform}";
		}

		return settings.Emulator switch
		{
			"bochs" when settings.ImageFormat == "vdi" => "Bochs does not support the VDI image format",
			"bochs" when settings.ImageFormat == "vmdk" => "Bochs does not support the VMDK image format",
			"vmware" when settings.ImageFormat == "img" => "VMware Workstation does not support the IMG image format",
			"vmware" when settings.ImageFormat == "vdi" => "VMware Workstation does not support the VHD image format",
			"virtualbox" when settings.ImageFormat == "img" => "VirtualBox does not support the IMG file format",
			_ => null,
		};
	}
}
