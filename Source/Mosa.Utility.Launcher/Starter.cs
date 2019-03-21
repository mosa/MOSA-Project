// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using Mosa.Utility.BootImage;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Utility.Launcher
{
	public class Starter : BaseLauncher
	{
		public IStarterEvent LauncherEvent { get; }

		public MosaLinker Linker { get; }

		public Starter(LauncherOptions launcherOptions, AppLocations appLocations, IStarterEvent launcherEvent)
			: base(launcherOptions, appLocations)
		{
			LauncherEvent = launcherEvent;
		}

		public Starter(LauncherOptions options, AppLocations appLocations, IStarterEvent launcherEvent, MosaLinker linker)
			: base(options, appLocations)
		{
			LauncherEvent = launcherEvent;
			Linker = linker;
		}

		protected override void OutputEvent(string status)
		{
			LauncherEvent?.NewStatus(status);
		}

		public Process Launch()
		{
			var process = LaunchVM();

			if (LauncherOptions.LaunchGDBDebugger)
			{
				LaunchGDBDebugger();
			}
			if (LauncherOptions.LaunchGDB)
			{
				LaunchGDB();
			}
			if (!LauncherOptions.ExitOnLaunch)
			{
				var output = GetOutput(process);
				AddOutput(output);
			}

			return process;
		}

		public Process LaunchVM()
		{
			switch (LauncherOptions.Emulator)
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

			if (LauncherOptions.PlatformType == PlatformType.x86)
			{
				arg += " -cpu qemu32,+sse4.1";
			}

			//arg = arg + " -vga vmware";

			if (LauncherOptions.NoDisplay)
				arg += " -display none";

			// We need as least 2 COM Ports:
			// COM1 = Kernel log
			// COM2 = MosaDebugger

			arg += " -serial null"; // TODO: Redirect to file

			if (LauncherOptions.SerialConnectionOption == SerialConnectionOption.Pipe)
			{
				arg = arg + " -serial pipe:" + LauncherOptions.SerialPipeName;
			}
			else if (LauncherOptions.SerialConnectionOption == SerialConnectionOption.TCPServer)
			{
				arg = arg + " -serial tcp:" + LauncherOptions.SerialConnectionHost + ":" + LauncherOptions.SerialConnectionPort.ToString() + ",server,nowait";
			}
			else if (LauncherOptions.SerialConnectionOption == SerialConnectionOption.TCPClient)
			{
				arg = arg + " -serial tcp:" + LauncherOptions.SerialConnectionHost + ":" + LauncherOptions.SerialConnectionPort.ToString() + ",client,nowait";
			}

			if (LauncherOptions.EnableQemuGDB)
			{
				arg += " -S -gdb tcp::" + LauncherOptions.GDBPort.ToString();
			}

			if (LauncherOptions.ImageFormat == ImageFormat.ISO)
			{
				arg = arg + " -cdrom " + Quote(LauncherOptions.ImageFile);
			}
			else
			{
				if (LauncherOptions.ImageFormat == ImageFormat.BIN)
					arg = arg + " -kernel " + Quote(LauncherOptions.ImageFile);
				else
					arg = arg + " -hda " + Quote(LauncherOptions.ImageFile);
			}

			return LaunchApplication(AppLocations.QEMU, arg, getOutput);
		}

		private Process LaunchBochs(bool getOutput)
		{
			var logfile = Path.Combine(LauncherOptions.DestinationDirectory, Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile) + "-bochs.log");
			var configfile = Path.Combine(LauncherOptions.DestinationDirectory, Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile) + ".bxrc");
			var exeDir = Path.GetDirectoryName(AppLocations.BOCHS);

			var fileVersionInfo = FileVersionInfo.GetVersionInfo(AppLocations.BOCHS);

			// simd or sse
			var simd = "simd";

			if (!(fileVersionInfo.FileMajorPart >= 2 && fileVersionInfo.FileMinorPart >= 6 && fileVersionInfo.FileBuildPart >= 5))
				simd = "sse";

			var sb = new StringBuilder();

			sb.AppendLine("megs: " + LauncherOptions.EmulatorMemoryInMB.ToString());
			sb.AppendLine("ata0: enabled=1,ioaddr1=0x1f0,ioaddr2=0x3f0,irq=14");
			sb.AppendLine("cpuid: mmx=1,sep=1," + simd + "=sse4_2,apic=xapic,aes=1,movbe=1,xsave=1");
			sb.AppendLine("boot: cdrom,disk");
			sb.AppendLine("log: " + Quote(logfile));
			sb.AppendLine("romimage: file=" + Quote(Path.Combine(AppLocations.BOCHSBIOSDirectory, "BIOS-bochs-latest")));
			sb.AppendLine("vgaromimage: file=" + Quote(Path.Combine(AppLocations.BOCHSBIOSDirectory, "VGABIOS-lgpl-latest")));
			sb.AppendLine("display_library: x, options=" + Quote("gui_debug"));

			if (LauncherOptions.ImageFormat == ImageFormat.ISO)
			{
				sb.AppendLine("ata0-master: type=cdrom,path=" + Quote(LauncherOptions.ImageFile) + ",status=inserted");
			}
			else
			{
				sb.AppendLine("ata0-master: type=disk,path=" + Quote(LauncherOptions.ImageFile) + ",biosdetect=none,cylinders=0,heads=0,spt=0");
			}

			sb.AppendLine(@"com1: enabled=1, mode=pipe-server, dev=\\.\pipe\MOSA1");
			if (LauncherOptions.SerialConnectionOption == SerialConnectionOption.Pipe)
			{
				sb.AppendLine(@"com2: enabled=1, mode=pipe-server, dev=\\.\pipe\MOSA2");
			}

			string arg = "-q -f " + Quote(configfile);

			File.WriteAllText(configfile, sb.ToString());

			return LaunchApplication(AppLocations.BOCHS, arg, getOutput);
		}

		private Process LaunchVMwarePlayer(bool getOutput)
		{
			var logfile = Path.Combine(LauncherOptions.DestinationDirectory, Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile) + "-vmx.log");
			var configfile = Path.Combine(LauncherOptions.DestinationDirectory, Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile) + ".vmx");

			var sb = new StringBuilder();

			sb.AppendLine(".encoding = \"windows-1252\"");
			sb.AppendLine("config.version = \"8\"");
			sb.AppendLine("virtualHW.version = \"4\"");
			sb.AppendLine("memsize = " + Quote(LauncherOptions.EmulatorMemoryInMB.ToString()));

			sb.AppendLine("displayName = \"MOSA - " + Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile) + "\"");
			sb.AppendLine("guestOS = \"other\"");
			sb.AppendLine("priority.grabbed = \"normal\"");
			sb.AppendLine("priority.ungrabbed = \"normal\"");
			sb.AppendLine("virtualHW.productCompatibility = \"hosted\"");
			sb.AppendLine("ide0:0.present = \"TRUE\"");
			sb.AppendLine("ide0:0.fileName = " + Quote(LauncherOptions.ImageFile));

			if (LauncherOptions.ImageFormat == ImageFormat.ISO)
			{
				sb.AppendLine("ide0:0.deviceType = \"cdrom-image\"");
			}

			sb.AppendLine("floppy0.present = \"FALSE\"");

			sb.AppendLine("serial0.present = \"TRUE\"");
			sb.AppendLine("serial0.yieldOnMsrRead = \"FALSE\"");
			sb.AppendLine("serial0.fileType = \"pipe\"");
			sb.AppendLine("serial0.fileName = \"\\\\.\\pipe\\MOSA1\"");
			sb.AppendLine("serial0.pipe.endPoint = \"server\"");
			sb.AppendLine("serial0.tryNoRxLoss = \"FALSE\"");

			if (LauncherOptions.SerialConnectionOption == SerialConnectionOption.Pipe)
			{
				sb.AppendLine("serial1.present = \"TRUE\"");
				sb.AppendLine("serial1.yieldOnMsrRead = \"FALSE\"");
				sb.AppendLine("serial1.fileType = \"pipe\"");
				sb.AppendLine("serial1.fileName = \"\\\\.\\pipe\\MOSA2\"");
				sb.AppendLine("serial1.pipe.endPoint = \"server\"");
				sb.AppendLine("serial1.tryNoRxLoss = \"FALSE\"");
			}

			File.WriteAllText(configfile, sb.ToString());

			string arg = Quote(configfile);

			return LaunchApplication(AppLocations.VMwarePlayer, arg, getOutput);
		}

		private void LaunchGDBDebugger()
		{
			string arg = " -debugfile " + Path.Combine(LauncherOptions.DestinationDirectory, Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile) + ".debug");
			arg += " -port " + LauncherOptions.GDBPort.ToString();
			arg += " -connect";
			arg += " -image " + Quote(LauncherOptions.ImageFile);
			LaunchApplication("Mosa.Tool.GDBDebugger.exe", arg);
		}

		private void LaunchGDB()
		{
			var gdbscript = Path.Combine(LauncherOptions.DestinationDirectory, Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile) + ".gdb");

			string arg = " -d " + Quote(LauncherOptions.DestinationDirectory);
			arg = arg + " -s " + Quote(Path.Combine(LauncherOptions.DestinationDirectory, Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile) + ".bin"));
			arg = arg + " -x " + Quote(gdbscript);

			// FIXME!
			ulong startingAddress = Linker.Sections[(int)SectionKind.Text].VirtualAddress + Builder.MultibootHeaderLength;

			var sb = new StringBuilder();

			sb.AppendLine("target remote localhost:" + LauncherOptions.GDBPort.ToString());
			sb.AppendLine("set confirm off ");
			sb.AppendLine("set disassemble-next-line on");
			sb.AppendLine("set disassembly-flavor intel");
			sb.AppendLine("set pagination off");
			sb.AppendLine("break *0x" + startingAddress.ToString("x"));
			sb.AppendLine("c");

			File.WriteAllText(gdbscript, sb.ToString());

			AddOutput("Created configuration file: " + gdbscript);
			AddOutput("==================");
			AddOutput(sb.ToString());
			AddOutput("==================");

			LaunchConsoleApplication(AppLocations.GDB, arg);
		}
	}
}
