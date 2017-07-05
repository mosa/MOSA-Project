// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Utility.BootImage;
using Mosa.Utility.Launcher;

namespace Mosa.Tool.GDBDebugger
{
	public class Options
	{
		public int GDBPort { get; set; }

		public bool AutoConnect { get; set; }

		public string ImageFile { get; set; }

		public string DebugInfoFile { get; set; }

		public bool LaunchEmulator { get; set; }

		public bool ExitOnLaunch { get; set; }

		public EmulatorType Emulator { get; set; }

		public ImageFormat ImageFormat { get; set; }

		public uint EmulatorMemoryInMB { get; set; }

		public PlatformType PlatformType { get; set; }

		public Options()
		{
			GDBPort = 2345;
			AutoConnect = false;
			ImageFile = null;
			EmulatorMemoryInMB = 256;
			PlatformType = PlatformType.X86;
			ImageFormat = ImageFormat.IMG;
			Emulator = EmulatorType.Qemu;
		}

		public void LoadArguments(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				var arg = args[i];

				switch (arg.ToLower())
				{
					case "-port": GDBPort = (int)args[++i].ParseHexOrDecimal(); continue;
					case "-image": ImageFile = args[++i]; continue;
					case "-connect": AutoConnect = true; continue;
					case "-debugfile": DebugInfoFile = args[++i]; continue;
					case "-qemu": Emulator = EmulatorType.Qemu; continue;
					case "-vmware": Emulator = EmulatorType.VMware; continue;
					case "-bochs": Emulator = EmulatorType.Bochs; continue;
					case "-vhd": ImageFormat = ImageFormat.VHD; continue;
					case "-img": ImageFormat = ImageFormat.IMG; continue;
					case "-vdi": ImageFormat = ImageFormat.VDI; continue;
					case "-iso": ImageFormat = ImageFormat.ISO; continue;
					case "-vmdk": ImageFormat = ImageFormat.VMDK; continue;
					default: break;
				}
			}
		}
	}
}
