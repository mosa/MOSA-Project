using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Mosa.Tool.Mosactl
{

	public class Application
	{
		private bool IsWin = false;
		private bool IsUnix = false;

		public Application()
		{
			IsWin = Environment.OSVersion.Platform != PlatformID.Unix;
			IsUnix = Environment.OSVersion.Platform == PlatformID.Unix;
			BinDir = Path.GetDirectoryName(typeof(Application).Assembly.Location);
			RootDir = Path.GetDirectoryName(BinDir);
			SourceDir = Path.Combine(RootDir, "Source");
		}

		private string BinDir;
		private string RootDir;
		private string SourceDir;

		public void Run(List<string> args)
		{
			if (args.Count == 0)
			{
				PrintHelp("usage");
				return;
			}

			switch (args[0])
			{
				case "framework":
					TaskFramework();
					break;
				case "net":
				case "dotnet":
					TaskCILBuild(args);
					break;
				case "bin":
				case "binary":
					TaskBinaryBuild(args);
					break;
				case "run":
					TaskRun(args);
					break;
				case "debug":
					TaskDebug(args);
					break;
			}
		}

		private void PrintHelp(string name)
		{
			using (var reader = new StreamReader(typeof(Application).Assembly.GetManifestResourceStream("Mosa.Tool.Mosactl.Help." + name + ".txt")))
			{
				Console.WriteLine(reader.ReadToEnd());
			}
		}

		public void TaskCILBuild(List<string> args)
		{
			if (IsUnix)
			{
				CallProcess(SourceDir, "msbuild", "Mosa.HelloWorld.x86/Mosa.HelloWorld.x86.csproj");
			}
			else
			{
			}
		}

		private bool CallProcess(string workdir, string cmd, params string[] args)
		{
			Console.WriteLine("Call: " + cmd + string.Join("", args.Select(a => " " + a)));
			var start = new ProcessStartInfo();
			start.FileName = cmd;
			start.Arguments = string.Join(" ", args);
			start.WorkingDirectory = workdir;

			var proc = Process.Start(start);
			proc.WaitForExit();

			return proc.ExitCode == 0;
		}

		public void TaskBinaryBuild(List<string> args)
		{
			if (IsUnix)
			{
				CallProcess(BinDir, "mono", "Mosa.Tool.Compiler.exe",
				"-o",
				"Mosa.HelloWorld.x86.bin",
				"-a",
				"x86",
				"--mboot",
				"v1",
				"--map",
				"Mosa.HelloWorld.x86.map",
				//"--debug-info",
				//"Mosa.HelloWorld.x86.debug",
				"--x86-irq-methods",
				"--base-address",
				"0x00500000",
				"mscorlib.dll",
				"Mosa.Plug.Korlib.dll",
				"Mosa.Plug.Korlib.x86.dll",
				"Mosa.HelloWorld.x86.exe");
			}
			else
			{
			}
		}

		public void TaskDiskBuild() { }

		public void TaskBuild(List<string> args)
		{
			TaskCILBuild(args);
			TaskBinaryBuild(args);
			TaskDiskBuild();
		}

		public void TaskRun(List<string> args)
		{
			if (IsUnix)
			{
				CallProcess(BinDir, "qemu-system-i386", "Mosa.HelloWorld.x86.bin");
			}
			else
			{
			}
		}

		public void TaskDebug(List<string> args)
		{
			if (IsUnix)
			{
				//CallProcess(BinDir, "gdb", "-x","../Demo");
			}
			else
			{
			}
		}

		public void TaskDebug()
		{
		}

		public void TaskFramework()
		{
			Console.WriteLine("Cannot use target 'framework' directly. Please use the mosactl script in the project root directory.");
		}
	}

}
