// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using static Reko.ImageLoaders.OdbgScript.OllyScript;

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

			if (LauncherSettings.LauncherTest)
			{
				IsSucccessful = MonitorTest(Process, 10000, "<SELFTEST:PASSED>");
				return IsSucccessful;
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
				var output = GetOutput(Process);
				Output(output);
			}

			IsSucccessful = true;
		}
		catch (Exception e)
		{
			IsSucccessful = false;
			Process = null;
			Output($"Exception: {e}");
		}

		// Fix for Linux
		if (waitForExit && Process != null)
			Process.WaitForExit();

		return IsSucccessful;
	}

	private bool MonitorTest(Process process, int timeoutMS, string successText)
	{
		var success = false;

		if (process == null)
		{
			Output("Test FAILED - not process");
			return false;
		}

		var sb = new StringBuilder();

		process.BeginOutputReadLine();

		process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
		{
			lock (sb)
			{
				sb.Append(e.Data);
			}
		});

		var readerThread = new Thread(() =>
		{
			while (true)
			{
				if (process.HasExited)
					break;

				lock (sb)
				{
					if (sb.Length == successText.Length && sb.ToString() == successText)
					{
						success = true;
						break;
					}
				}

				Thread.Sleep(10);
			}

			process.Kill();
		});

		readerThread.Start();

		var timeoutThread = new Thread(() =>
		{
			Thread.Sleep(timeoutMS);

			if (!process.HasExited)
			{
				Output("Test Timeout");
				process.Kill();
			}
		});
		timeoutThread.Start();

		process.WaitForExit();

		Output($"VM Output: {sb}");
		Output($"VM Exit Code: {process.ExitCode}");

		if (success)
			Output("Test PASSED");
		else
			Output("Test FAILED");

		if (LauncherSettings.LauncherExit)
		{
			Environment.Exit(success ? 0 : 1);
		}

		return success;
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

		// COM1 = Kernel Log
		if (LauncherSettings.LauncherTest)
		{
			arg.Append(" -serial stdio");
		}
		else
		{
			arg.Append(" -serial null");
		}

		// COM2 = Mosa Internal Debugger
		switch (LauncherSettings.EmulatorSerial)
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

		return LaunchApplication(LauncherSettings.QEMU, arg.ToString());
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

		// COM1 = Kernel Log
		if (LauncherSettings.LauncherTest)
		{
			// Not supported
			sb.AppendLine("com1: enabled=1, mode=null");
		}
		else
		{
			sb.AppendLine("com1: enabled=1, mode=null");
		}

		// COM2 = Mosa Internal Debugger
		switch (LauncherSettings.EmulatorSerial)
		{
			case "pipe":
				{
					sb.Append("com2: enabled=1, mode=pipe-server, dev=\\\\.\\pipe\\");
					sb.AppendLine(LauncherSettings.EmulatorSerialPipe);
					break;
				}
			case "tcpserver":
				{
					sb.Append("com2: enabled=1, mode=socket-server, dev=");
					sb.Append(LauncherSettings.EmulatorSerialHost);
					sb.Append(':');
					sb.Append(LauncherSettings.EmulatorSerialPort);
					sb.AppendLine();
					break;
				}
			case "tcpclient":
				{
					sb.Append("com2: enabled=1, mode=socket-client, dev=");
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

		return LaunchApplication(LauncherSettings.Bochs, $"-q -f {Quote(configfile)}");
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

		// COM1 = Kernel Log
		sb.AppendLine("serial0.present = \"TRUE\"");
		sb.AppendLine("serial0.yieldOnMsrRead = \"FALSE\"");
		sb.AppendLine("serial0.fileType = \"pipe\"");
		sb.AppendLine("serial0.fileName = \"\\\\.\\pipe\\MOSA1\"");
		sb.AppendLine("serial0.pipe.endPoint = \"server\"");
		sb.AppendLine("serial0.tryNoRxLoss = \"FALSE\"");

		// COM2 = Mosa Internal Debugger
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
			return LaunchApplication(LauncherSettings.VmwareWorkstation, arg);
		}

		if (!string.IsNullOrWhiteSpace(LauncherSettings.VmwarePlayer))
		{
			return LaunchApplication(LauncherSettings.VmwarePlayer, arg);
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

		return LaunchApplication(LauncherSettings.VirtualBox, $"startvm {LauncherSettings.OSName}");
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
