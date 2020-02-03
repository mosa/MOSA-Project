// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Mosa.Utility.Launcher
{
	public class Starter : BaseLauncher
	{
		public MosaLinker Linker { get; }

		public Starter(Settings settings, CompilerHooks compilerHooks)
			: base(settings, compilerHooks)
		{
		}

		public Starter(Settings settings, CompilerHooks compilerHooks, MosaLinker linker)
			: base(settings, compilerHooks)
		{
			Linker = linker;
		}

		public Process Launch()
		{
			var process = LaunchVM();

			if (LauncherSettings.MonitorTest)
			{
				MonitorTest(process, 5000, "<SELFTEST:PASSED>");
				return process;
			}

			if (LauncherSettings.LaunchDebugger)
			{
				LaunchDebugger();
			}

			if (LauncherSettings.LaunchGDB)
			{
				LaunchGDB();
			}

			if (!LauncherSettings.LauncherExit)
			{
				var output = GetOutput(process);
				AddOutput(output);
			}

			return process;
		}

		private bool MonitorTest(Process process, int timeoutMS, string successText)
		{
			bool success = false;

			if (process != null)
			{
				var readerThread = new Thread(() =>
				{
					while (true)
					{
						if (process.HasExited)
							break;

						var line = process.StandardOutput.ReadLine();

						AddOutput($"VM Output: {line}");

						if (line == successText)
						{
							success = true;
							break;
						}
					}

					process.Kill();
				});
				readerThread.Start();

				var timeoutThread = new Thread(() =>
				{
					Thread.Sleep(timeoutMS);
					if (!process.HasExited)
					{
						AddOutput("Test Timeout");
						process.Kill();
					}
				});
				timeoutThread.Start();

				process.WaitForExit();

				AddOutput($"VM Exit Code: {process.ExitCode}");
			}

			if (success)
				AddOutput("Test PASSED");
			else
				AddOutput("Test FAILED");

			if (LauncherSettings.LauncherExit)
			{
				Environment.Exit(success ? 0 : 1);
			}

			return success;
		}

		public Process LaunchVM()
		{
			switch (LauncherSettings.Emulator)
			{
				case "qemu": return LaunchQemu(false);
				case "bochs": return LaunchBochs(false);
				case "vmware": return LaunchVMwarePlayer(false);
				default: throw new InvalidOperationException();
			}
		}

		private Process LaunchQemu(bool getOutput)
		{
			var arg = new StringBuilder();

			arg.Append(" -L " + Quote(LauncherSettings.QEMUBios));

			if (LauncherSettings.Platform == "x86")
			{
				arg.Append(" -cpu qemu32,+sse4.1");
			}

			//arg = arg + " -vga vmware";

			if (!LauncherSettings.EmulatorDisplay || LauncherSettings.MonitorTest)
			{
				arg.Append(" -display none");
			}

			// COM1 = Kernel Log
			if (LauncherSettings.MonitorTest)
			{
				arg.Append(" -serial stdio");
			}
			else
			{
				arg.Append(" -serial null");
			}

			// COM2 = Mosa Internal Debugger
			if (!string.IsNullOrWhiteSpace(LauncherSettings.EmulatorSerial))
			{
				switch (LauncherSettings.EmulatorSerial)
				{
					case "pipe": arg.Append($" -serial pipe:{LauncherSettings.EmulatorSerialPipe}"); break;
					case "tcpserver": arg.Append($" -serial tcp:{LauncherSettings.EmulatorSerialHost}:{LauncherSettings.EmulatorSerialPort},server,nowait"); break;
					case "tcpclient": arg.Append($" -serial tcp:{LauncherSettings.EmulatorSerialHost}:{LauncherSettings.EmulatorSerialPort},client,nowait"); break;
				}
			}

			if (LauncherSettings.EmulatorGDB)
			{
				arg.Append($" -S -gdb tcp::{LauncherSettings.GDBPort}");
			}

			if (LauncherSettings.ImageFormat == "iso")
			{
				arg.Append($" -cdrom {Quote(LauncherSettings.ImageFile)}");
			}
			else
			{
				if (LauncherSettings.ImageFormat == "bin")
				{
					arg.Append($" -kernel {Quote(LauncherSettings.ImageFile)}");
				}
				else
				{
					arg.Append($" -hda {Quote(LauncherSettings.ImageFile)}");
				}
			}

			return LaunchApplication(LauncherSettings.QEMU, arg.ToString(), getOutput);
		}

		private Process LaunchBochs(bool getOutput)
		{
			var bochsdirectory = Path.GetDirectoryName(LauncherSettings.Bochs);

			var logfile = Path.Combine(LauncherSettings.TemporaryFolder, Path.GetFileNameWithoutExtension(LauncherSettings.ImageFile) + "-bochs.log");
			var configfile = Path.Combine(LauncherSettings.TemporaryFolder, Path.GetFileNameWithoutExtension(LauncherSettings.ImageFile) + ".bxrc");

			var fileVersionInfo = FileVersionInfo.GetVersionInfo(LauncherSettings.Bochs);

			// simd or sse
			var simd = "simd";

			if (!(fileVersionInfo.FileMajorPart >= 2 && fileVersionInfo.FileMinorPart >= 6 && fileVersionInfo.FileBuildPart >= 5))
				simd = "sse";

			var sb = new StringBuilder();

			sb.AppendLine($"megs: {LauncherSettings.EmulatorMemory}");
			sb.AppendLine($"ata0: enabled=1,ioaddr1=0x1f0,ioaddr2=0x3f0,irq=14");
			sb.AppendLine($"cpuid: mmx=1,sep=1,{simd}=sse4_2,apic=xapic,aes=1,movbe=1,xsave=1");
			sb.AppendLine($"boot: cdrom,disk");
			sb.AppendLine($"log: {Quote(logfile)}");
			sb.AppendLine($"romimage: file={Quote(Path.Combine(bochsdirectory, "BIOS-bochs-latest"))}");
			sb.AppendLine($"vgaromimage: file={Quote(Path.Combine(bochsdirectory, "VGABIOS-lgpl-latest"))}");
			sb.AppendLine($"display_library: x, options={Quote("gui_debug")}");

			if (LauncherSettings.ImageFormat == "iso")
			{
				sb.AppendLine($"ata0-master: type=cdrom,path={Quote(LauncherSettings.ImageFile)},status=inserted");
			}
			else
			{
				sb.AppendLine($"ata0-master: type=disk,path={Quote(LauncherSettings.ImageFile)},biosdetect=none,cylinders=0,heads=0,spt=0");
			}

			sb.AppendLine(@"com1: enabled=1, mode=pipe-server, dev=\\.\pipe\MOSA1");

			if (LauncherSettings.EmulatorSerial == "pipe")
			{
				sb.AppendLine(@"com2: enabled=1, mode=pipe-server, dev=\\.\pipe\MOSA2");
			}

			File.WriteAllText(configfile, sb.ToString());

			var arg = $"-q -f {Quote(configfile)}";

			return LaunchApplication(LauncherSettings.Bochs, arg, getOutput);
		}

		private Process LaunchVMwarePlayer(bool getOutput)
		{
			var logfile = Path.Combine(LauncherSettings.TemporaryFolder, Path.GetFileNameWithoutExtension(LauncherSettings.ImageFile) + "-vmx.log");
			var configfile = Path.Combine(LauncherSettings.TemporaryFolder, Path.GetFileNameWithoutExtension(LauncherSettings.ImageFile) + ".vmx");

			var sb = new StringBuilder();

			sb.AppendLine(".encoding = \"windows-1252\"");
			sb.AppendLine("config.version = \"8\"");
			sb.AppendLine("virtualHW.version = \"4\"");
			sb.AppendLine($"memsize = {Quote(LauncherSettings.EmulatorMemory.ToString())}");
			sb.AppendLine($"displayName = \"MOSA - {Path.GetFileNameWithoutExtension(LauncherSettings.SourceFiles[0])}\"");
			sb.AppendLine("guestOS = \"other\"");
			sb.AppendLine("priority.grabbed = \"normal\"");
			sb.AppendLine("priority.ungrabbed = \"normal\"");
			sb.AppendLine("virtualHW.productCompatibility = \"hosted\"");
			sb.AppendLine("ide0:0.present = \"TRUE\"");
			sb.AppendLine($"ide0:0.fileName = {Quote(LauncherSettings.ImageFile)}");

			if (LauncherSettings.ImageFormat == "iso")
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

			if (LauncherSettings.EmulatorSerial == "pipe")
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

			return LaunchApplication(LauncherSettings.VmwarePlayer, arg, getOutput);
		}

		private void LaunchDebugger()
		{
			// FIXME!!!
			var arg = $" -debugfile {Path.Combine(LauncherSettings.TemporaryFolder, Path.GetFileNameWithoutExtension(LauncherSettings.ImageFile) + ".debug")}";
			arg += $" -port {LauncherSettings.GDBPort}";
			arg += $" -connect";
			arg += $" -image {Quote(LauncherSettings.ImageFile)}";

			LaunchApplication("Mosa.Tool.Debugger.exe", arg);
		}

		private void LaunchGDB()
		{
			var gdbscript = Path.Combine(LauncherSettings.TemporaryFolder, Path.GetFileNameWithoutExtension(LauncherSettings.ImageFile) + ".gdb");

			var arg = $" -d {Quote(LauncherSettings.TemporaryFolder)}";
			arg = $"{arg} -s {Quote(Path.Combine(LauncherSettings.TemporaryFolder, Path.GetFileNameWithoutExtension(LauncherSettings.ImageFile) + ".bin"))}";
			arg = $"{arg} -x {Quote(gdbscript)}";

			// FIXME!
			ulong startingAddress = Linker.Sections[(int)SectionKind.Text].VirtualAddress + Builder.MultibootHeaderLength;

			var sb = new StringBuilder();

			sb.AppendLine($"target remote localhost:{LauncherSettings.GDBPort}");
			sb.AppendLine($"set confirm off ");
			sb.AppendLine($"set disassemble-next-line on");
			sb.AppendLine($"set disassembly-flavor intel");
			sb.AppendLine($"set pagination off");
			sb.AppendLine($"break *0x{startingAddress.ToString("x")}");
			sb.AppendLine($"c");

			File.WriteAllText(gdbscript, sb.ToString());

			AddOutput("Created configuration file: " + gdbscript);
			AddOutput("==================");
			AddOutput(sb.ToString());
			AddOutput("==================");

			LaunchConsoleApplication(LauncherSettings.GDB, arg);
		}
	}
}
