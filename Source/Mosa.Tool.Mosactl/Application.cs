using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

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

			appLocations = new Mosa.Utility.Launcher.AppLocations();
			appLocations.FindApplications();

			RootDir = GetEnv("MOSA_ROOT");
			BinDir = GetEnv("MOSA_BIN");
			SourceDir = GetEnv("MOSA_SOURCE");
		}

		private string BinDir;
		private string RootDir;
		private string SourceDir;

		private static Mosa.Utility.Launcher.AppLocations appLocations;

		public static string GetEnv(string name)
		{
			var value = Environment.GetEnvironmentVariable(name);
			if (string.IsNullOrEmpty(value))
			{
				switch (name)
				{
					case "MOSA_ROOT":
						value = Path.GetDirectoryName(Path.GetDirectoryName(new Uri(typeof(Program).Assembly.Location).AbsolutePath));
						break;
					case "MOSA_BIN":
						value = Path.Combine(GetEnv("MOSA_ROOT"), "bin");
						break;
					case "MOSA_SOURCE":
						value = Path.Combine(GetEnv("MOSA_ROOT"), "Source");
						break;
					case "MOSA_TOOLS":
						value = Path.Combine(GetEnv("MOSA_ROOT"), "Tools");
						break;
					case "MOSA_NUGET":
						value = Path.Combine(GetEnv("MOSA_TOOLS"), "Nuget", "Nuget.exe");
						break;
					case "MOSA_MSBUILD":
						value = appLocations.MsBuild;
						break;
					case "MOSA_WIN_OSDIR":
						value = @"C:\Windows";
						break;
					case "MOSA_WIN_PROGRAMS":
						value = @"C:\Program Files"; // TODO
						break;
					case "MOSA_WIN_PROGRAMS_X86":
						value = @"C:\Program Files (x86)"; // TODO
						break;
				}
			}

			if (string.IsNullOrEmpty(value))
				return "";

			var regex = new Regex(@"\$\{(\w+)\}", RegexOptions.RightToLeft);
			foreach (Match m in regex.Matches(value))
				value = value.Replace(m.Value, GetEnv(m.Groups[1].Value));
			return value;
		}

		public void Run(List<string> args)
		{
			if (args.Count == 0)
			{
				PrintHelp("usage");
				return;
			}

			switch (args[0])
			{
				case "tools":
					TaskTools();
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
			CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.HelloWorld.x86/Mosa.HelloWorld.x86.csproj");
		}

		private bool CallMonoProcess(string workdir, string cmd, params string[] args)
		{
			var info = PrepareNetRuntime(cmd, args.ToList());
			return CallProcess(workdir, info.target, info.Args);
		}

		private bool CallProcess(string workdir, string cmd, params string[] args)
		{
			Console.WriteLine("Call: " + cmd + string.Join("", args.Select(a => " " + a)));
			var start = new ProcessStartInfo();
			start.FileName = cmd;
			start.Arguments = string.Join(" ", args);
			start.WorkingDirectory = workdir;
			start.UseShellExecute = false;

			var proc = Process.Start(start);
			proc.WaitForExit();

			return proc.ExitCode == 0;
		}

		public void TaskBinaryBuild(List<string> args)
		{
			var compilerArgs = new List<string>() {
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
				"Mosa.HelloWorld.x86.exe"
			};


			CallProcess(BinDir, "Mosa.Tool.Compiler.exe", compilerArgs.ToArray());
		}

		private class PlattformAppCall
		{
			public string target;
			public string[] Args;
		}

		private PlattformAppCall PrepareNetRuntime(string netApplication, List<string> appArgs)
		{
			appArgs = new List<string>(appArgs);

			if (IsUnix)
			{
				appArgs.Insert(0, netApplication);
				return new PlattformAppCall
				{
					target = "mono",
					Args = appArgs.ToArray()
				};
			}
			else
			{
				return new PlattformAppCall
				{
					target = netApplication,
					Args = appArgs.ToArray()
				};
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

		public void TaskTools()
		{
			CallMonoProcess(SourceDir, GetEnv("MOSA_NUGET"), "restore", "Mosa.sln");
			CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Tool.Compiler/Mosa.Tool.Compiler.csproj");
		}
	}

}
