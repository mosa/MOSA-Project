// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Reflection;
using System.Resources;
using Mosa.Compiler.Framework;
using Mosa.Utility.Configuration;

namespace Mosa.Utility.Launcher;

public class BaseLauncher
{
	public CompilerHooks CompilerHooks { get; }

	public MosaSettings MosaSettings { get; }

	public BaseLauncher(MosaSettings mosaSettings, CompilerHooks compilerHook)
	{
		CompilerHooks = compilerHook;

		MosaSettings = new MosaSettings();
		MosaSettings.LoadAppLocations();
		MosaSettings.SetDefaultSettings();
		MosaSettings.ResolveDefaults();
		MosaSettings.Merge(mosaSettings);
		MosaSettings.NormalizeSettings();
		MosaSettings.ResolveFileAndPathSettings();
		MosaSettings.AddStandardPlugs();
	}

	protected void OutputStatus(string status)
	{
		if (string.IsNullOrEmpty(status))
			return;

		OutputEvent(status);
	}

	protected virtual void OutputEvent(string status) => CompilerHooks.NotifyStatus?.Invoke(status);

	protected static byte[] GetResource(string path, string name) => GetResource($"{path
		.Replace(".", "._")
		.Replace(@"\", "._")
		.Replace("/", "._")
		.Replace("-", "_")}.{name}");

	protected Process CreateApplicationProcess(string app, string args)
	{
		OutputStatus($"Starting Application: {app}");
		OutputStatus($"Arguments: {args}");

		var startInfo = new ProcessStartInfo
		{
			FileName = app,
			Arguments = args,
			UseShellExecute = false,
			CreateNoWindow = true,
			RedirectStandardOutput = false,
			RedirectStandardError = false
		};

		return new Process { StartInfo = startInfo };
	}

	protected Process LaunchApplication(string app, string args)
	{
		OutputStatus($"Launching Application: {app}");
		OutputStatus($"Arguments: {args}");

		var startInfo = new ProcessStartInfo
		{
			FileName = app,
			Arguments = args,
			UseShellExecute = false,
			CreateNoWindow = true,
			RedirectStandardOutput = true,
			RedirectStandardError = true
		};

		return Process.Start(startInfo);
	}

	protected Process LaunchConsoleApplication(string app, string args)
	{
		OutputStatus($"Launching Console Application: {app}");
		OutputStatus($"Arguments: {args}");

		var startInfo = new ProcessStartInfo
		{
			FileName = app,
			Arguments = args,
			UseShellExecute = false,
			CreateNoWindow = false,
			RedirectStandardOutput = false,
			RedirectStandardError = false
		};

		return Process.Start(startInfo);
	}

	protected static string GetOutput(Process process)
	{
		process.WaitForExit();

		var output = process.StandardOutput.ReadToEnd();
		var error = process.StandardError.ReadToEnd();

		return $"{output}{error}";
	}

	protected Process LaunchApplicationWithOutput(string app, string arg)
	{
		var process = LaunchApplication(app, arg);

		var output = GetOutput(process);
		OutputStatus(output);

		return process;
	}

	private static byte[] GetResource(string name)
	{
		var assembly = Assembly.GetExecutingAssembly();
		var stream = assembly.GetManifestResourceStream($"Mosa.Utility.Launcher.Resources.{name}");
		if (stream == null)
			throw new MissingManifestResourceException($"Cannot find resource: {name}");

		var binary = new BinaryReader(stream);
		return binary.ReadBytes((int)stream.Length);
	}
}
