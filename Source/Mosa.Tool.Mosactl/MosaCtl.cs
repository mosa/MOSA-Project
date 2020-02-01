// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Mosa.Tool.Mosactl
{
	public class MosaCtl
	{
		private static bool IsWin = Environment.OSVersion.Platform == PlatformID.Win32NT;
		private static bool IsUnix = Environment.OSVersion.Platform == PlatformID.Unix;

		private string BinDir;
		private string RootDir;
		private string SourceDir;

		private static AppLocations appLocations;

		public MosaCtl()
		{
			appLocations = new AppLocations();

			RootDir = GetEnv("MOSA_ROOT");
			BinDir = GetEnv("MOSA_BIN");
			SourceDir = GetEnv("MOSA_SOURCE");
		}

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
						value = Path.Combine(GetEnv("MOSA_TOOLS"), "nuget", "nuget.exe");
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

					case "MOSA_TOOL_EXT":
						value = IsWin ? ".exe" : "";
						break;
				}
			}

			if (value == null)
				value = name;

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
				Environment.Exit(1);
				return;
			}

			if (args.Count > 1)
				OsName = args.Last(); // TODO!

			switch (args[0])
			{
				case "tools":
					if (!TaskTools(CheckType.force))
						Environment.Exit(1);
					break;

				case "runtime":
					if (!TaskRuntime(CheckType.force))
						Environment.Exit(1);
					break;

				case "net":
				case "dotnet":
					if (!TaskCILBuild(CheckType.force, args))
						Environment.Exit(1);
					break;

				case "bin":
				case "binary":
					if (!TaskBinaryBuild(CheckType.force, args))
						Environment.Exit(1);
					break;

				case "run":
					if (!TaskRun(args))
						Environment.Exit(1);
					break;

				case "test":
					if (!TaskTest(args))
						Environment.Exit(1);
					break;

				case "unittest":
					if (!TaskUnitTest(args))
						Environment.Exit(1);
					break;

				case "debug":
					TaskDebug(args);
					break;

				case "help":
					PrintHelp("usage");
					break;
			}
		}

		private string OsName = "all";

		private string[] OsNames = new string[] { "helloworld", "coolworld" };

		private void PrintHelp(string name)
		{
			using (var reader = new StreamReader(typeof(MosaCtl).Assembly.GetManifestResourceStream("Mosa.Tool.Mosactl.Help." + name + ".txt")))
			{
				Console.WriteLine(reader.ReadToEnd());
			}
		}

		public bool TaskCILBuild(CheckType ct, List<string> args)
		{
			TaskRuntime(CheckType.changed);

			if (!File.Exists(GetEnv(ExpandKernelBinPath(OsName) + ".exe")) || ct == CheckType.force)
			{
				if (!CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), ExpandKernelCsProjPath(OsName), "-verbosity:minimal"))
					return false;
			}
			return true;
		}

		private bool CallMonoProcess(string workdir, string cmd, params string[] args)
		{
			var info = PrepareNetRuntime(cmd, args.ToList());
			return CallProcess(workdir, info.target, info.Args);
		}

		private bool CallProcess(string workdir, string cmd, params string[] args)
		{
			Console.WriteLine("Call: " + cmd + string.Join("", args.Select(a => " " + a)));
			Console.WriteLine("WorkDir: " + workdir);
			var start = new ProcessStartInfo();
			start.FileName = cmd;
			start.Arguments = string.Join(" ", args);
			start.WorkingDirectory = workdir;
			start.UseShellExecute = false;

			var proc = Process.Start(start);
			proc.WaitForExit();

			if (proc.ExitCode > 0)
			{
				Console.WriteLine("Exit Code " + proc.ExitCode);
			}

			return proc.ExitCode == 0;
		}

		public bool TaskBinaryBuild(CheckType ct, List<string> args)
		{
			if (!TaskTools(CheckType.changed))
				return false;
			if (!TaskCILBuild(CheckType.changed, args))
				return false;

			if (!File.Exists(ExpandKernelBinPath(OsName) + ".bin") || ct == CheckType.force)
			{
				var compilerArgs = new List<string>() {
					"-o",
					ExpandKernelBinPath(OsName)+".bin",
					"-a",
					"x86",
					"--mboot",
					"v1",
					"--map",
					ExpandKernelBinPath(OsName)+".map",
					"--debug-info",
					ExpandKernelBinPath(OsName)+".debug",
					"--base-address",
					"0x00500000",
					"mscorlib.dll",
					"Mosa.Plug.Korlib.dll",
					"Mosa.Plug.Korlib.x86.dll",
					ExpandKernelBinPath(OsName)+".exe"
				};

				if (!CallProcess(BinDir, GetEnv("${MOSA_BIN}/Mosa.Tool.Compiler${MOSA_TOOL_EXT}"), compilerArgs.ToArray()))
					return false;
			}

			return true;
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

		public void TaskDiskBuild()
		{
		}

		public void TaskBuild(List<string> args)
		{
			TaskCILBuild(CheckType.force, args);
			TaskBinaryBuild(CheckType.force, args);
			TaskDiskBuild();
		}

		public bool TaskRun(List<string> args)
		{
			var ct = args.Contains("--build") ? CheckType.force : CheckType.changed;
			if (!TaskCILBuild(ct, args))
				return false;
			if (!TaskBinaryBuild(ct, args))
				return false;

			if (!CallQemu(false, null))
				return false;

			return true;
		}

		public bool TaskTestAll(List<string> args)
		{
			foreach (var osName in OsNames)
				if (!CallProcess(BinDir, GetEnv("${MOSA_BIN}/Mosa.Tool.Mosactl${MOSA_TOOL_EXT}"), "test", osName))
					return false;

			return true;
		}

		public bool TaskTest(List<string> args)
		{
			if (OsName == "all")
				return TaskTestAll(args);

			if (!TaskCILBuild(CheckType.changed, args))
				return false;
			if (!TaskBinaryBuild(CheckType.changed, args))
				return false;

			var testSuccess = false;
			if (!CallQemu(true, (line, proc) =>
			 {
				 if (line == "<SELFTEST:PASSED>")
				 {
					 testSuccess = true;
					 proc.Kill();
				 }
			 }))
				return false;

			if (testSuccess)
			{
				Console.WriteLine("Test PASSED");
				return true;
			}
			else
			{
				Console.WriteLine("Test FAILED");
				return false;
			}
		}

		public bool TaskUnitTest(List<string> args)
		{
			OsName = "UnitTests";

			if (!TaskCILBuild(CheckType.changed, args))
				return false;

			if (!CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Utility.UnitTests/Mosa.Utility.UnitTests.csproj", "/p:Configuration=Debug", "/p:Platform=\"AnyCPU\"", "-verbosity:minimal"))
				return false;

			if (!CallProcess(BinDir, GetEnv("${MOSA_BIN}/Mosa.Utility.UnitTests${MOSA_TOOL_EXT}")))
				return false;

			return true;
		}

		private bool CallQemu(bool nographic, Action<string, Process> OnKernelLog)
		{
			var logFile = ExpandKernelBinPath(OsName) + ".log";
			if (File.Exists(logFile))
				File.Delete(logFile);

			var args = new List<string>() {
				"-kernel", ExpandKernelBinPath(OsName) + ".bin",
			};

			args.Add("-serial");
			args.Add("stdio");

			args.Add("-serial");
			args.Add("null");

			if (nographic)
				args.Add("-display none");

			var cmd = appLocations.QEMU;
			var workDir = BinDir;
			Console.WriteLine("Call: " + cmd + string.Join("", args.Select(a => " " + a)));
			Console.WriteLine("WorkDir: " + workDir);

			var start = new ProcessStartInfo();
			start.FileName = cmd;
			start.Arguments = string.Join(" ", args);
			start.WorkingDirectory = workDir;
			start.UseShellExecute = false;
			start.RedirectStandardOutput = true;

			var p = Process.Start(start);

			var th = new Thread(() =>
			{
				var buf = new char[1];
				var sb = new StringBuilder();
				while (true)
				{
					var count = p.StandardOutput.Read(buf, 0, 1);
					if (count == 0)
						break;
					Console.Write(buf[0]);
					if (buf[0] == '\n')
					{
						var line = sb.ToString();
						if (OnKernelLog != null) OnKernelLog(line, p);
						sb.Clear();
					}
					else
					{
						if (buf[0] != '\r')
							sb.Append(buf[0]);
					}
				}
			});
			th.Start();

			if (nographic)
			{
				var th2 = new Thread(() =>
				{
					Thread.Sleep(5000);
					if (!p.HasExited)
					{
						Console.WriteLine("Test Timeout");
						p.Kill();
					}
				});
				th2.Start();
			}

			p.WaitForExit();

			Console.WriteLine("Qemu exit code " + p.ExitCode);

			return p.ExitCode == 0 || p.ExitCode == 137 || p.ExitCode == -1;
		}

		public void TaskDebug(List<string> args)
		{
			TaskCILBuild(CheckType.changed, args);
			TaskBinaryBuild(CheckType.changed, args);

			if (IsWin)
			{
				CallProcess(BinDir, GetEnv("${MOSA_BIN}/Mosa.Tool.Debugger${MOSA_TOOL_EXT}"), "--image", ExpandKernelBinPath(OsName) + ".bin", "--autostart", "--debugfile", ExpandKernelBinPath(OsName) + ".debug");
			}
			else
			{
				GenerateGDBFile();
				Environment.Exit(0);
				return;
			}
		}

		private void GenerateGDBFile()
		{
			var expand = ExpandKernelBinPath(OsName);
			var bin = expand + ".bin";
			var log = expand + ".log";
			var gdbLaunch = GetEnv("${MOSA_BIN}/.mosactl.tmp.script");
			var gdb = expand + ".gdb.script";
			var gdbqemu = expand + ".gdb.qemu";

			if (File.Exists(log))
				File.Delete(log);

			var sb = new StringBuilder();
			sb.AppendLine($"file {bin}");
			sb.AppendLine($"target remote | {gdbqemu}");
			File.WriteAllText(gdb, sb.ToString());

			sb.Clear();
			sb.AppendLine($"#!/bin/bash");
			sb.AppendLine($"# This is a autogenerated file.");
			sb.AppendLine($"gdb -x {ExpandKernelBinPath(OsName)}.gdb\t.script -x {GetEnv("${MOSA_ROOT}/Ressources/settings.gdb")}");
			File.WriteAllText(gdbLaunch, sb.ToString());

			sb.Clear();
			sb.AppendLine($"#!/bin/bash");
			sb.AppendLine($"qemu-system-i386 -kernel {bin} -S -gdb stdio -serial file:{log} -serial null");
			File.WriteAllText(gdbqemu, sb.ToString());
			CallProcess(BinDir, "chmod", "+x", gdbqemu);
		}

		public bool TaskTools(CheckType ct)
		{
			var exists = File.Exists(GetEnv("${MOSA_BIN}/Mosa.Tool.Compiler${MOSA_TOOL_EXT}"));
			if (!exists || ct == CheckType.force)
			{
				if (!CallMonoProcess(SourceDir, GetEnv("MOSA_NUGET"), "restore", "Mosa.sln"))
					return false;
				if (!CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Tool.Compiler/Mosa.Tool.Compiler.csproj"))
					return false;
				if (IsWin)
				{
					if (!CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Tool.Debugger/Mosa.Tool.Debugger.csproj"))
						return false;
				}
			}

			return true;
		}

		public bool TaskRuntime(CheckType ct)
		{
			var exists = File.Exists(GetEnv("${MOSA_BIN}/Mosa.Plug.Korlib.dll"));
			if (!exists || ct == CheckType.force)
			{
				if (!CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Runtime.x86/Mosa.Runtime.x86.csproj"))
					return false;
				if (!CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Runtime.x64/Mosa.Runtime.x64.csproj"))
					return false;
				if (!CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Korlib/Mosa.Korlib.csproj"))
					return false;
				if (!CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Plug.Korlib/Mosa.Plug.Korlib.csproj"))
					return false;
				if (!CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Plug.Korlib.x86/Mosa.Plug.Korlib.x86.csproj"))
					return false;
				if (!CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Plug.Korlib.x64/Mosa.Plug.Korlib.x64.csproj"))
					return false;
			}
			return true;
		}

		public enum CheckType
		{
			force,
			changed,
			notAvailable
		}

		public string ExpandKernelName(string name)
		{
			switch (name.ToLower())
			{
				case "helloworld":
					return "Mosa.HelloWorld.x86";

				case "coolworld":
					return "Mosa.CoolWorld.x86";

				case "testworld":
					return "Mosa.TestWorld.x86";

				case "unittests":
					return "Mosa.UnitTests.x86";
			}
			return name;
		}

		public string ExpandKernelCsProjPath(string name)
		{
			var expandedName = ExpandKernelName(name);
			if (expandedName != name)
			{ // valid Alias
				return GetEnv("${MOSA_SOURCE}/" + expandedName + "/" + expandedName + ".csproj");
			}
			return name;
		}

		public string ExpandKernelBinPath(string name)
		{
			var expandedName = ExpandKernelName(name);
			if (expandedName != name)
			{ // valid Alias
				return GetEnv("${MOSA_BIN}/" + expandedName);
			}
			return name;
		}
	}
}
