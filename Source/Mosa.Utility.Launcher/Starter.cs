// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Linker;
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

		public BaseLinker Linker { get; private set; }

		public Starter(Options options, AppLocations appLocations, string imagefile, IStarterEvent launcherEvent)
			: base(options, appLocations)
		{
			ImageFile = imagefile;
			LauncherEvent = launcherEvent;
		}

		public Starter(Options options, AppLocations appLocations, string imagefile, IStarterEvent launcherEvent, BaseLinker linker)
			: base(options, appLocations)
		{
			ImageFile = imagefile;
			LauncherEvent = launcherEvent;
			Linker = linker;
		}

		protected override void OutputEvent(string status)
		{
			if (LauncherEvent != null)
				LauncherEvent.NewStatus(status);
		}

		public Process Launch()
		{
			var process = LaunchVM();

			if (Options.LaunchMosaDebugger)
			{
				LaunchMosaDebugger();
			}
			if (Options.LaunchGDB)
			{
				LaunchGDB();
			}
			if (!Options.ExitOnLaunch)
			{
				var output = GetOutput(process);
				AddOutput(output);
			}

			return process;
		}

		private Process LaunchVM()
		{
			switch (Options.Emulator)
			{
				case EmulatorType.Qemu: return LaunchQemu(false);
				case EmulatorType.Bochs: return LaunchBochs(false);
				case EmulatorType.VMware: return LaunchVMwarePlayer(false);
				default: throw new InvalidOperationException();
			}
		}

		private Process LaunchQemu(bool getOutput)
		{
			string arg = " -L " + Quote(AppLocations.QEMUBIOSDirectory);

			if (Options.PlatformType == PlatformType.X86)
			{
				arg = arg + " -cpu qemu32,+sse4.1";
			}

			//arg = arg + " -vga vmware";

			if (Options.DebugConnectionOption == DebugConnectionOption.Pipe)
			{
				arg = arg + " -serial pipe:" + Options.DebugPipeName;
			}
			else if (Options.DebugConnectionOption == DebugConnectionOption.TCPServer)
			{
				arg = arg + " -serial tcp:" + Options.DebugConnectionAddress + ":" + Options.DebugConnectionPort.ToString() + ",server,nowait";
			}
			else if (Options.DebugConnectionOption == DebugConnectionOption.TCPClient)
			{
				arg = arg + " -serial tcp:" + Options.DebugConnectionAddress + ":" + Options.DebugConnectionPort.ToString() + ",client,nowait";
			}

			if (Options.EnableQemuGDB)
			{
				arg = arg + " -S -gdb tcp::1234";
			}

			if (Options.ImageFormat == ImageFormat.ISO)
			{
				arg = arg + " -cdrom " + Quote(ImageFile);
			}
			else
			{
				arg = arg + " -hda " + Quote(ImageFile);
			}

			return LaunchApplication(AppLocations.QEMU, arg, getOutput);
		}

		private Process LaunchBochs(bool getOutput)
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

			sb.AppendLine("megs: " + Options.EmulatorMemoryInMB.ToString());
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

			string arg = "-q " + "-f " + Quote(configfile);

			File.WriteAllText(configfile, sb.ToString());

			return LaunchApplication(AppLocations.BOCHS, arg, getOutput);
		}

		private Process LaunchVMwarePlayer(bool getOutput)
		{
			var logfile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + "-vmx.log");
			var configfile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".vmx");

			var sb = new StringBuilder();

			sb.AppendLine(".encoding = \"windows-1252\"");
			sb.AppendLine("config.version = \"8\"");
			sb.AppendLine("virtualHW.version = \"4\"");
			sb.AppendLine("memsize = " + Quote(Options.EmulatorMemoryInMB.ToString()));

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

			return LaunchApplication(AppLocations.VMwarePlayer, arg, getOutput);
		}

		private void LaunchMosaDebugger()
		{
			string arg = " -debugfile " + Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".debug");
			arg = arg + " -port 1234";
			LaunchApplication("Mosa.Tool.GDBDebugger.exe", arg);
		}

		private void LaunchGDB()
		{
			var gdbscript = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".gdb");

			string arg = " -d " + Quote(Options.DestinationDirectory);
			arg = arg + " -s " + Quote(Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".bin"));
			arg = arg + " -x " + Quote(gdbscript);

			var textSection = Linker.LinkerSections[(int)SectionKind.Text];

			uint multibootHeaderLength = Builder.MultibootHeaderLength;
			ulong startingAddress = textSection.VirtualAddress + multibootHeaderLength;

			var sb = new StringBuilder();

			sb.AppendLine("target remote localhost:1234");
			sb.AppendLine("set confirm off ");
			sb.AppendLine("set disassemble-next-line on");
			sb.AppendLine("set disassembly-flavor intel");
			sb.AppendLine("set pagination off");
			sb.AppendLine("break *0x" + startingAddress.ToString("x"));
			sb.AppendLine("c");

			File.WriteAllText(gdbscript, sb.ToString());

			LaunchConsoleApplication(AppLocations.GDB, arg);
		}
	}
}
