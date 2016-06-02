// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.BootImage;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Utility.Launcher
{
	public class Starter : BaseLauncher
	{
		public IStarterEvent LauncherEvent { get; private set; }

		public string ImageFile { get; private set; }

		public Starter(Options options, AppLocations appLocations, string imagefile, IStarterEvent launcherEvent)
			: base(options, appLocations)
		{
			Options = options;
			AppLocations = appLocations;
			ImageFile = imagefile;
			LauncherEvent = launcherEvent;
		}

		protected override void OutputEvent(string status)
		{
			if (LauncherEvent != null)
				LauncherEvent.NewStatus(status);
		}

		public void Launch()
		{
			switch (Options.Emulator)
			{
				case EmulatorType.Qemu: LaunchQemu(Options.ExitOnLaunch); break;
				case EmulatorType.Bochs: LaunchBochs(Options.ExitOnLaunch); break;
				case EmulatorType.VMware: LaunchVMwarePlayer(Options.ExitOnLaunch); break;
				default: throw new InvalidOperationException();
			}
		}

		private Process LaunchQemu(bool exit)
		{
			string arg = " -L " + Quote(AppLocations.QEMUBIOSDirectory);

			if (Options.ImageFormat == ImageFormat.ISO)
			{
				arg = arg + " -cdrom " + Quote(ImageFile);
			}
			else
			{
				arg = arg + " -hda " + Quote(ImageFile);
			}

			if (Options.PlatformType == PlatformType.X86)
			{
				arg = arg + " -cpu qemu32,+sse4.1";
			}

			//arg = arg + " -vga vmware";

			if (Options.DebugConnectionOption == DebugConnectionOption.Pipe)
			{
				arg = arg + " -serial pipe:MOSA";
			}
			else if (Options.DebugConnectionOption == DebugConnectionOption.TCPServer)
			{
				arg = arg + " -serial tcp:127.0.0.1:9999,server,nowait,nodelay";
			}
			else if (Options.DebugConnectionOption == DebugConnectionOption.TCPClient)
			{
				arg = arg + " -serial tcp:127.0.0.1:9999,client,nowait";
			}

			return LaunchApplication(AppLocations.QEMU, arg, exit);
		}

		private Process LaunchBochs(bool exit)
		{
			var logfile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + "-bochs.log");
			var configfile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".bxrc");
			var exeDir = Path.GetDirectoryName(AppLocations.BOCHS);

			var fileVersionInfo = FileVersionInfo.GetVersionInfo(AppLocations.BOCHS);

			// simd or sse
			var simd = "simd";

			if (!(fileVersionInfo.FileMajorPart >= 2 && fileVersionInfo.FileMinorPart >= 6 && fileVersionInfo.FileBuildPart >= 5))
				simd = "sse";

			var sb = new StringBuilder();

			sb.AppendLine("megs: " + Options.MemoryInMB.ToString());
			sb.AppendLine("ata0: enabled=1,ioaddr1=0x1f0,ioaddr2=0x3f0,irq=14");
			sb.AppendLine("cpuid: mmx=1,sep=1," + simd + "=sse4_2,apic=xapic,aes=1,movbe=1,xsave=1");
			sb.AppendLine("boot: cdrom,disk");
			sb.AppendLine("log: " + Quote(logfile));
			sb.AppendLine("romimage: file=" + Quote(Path.Combine(exeDir, "BIOS-bochs-latest")));
			sb.AppendLine("vgaromimage: file=" + Quote(Path.Combine(exeDir, "VGABIOS-lgpl-latest")));

			if (Options.ImageFormat == ImageFormat.ISO)
			{
				sb.AppendLine("ata0-master: type=cdrom,path=" + Quote(ImageFile) + ",status=inserted");
			}
			else
			{
				sb.AppendLine("ata0-master: type=disk,path=" + Quote(ImageFile) + ",biosdetect=none,cylinders=0,heads=0,spt=0");
			}

			if (Options.DebugConnectionOption == DebugConnectionOption.Pipe)
			{
				sb.AppendLine(@"com1: enabled=1, mode=pipe-server, dev=\\.\pipe\MOSA");
			}

			File.WriteAllText(configfile, sb.ToString());

			string arg = "-q " + "-f " + Quote(configfile);

			return LaunchApplication(AppLocations.BOCHS, arg, exit);
		}

		private Process LaunchVMwarePlayer(bool exit)
		{
			var logfile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + "-vmx.log");
			var configfile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".vmx");

			var sb = new StringBuilder();

			sb.AppendLine(".encoding = \"windows-1252\"");
			sb.AppendLine("config.version = \"8\"");
			sb.AppendLine("virtualHW.version = \"4\"");
			sb.AppendLine("memsize = " + Quote(Options.MemoryInMB.ToString()));

			sb.AppendLine("displayName = \"MOSA - " + Path.GetFileNameWithoutExtension(Options.SourceFile) + "\"");
			sb.AppendLine("guestOS = \"other\"");
			sb.AppendLine("priority.grabbed = \"normal\"");
			sb.AppendLine("priority.ungrabbed = \"normal\"");
			sb.AppendLine("virtualHW.productCompatibility = \"hosted\"");
			sb.AppendLine("ide0:0.present = \"TRUE\"");
			sb.AppendLine("ide0:0.fileName = " + Quote(ImageFile));

			if (Options.ImageFormat == ImageFormat.ISO)
			{
				sb.AppendLine("ide0:0.deviceType = \"cdrom-image\"");
			}

			sb.AppendLine("floppy0.present = \"FALSE\"");

			if (Options.DebugConnectionOption == DebugConnectionOption.Pipe)
			{
				sb.AppendLine("serial0.present = \"TRUE\"");
				sb.AppendLine("serial0.yieldOnMsrRead = \"FALSE\"");
				sb.AppendLine("serial0.fileType = \"pipe\"");
				sb.AppendLine("serial0.fileName = \"\\\\.\\pipe\\MOSA\"");
				sb.AppendLine("serial0.pipe.endPoint = \"server\"");
				sb.AppendLine("serial0.tryNoRxLoss = \"FALSE\"");
			}

			File.WriteAllText(configfile, sb.ToString());

			string arg = Quote(configfile);

			return LaunchApplication(AppLocations.VMwarePlayer, arg, exit);
		}
	}
}
