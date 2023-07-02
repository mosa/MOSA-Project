// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.MosaTypeSystem.CLR;
using Mosa.Utility.BootImage;

namespace Mosa.Utility.Launcher;

public class Builder : BaseLauncher
{
	public List<string> Counters { get; }

	public Stopwatch Stopwatch { get; } = new Stopwatch();

	public bool IsSucccessful { get; private set; }

	public MosaLinker Linker { get; private set; }

	public TypeSystem TypeSystem { get; private set; }

	public const uint MultibootHeaderLength = 3 * 16;

	public Builder(Settings settings, CompilerHooks compilerHooks)
		: base(settings, compilerHooks)
	{
		Counters = new List<string>();

		if (CompilerHooks.NotifyEvent == null)
		{
			CompilerHooks.NotifyEvent = NotifyEvent;
		}
	}

	public void Build()
	{
		Counters.Clear();
		IsSucccessful = false;

		Stopwatch.StartNew();

		try
		{
			if (!Directory.Exists(Settings.TemporaryFolder))
			{
				Directory.CreateDirectory(Settings.ImageFolder);
			}

			if (!Directory.Exists(Settings.ImageFolder))
			{
				Directory.CreateDirectory(Settings.ImageFolder);
			}

			if (string.IsNullOrEmpty(Settings.SourceFiles[0]))
			{
				Output("ERROR: Missing source file");
				return;
			}
			else if (!File.Exists(Settings.SourceFiles[0]))
			{
				Output($"File {Settings.SourceFiles[0]} does not exists");
				return;
			}

			if (!Compile())
			{
				IsSucccessful = false;
				return;
			}

			BuildImage();

			if (!string.IsNullOrWhiteSpace(Settings.NasmFile))
			{
				LaunchNDISASM();
			}

			if (!string.IsNullOrWhiteSpace(Settings.AsmFile))
			{
				GenerateASMFile();
			}

			IsSucccessful = true;
		}
		catch (Exception e)
		{
			IsSucccessful = false;
			Output($"Exception: {e}");
		}
		finally
		{
			//compiler = null;

			Stopwatch.Stop();
		}
	}

	private bool Compile()
	{
		var fileHunter = new FileHunter(Path.GetDirectoryName(Settings.SourceFiles[0]));

		if (Settings.PlugKorlib)
		{
			var fileKorlib = fileHunter.HuntFile("Mosa.Plug.Korlib.dll");

			if (fileKorlib != null)
			{
				ConfigurationSettings.AddPropertyListValue("Compiler.SourceFiles", fileKorlib.FullName);
			}

			var platform = Settings.Platform;

			if (platform == "armv8a32")
			{
				platform = "ARMv8A32";
			}

			var fileKorlibPlatform = fileHunter.HuntFile($"Mosa.Plug.Korlib.{platform}.dll");

			if (fileKorlibPlatform != null)
			{
				ConfigurationSettings.AddPropertyListValue("Compiler.SourceFiles", fileKorlibPlatform.FullName);
			}
		}

		Output($"Compiling: {Settings.SourceFiles[0]}");

		var compiler = new MosaCompiler(ConfigurationSettings, CompilerHooks, new ClrModuleLoader(), new ClrTypeResolver());

		compiler.Load();
		compiler.Initialize();
		compiler.Setup();
		compiler.Compile();

		Linker = compiler.Linker;
		TypeSystem = compiler.TypeSystem;

		return compiler.IsSuccess;
	}

	private void BuildImage()
	{
		if (string.IsNullOrWhiteSpace(Settings.ImageFormat))
			return;

		Output($"Generating Image: {Settings.ImageFormat}");

		if (Settings.ImageFormat == "vmdk")
		{
			var tmpimagefile = Path.Combine(Settings.TemporaryFolder, $"{Path.GetFileNameWithoutExtension(Settings.ImageFile)}.img");

			CreateDiskImage(tmpimagefile);

			CreateVMDK(tmpimagefile);
		}
		else if (Settings.ImageFormat == "vdi")
		{
			var tmpimagefile = Path.Combine(Settings.TemporaryFolder, $"{Path.GetFileNameWithoutExtension(Settings.ImageFile)}.img");

			CreateDiskImage(tmpimagefile);

			CreateVDI(tmpimagefile);
		}
		else
		{
			CreateDiskImage(Settings.ImageFile);
		}
	}

	private void AddCounters(string data)
	{
		if (data == null)
			return;

		Counters.Add(data);
	}

	private void CreateDiskImage(string imagefile)
	{
		var bootImageOptions = new BootImageOptions();

		bootImageOptions.IncludeFiles.Add(new IncludeFile("limine.cfg", GetLimineCFG()));
		bootImageOptions.IncludeFiles.Add(new IncludeFile("limine.sys", GetResource("limine", "limine.sys")));
		bootImageOptions.IncludeFiles.Add(new IncludeFile(Settings.OutputFile, "kernel.bin"));
		bootImageOptions.IncludeFiles.Add(new IncludeFile("TEST.TXT", Encoding.ASCII.GetBytes("This is a test file.")));

		if (!string.IsNullOrEmpty(Settings.FileSystemRootInclude))
		{
			var dir = Path.GetFullPath(Settings.FileSystemRootInclude);
			foreach (var file in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
			{
				var name = Path.GetFileName(file).ToUpper();

				Output($"Adding file: {name}");
				bootImageOptions.IncludeFiles.Add(new IncludeFile(name, File.ReadAllBytes(file)));
			}
		}

		bootImageOptions.VolumeLabel = Settings.OSName;
		bootImageOptions.DiskImageFileName = imagefile;
		bootImageOptions.ImageFirmware = Settings.ImageFirmware switch
		{
			"bios" => ImageFirmware.Bios,
			//"uefi" => ImageFirmware.Uefi,
			_ => throw new NotImplementCompilerException($"Unknown image firmware: {Settings.ImageFirmware}")
		};
		bootImageOptions.ImageFormat = Settings.ImageFormat switch
		{
			"img" => ImageFormat.IMG,
			"vhd" => ImageFormat.VHD,
			"vdi" => ImageFormat.VDI,
			"vmdk" => ImageFormat.VMDK,
			_ => throw new NotImplementCompilerException($"Unknown image format: {Settings.ImageFormat}")
		};
		bootImageOptions.FileSystem = Settings.FileSystem switch
		{
			"fat12" => BootImage.FileSystem.FAT12,
			"fat16" => BootImage.FileSystem.FAT16,
			"fat32" => BootImage.FileSystem.FAT32,
			_ => throw new NotImplementCompilerException($"Unknown file system: {Settings.FileSystem}")
		};

		Generator.Create(bootImageOptions);
	}

	private byte[] GetLimineCFG()
	{
		return Encoding.ASCII.GetBytes($"TIMEOUT=0\nINTERFACE_RESOLUTION=640x480\nINTERFACE_BRANDING=Managed Operating System Alliance\n:{Settings.OSName}\nPROTOCOL={(Settings.MultibootVersion == "v2" ? "multiboot2" : "multiboot1")}\nKERNEL_PATH=boot:///kernel.bin");
	}

	private void CreateVMDK(string source)
	{
		var arg = $"convert -f raw -O vmdk {Quote(source)} {Quote(Settings.ImageFile)}";

		LaunchApplicationWithOutput(Settings.QemuImg, arg);
	}

	private void CreateVDI(string source)
	{
		var arg = $"convert -f raw -O vdi {Quote(source)} {Quote(Settings.ImageFile)}";

		LaunchApplicationWithOutput(Settings.QemuImg, arg);
	}

	private void LaunchNDISASM()
	{
		//var textSection = Linker.Sections[(int)SectionKind.Text];
		var startingAddress = Settings.BaseAddress + MultibootHeaderLength;
		var fileOffset = Linker.BaseFileOffset + MultibootHeaderLength;

		var arg = $"-b 32 -o0x{startingAddress:x} -e0x{fileOffset:x} {Quote(Settings.OutputFile)}";

		//var nasmfile = Path.Combine(LauncherSettings.ImageFolder, $"{Path.GetFileNameWithoutExtension(LauncherSettings.SourceFiles[0])}.nasm");

		var process = LaunchApplication(Settings.Ndisasm, arg);

		var output = GetOutput(process);

		File.WriteAllText(Settings.NasmFile, output);
	}

	private void GenerateASMFile()
	{
		var map = new Dictionary<ulong, List<string>>();

		foreach (var symbol in Linker.Symbols)
		{
			if (!map.TryGetValue(symbol.VirtualAddress, out List<string> list))
			{
				list = new List<string>();
				map.Add(symbol.VirtualAddress, list);
			}

			list.Add(symbol.Name);
		}

		var textSection = Linker.Sections[(int)SectionKind.Text];
		var startingAddress = textSection.VirtualAddress + MultibootHeaderLength;
		var fileOffset = Linker.BaseFileOffset + MultibootHeaderLength;
		var length = textSection.Size;

		var code2 = File.ReadAllBytes(Settings.OutputFile);

		var code = new byte[code2.Length];

		for (ulong i = fileOffset; i < (ulong)code2.Length; i++)
			code[i - fileOffset] = code2[i];

		var disassembler = new Disassembler.Disassembler(Settings.Platform);
		disassembler.SetMemory(code, startingAddress);

		using (var dest = File.CreateText(Settings.AsmFile))
		{
			foreach (var instruction in disassembler.Decode())
			{
				if (map.TryGetValue(instruction.Address, out List<string> list))
				{
					foreach (var entry in list)
					{
						dest.WriteLine($"; {entry}");
					}
				}

				dest.WriteLine(instruction.Full);

				if (instruction.Address > startingAddress + length)
					break;
			}
		}
	}

	private void NotifyEvent(CompilerEvent compilerEvent, string message, int threadID)
	{
		if (compilerEvent is CompilerEvent.Exception)
		{
			var status = $"[Exception] {message}";

			Output(status);
		}
		else if (compilerEvent is CompilerEvent.CompilerStart or CompilerEvent.CompilerEnd or CompilerEvent.CompilingMethodsStart or CompilerEvent.CompilingMethodsCompleted or CompilerEvent.InlineMethodsScheduled or CompilerEvent.LinkingStart or CompilerEvent.LinkingEnd or CompilerEvent.Warning or CompilerEvent.Error)
		{
			var status = $"{compilerEvent.ToText()}";

			if (!string.IsNullOrEmpty(message))
				status += $" => {message}";

			Output(status);
		}
		else if (compilerEvent == CompilerEvent.Counter)
		{
			AddCounters(message);
		}
	}
}
