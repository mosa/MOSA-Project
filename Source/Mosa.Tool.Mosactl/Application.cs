using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;

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

			appLocations = new Utility.Launcher.AppLocations();
			appLocations.FindApplications();

			RootDir = GetEnv("MOSA_ROOT");
			BinDir = GetEnv("MOSA_BIN");
			SourceDir = GetEnv("MOSA_SOURCE");
		}

		private string BinDir;
		private string RootDir;
		private string SourceDir;

		private static Utility.Launcher.AppLocations appLocations;

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
				}
			}

			if (string.IsNullOrEmpty(value))
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
				return;
			}

			if (args.Count > 1)
				OsName = args.Last(); // TODO!

			switch (args[0])
			{
				case "tools":
					TaskTools(CheckType.force);
					break;
				case "runtime":
					TaskRuntime(CheckType.force);
					break;
				case "net":
				case "dotnet":
					TaskCILBuild(CheckType.force, args);
					break;
				case "bin":
				case "binary":
					TaskBinaryBuild(CheckType.force, args);
					break;
				case "run":
					TaskRun(args);
					break;
				case "test":
					TaskTest(args);
					break;
				case "debug":
					TaskDebug(args);
					break;
			}
		}

		private string OsName = "HelloWorld";

		private void PrintHelp(string name)
		{
			using (var reader = new StreamReader(typeof(Application).Assembly.GetManifestResourceStream("Mosa.Tool.Mosactl.Help." + name + ".txt")))
			{
				Console.WriteLine(reader.ReadToEnd());
			}
		}

		public void TaskCILBuild(CheckType ct, List<string> args)
		{
			TaskRuntime(CheckType.changed);

			if (!File.Exists(GetEnv(ExpandKernelBinPath(OsName) + ".exe")) || ct == CheckType.force)
			{
				CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), ExpandKernelCsProjPath(OsName));
			}
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

			return proc.ExitCode == 0;
		}

		public void TaskBinaryBuild(CheckType ct, List<string> args)
		{
			TaskTools(CheckType.changed);
			TaskCILBuild(CheckType.changed, args);

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

				CallMonoProcess(BinDir, "Mosa.Tool.Compiler.exe", compilerArgs.ToArray());
			}
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
			TaskCILBuild(CheckType.force, args);
			TaskBinaryBuild(CheckType.force, args);
			TaskDiskBuild();
		}

		public void TaskRun(List<string> args)
		{
			TaskCILBuild(CheckType.changed, args);
			TaskBinaryBuild(CheckType.changed, args);

			CallQemu(false, null);
		}

		public void TaskTest(List<string> args)
		{
			TaskCILBuild(CheckType.changed, args);
			TaskBinaryBuild(CheckType.changed, args);

			var testSuccess = false;
			CallQemu(true, (line, proc) =>
			{
				if (line == "<TEST:PASSED:Boot.Main>")
				{
					testSuccess = true;
					proc.Kill();
				}
			});

			if (testSuccess)
				Console.WriteLine("Test PASSED");
			else
				Console.WriteLine("Test FAILED");
		}

		private bool CallQemu(bool nographic, Action<string, Process> OnKernelLog)
		{
			var logFile = ExpandKernelBinPath(OsName) + ".log";
			if (File.Exists(logFile))
				File.Delete(logFile);

			var args = new List<string>() {
				"-kernel", ExpandKernelBinPath(OsName) + ".bin",
				"-serial", "stdio",
				"-serial", "tcp::1235,server,nowait"
			};

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
						sb.Append(buf[0]);
					}

				}
			});
			th.Start();

			p.WaitForExit();
			return true;
		}

		public void TaskDebug(List<string> args)
		{
			TaskCILBuild(CheckType.changed, args);
			TaskBinaryBuild(CheckType.changed, args);

			if (IsWin)
			{
				CallProcess(BinDir, GetEnv("${MOSA_BIN}/Mosa.Tool.GDBDebugger.exe"), "--image", ExpandKernelBinPath(OsName) + ".bin", "--autostart", "--debugfile", ExpandKernelBinPath(OsName) + ".debug");
			}
			else
			{
				GenerateGDBFile();
				CallProcess(RootDir, "gdb", "-x", ExpandKernelBinPath(OsName) + ".gdb.load", "-x", GetEnv("${MOSA_ROOT}/Ressources/settings.gdb"));
			}
		}

		private void GenerateGDBFile()
		{
			var expand = ExpandKernelBinPath(OsName);
			var bin = expand + ".bin";
			var gdb = expand + ".gdb.load";
			var gdbqemu = expand + ".gdb.qemu";

			var sb = new StringBuilder();
			sb.AppendLine($"file {bin}");
			sb.AppendLine($"target remote | {gdbqemu}");
			File.WriteAllText(gdb, sb.ToString());

			sb.Clear();
			sb.AppendLine($"#/bin/bash");
			sb.AppendLine($"qemu-system-i386 -kernel {bin} -S -gdb stdio");
			File.WriteAllText(gdbqemu, sb.ToString());
			CallProcess(BinDir, "chmod", "+x", gdbqemu);
		}

		public void TaskTools(CheckType ct)
		{
			var exists = File.Exists(GetEnv("${MOSA_BIN}/Mosa.Tool.Compiler.exe"));
			if (!exists || ct == CheckType.force)
			{
				CallMonoProcess(SourceDir, GetEnv("MOSA_NUGET"), "restore", "Mosa.sln");
				CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Tool.Compiler/Mosa.Tool.Compiler.csproj");
				CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Tool.GDBDebugger/Mosa.Tool.GDBDebugger.csproj");
			}
		}

		public void TaskRuntime(CheckType ct)
		{
			var exists = File.Exists(GetEnv("${MOSA_BIN}/Mosa.Plug.Korlib.dll"));
			if (!exists || ct == CheckType.force)
			{
				CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Runtime.x86/Mosa.Runtime.x86.csproj");
				CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Runtime.x64/Mosa.Runtime.x64.csproj");
				CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Korlib/Mosa.Korlib.csproj");
				CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Plug.Korlib/Mosa.Plug.Korlib.csproj");
				CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Plug.Korlib.x86/Mosa.Plug.Korlib.x86.csproj");
				CallProcess(SourceDir, GetEnv("MOSA_MSBUILD"), "Mosa.Plug.Korlib.x64/Mosa.Plug.Korlib.x64.csproj");
			}
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

	public class TProcess
	{

		private Process proc;
		private ProcessStartInfo psi;

		public void waitForExit()
		{
			proc.WaitForExit();
		}

		public TProcess(string bin, string args)
		{
			psi = new ProcessStartInfo(bin, args);
		}

		public void start()
		{
			psi.RedirectStandardOutput = true;
			psi.UseShellExecute = false;

			proc = Process.Start(psi);
			try
			{
				proc.WaitForInputIdle();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		public static TProcess start(string bin, string args)
		{
			Console.WriteLine("EXEC: " + bin + " " + args);
			var proc = new TProcess(bin, args);
			proc.start();
			return proc;
		}

		public static int startWait(string bin, string args = "")
		{
			var proc = start(bin, args);
			proc.proc.WaitForExit();
			return proc.proc.ExitCode;
		}

		public void GetOutputAsync(Action<string> lineHandler)
		{
			var th = new Thread(() =>
			{
				foreach (var line in GetOutput())
				{
					callHandler(() => lineHandler(line));
				}
			});
			th.Start();
		}

		protected virtual void callHandler(Action cb)
		{
			cb();
		}

		public void GetAllOutputAsync(Action<IEnumerable<string>> lines)
		{
			var th = new Thread(() =>
			{
				callHandler(() => lines(GetAllOutput()));
			});
			th.Start();
		}

		public IEnumerable<string> GetOutput()
		{
			var buf = new char[1];
			var sb = new StringBuilder();
			while (!proc.HasExited)
			{
				var count = proc.StandardOutput.Read(buf, 0, 1);
				if (count > 0)
				{
					if (buf[0].ToString() == Environment.NewLine)
					{
						var line = sb.ToString();
						Console.WriteLine("EXEC-OUTPUT: " + line);
						if (onNewLine != null)
							onNewLine(line);
						yield return line;
						sb.Clear();
					}
					else
					{
						sb.Append(buf[0]);
					}
				}
				else
				{
					Thread.Sleep(50);
				}
			}
		}

		public static IEnumerable<string> GetAllOutput(string bin, string args)
		{
			var proc = new TProcess(bin, args);
			proc.start();
			return proc.GetAllOutput();
		}

		public static string GetAllOutputString(string bin, string args)
		{
			var proc = new TProcess(bin, args);
			proc.start();
			return proc.GetAllOutputString();
		}

		public IEnumerable<string> GetAllOutput()
		{
			var lines = proc.StandardOutput.ReadToEnd().Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
			foreach (var line in lines)
				Console.WriteLine("EXEC-OUTPUT: " + line);
			return lines;
		}

		public string GetAllOutputString()
		{
			return string.Join(Environment.NewLine, GetAllOutput());
		}

		public event Action<string> onNewLine;

		public static IEnumerable<string> GetOutput(string bin, string args)
		{
			var proc = TProcess.start(bin, args);
			return proc.GetOutput();
		}

	}


}
