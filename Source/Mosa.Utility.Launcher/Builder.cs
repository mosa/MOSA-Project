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
using Mosa.Utility.Configuration;

namespace Mosa.Utility.Launcher;

public class Builder : BaseLauncher
{
	public List<string> Counters { get; }

	public Stopwatch Stopwatch { get; } = new Stopwatch();

	public bool IsSucccessful { get; private set; }

	public MosaLinker Linker { get; private set; }

	public TypeSystem TypeSystem { get; private set; }

	public const uint MultibootHeaderLength = 3 * 16;

	public Builder(MosaSettings mosaSettings, CompilerHooks compilerHooks)
		: base(mosaSettings, compilerHooks)
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
			if (!Directory.Exists(MosaSettings.TemporaryFolder))
			{
				Directory.CreateDirectory(MosaSettings.ImageFolder);
			}

			if (!Directory.Exists(MosaSettings.ImageFolder))
			{
				Directory.CreateDirectory(MosaSettings.ImageFolder);
			}

			if (string.IsNullOrEmpty(MosaSettings.SourceFiles[0]))
			{
				Output("ERROR: Missing source file");
				return;
			}
			else if (!File.Exists(MosaSettings.SourceFiles[0]))
			{
				Output($"File {MosaSettings.SourceFiles[0]} does not exists");
				return;
			}

			if (!Compile())
			{
				IsSucccessful = false;
				return;
			}

			BuildImage();

			if (!string.IsNullOrWhiteSpace(MosaSettings.NasmFile))
			{
				LaunchNDISASM();
			}

			if (!string.IsNullOrWhiteSpace(MosaSettings.AsmFile))
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
		var fileHunter = new FileHunter(Path.GetDirectoryName(MosaSettings.SourceFiles[0]));

		if (MosaSettings.PlugKorlib)
		{
			var fileKorlib = fileHunter.HuntFile("Mosa.Plug.Korlib.dll");

			if (fileKorlib != null)
			{
				//MosaSettings.SourceFiles.Add(fileKorlib.FullName);
				MosaSettings.AddSourceFile(fileKorlib.FullName);
			}

			var platform = MosaSettings.Platform;

			if (platform == "armv8a32")
			{
				platform = "ARMv8A32";
			}

			var fileKorlibPlatform = fileHunter.HuntFile($"Mosa.Plug.Korlib.{platform}.dll");

			if (fileKorlibPlatform != null)
			{
				MosaSettings.AddSourceFile(fileKorlibPlatform.FullName);
			}
		}

		Output($"Compiling: {MosaSettings.SourceFiles[0]}");

		var compiler = new MosaCompiler(MosaSettings, CompilerHooks, new ClrModuleLoader(), new ClrTypeResolver());

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
		if (string.IsNullOrWhiteSpace(MosaSettings.ImageFormat))
			return;

		Output($"Generating Image: {MosaSettings.ImageFormat}");

		if (MosaSettings.ImageFormat == "vmdk")
		{
			var tmpimagefile = Path.Combine(MosaSettings.TemporaryFolder, $"{Path.GetFileNameWithoutExtension(MosaSettings.ImageFile)}.img");

			CreateDiskImage(tmpimagefile);

			CreateVMDK(tmpimagefile);
		}
		else if (MosaSettings.ImageFormat == "vdi")
		{
			var tmpimagefile = Path.Combine(MosaSettings.TemporaryFolder, $"{Path.GetFileNameWithoutExtension(MosaSettings.ImageFile)}.img");

			CreateDiskImage(tmpimagefile);

			CreateVDI(tmpimagefile);
		}
		else
		{
			CreateDiskImage(MosaSettings.ImageFile);
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
		bootImageOptions.IncludeFiles.Add(new IncludeFile(MosaSettings.OutputFile, "kernel.bin"));
		bootImageOptions.IncludeFiles.Add(new IncludeFile("TEST.TXT", Encoding.ASCII.GetBytes("This is a test file.")));

		if (!string.IsNullOrEmpty(MosaSettings.FileSystemRootInclude))
		{
			var dir = Path.GetFullPath(MosaSettings.FileSystemRootInclude);
			foreach (var file in Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories))
			{
				var name = Path.GetFileName(file).ToUpper();

				Output($"Adding file: {name}");
				bootImageOptions.IncludeFiles.Add(new IncludeFile(name, File.ReadAllBytes(file)));
			}
		}

		bootImageOptions.VolumeLabel = MosaSettings.OSName;
		bootImageOptions.DiskImageFileName = imagefile;
		bootImageOptions.ImageFirmware = MosaSettings.ImageFirmware switch
		{
			"bios" => ImageFirmware.Bios,
			//"uefi" => ImageFirmware.Uefi,
			_ => throw new NotImplementCompilerException($"Unknown image firmware: {MosaSettings.ImageFirmware}")
		};
		bootImageOptions.ImageFormat = MosaSettings.ImageFormat switch
		{
			"img" => ImageFormat.IMG,
			"vhd" => ImageFormat.VHD,
			"vdi" => ImageFormat.VDI,
			"vmdk" => ImageFormat.VMDK,
			_ => throw new NotImplementCompilerException($"Unknown image format: {MosaSettings.ImageFormat}")
		};
		bootImageOptions.FileSystem = MosaSettings.FileSystem switch
		{
			"fat12" => BootImage.FileSystem.FAT12,
			"fat16" => BootImage.FileSystem.FAT16,
			"fat32" => BootImage.FileSystem.FAT32,
			_ => throw new NotImplementCompilerException($"Unknown file system: {MosaSettings.FileSystem}")
		};

		Generator.Create(bootImageOptions);
	}

	private byte[] GetLimineCFG()
	{
		return Encoding.ASCII.GetBytes($"TIMEOUT=0\nINTERFACE_RESOLUTION=640x480\nINTERFACE_BRANDING=Managed Operating System Alliance\n:{MosaSettings.OSName}\nPROTOCOL={(MosaSettings.MultibootVersion == "v2" ? "multiboot2" : "multiboot1")}\nKERNEL_PATH=boot:///kernel.bin");
	}

	private void CreateVMDK(string source)
	{
		var arg = $"convert -f raw -O vmdk {Quote(source)} {Quote(MosaSettings.ImageFile)}";

		LaunchApplicationWithOutput(MosaSettings.QemuImgApp, arg);
	}

	private void CreateVDI(string source)
	{
		var arg = $"convert -f raw -O vdi {Quote(source)} {Quote(MosaSettings.ImageFile)}";

		LaunchApplicationWithOutput(MosaSettings.QemuImgApp, arg);
	}

	private void LaunchNDISASM()
	{
		//var textSection = Linker.Sections[(int)SectionKind.Text];
		var startingAddress = MosaSettings.BaseAddress + MultibootHeaderLength;
		var fileOffset = Linker.BaseFileOffset + MultibootHeaderLength;

		var arg = $"-b 32 -o0x{startingAddress:x} -e0x{fileOffset:x} {Quote(MosaSettings.OutputFile)}";

		//var nasmfile = Path.Combine(LauncherSettings.ImageFolder, $"{Path.GetFileNameWithoutExtension(LauncherSettings.SourceFiles[0])}.nasm");

		var process = LaunchApplication(MosaSettings.NdisasmApp, arg);

		var output = GetOutput(process);

		File.WriteAllText(MosaSettings.NasmFile, output);
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

		var code2 = File.ReadAllBytes(MosaSettings.OutputFile);

		var code = new byte[code2.Length];

		for (ulong i = fileOffset; i < (ulong)code2.Length; i++)
			code[i - fileOffset] = code2[i];

		var disassembler = new Disassembler.Disassembler(MosaSettings.Platform);
		disassembler.SetMemory(code, startingAddress);

		using (var dest = File.CreateText(MosaSettings.AsmFile))
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
