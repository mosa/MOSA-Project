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
		Process = null;

		try
		{
			Process = LaunchVM();

			if (Settings.LauncherTest)
			{
				IsSucccessful = StartTest(Process, "##PASS##");
				return IsSucccessful;
			}

			if (Settings.LauncherSerialConsole)
			{
				IsSucccessful = StartDebug(Process);
				return IsSucccessful;
			}

			Process.Start();

			if (Settings.LaunchDebugger)
			{
				LaunchDebugger();
			}
			else if (Settings.LaunchGDB)
			{
				LaunchGDB();
			}

			if (!Settings.LauncherExit)
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

	private bool StartTest(Process process, string successText)
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

			if (!client.Connect(Settings.EmulatorSerialHost, Settings.EmulatorSerialPort, 10000))
				return false;

			var watchDog = new WatchDog(Settings.EmulatorRuntimeMaximum * 1000);

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

		if (Settings.LauncherExit)
		{
			Environment.Exit(success ? 0 : 1);
		}

		return success;
	}

	private bool StartDebug(Process process)
	{
		var output = new StringBuilder();
		var success = false;
		var kill = false;

		var client = new SimpleTCP();

		client.OnStatusUpdate = Output;

		client.OnDataAvailable = () =>
		{
			while (client.HasLine)
			{
				var line = client.GetLine();

				lock (this)
				{
					Output(line);
				}

				if (line == "##KILL##")
					kill = true;
			}
		};

		try
		{
			process.Start();

			Thread.Sleep(50); // wait a bit for the process to start

			if (!client.Connect(Settings.EmulatorSerialHost, Settings.EmulatorSerialPort, 10000))
				return false;

			Output("VM Output");
			Output("========================");

			var watchDog = new WatchDog(Settings.EmulatorRuntimeMaximum * 1000);

			while (!(success || watchDog.IsTimedOut || kill))
			{
				if (!client.IsConnected)
					return false;

				Thread.Yield();
			}
		}
		finally
		{
			client.Disconnect();
			process.Kill(true);
			process.WaitForExit();
		}

		Output("========================");

		if (kill)
			Output("Kill command received");

		Output($"VM Exit Code: {process.ExitCode}");

		if (Settings.LauncherExit)
		{
			Environment.Exit(0);
		}

		return success;
	}

	public Process LaunchVM()
	{
		return Settings.Emulator switch
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
		arg.Append(Settings.EmulatorMemory);
		arg.Append('M');

		arg.Append(" -smp cores=");
		arg.Append(Settings.EmulatorCores);

		if (Settings.Platform == "x86")
		{
			arg.Append(" -cpu qemu32,+sse4.1,abm,bmi1,bmi2,popcnt");
		}

		switch (Settings.EmulatorSVGA)
		{
			case "virtio": arg.Append(" -device virtio-vga"); break;
			case "vmware": arg.Append(" -vga vmware"); break;
			case "cirrus": arg.Append(" -vga cirrus"); break;
			case "std": arg.Append(" -vga std"); break;
		}

		if (!Settings.EmulatorDisplay || Settings.LauncherTest)
		{
			arg.Append(" -display none");
		}
		else
		{
			arg.Append(" -display sdl");
		}

		var serial = Settings.EmulatorSerial;

		if (Settings.LauncherSerialConsole || Settings.LauncherTest)
		{
			serial = "tcpserver";
		}

		switch (serial)
		{
			case "pipe":
				{
					arg.Append(" -serial pipe:");
					arg.Append(Settings.EmulatorSerialPipe);
					break;
				}
			case "tcpserver":
				{
					arg.Append(" -serial tcp:");
					arg.Append(Settings.EmulatorSerialHost);
					arg.Append(':');
					arg.Append(Settings.EmulatorSerialPort);
					arg.Append(",server,nowait");
					break;
				}
			case "tcpclient":
				{
					arg.Append(" -serial tcp:");
					arg.Append(Settings.EmulatorSerialHost);
					arg.Append(':');
					arg.Append(Settings.EmulatorSerialPort);
					arg.Append(",client,nowait");
					break;
				}
			default:
				{
					arg.Append(" -serial null");
					break;
				}
		}

		if (Settings.EmulatorGDB)
		{
			arg.Append(" -S -gdb tcp::");
			arg.Append(Settings.GDBPort);
		}

		switch (Settings.ImageFormat)
		{
			case "bin":
				{
					arg.Append(" -kernel ");
					arg.Append(Quote(Settings.ImageFile));
					break;
				}
			default:
				{
					arg.Append(" -drive format=raw,file=");
					arg.Append(Quote(Settings.ImageFile));
					break;
				}
		}

		if (Settings.ImageFirmware == "bios")
		{
			arg.Append(" -L ");
			arg.Append(Quote(Settings.QEMUBios));
		}
		else if (Settings.ImageFirmware == "uefi")
		{
			if (Settings.Platform == "x86")
			{
				arg.Append(" -drive if=pflash,format=raw,readonly=on,file=");
				arg.Append(Quote(Settings.QEMUEdk2X86));
			}
			else if (Settings.Platform == "x64")
			{
				arg.Append(" -drive if=pflash,format=raw,readonly=on,file=");
				arg.Append(Quote(Settings.QEMUEdk2X64));
			}
			else if (Settings.Platform == "ARMv8A32")
			{
				arg.Append(" -drive if=pflash,format=raw,readonly=on,file=");
				arg.Append(Quote(Settings.QEMUEdk2ARM));
			}
		}

		return CreateApplicationProcess(Settings.QEMU, arg.ToString());
	}

	private Process LaunchBochs()
	{
		var bochsdirectory = Path.GetDirectoryName(Settings.Bochs);

		var logfile = Path.Combine(Settings.TemporaryFolder, Path.GetFileNameWithoutExtension(Settings.ImageFile) + "-bochs.log");
		var configfile = Path.Combine(Settings.TemporaryFolder, Path.GetFileNameWithoutExtension(Settings.ImageFile) + ".bxrc");

		var sb = new StringBuilder();

		sb.Append("megs: ");
		sb.Append(Settings.EmulatorMemory);
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
		sb.Append(Quote(Settings.ImageFile));
		sb.AppendLine(",biosdetect=none,cylinders=0,heads=0,spt=0");

		switch (Settings.EmulatorSVGA)
		{
			case "vbe": sb.AppendLine("vga: extension=vbe"); break;
			case "cirrus": sb.AppendLine("vga: extension=cirrus"); break;
		}

		switch (Settings.EmulatorSerial)
		{
			case "pipe":
				{
					sb.Append("com1: enabled=1, mode=pipe-server, dev=\\\\.\\pipe\\");
					sb.AppendLine(Settings.EmulatorSerialPipe);
					break;
				}
			case "tcpserver":
				{
					sb.Append("com1: enabled=1, mode=socket-server, dev=");
					sb.Append(Settings.EmulatorSerialHost);
					sb.Append(':');
					sb.Append(Settings.EmulatorSerialPort);
					sb.AppendLine();
					break;
				}
			case "tcpclient":
				{
					sb.Append("com1: enabled=1, mode=socket-client, dev=");
					sb.Append(Settings.EmulatorSerialHost);
					sb.Append(':');
					sb.Append(Settings.EmulatorSerialPort);
					sb.AppendLine();
					break;
				}
		}

		if (Settings.EmulatorGDB)
		{
			// Untested
			sb.Append("gdbstub: enabled=1, port=");
			sb.Append(Settings.GDBPort);
			sb.AppendLine(", text_base=0, data_base=0, bss_base=0");
		}

		File.WriteAllText(configfile, sb.ToString());

		return CreateApplicationProcess(Settings.Bochs, $"-q -f {Quote(configfile)}");
	}

	private Process LaunchVMware()
	{
		var configFile = Path.Combine(Settings.TemporaryFolder, Path.ChangeExtension(Settings.ImageFile, ".vmx")!);
		var sb = new StringBuilder();

		sb.AppendLine(".encoding = \"windows-1252\"");
		sb.AppendLine("config.version = \"8\"");
		sb.AppendLine("virtualHW.version = \"14\"");

		sb.Append("memsize = ");
		sb.AppendLine(Quote(Settings.EmulatorMemory.ToString()));

		sb.Append("displayName = \"MOSA - ");
		sb.Append(Path.GetFileNameWithoutExtension(Settings.SourceFiles[0]));
		sb.AppendLine("\"");

		sb.AppendLine("guestOS = \"other\"");
		sb.AppendLine("priority.grabbed = \"normal\"");
		sb.AppendLine("priority.ungrabbed = \"normal\"");
		sb.AppendLine("virtualHW.productCompatibility = \"hosted\"");
		sb.AppendLine("numvcpus = \"1\"");

		sb.Append("cpuid.coresPerSocket = ");
		sb.AppendLine(Quote(Settings.EmulatorCores.ToString()));

		sb.AppendLine("ide0:0.present = \"TRUE\"");

		sb.Append("ide0:0.fileName = ");
		sb.AppendLine(Quote(Settings.ImageFile));

		sb.AppendLine("sound.present = \"TRUE\"");
		sb.AppendLine("sound.opl3.enabled = \"TRUE\"");
		sb.AppendLine("sound.virtualDev = \"sb16\"");

		sb.AppendLine("floppy0.present = \"FALSE\"");

		// COM1
		if (NullToEmpty(Settings.EmulatorSerial) == "pipe")
		{
			sb.AppendLine("serial1.present = \"TRUE\"");
			sb.AppendLine("serial1.yieldOnMsrRead = \"FALSE\"");
			sb.AppendLine("serial1.fileType = \"pipe\"");

			sb.Append("serial1.fileName = \"\\\\.\\pipe\\");
			sb.AppendLine(Settings.EmulatorSerialPipe);
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

		if (!string.IsNullOrWhiteSpace(Settings.VmwareWorkstation))
		{
			return CreateApplicationProcess(Settings.VmwareWorkstation, arg);
		}

		if (!string.IsNullOrWhiteSpace(Settings.VmwarePlayer))
		{
			return CreateApplicationProcess(Settings.VmwarePlayer, arg);
		}

		return null;
	}

	private Process LaunchVirtualBox()
	{
		if (GetOutput(LaunchApplication(Settings.VirtualBox, "list vms")).Contains(Settings.OSName))
		{
			var newFile = Path.ChangeExtension(Settings.ImageFile, "bak");

			// Janky method to keep the image file
			File.Move(Settings.ImageFile, newFile);

			// Delete the VM first
			LaunchApplication(Settings.VirtualBox, $"unregistervm {Settings.OSName} --delete").WaitForExit();

			// Restore the image file
			File.Move(newFile, Settings.ImageFile);
		}

		LaunchApplication(Settings.VirtualBox, $"createvm --name {Settings.OSName} --ostype Other --register").WaitForExit();
		LaunchApplication(Settings.VirtualBox, $"modifyvm {Settings.OSName} --memory {Settings.EmulatorMemory.ToString()} --cpus {Settings.EmulatorCores} --graphicscontroller vmsvga").WaitForExit();
		LaunchApplication(Settings.VirtualBox, $"storagectl {Settings.OSName} --name Controller --add ide --controller PIIX4").WaitForExit();
		LaunchApplication(Settings.VirtualBox, $"storageattach {Settings.OSName} --storagectl Controller --port 0 --device 0 --type hdd --medium {Quote(Settings.ImageFile)}").WaitForExit();

		return CreateApplicationProcess(Settings.VirtualBox, $"startvm {Settings.OSName}");
	}

	private void LaunchDebugger()
	{
		// FIXME!!!
		var sb = new StringBuilder();

		sb.Append("-output-debug-file ");
		sb.Append(Path.Combine(Settings.TemporaryFolder, Path.ChangeExtension(Settings.ImageFile, ".debug")!));
		sb.Append(' ');
		sb.Append("-gdb-host ");
		sb.Append(Settings.GDBHost);
		sb.Append(' ');
		sb.Append("-gdb-port ");
		sb.Append(Settings.GDBPort);
		sb.Append(' ');
		sb.Append("-image ");
		sb.Append(Quote(Settings.ImageFile));

		LaunchApplication("Mosa.Tool.Debugger.exe", sb.ToString());
	}

	private void LaunchGDB()
	{
		var gdbScript = Path.Combine(Settings.TemporaryFolder, Path.GetFileNameWithoutExtension(Settings.ImageFile) + ".gdb");

		var symbol = Linker.EntryPoint;
		var breakAddress = symbol.VirtualAddress;

		var sb = new StringBuilder();

		sb.Append("target remote localhost:");
		sb.Append(Settings.GDBPort);
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
		sb.Append(Quote(Settings.TemporaryFolder));
		sb.Append(" -s ");
		sb.Append(Quote(Path.Combine(Settings.TemporaryFolder, Path.ChangeExtension(Settings.ImageFile, ".bin")!)));
		sb.Append(" -x ");
		sb.Append(Quote(gdbScript));

		LaunchConsoleApplication(Settings.GDB, sb.ToString());
	}
}
