// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.IO;
using System.Reflection;
using Mosa.Compiler.Common.Configuration;
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
		MosaSettings.SetDetfaultSettings();
		MosaSettings.Merge(mosaSettings);
		MosaSettings.NormalizeSettings();
		MosaSettings.UpdateFileAndPathSettings();
	}

	protected void Output(string status)
	{
		if (status == null)
			return;

		OutputEvent(status);
	}

	protected virtual void OutputEvent(string status)
	{
		CompilerHooks.NotifyStatus?.Invoke(status);
	}

	protected static byte[] GetResource(string path, string name)
	{
		var newname = path.Replace(".", "._").Replace(@"\", "._").Replace("/", "._").Replace("-", "_") + "." + name;
		return GetResource(newname);
	}

	protected static byte[] GetResource(string name)
	{
		var assembly = Assembly.GetExecutingAssembly();
		var stream = assembly.GetManifestResourceStream("Mosa.Utility.Launcher.Resources." + name);
		var binary = new BinaryReader(stream);
		return binary.ReadBytes((int)stream.Length);
	}

	protected static string Quote(string location)
	{
		return '"' + location + '"';
	}

	protected Process CreateApplicationProcess(string app, string args)
	{
		Output($"Starting Application: {app}");
		Output($"Arguments: {args}");

		var process = new Process();

		process.StartInfo.FileName = app;
		process.StartInfo.Arguments = args;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardOutput = false;
		process.StartInfo.RedirectStandardError = false;
		process.StartInfo.CreateNoWindow = true;

		return process;
	}

	protected Process LaunchApplication(string app, string args)
	{
		Output($"Launching Application: {app}");
		Output($"Arguments: {args}");

		var start = new ProcessStartInfo
		{
			FileName = app,
			Arguments = args,
			UseShellExecute = false,
			CreateNoWindow = true,
			RedirectStandardOutput = true,
			RedirectStandardError = true
		};

		return Process.Start(start);
	}

	protected Process LaunchConsoleApplication(string app, string args)
	{
		Output($"Launching Console Application: {app}");
		Output($"Arguments: {args}");

		var start = new ProcessStartInfo
		{
			FileName = app,
			Arguments = args,
			UseShellExecute = false,
			CreateNoWindow = false,
			RedirectStandardOutput = false,
			RedirectStandardError = false
		};

		return Process.Start(start);
	}

	protected string GetOutput(Process process)
	{
		var output = process.StandardOutput.ReadToEnd();

		process.WaitForExit();

		var error = process.StandardError.ReadToEnd();

		return output + error;
	}

	protected Process LaunchApplicationWithOutput(string app, string arg)
	{
		var process = LaunchApplication(app, arg);

		var output = GetOutput(process);
		Output(output);

		return process;
	}

	protected string NullToEmpty(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			return string.Empty;

		return value;
	}
}
