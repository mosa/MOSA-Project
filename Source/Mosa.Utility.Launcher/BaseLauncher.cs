// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.IO;
using System.Reflection;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;

namespace Mosa.Utility.Launcher;

public class BaseLauncher
{
	public CompilerHooks CompilerHooks { get; }

	public LauncherSettings Settings { get; }

	public Settings ConfigurationSettings => Settings.Settings;

	public BaseLauncher(Settings settings, CompilerHooks compilerHook)
	{
		CompilerHooks = compilerHook;

		var startSettings = new Settings();

		SetDefaultSettings(startSettings);

		startSettings.Merge(settings);

		Settings = new LauncherSettings(startSettings);

		NormalizeSettings();

		SetDefaults();
	}

	private void SetDefaultSettings(Settings settings)
	{
		settings.SetValue("Emulator", "Qemu");
		settings.SetValue("Emulator.Memory", 128);
		settings.SetValue("Emulator.Cores", 1);

		//settings.SetValue("Emulator.Serial", "none");
		settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
		settings.SetValue("Emulator.Serial.Port", 9999);
		settings.SetValue("Emulator.Serial.Pipe", "MOSA");
		settings.SetValue("Launcher.PlugKorlib", true);
	}

	protected void NormalizeSettings()
	{
		// Normalize inputs
		Settings.ImageFormat = Settings.ImageFormat == null ? string.Empty : Settings.ImageFormat.ToLowerInvariant().Trim();
		Settings.FileSystem = Settings.FileSystem == null ? string.Empty : Settings.FileSystem.ToLowerInvariant().Trim();
		Settings.EmulatorSerial = Settings.EmulatorSerial == null ? string.Empty : Settings.EmulatorSerial.ToLowerInvariant().Trim();
		Settings.Emulator = Settings.Emulator == null ? string.Empty : Settings.Emulator.ToLowerInvariant().Trim();
		Settings.Platform = Settings.Platform.ToLowerInvariant().Trim();
	}

	private void SetDefaults()
	{
		if (string.IsNullOrWhiteSpace(Settings.TemporaryFolder) || Settings.TemporaryFolder != "%DEFAULT%")
		{
			Settings.TemporaryFolder = Path.Combine(Path.GetTempPath(), "MOSA");
		}

		if (string.IsNullOrWhiteSpace(Settings.DefaultFolder) || Settings.DefaultFolder != "%DEFAULT%")
		{
			if (Settings.OutputFile != null && Settings.OutputFile != "%DEFAULT%")
			{
				Settings.DefaultFolder = Path.GetDirectoryName(Path.GetFullPath(Settings.OutputFile));
			}
			else
			{
				Settings.DefaultFolder = Settings.TemporaryFolder;
			}
		}

		if (Settings.ImageFolder != null && Settings.ImageFolder != "%DEFAULT%")
		{
			Settings.ImageFolder = Settings.DefaultFolder;
		}

		string baseFilename;

		if (Settings.OutputFile != null && Settings.OutputFile != "%DEFAULT%")
		{
			baseFilename = Path.GetFileNameWithoutExtension(Settings.OutputFile);
		}
		else if (Settings.SourceFiles != null && Settings.SourceFiles.Count != 0)
		{
			baseFilename = Path.GetFileNameWithoutExtension(Settings.SourceFiles[0]);
		}
		else if (Settings.ImageFile != null && Settings.ImageFile != "%DEFAULT%")
		{
			baseFilename = Path.GetFileNameWithoutExtension(Settings.ImageFile);
		}
		else
		{
			baseFilename = "_mosa_";
		}

		var defaultFolder = Settings.DefaultFolder;

		if (Settings.OutputFile is null or "%DEFAULT%")
		{
			Settings.OutputFile = Path.Combine(defaultFolder, $"{baseFilename}.bin");
		}

		if (Settings.ImageFile == "%DEFAULT%")
		{
			Settings.ImageFile = Path.Combine(Settings.ImageFolder, $"{baseFilename}.{Settings.ImageFormat}");
		}

		if (Settings.MapFile == "%DEFAULT%")
		{
			Settings.MapFile = Path.Combine(defaultFolder, $"{baseFilename}-map.txt");
		}

		if (Settings.CompileTimeFile == "%DEFAULT%")
		{
			Settings.CompileTimeFile = Path.Combine(defaultFolder, $"{baseFilename}-time.txt");
		}

		if (Settings.DebugFile == "%DEFAULT%")
		{
			Settings.DebugFile = Path.Combine(defaultFolder, $"{baseFilename}.debug");
		}

		if (Settings.InlinedFile == "%DEFAULT%")
		{
			Settings.InlinedFile = Path.Combine(defaultFolder, $"{baseFilename}-inlined.txt");
		}

		if (Settings.PreLinkHashFile == "%DEFAULT%")
		{
			Settings.PreLinkHashFile = Path.Combine(defaultFolder, $"{baseFilename}-prelink-hash.txt");
		}

		if (Settings.PostLinkHashFile == "%DEFAULT%")
		{
			Settings.PostLinkHashFile = Path.Combine(defaultFolder, $"{baseFilename}-postlink-hash.txt");
		}

		if (Settings.AsmFile == "%DEFAULT%")
		{
			Settings.AsmFile = Path.Combine(defaultFolder, $"{baseFilename}.asm");
		}

		if (Settings.NasmFile == "%DEFAULT%")
		{
			Settings.NasmFile = Path.Combine(defaultFolder, $"{baseFilename}.nasm");
		}
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
