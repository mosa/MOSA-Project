// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common;
using Mosa.Utility.BootImage;
using Mosa.Utility.Launcher;

namespace Mosa.Tool.GDBDebugger
{
	public class Options
	{
		[Option("port", Default = 2345)]
		public string GDBPortString
		{
			get { return GDBPort.ToString(); }
			set { GDBPort = (int)value.ParseHexOrDecimal(); }
		}

		public int GDBPort { get; set; }

		public string GDBHost { get; set; }

		[Option("connect")]
		public bool AutoConnect { get; set; }

		[Option("image")]
		public string ImageFile { get; set; }

		[Option("debugfile")]
		public string DebugInfoFile { get; set; }

		public bool LaunchEmulator { get; set; }

		public bool ExitOnLaunch { get; set; }

		public EmulatorType Emulator { get; set; }

		[Option("qemu")]
		public bool EmulatorQEMU
		{
			get { return (Emulator == EmulatorType.Qemu); }
			set { Emulator = EmulatorType.Qemu; }
		}

		[Option("vmware")]
		public bool EmulatorVMWare
		{
			get { return (Emulator == EmulatorType.VMware); }
			set { Emulator = EmulatorType.VMware; }
		}

		[Option("bochs")]
		public bool EmulatorBochs
		{
			get { return (Emulator == EmulatorType.Bochs); }
			set { Emulator = EmulatorType.Bochs; }
		}

		public ImageFormat ImageFormat { get; set; }

		[Option("vhd")]
		public bool ImageVHD
		{
			get { return (ImageFormat == ImageFormat.VHD); }
			set { ImageFormat = ImageFormat.VHD; }
		}

		[Option("img")]
		public bool ImageIMG
		{
			get { return (ImageFormat == ImageFormat.IMG); }
			set { ImageFormat = ImageFormat.IMG; }
		}

		[Option("vdi")]
		public bool ImageVDI
		{
			get { return (ImageFormat == ImageFormat.VDI); }
			set { ImageFormat = ImageFormat.VDI; }
		}

		[Option("iso")]
		public bool ImageISO
		{
			get { return (ImageFormat == ImageFormat.ISO); }
			set { ImageFormat = ImageFormat.ISO; }
		}

		[Option("vmdk")]
		public bool ImageVMDK
		{
			get { return (ImageFormat == ImageFormat.VMDK); }
			set { ImageFormat = ImageFormat.VMDK; }
		}

		public uint EmulatorMemoryInMB { get; set; }

		public PlatformType PlatformType { get; set; }

		public Options()
		{
			GDBHost = "localhost";
			AutoConnect = false;
			ImageFile = null;
			EmulatorMemoryInMB = 256;
			PlatformType = PlatformType.X86;
			ImageFormat = ImageFormat.IMG;
			Emulator = EmulatorType.Qemu;
		}
	}
}
