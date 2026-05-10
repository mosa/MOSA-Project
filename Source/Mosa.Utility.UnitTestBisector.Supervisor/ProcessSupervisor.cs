// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Text.Json;
using Mosa.Utility.Configuration;

namespace Mosa.Utility.UnitTestBisector.Supervisor;

internal sealed class ProcessSupervisor
{
	private const int RestartDelayMs = 1000;
	private const int WorkerContinueExitCode = 2;

	private const string OptionBisectWorkerIteration = "-bisect-worker-iteration";
	private const string OptionBisectState = "-bisect-state";
	private const string OptionBisectWorkingDir = "-bisect-working-dir";
	private const string OptionBisectMaxRestarts = "-bisect-max-restarts";
	private const string OptionBisectReset = "-bisect-reset";

	private readonly struct StateSnapshot(bool found, bool completed, int iterationNumber, int totalIterationCount, int passCount, int nextIndex, string lastExitKind, int lastExitCode)
	{
		public bool Found { get; } = found;

		public bool Completed { get; } = completed;

		public int IterationNumber { get; } = iterationNumber;

		public int TotalIterationCount { get; } = totalIterationCount;

		public int PassCount { get; } = passCount;

		public int NextIndex { get; } = nextIndex;

		public string LastExitKind { get; } = lastExitKind;

		public int LastExitCode { get; } = lastExitCode;
	}

	private readonly Stopwatch stopwatch = new();
	private readonly MosaSettings settings;
	private readonly string[] args;
	private int restartCount;

	public ProcessSupervisor(MosaSettings settings, string[] args)
	{
		this.settings = settings;
		this.args = args ?? Array.Empty<string>();
	}

	public int Run()
	{
		var targetPath = ResolveAndValidateTargetPath();
		settings.BisectorWorkerIteration = true;
		var workingDirectory = ResolveAndValidateWorkingDirectory(targetPath);
		var maxRestarts = GetValidatedMaxRestarts();
		var stateFile = ResolveStateFilePath(settings.BisectorStateFile, workingDirectory);
		var targetArguments = BuildTargetArguments(stateFile);
		var supervisorIteration = 0;

		if (settings.BisectorResetState && File.Exists(stateFile))
			File.Delete(stateFile);

		stopwatch.Start();
		OutputStatus("Supervisor started");
		OutputStatus($"Target Arguments: {targetArguments}");

		while (true)
		{
			supervisorIteration++;
			OutputStatus($"Supervisor Iteration: {supervisorIteration}");

			var before = ReadStateSnapshot(stateFile, $"Supervisor iteration {supervisorIteration} pre-launch state");
			if (before.Found && before.Completed)
			{
				OutputStatus("State indicates completed=true before launch. Exiting supervisor.");
				if (before.LastExitCode != 0)
					OutputStatus($"WARNING: completed state has non-zero last exit code {before.LastExitCode} ({before.LastExitKind}). Treating as complete.");

				return 0;
			}

			using var process = StartTarget(targetPath, targetArguments, workingDirectory);
			process.WaitForExit();
			var exitCode = process.ExitCode;

			var after = ReadStateSnapshot(stateFile, $"Supervisor iteration {supervisorIteration} post-exit state");

			if (after.Found && after.Completed)
			{
				if (exitCode != 0)
					OutputStatus($"WARNING: target exited with non-zero code {exitCode} but state reports completed=true. Treating as complete.");

				OutputStatus("completed-from-state");
				return 0;
			}

			if (exitCode == WorkerContinueExitCode)
			{
				OutputStatus("continue-iteration");
				continue;
			}

			if (exitCode == 0)
			{
				OutputStatus("Target exited successfully.");
				return 0;
			}

			if (after.Found && string.Equals(after.LastExitKind, "Failure", StringComparison.Ordinal))
			{
				OutputStatus($"abnormal-exit-code: {exitCode}");
				OutputStatus("State indicates a terminal failure (non-retriable). Exiting supervisor.");
				return exitCode;
			}

			var verifiedExitCode = !after.Found || after.LastExitCode == 0 || after.LastExitCode == exitCode;
			if (verifiedExitCode)
			{
				OutputStatus($"abnormal-exit-code: {exitCode}");
			}
			else
			{
				OutputStatus($"WARNING: abnormal exit code mismatch. Process={exitCode}, State={after.LastExitCode} ({after.LastExitKind})");
				OutputStatus($"abnormal-exit-code: {exitCode}");
			}

			restartCount++;
			OutputStatus($"abnormal-exit-retry (restart #{restartCount})");

			if (maxRestarts > 0 && restartCount > maxRestarts)
			{
				OutputStatus("Maximum restart count reached. Exiting supervisor.");
				return exitCode;
			}

			Thread.Sleep(RestartDelayMs);
		}
	}

	private string ResolveAndValidateTargetPath()
	{
		settings.LoadAppLocations();

		var discoveredTargetPath = settings.BisectorApp;
		if (string.IsNullOrWhiteSpace(discoveredTargetPath))
			throw new InvalidOperationException("Unable to locate bisector target from app locations.");

		var resolvedDiscoveredTargetPath = Path.IsPathRooted(discoveredTargetPath)
			? discoveredTargetPath
			: Path.GetFullPath(discoveredTargetPath);

		if (!File.Exists(resolvedDiscoveredTargetPath))
			throw new InvalidOperationException($"Discovered target does not exist: {resolvedDiscoveredTargetPath}");

		return resolvedDiscoveredTargetPath;
	}

	private string BuildTargetArguments(string stateFile)
	{
		var forwarded = new List<string>();

		for (var i = 0; i < args.Length; i++)
		{
			var arg = args[i];

			if (IsSupervisorOption(arg, out var takesValue))
			{
				if (takesValue && i + 1 < args.Length)
					i++;
				continue;
			}

			forwarded.Add(QuoteIfNeeded(arg));
		}

		if (settings.BisectorWorkerIteration)
			forwarded.Add(OptionBisectWorkerIteration);

		forwarded.Add(OptionBisectState);
		forwarded.Add(QuoteIfNeeded(stateFile));

		return string.Join(" ", forwarded);
	}

	private static bool IsSupervisorOption(string arg, out bool takesValue)
	{
		takesValue = arg switch
		{
			var option when string.Equals(option, OptionBisectWorkerIteration, StringComparison.OrdinalIgnoreCase) => false,
			var option when string.Equals(option, OptionBisectReset, StringComparison.OrdinalIgnoreCase) => false,
			var option when string.Equals(option, OptionBisectWorkingDir, StringComparison.OrdinalIgnoreCase) => true,
			var option when string.Equals(option, OptionBisectMaxRestarts, StringComparison.OrdinalIgnoreCase) => true,
			var option when string.Equals(option, OptionBisectState, StringComparison.OrdinalIgnoreCase) => true,
			_ => false,
		};

		return takesValue || string.Equals(arg, OptionBisectWorkerIteration, StringComparison.OrdinalIgnoreCase) || string.Equals(arg, OptionBisectReset, StringComparison.OrdinalIgnoreCase);
	}

	private static string QuoteIfNeeded(string arg)
	{
		if (arg.Contains(' '))
			return $"\"{arg.Replace("\"", "\\\"")}" + "\"";

		return arg;
	}

	private string ResolveAndValidateWorkingDirectory(string targetPath)
	{
		var workingDirectory = settings.BisectorWorkingDirectory;
		if (string.IsNullOrWhiteSpace(workingDirectory))
			workingDirectory = Environment.CurrentDirectory;
		else if (!Path.IsPathRooted(workingDirectory))
			workingDirectory = Path.GetFullPath(workingDirectory);

		if (string.IsNullOrWhiteSpace(workingDirectory))
			workingDirectory = Path.GetDirectoryName(targetPath) ?? Environment.CurrentDirectory;

		if (!Directory.Exists(workingDirectory))
			throw new InvalidOperationException($"Working directory does not exist: {workingDirectory}");

		return workingDirectory;
	}

	private int GetValidatedMaxRestarts()
	{
		var value = settings.BisectorMaxRestarts;
		if (value < 0)
			throw new InvalidOperationException($"Invalid value for {OptionBisectMaxRestarts}. Minimum is 0.");

		return value;
	}

	private static string ResolveStateFilePath(string stateFile, string workingDirectory)
	{
		if (string.IsNullOrWhiteSpace(stateFile))
			stateFile = MosaSettings.Constant.BisectorStateFile;

		if (Path.IsPathRooted(stateFile))
			return stateFile;

		return Path.GetFullPath(Path.Combine(workingDirectory, stateFile));
	}

	private StateSnapshot ReadStateSnapshot(string stateFile, string context)
	{
		if (!File.Exists(stateFile))
			return new StateSnapshot(false, false, 0, 0, 0, 0, "Unknown", 0);

		try
		{
			var content = File.ReadAllText(stateFile);
			using var jsonDocument = JsonDocument.Parse(content);
			var root = jsonDocument.RootElement;

			var completed = ReadBoolean(root, "Completed");
			var iterationNumber = ReadInt32(root, "IterationNumber");
			var totalIterations = ReadInt32(root, "TotalIterationCount");
			var passCount = ReadInt32(root, "PassCount");
			var nextIndex = ReadInt32(root, "NextIndex");
			var lastExitKind = ReadString(root, "LastExitKind") ?? "Unknown";
			var lastExitCode = ReadInt32(root, "LastExitCode");

			return new StateSnapshot(true, completed, iterationNumber, totalIterations, passCount, nextIndex, lastExitKind, lastExitCode);
		}
		catch (Exception ex)
		{
			OutputStatus($"WARNING: failed to read state file: {ex.Message}");
			return new StateSnapshot(false, false, 0, 0, 0, 0, "Unknown", 0);
		}
	}

	private static bool ReadBoolean(JsonElement root, string propertyName)
	{
		if (!root.TryGetProperty(propertyName, out var value))
			return false;

		if (value.ValueKind == JsonValueKind.True)
			return true;

		if (value.ValueKind == JsonValueKind.False)
			return false;

		return false;
	}

	private static string ReadString(JsonElement root, string propertyName)
	{
		if (!root.TryGetProperty(propertyName, out var value))
			return null;

		if (value.ValueKind == JsonValueKind.String)
			return value.GetString();

		return null;
	}

	private static int ReadInt32(JsonElement root, string propertyName)
	{
		if (!root.TryGetProperty(propertyName, out var value))
			return 0;

		if (value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out var number))
			return number;

		return 0;
	}

	private Process StartTarget(string targetPath, string targetArguments, string workingDirectory)
	{
		var startInfo = new ProcessStartInfo
		{
			FileName = targetPath,
			Arguments = targetArguments,
			WorkingDirectory = workingDirectory,
			UseShellExecute = false,
		};

		var process = Process.Start(startInfo);
		if (process == null)
			throw new InvalidOperationException($"Failed to start target: {targetPath}");

		TryEnableAllProcessors(process);

		OutputStatus($"Target started (PID: {process.Id})");
		return process;
	}

	private static void TryEnableAllProcessors(Process process)
	{
		try
		{
			var maxBits = IntPtr.Size * 8;
			var processorCount = Math.Max(1, Math.Min(Environment.ProcessorCount, maxBits));

			long mask;
			if (processorCount >= 63)
				mask = long.MaxValue;
			else
				mask = (1L << processorCount) - 1;

			process.ProcessorAffinity = (IntPtr)mask;
		}
		catch
		{
		}
	}

	private void OutputStatus(string status)
	{
		Console.WriteLine($"{stopwatch.Elapsed.TotalSeconds:00.00} | [Supervisor] {status}");
	}
}
