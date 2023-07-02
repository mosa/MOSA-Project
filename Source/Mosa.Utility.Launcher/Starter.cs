// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;

namespace Mosa.Utility.Launcher;

public class Starter : BaseLauncher
{
	public MosaLinker Linker { get; }

	public bool IsSucccessful { get; private set; }

	public Process Process { get; private set; }

	public Starter(Settings settings, CompilerHooks compilerHooks)
		: base(settings, compilerHooks)
	{
	}

	public Starter(Settings settings, CompilerHooks compilerHooks, MosaLinker linker)
		: base(settings, compilerHooks)
	{
		Linker = linker;
	}

	public bool Launch(bool waitForExit = false)
	{
		IsSucccessful = false;

		try
		{
			Process = LaunchVM();

			if (LauncherSettings.LauncherTest)
			{
				IsSucccessful = StartTestMonitor(Process, 10000, "<SELFTEST:PASSED>");
				return IsSucccessful;
			}

			if (LauncherSettings.LauncherDebugLog)
			{
				IsSucccessful = StartDebug(Process);
				return IsSucccessful;
			}

			Process.Start();

			if (LauncherSettings.LaunchDebugger)
			{
				LaunchDebugger();
			}
			else if (LauncherSettings.LaunchGDB)
			{
				LaunchGDB();
			}

			if (!LauncherSettings.LauncherExit)
			{
				//Output(process.Output);
			}

			IsSucccessful = true;
		}
		catch (Exception e)
		{
			IsSucccessful = false;
			Process = null;
			Output($"Exception: {e}");
		}
		finally
		{
			// Fix for Linux
			if (waitForExit && Process != null)
				Process.WaitForExit();
		}

		return IsSucccessful;
	}

	private bool StartTestMonitor(Process process, int timeoutMS, string successText)
	{
		var output = new StringBuilder();
		var lastLength = 0;
		var success = false;

		var client = new SimpleTCP();

		client.OnStatusUpdate = Output;

		client.OnDataAvailable = () =>
		{
			lock (output)
			{
				var line = client.GetFullLine();
				output.Append(line);
			}
		};

		try
		{
			process.Start();

			Thread.Sleep(50); // wait a bit for the process to start

			if (!client.Connect(LauncherSettings.EmulatorSerialHost, (ushort)LauncherSettings.EmulatorSerialPort, 10000))
				return false;

			var watchDog = new WatchDog(timeoutMS);

			while (!(success || watchDog.IsTimedOut))
			{
				lock (output)
				{
					var length = output.Length;

					if (length >= successText.Length && lastLength != length)
					{
						if (output.ToString().Contains(successText))
						{
							success = true;
							break;
						}
					}

					lastLength = length;
				}

				if (!client.IsConnected)
					return false;

				Thread.Yield();
			}
		}
		finally
		{
			process.Kill(true);
			process.WaitForExit();
		}

		Output($"VM Output: {output.Replace('\n', '|')}");
		Output($"VM Exit Code: {process.ExitCode}");

		if (success)
			Output("Test Ressult: PASSED");
		else
			Output("Test Ressult: FAILED");

		if (LauncherSettings.LauncherExit)
		{
			Environment.Exit(success ? 0 : 1);
		}

		return success;
	}

	private void StartWithOutput(Process process)
	{
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.CreateNoWindow = false;

		process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
		{
			Console.Write(e.Data);

			//var line = new StringBuilder();

			//foreach (var c in e.Data)
			//{
			//	if (c == '\n')
			//	{
			//		lock (process)
			//		{
			//			Output(line.ToString());
			//		}

			//		line.Length = 0;
			//		continue;
			//	}

			//	line.Append(c);
			//}
		});

		process.Start();
		process.BeginOutputReadLine();
	}

	private bool StartDebug(Process process)
	{
		if (process == null)
			return false;

		Output($"VM Output:");

		StartWithOutput(process);

		process.WaitForExit();

		Output($"VM Exit Code: {process.ExitCode}");

		return true;
	}

	public Process LaunchVM()
	{
		return LauncherSettings.Emulator switch
		{
			"qemu" => LaunchQemu(),
			"bochs" => LaunchBochs(),
			"vmware" => LaunchVMware(),
			"virtualbox" => LaunchVirtualBox(),
			_ => throw new InvalidCompilerOperationException()
		};
	}

	private Process LaunchQemu()
	{
		var arg = new StringBuilder();

		arg.Append("-m ");
		arg.Append(LauncherSettings.EmulatorMemory);
		arg.Append('M');

		arg.Append(" -smp cores=");
		arg.Append(LauncherSettings.EmulatorCores);

		if (LauncherSettings.Platform == "x86")
		{
			arg.Append(" -cpu qemu32,+sse4.1,abm,bmi1,bmi2,popcnt");
		}

		switch (LauncherSettings.EmulatorSVGA)
		{
			case "virtio": arg.Append(" -device virtio-vga"); break;
			case "vmware": arg.Append(" -vga vmware"); break;
			case "cirrus": arg.Append(" -vga cirrus"); break;
			case "std": arg.Append(" -vga std"); break;
		}

		if (!LauncherSettings.EmulatorDisplay || LauncherSettings.LauncherTest)
		{
			arg.Append(" -display none");
		}
		else
		{
			arg.Append(" -display sdl");
		}

		var serial = LauncherSettings.EmulatorSerial;

		if (LauncherSettings.LauncherDebugLog || LauncherSettings.LauncherTest)
		{
			serial = "tcpserver";
		}

		switch (serial)
		{
			case "pipe":
				{
					arg.Append(" -serial pipe:");
					arg.Append(LauncherSettings.EmulatorSerialPipe);
					break;
				}
			case "tcpserver":
				{
					arg.Append(" -serial tcp:");
					arg.Append(LauncherSettings.EmulatorSerialHost);
					arg.Append(':');
					arg.Append(LauncherSettings.EmulatorSerialPort);
					arg.Append(",server,nowait");
					break;
				}
			case "tcpclient":
				{
					arg.Append(" -serial tcp:");
					arg.Append(LauncherSettings.EmulatorSerialHost);
					arg.Append(':');
					arg.Append(LauncherSettings.EmulatorSerialPort);
					arg.Append(",client,nowait");
					break;
				}
			default:
				{
					arg.Append(" -serial null");
					break;
				}
		}

		if (LauncherSettings.EmulatorGDB)
		{
			arg.Append(" -S -gdb tcp::");
			arg.Append(LauncherSettings.GDBPort);
		}

		switch (LauncherSettings.ImageFormat)
		{
			case "bin":
				{
					arg.Append(" -kernel ");
					arg.Append(Quote(LauncherSettings.ImageFile));
					break;
				}
			default:
				{
					arg.Append(" -drive format=raw,file=");
					arg.Append(Quote(LauncherSettings.ImageFile));
					break;
				}
		}

		if (LauncherSettings.ImageFirmware == "bios")
		{
			arg.Append(" -L ");
			arg.Append(Quote(LauncherSettings.QEMUBios));
		}
		else if (LauncherSettings.ImageFirmware == "uefi")
		{
			if (LauncherSettings.Platform == "x86")
			{
				arg.Append(" -drive if=pflash,format=raw,readonly=on,file=");
				arg.Append(Quote(LauncherSettings.QEMUEdk2X86));
			}
			else if (LauncherSettings.Platform == "x64")
			{
				arg.Append(" -drive if=pflash,format=raw,readonly=on,file=");
				arg.Append(Quote(LauncherSettings.QEMUEdk2X64));
			}
			else if (LauncherSettings.Platform == "ARMv8A32")
			{
				arg.Append(" -drive if=pflash,format=raw,readonly=on,file=");
				arg.Append(Quote(LauncherSettings.QEMUEdk2ARM));
			}
		}

		return CreateApplicationProcess(LauncherSettings.QEMU, arg.ToString());
	}

	private Process LaunchBochs()
	{
		var bochsdirectory = Path.GetDirectoryName(LauncherSettings.Bochs);

		var logfile = Path.Combine(LauncherSettings.TemporaryFolder, Path.GetFileNameWithoutExtension(LauncherSettings.ImageFile) + "-bochs.log");
		var configfile = Path.Combine(LauncherSettings.TemporaryFolder, Path.GetFileNameWithoutExtension(LauncherSettings.ImageFile) + ".bxrc");

		var sb = new StringBuilder();

		sb.Append("megs: ");
		sb.Append(LauncherSettings.EmulatorMemory);
		sb.AppendLine();

		sb.AppendLine("ata0: enabled=1,ioaddr1=0x1f0,ioaddr2=0x3f0,irq=14");
		sb.AppendLine("cpuid: mmx=1,sep=1,simd=sse4_2,apic=xapic,aes=1,movbe=1,xsave=1");
		sb.AppendLine("boot: cdrom,disk");

		sb.Append("log: ");
		sb.AppendLine(Quote(logfile));

		sb.Append("romimage: file=");
		sb.AppendLine(Quote(Path.Combine(bochsdirectory, "BIOS-bochs-latest")));

		sb.Append("vgaromimage: file=");
		sb.AppendLine(Quote(Path.Combine(bochsdirectory, "VGABIOS-lgpl-latest")));

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			sb.AppendLine("display_library: x, options=gui_debug");
		}

		sb.Append("ata0-master: type=disk,path=");
		sb.Append(Quote(LauncherSettings.ImageFile));
		sb.AppendLine(",biosdetect=none,cylinders=0,heads=0,spt=0");

		switch (LauncherSettings.EmulatorSVGA)
		{
			case "vbe": sb.AppendLine("vga: extension=vbe"); break;
			case "cirrus": sb.AppendLine("vga: extension=cirrus"); break;
		}

		switch (LauncherSettings.EmulatorSerial)
		{
			case "pipe":
				{
					sb.Append("com1: enabled=1, mode=pipe-server, dev=\\\\.\\pipe\\");
					sb.AppendLine(LauncherSettings.EmulatorSerialPipe);
					break;
				}
			case "tcpserver":
				{
					sb.Append("com1: enabled=1, mode=socket-server, dev=");
					sb.Append(LauncherSettings.EmulatorSerialHost);
					sb.Append(':');
					sb.Append(LauncherSettings.EmulatorSerialPort);
					sb.AppendLine();
					break;
				}
			case "tcpclient":
				{
					sb.Append("com1: enabled=1, mode=socket-client, dev=");
					sb.Append(LauncherSettings.EmulatorSerialHost);
					sb.Append(':');
					sb.Append(LauncherSettings.EmulatorSerialPort);
					sb.AppendLine();
					break;
				}
		}

		if (LauncherSettings.EmulatorGDB)
		{
			// Untested
			sb.Append("gdbstub: enabled=1, port=");
			sb.Append(LauncherSettings.GDBPort);
			sb.AppendLine(", text_base=0, data_base=0, bss_base=0");
		}

		File.WriteAllText(configfile, sb.ToString());

		return CreateApplicationProcess(LauncherSettings.Bochs, $"-q -f {Quote(configfile)}");
	}

	private Process LaunchVMware()
	{
		var configFile = Path.Combine(LauncherSettings.TemporaryFolder, Path.ChangeExtension(LauncherSettings.ImageFile, ".vmx")!);
		var sb = new StringBuilder();

		sb.AppendLine(".encoding = \"windows-1252\"");
		sb.AppendLine("config.version = \"8\"");
		sb.AppendLine("virtualHW.version = \"14\"");

		sb.Append("memsize = ");
		sb.AppendLine(Quote(LauncherSettings.EmulatorMemory.ToString()));

		sb.Append("displayName = \"MOSA - ");
		sb.Append(Path.GetFileNameWithoutExtension(LauncherSettings.SourceFiles[0]));
		sb.AppendLine("\"");

		sb.AppendLine("guestOS = \"other\"");
		sb.AppendLine("priority.grabbed = \"normal\"");
		sb.AppendLine("priority.ungrabbed = \"normal\"");
		sb.AppendLine("virtualHW.productCompatibility = \"hosted\"");
		sb.AppendLine("numvcpus = \"1\"");

		sb.Append("cpuid.coresPerSocket = ");
		sb.AppendLine(Quote(LauncherSettings.EmulatorCores.ToString()));

		sb.AppendLine("ide0:0.present = \"TRUE\"");

		sb.Append("ide0:0.fileName = ");
		sb.AppendLine(Quote(LauncherSettings.ImageFile));

		sb.AppendLine("sound.present = \"TRUE\"");
		sb.AppendLine("sound.opl3.enabled = \"TRUE\"");
		sb.AppendLine("sound.virtualDev = \"sb16\"");

		sb.AppendLine("floppy0.present = \"FALSE\"");

		// COM1
		if (NullToEmpty(LauncherSettings.EmulatorSerial) == "pipe")
		{
			sb.AppendLine("serial1.present = \"TRUE\"");
			sb.AppendLine("serial1.yieldOnMsrRead = \"FALSE\"");
			sb.AppendLine("serial1.fileType = \"pipe\"");

			sb.Append("serial1.fileName = \"\\\\.\\pipe\\");
			sb.AppendLine(LauncherSettings.EmulatorSerialPipe);
			sb.AppendLine("\"");

			sb.AppendLine("serial1.pipe.endPoint = \"server\"");
			sb.AppendLine("serial1.tryNoRxLoss = \"FALSE\"");

			// FUTURE - when adding TCP instead of PIPE support (TCP is preferred)
			//serial0.present = "TRUE"
			//serial0.yieldOnMsrRead = "TRUE"
			//serial0.fileType = "network"
			//serial0.fileName = "telnet://192.168.1.2:2001"
		}

		File.WriteAllText(configFile, sb.ToString());

		var arg = Quote(configFile);

		if (!string.IsNullOrWhiteSpace(LauncherSettings.VmwareWorkstation))
		{
			return CreateApplicationProcess(LauncherSettings.VmwareWorkstation, arg);
		}

		if (!string.IsNullOrWhiteSpace(LauncherSettings.VmwarePlayer))
		{
			return CreateApplicationProcess(LauncherSettings.VmwarePlayer, arg);
		}

		return null;
	}

	private Process LaunchVirtualBox()
	{
		if (GetOutput(LaunchApplication(LauncherSettings.VirtualBox, "list vms")).Contains(LauncherSettings.OSName))
		{
			var newFile = Path.ChangeExtension(LauncherSettings.ImageFile, "bak");

			// Janky method to keep the image file
			File.Move(LauncherSettings.ImageFile, newFile);

			// Delete the VM first
			LaunchApplication(LauncherSettings.VirtualBox, $"unregistervm {LauncherSettings.OSName} --delete").WaitForExit();

			// Restore the image file
			File.Move(newFile, LauncherSettings.ImageFile);
		}

		LaunchApplication(LauncherSettings.VirtualBox, $"createvm --name {LauncherSettings.OSName} --ostype Other --register").WaitForExit();
		LaunchApplication(LauncherSettings.VirtualBox, $"modifyvm {LauncherSettings.OSName} --memory {LauncherSettings.EmulatorMemory.ToString()} --cpus {LauncherSettings.EmulatorCores} --graphicscontroller vmsvga").WaitForExit();
		LaunchApplication(LauncherSettings.VirtualBox, $"storagectl {LauncherSettings.OSName} --name Controller --add ide --controller PIIX4").WaitForExit();
		LaunchApplication(LauncherSettings.VirtualBox, $"storageattach {LauncherSettings.OSName} --storagectl Controller --port 0 --device 0 --type hdd --medium {Quote(LauncherSettings.ImageFile)}").WaitForExit();

		return CreateApplicationProcess(LauncherSettings.VirtualBox, $"startvm {LauncherSettings.OSName}");
	}

	private void LaunchDebugger()
	{
		// FIXME!!!
		var sb = new StringBuilder();

		sb.Append("-output-debug-file ");
		sb.Append(Path.Combine(LauncherSettings.TemporaryFolder, Path.ChangeExtension(LauncherSettings.ImageFile, ".debug")!));
		sb.Append(' ');
		sb.Append("-gdb-host ");
		sb.Append(LauncherSettings.GDBHost);
		sb.Append(' ');
		sb.Append("-gdb-port ");
		sb.Append(LauncherSettings.GDBPort);
		sb.Append(' ');
		sb.Append("-image ");
		sb.Append(Quote(LauncherSettings.ImageFile));

		LaunchApplication("Mosa.Tool.Debugger.exe", sb.ToString());
	}

	private void LaunchGDB()
	{
		var gdbScript = Path.Combine(LauncherSettings.TemporaryFolder, Path.GetFileNameWithoutExtension(LauncherSettings.ImageFile) + ".gdb");

		var symbol = Linker.EntryPoint;
		var breakAddress = symbol.VirtualAddress;

		var sb = new StringBuilder();

		sb.Append("target remote localhost:");
		sb.Append(LauncherSettings.GDBPort);
		sb.AppendLine();

		sb.AppendLine("set confirm off ");
		sb.AppendLine("set disassemble-next-line on");
		sb.AppendLine("set disassembly-flavor intel");
		sb.AppendLine("set pagination off");

		sb.Append("break *0x");
		sb.AppendLine(breakAddress.ToString("x"));

		sb.AppendLine("c");

		var script = sb.ToString();

		File.WriteAllText(gdbScript, script);

		Output($"Created configuration file: {gdbScript}");
		Output("==================");
		Output(script);
		Output("==================");

		sb.Clear();
		sb.Append("-d ");
		sb.Append(Quote(LauncherSettings.TemporaryFolder));
		sb.Append(" -s ");
		sb.Append(Quote(Path.Combine(LauncherSettings.TemporaryFolder, Path.ChangeExtension(LauncherSettings.ImageFile, ".bin")!)));
		sb.Append(" -x ");
		sb.Append(Quote(gdbScript));

		LaunchConsoleApplication(LauncherSettings.GDB, sb.ToString());
	}
}
