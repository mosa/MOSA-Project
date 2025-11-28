// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Utility.Configuration;

namespace Mosa.Utility.Launcher;

public class Starter : BaseLauncher
{
	public MosaLinker Linker { get; }

	public bool IsSuccessful { get; private set; }

	public Process Process { get; private set; }

	private readonly object lockObject = new object();

	public Starter(MosaSettings mosaSettings, CompilerHooks compilerHooks)
		: base(mosaSettings, compilerHooks) {}

	public Starter(MosaSettings mosaSettings, CompilerHooks compilerHooks, MosaLinker linker)
		: base(mosaSettings, compilerHooks) => Linker = linker;

	public bool Launch(bool waitForExit = false)
	{
		IsSuccessful = false;
		Process = null;

		try
		{
			Process = LaunchVM();

			if (MosaSettings.LauncherTest)
			{
				IsSuccessful = StartTestMonitor(Process, "##PASS##");
				return IsSuccessful;
			}

			if (MosaSettings.LauncherSerial)
			{
				IsSuccessful = StartSerialMonitor(Process);
				return IsSuccessful;
			}

			Process.Start();

			if (MosaSettings.LaunchDebugger)
				LaunchDebugger();
			else if (MosaSettings.LaunchGDB)
				LaunchGDB();

			IsSuccessful = true;
		}
		catch (Exception e)
		{
			IsSuccessful = false;
			Process = null;
			OutputStatus($"Exception: {e}");
		}
		finally
		{
			// Fix for Linux
			if (waitForExit && Process != null)
				Process.WaitForExit();
		}

		return IsSuccessful;
	}

	private bool StartTestMonitor(Process process, string successText)
	{
		var success = false;
		var kill = false;

		var client = new SimpleTCP();
		var watchDog = new WatchDog(MosaSettings.EmulatorMaxRuntime * 1000);

		client.OnStatusUpdate = OutputStatus;
		client.OnDataAvailable = () =>
		{
			while (client.HasLine)
			{
				var line = client.GetLine();

				lock (lockObject)
					OutputStatus(line);

				if (line.Contains(successText))
					success = true;

				if (line == "##KILL##")
					kill = true;

				watchDog.Restart();
			}
		};

		try
		{
			process.Start();

			OutputStatus("VM Output");
			OutputStatus("========================");

			if (!client.Connect(MosaSettings.EmulatorSerialHost, MosaSettings.EmulatorSerialPort, 10000))
				return false;

			watchDog.Restart();

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

		OutputStatus("========================");
		if (kill)
			OutputStatus("Kill command received");

		OutputStatus($"VM Exit Code: {process.ExitCode}");
		OutputStatus(success ? "Test Result: PASSED" : "Test Result: FAILED");

		if (MosaSettings.LauncherExit)
			Environment.Exit(success ? 0 : 1);

		return success;
	}

	private bool StartSerialMonitor(Process process)
	{
		var success = false; // TODO: Why is this always false?
		var kill = false;

		var client = new SimpleTCP();
		var watchDog = new WatchDog(MosaSettings.EmulatorMaxRuntime * 1000);

		client.OnStatusUpdate = OutputStatus;
		client.OnDataAvailable = () =>
		{
			while (client.HasLine)
			{
				var line = client.GetLine();

				lock (lockObject)
					OutputStatus(line);

				if (line == "##KILL##")
					kill = true;

				watchDog.Restart();
			}
		};

		try
		{
			process.Start();

			OutputStatus("VM Output");
			OutputStatus("========================");

			if (!client.Connect(MosaSettings.EmulatorSerialHost, MosaSettings.EmulatorSerialPort, 10000))
				return false;

			watchDog.Restart();

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

		OutputStatus("========================");
		if (kill)
			OutputStatus("Kill command received");

		OutputStatus($"VM Exit Code: {process.ExitCode}");

		if (MosaSettings.LauncherExit)
			Environment.Exit(0);

		return success;
	}

	public Process LaunchVM() => MosaSettings.Emulator switch
	{
		"qemu" => LaunchQemu(),
		"bochs" => LaunchBochs(),
		"vmware" => LaunchVMware(),
		"virtualbox" => LaunchVirtualBox(),
		_ => throw new InvalidOperationCompilerException()
	};

	private Process LaunchQemu()
	{
		OutputStatus("Launching QEMU");

		string qemuApp, qemuUefi;

		var sb = new StringBuilder();

		if (MosaSettings.EmulatorAcceleration)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				sb.Append("-accel whpx ");
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				sb.Append("-accel hvf ");
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				sb.Append("-accel kvm ");
			else
				throw new NotSupportedException("Requested unsupported QEMU acceleration for host platform.");
		}

		sb.Append("-m ");
		sb.Append(MosaSettings.EmulatorMemory);
		sb.Append('M');

		sb.Append(" -smp cores=");
		sb.Append(MosaSettings.EmulatorCores);

		switch (MosaSettings.Platform.ToLowerInvariant())
		{
			case "x86":
			{
				qemuApp = MosaSettings.QemuX86App;
				qemuUefi = MosaSettings.QemuEdk2X86;
				sb.Append(" -cpu qemu32,+sse4.1,abm,bmi1,bmi2,popcnt");
				break;
			}
			case "x64":
			{
				qemuApp = MosaSettings.QemuX64App;
				qemuUefi = MosaSettings.QemuEdk2X64;
				sb.Append(" -cpu qemu64,+sse4.1,abm,bmi1,bmi2,popcnt");
				break;
			}
			case "arm32":
			{
				qemuApp = MosaSettings.QemuARM32App;
				qemuUefi = MosaSettings.QemuEdk2ARM32;
				sb.Append(" -cpu arm1176");
				break;
			}
			case "arm64":
			{
				qemuApp = MosaSettings.QemuARM64App;
				qemuUefi = MosaSettings.QemuEdk2ARM64;
				sb.Append(" -cpu cortex-a7");
				break;
			}
			default: throw new CompilerException($"Unknown platform: {MosaSettings.Platform}");
		}

		switch (MosaSettings.EmulatorGraphics)
		{
			case "virtio": sb.Append(" -device virtio-vga"); break;
			case "vmware": sb.Append(" -vga vmware"); break;
			case "cirrus": sb.Append(" -vga cirrus"); break;
			case "std": sb.Append(" -vga std"); break;
		}

		if (!MosaSettings.EmulatorDisplay || MosaSettings.LauncherTest)
			sb.Append(" -display none");
		else
			sb.Append(RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? " -display cocoa" : " -display sdl");

		var serial = MosaSettings.EmulatorSerial;

		if (MosaSettings.LauncherSerial || MosaSettings.LauncherTest)
			serial = "tcpserver";

		switch (serial)
		{
			case "pipe":
			{
				sb.Append(" -serial pipe:\"");
				sb.Append(MosaSettings.EmulatorSerialPipe);
				sb.Append('"');
				break;
			}
			case "tcpserver":
			{
				sb.Append(" -serial tcp:");
				sb.Append(MosaSettings.EmulatorSerialHost);
				sb.Append(':');
				sb.Append(MosaSettings.EmulatorSerialPort);
				sb.Append(",server,nowait");
				break;
			}
			case "tcpclient":
			{
				sb.Append(" -serial tcp:");
				sb.Append(MosaSettings.EmulatorSerialHost);
				sb.Append(':');
				sb.Append(MosaSettings.EmulatorSerialPort);
				sb.Append(",client,nowait");
				break;
			}
			default:
			{
				sb.Append(" -serial null");
				break;
			}
		}

		if (MosaSettings.EmulatorGDB)
		{
			sb.Append(" -S -gdb tcp::");
			sb.Append(MosaSettings.GDBPort);
		}

		switch (MosaSettings.ImageFormat)
		{
			case "bin":
			{
				sb.Append(" -kernel \"");
				sb.Append(MosaSettings.ImageFile);
				sb.Append('"');
				break;
			}
			default:
			{
				sb.Append(" -drive format=raw,file=\"");
				sb.Append(MosaSettings.ImageFile);
				sb.Append('"');
				break;
			}
		}

		switch (MosaSettings.ImageFirmware)
		{
			case "bios":
			{
				sb.Append(" -L \"");
				sb.Append(MosaSettings.QemuBIOS);
				sb.Append('"');
				break;
			}
			case "uefi":
			{
				sb.Append(" -drive if=pflash,format=raw,readonly=on,file=\"");
				sb.Append(qemuUefi);
				sb.Append('"');
				break;
			}
		}

		return CreateApplicationProcess(qemuApp, sb.ToString());
	}

	private Process LaunchBochs()
	{
		OutputStatus("Launching Bochs");

		var logFile = Path.Combine(MosaSettings.TemporaryFolder, $"{Path.GetFileNameWithoutExtension(MosaSettings.ImageFile)}-bochs.log");
		var configFile = Path.Combine(MosaSettings.TemporaryFolder, $"{Path.GetFileNameWithoutExtension(MosaSettings.ImageFile)}.bxrc");

		var sb = new StringBuilder();

		sb.Append("megs: ");
		sb.AppendLine(MosaSettings.EmulatorMemory.ToString());

		sb.AppendLine("ata0: enabled=1,ioaddr1=0x1f0,ioaddr2=0x3f0,irq=14");
		sb.AppendLine("cpu: model=core2_penryn_t9600, count=1");
		sb.AppendLine("boot: cdrom,disk");

		sb.Append("log: \"");
		sb.Append(logFile);
		sb.AppendLine("\"");

		sb.Append("romimage: file=\"");
		sb.Append(MosaSettings.BochsBIOS);
		sb.AppendLine("\"");

		sb.Append("vgaromimage: file=\"");
		sb.Append(MosaSettings.BochsVGABIOS);
		sb.AppendLine("\"");

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) sb.AppendLine("display_library: x, options=gui_debug");

		sb.Append("ata0-master: type=disk,path=\"");
		sb.Append(MosaSettings.ImageFile);
		sb.AppendLine("\",biosdetect=none,cylinders=0,heads=0,spt=0");

		switch (MosaSettings.EmulatorGraphics)
		{
			case "vbe": sb.AppendLine("vga: extension=vbe"); break;
			case "cirrus": sb.AppendLine("vga: extension=cirrus"); break;
		}

		// FIXME: Bochs waits for the server to be available before starting the VM
		switch (MosaSettings.EmulatorSerial)
		{
			case "pipe":
			{
				sb.Append("com1: enabled=1, mode=pipe-server, dev=\"\\\\.\\pipe\\");
				sb.Append(MosaSettings.EmulatorSerialPipe);
				sb.AppendLine("\"");
				break;
			}
			case "tcpserver":
			{
				sb.Append("com1: enabled=1, mode=socket-server, dev=");
				sb.Append(MosaSettings.EmulatorSerialHost);
				sb.Append(':');
				sb.AppendLine(MosaSettings.EmulatorSerialPort.ToString());
				break;
			}
			case "tcpclient":
			{
				sb.Append("com1: enabled=1, mode=socket-client, dev=");
				sb.Append(MosaSettings.EmulatorSerialHost);
				sb.Append(':');
				sb.AppendLine(MosaSettings.EmulatorSerialPort.ToString());
				break;
			}
		}

		if (MosaSettings.EmulatorGDB)
		{
			// Untested
			sb.Append("gdbstub: enabled=1, port=");
			sb.Append(MosaSettings.GDBPort);
			sb.AppendLine(", text_base=0, data_base=0, bss_base=0");
		}

		File.WriteAllText(configFile, sb.ToString());

		return CreateApplicationProcess(MosaSettings.BochsApp, $"-q -f \"{configFile}\"");
	}

	private Process LaunchVMware()
	{
		OutputStatus("Launching VMware Workstation");

		var configFile = Path.Combine(MosaSettings.TemporaryFolder, Path.ChangeExtension(MosaSettings.ImageFile, ".vmx")!);
		var sb = new StringBuilder();

		sb.AppendLine(".encoding = \"windows-1252\"");
		sb.AppendLine("config.version = \"8\"");
		sb.AppendLine("virtualHW.version = \"17\"");

		sb.Append("memsize = \"");
		sb.Append(MosaSettings.EmulatorMemory);
		sb.AppendLine("\"");

		sb.Append("displayName = \"MOSA - ");
		sb.Append(Path.GetFileNameWithoutExtension(MosaSettings.SourceFiles[0]));
		sb.AppendLine("\"");

		sb.AppendLine("guestOS = \"other\"");
		sb.AppendLine("priority.grabbed = \"normal\"");
		sb.AppendLine("priority.ungrabbed = \"normal\"");
		sb.AppendLine("virtualHW.productCompatibility = \"hosted\"");
		sb.AppendLine("numvcpus = \"1\"");

		sb.Append("cpuid.coresPerSocket = \"");
		sb.Append(MosaSettings.EmulatorCores);
		sb.AppendLine("\"");

		sb.AppendLine("ide0:0.present = \"TRUE\"");

		sb.Append("ide0:0.fileName = \"");
		sb.Append(MosaSettings.ImageFile);
		sb.AppendLine("\"");

		sb.AppendLine("sound.present = \"TRUE\"");
		sb.AppendLine("sound.opl3.enabled = \"TRUE\"");
		sb.AppendLine("sound.virtualDev = \"sb16\"");

		sb.AppendLine("floppy0.present = \"FALSE\"");

		// COM1
		if (!string.IsNullOrEmpty(MosaSettings.EmulatorSerial) && MosaSettings.EmulatorSerial == "pipe")
		{
			sb.AppendLine("serial1.present = \"TRUE\"");
			sb.AppendLine("serial1.yieldOnMsrRead = \"FALSE\"");
			sb.AppendLine("serial1.fileType = \"pipe\"");

			sb.Append("serial1.fileName = \"\\\\.\\pipe\\");
			sb.Append(MosaSettings.EmulatorSerialPipe);
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

		var args = $"\"{configFile}\" -x -q";

		if (!string.IsNullOrWhiteSpace(MosaSettings.VmwareWorkstationApp))
			return CreateApplicationProcess(MosaSettings.VmwareWorkstationApp, args);

		return !string.IsNullOrWhiteSpace(MosaSettings.VmwarePlayerApp)
			? CreateApplicationProcess(MosaSettings.VmwarePlayerApp, args)
			: null;
	}

	private Process LaunchVirtualBox()
	{
		OutputStatus("Launching VirtualBox");

		if (GetOutput(LaunchApplication(MosaSettings.VirtualBoxApp, "list vms")).Contains(MosaSettings.OSName))
		{
			var newFile = Path.ChangeExtension(MosaSettings.ImageFile, "bak");

			// Janky way of keeping the image file
			File.Move(MosaSettings.ImageFile, newFile);

			// Delete the VM first
			LaunchApplication(MosaSettings.VirtualBoxApp, $"unregistervm {MosaSettings.OSName} --delete").WaitForExit();

			// Restore the image file
			File.Move(newFile, MosaSettings.ImageFile);
		}

		LaunchApplication(MosaSettings.VirtualBoxApp, $"createvm --name {MosaSettings.OSName} --ostype Other --register").WaitForExit();
		LaunchApplication(MosaSettings.VirtualBoxApp, $"modifyvm {MosaSettings.OSName} --memory {MosaSettings.EmulatorMemory} --cpus {MosaSettings.EmulatorCores} --graphicscontroller vmsvga").WaitForExit();
		LaunchApplication(MosaSettings.VirtualBoxApp, $"storagectl {MosaSettings.OSName} --name Controller --add ide --controller PIIX4").WaitForExit();
		LaunchApplication(MosaSettings.VirtualBoxApp, $"storageattach {MosaSettings.OSName} --storagectl Controller --port 0 --device 0 --type hdd --medium \"{MosaSettings.ImageFile}\"").WaitForExit();

		return CreateApplicationProcess(MosaSettings.VirtualBoxApp, $"startvm {MosaSettings.OSName}");
	}

	private void LaunchDebugger()
	{
		// FIXME!!!
		var sb = new StringBuilder();

		sb.Append("-output-debug-file ");
		sb.Append(Path.Combine(MosaSettings.TemporaryFolder, Path.ChangeExtension(MosaSettings.ImageFile, ".debug")!));
		sb.Append(' ');
		sb.Append("-gdb-host ");
		sb.Append(MosaSettings.GDBHost);
		sb.Append(' ');
		sb.Append("-gdb-port ");
		sb.Append(MosaSettings.GDBPort);
		sb.Append(' ');
		sb.Append("-image \"");
		sb.Append(MosaSettings.ImageFile);
		sb.Append('"');

		LaunchApplication("Mosa.Tool.Debugger.exe", sb.ToString());
	}

	private void LaunchGDB()
	{
		var gdbScript = Path.Combine(MosaSettings.TemporaryFolder, Path.GetFileNameWithoutExtension(MosaSettings.ImageFile) + ".gdb");

		var symbol = Linker.EntryPoint;
		var breakAddress = symbol.VirtualAddress;

		var sb = new StringBuilder();

		sb.Append("target remote localhost:");
		sb.Append(MosaSettings.GDBPort);
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

		OutputStatus($"Created configuration file: {gdbScript}");
		OutputStatus("==================");
		OutputStatus(script);
		OutputStatus("==================");

		sb.Clear();
		sb.Append("-d \"");
		sb.Append(MosaSettings.TemporaryFolder);
		sb.Append("\" -s \"");
		sb.Append(Path.Combine(MosaSettings.TemporaryFolder, Path.ChangeExtension(MosaSettings.ImageFile, ".bin")!));
		sb.Append("\" -x \"");
		sb.Append(gdbScript);
		sb.Append('"');

		LaunchConsoleApplication(MosaSettings.GDBApp, sb.ToString());
	}
}
