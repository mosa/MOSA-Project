// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Text;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
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

		CompilerHooks.NotifyEvent ??= NotifyEvent;
	}

	public void Build()
	{
		Counters.Clear();
		IsSucccessful = false;

		Stopwatch.Start();

		try
		{
			Directory.CreateDirectory(MosaSettings.ImageFolder);

			if (string.IsNullOrEmpty(MosaSettings.SourceFiles[0]))
			{
				OutputStatus("ERROR: Missing source file");
				return;
			}
			else if (!File.Exists(FileFinder.Find(MosaSettings.SourceFiles[0], MosaSettings.SearchPaths)))
			{
				OutputStatus($"ERROR: File {MosaSettings.SourceFiles[0]} does not exists");
				return;
			}

			if (!Compile())
			{
				IsSucccessful = false;
				return;
			}

			BuildImage();

			if (!string.IsNullOrWhiteSpace(MosaSettings.NasmFile))
				LaunchNDISASM();

			if (!string.IsNullOrWhiteSpace(MosaSettings.AsmFile))
				GenerateASMFile();

			IsSucccessful = true;
		}
		catch (Exception e)
		{
			IsSucccessful = false;
			OutputStatus($"Exception: {e}");
		}
		finally
		{
			Stopwatch.Stop();
		}
	}

	private bool Compile()
	{
		OutputStatus($"Compiling: {MosaSettings.SourceFiles[0]}");

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

		OutputStatus($"Generating Image: {MosaSettings.ImageFormat}");

		switch (MosaSettings.ImageFormat)
		{
			case "vmdk":
				{
					var imageFile = Path.Combine(MosaSettings.TemporaryFolder, $"{Path.GetFileNameWithoutExtension(MosaSettings.ImageFile)}.img");
					CreateDiskImage(imageFile);
					CreateVMDK(imageFile);
					break;
				}
			case "vdi":
				{
					var imageFile = Path.Combine(MosaSettings.TemporaryFolder, $"{Path.GetFileNameWithoutExtension(MosaSettings.ImageFile)}.img");
					CreateDiskImage(imageFile);
					CreateVDI(imageFile);
					break;
				}
			default:
				{
					CreateDiskImage(MosaSettings.ImageFile);
					break;
				}
		}
	}

	private void AddCounters(string data)
	{
		if (data == null)
			return;

		Counters.Add(data);
	}

	private void CreateDiskImage(string imageFile)
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

				OutputStatus($"Adding file: {name}");
				bootImageOptions.IncludeFiles.Add(new IncludeFile(name, File.ReadAllBytes(file)));
			}
		}

		bootImageOptions.VolumeLabel = MosaSettings.VolumeLabel;
		bootImageOptions.BlockCount = (uint)MosaSettings.DiskBlocks;
		bootImageOptions.DiskImageFileName = imageFile;
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

	private byte[] GetLimineCFG() => Encoding.ASCII.GetBytes($"""
		TIMEOUT={MosaSettings.BootLoaderTimeout}
		INTERFACE_RESOLUTION=640x480
		INTERFACE_BRANDING=Managed Operating System Alliance
		:{MosaSettings.OSName}
		PROTOCOL=multiboot2
		KERNEL_PATH=boot:///kernel.bin
		""");

	private void CreateVMDK(string source)
		=> LaunchApplicationWithOutput(MosaSettings.QemuImgApp, $"convert -f raw -O vmdk \"{source}\" \"{MosaSettings.ImageFile}\"");

	private void CreateVDI(string source)
		=> LaunchApplicationWithOutput(MosaSettings.QemuImgApp, $"convert -f raw -O vdi \"{source}\" \"{MosaSettings.ImageFile}\"");

	private void LaunchNDISASM()
	{
		OutputStatus($"Executing NDISASM: {MosaSettings.NasmFile}");

		var startingAddress = MosaSettings.BaseAddress + MultibootHeaderLength;
		var fileOffset = Linker.BaseFileOffset + MultibootHeaderLength;
		var process = LaunchApplication(MosaSettings.NdisasmApp, $"-b 32 -o0x{startingAddress:x} -e0x{fileOffset:x} \"{MosaSettings.OutputFile}\"");
		var output = GetOutput(process);

		File.WriteAllText(MosaSettings.NasmFile, output);
	}

	private void GenerateASMFile()
	{
		OutputStatus($"Executing Reko Disassembly: {MosaSettings.AsmFile}");

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

		var disassembler = new Disassembler.Disassembler(MosaSettings.Platform, code, startingAddress);

		using var dest = File.CreateText(MosaSettings.AsmFile);

		var instruction = disassembler.DecodeNext();
		while (instruction != null)
		{
			if (map.TryGetValue(instruction.Address, out List<string> list))
				foreach (var entry in list)
					dest.WriteLine($"; {entry}");

			dest.WriteLine(instruction.Full);

			if (instruction.Address > startingAddress + length)
				break;

			instruction = disassembler.DecodeNext();
		}
	}

	private void NotifyEvent(CompilerEvent compilerEvent, string message, int threadID)
	{
		switch (compilerEvent)
		{
			case CompilerEvent.Exception:
				{
					var status = $"[Exception] {message}";

					OutputStatus(status);
					break;
				}
			case CompilerEvent.CompilerStart or CompilerEvent.CompilerEnd or CompilerEvent.CompilingMethodsStart or CompilerEvent.CompilingMethodsCompleted or CompilerEvent.InlineMethodsScheduled or CompilerEvent.LinkingStart or CompilerEvent.LinkingEnd or CompilerEvent.Warning or CompilerEvent.Error:
				{
					var status = compilerEvent.ToText();
					if (!string.IsNullOrEmpty(message))
						status += $" => {message}";

					OutputStatus(status);
					break;
				}
			case CompilerEvent.Counter:
				{
					AddCounters(message);
					break;
				}
		}
	}
}
