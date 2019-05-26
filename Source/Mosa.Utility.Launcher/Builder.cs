// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.BootImage;
using SharpDisasm;
using SharpDisasm.Translators;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Mosa.Utility.Launcher
{
	public class Builder : BaseLauncher
	{
		public List<string> Counters { get; }

		public DateTime CompileStartTime { get; private set; }

		public IBuilderEvent BuilderEvent { get; }

		public bool HasCompileError { get; private set; }

		public string CompiledFile { get; private set; }

		public string ImageFile { get; private set; }

		public MosaLinker Linker { get; private set; }

		public TypeSystem TypeSystem { get; private set; }

		public const uint MultibootHeaderLength = 3 * 16;

		protected ITraceListener traceListener;

		public Builder(LauncherOptions options, AppLocations appLocations, IBuilderEvent builderEvent)
			: base(options, appLocations)
		{
			Counters = new List<string>();
			traceListener = new BuilderEventListener(this);
			BuilderEvent = builderEvent;
		}

		protected override void OutputEvent(string status)
		{
			BuilderEvent?.NewStatus(status);
		}

		public void AddCounters(string data)
		{
			if (data == null)
				return;

			Counters.Add(data);
		}

		private FileInfo HuntFor(string filename)
		{
			var directory = Hunt(filename);

			if (directory == null)
				return null;

			var full = Path.Combine(directory, filename);

			var file = new FileInfo(full);

			return file;
		}

		private List<BaseCompilerExtension> GetCompilerExtensions()
		{
			return new List<BaseCompilerExtension>()
			{
				new Compiler.Extensions.Dwarf.DwarfCompilerExtension()
			};
		}

		public void Compile()
		{
			Log.Clear();
			Counters.Clear();
			HasCompileError = true;

			var compiler = new MosaCompiler(GetCompilerExtensions());

			try
			{
				CompileStartTime = DateTime.Now;

				CompiledFile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile)}.bin");

				compiler.CompilerOptions.EnableSSA = LauncherOptions.EnableSSA;
				compiler.CompilerOptions.EnableIROptimizations = LauncherOptions.EnableIROptimizations;
				compiler.CompilerOptions.EnableSparseConditionalConstantPropagation = LauncherOptions.EnableSparseConditionalConstantPropagation;
				compiler.CompilerOptions.EnableInlinedMethods = LauncherOptions.EnableInlinedMethods;
				compiler.CompilerOptions.InlinedIRMaximum = LauncherOptions.InlinedIRMaximum;
				compiler.CompilerOptions.EnableIRLongExpansion = LauncherOptions.EnableIRLongExpansion;
				compiler.CompilerOptions.TwoPassOptimizations = LauncherOptions.TwoPassOptimizations;
				compiler.CompilerOptions.EnableValueNumbering = LauncherOptions.EnableValueNumbering;
				compiler.CompilerOptions.OutputFile = CompiledFile;
				compiler.CompilerOptions.Architecture = SelectArchitecture(LauncherOptions.PlatformType);
				compiler.CompilerOptions.LinkerFormatType = LauncherOptions.LinkerFormatType;
				compiler.CompilerOptions.MultibootSpecification = LauncherOptions.MultibootSpecification;
				compiler.CompilerOptions.SetCustomOption("multiboot.video", LauncherOptions.VBEVideo ? "true" : "false");
				compiler.CompilerOptions.SetCustomOption("multiboot.width", LauncherOptions.Width.ToString());
				compiler.CompilerOptions.SetCustomOption("multiboot.height", LauncherOptions.Height.ToString());
				compiler.CompilerOptions.SetCustomOption("multiboot.depth", LauncherOptions.Depth.ToString());
				compiler.CompilerOptions.BaseAddress = LauncherOptions.BaseAddress;
				compiler.CompilerOptions.EmitAllSymbols = LauncherOptions.EmitAllSymbols;
				compiler.CompilerOptions.EmitStaticRelocations = LauncherOptions.EmitStaticRelocations;
				compiler.CompilerOptions.EnableMethodScanner = LauncherOptions.EnableMethodScanner;
				compiler.CompilerOptions.EnableBitTracker = LauncherOptions.EnableBitTracker;

				compiler.CompilerOptions.CreateExtraSections = LauncherOptions.CreateExtraSections;
				compiler.CompilerOptions.CreateExtraProgramHeaders = LauncherOptions.CreateExtraProgramHeaders;

				if (LauncherOptions.GenerateMapFile)
				{
					compiler.CompilerOptions.MapFile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile)}.map");
				}

				if (LauncherOptions.GenerateCompileTimeFile)
				{
					compiler.CompilerOptions.CompileTimeFile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile)}-time.txt");
				}

				if (LauncherOptions.GenerateDebugFile)
				{
					var debugFile = LauncherOptions.DebugFile ?? Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile) + ".debug";

					compiler.CompilerOptions.DebugFile = Path.Combine(LauncherOptions.DestinationDirectory, debugFile);
				}

				if (!Directory.Exists(LauncherOptions.DestinationDirectory))
				{
					Directory.CreateDirectory(LauncherOptions.DestinationDirectory);
				}

				compiler.CompilerTrace.SetTraceListener(traceListener);

				if (string.IsNullOrEmpty(LauncherOptions.SourceFile))
				{
					AddOutput("Please select a source file");
					return;
				}
				else if (!File.Exists(LauncherOptions.SourceFile))
				{
					AddOutput($"File {LauncherOptions.SourceFile} does not exists");
					return;
				}

				compiler.CompilerOptions.AddSourceFile(LauncherOptions.SourceFile);
				compiler.CompilerOptions.AddSearchPaths(LauncherOptions.Paths);

				var inputFiles = new List<FileInfo>
				{
					(LauncherOptions.HuntForCorLib) ? HuntFor("mscorlib.dll") : null,
					(LauncherOptions.PlugKorlib) ? HuntFor("Mosa.Plug.Korlib.dll") : null,
					(LauncherOptions.PlugKorlib) ? HuntFor("Mosa.Plug.Korlib." + LauncherOptions.PlatformType.ToString() + ".dll"): null,
				};

				compiler.CompilerOptions.AddSourceFiles(inputFiles);
				compiler.CompilerOptions.AddSearchPaths(inputFiles);

				compiler.Load();
				compiler.Initialize();
				compiler.Setup();

				// TODO Include Unit Tests

				if (LauncherOptions.EnableMultiThreading)
				{
					compiler.ThreadedCompile();
				}
				else
				{
					compiler.Compile();
				}

				Linker = compiler.Linker;
				TypeSystem = compiler.TypeSystem;

				if (LauncherOptions.ImageFormat == ImageFormat.ISO)
				{
					if (LauncherOptions.BootLoader == BootLoader.Grub_0_97 || LauncherOptions.BootLoader == BootLoader.Grub_2_00)
					{
						CreateISOImageWithGrub(CompiledFile);
					}
					else // assuming syslinux
					{
						CreateISOImageWithSyslinux(CompiledFile);
					}
				}
				else
				{
					CreateDiskImage(CompiledFile);

					if (LauncherOptions.ImageFormat == ImageFormat.VMDK)
					{
						CreateVMDK();
					}
				}

				if (LauncherOptions.GenerateNASMFile)
				{
					LaunchNDISASM();
				}

				if (LauncherOptions.GenerateASMFile)
				{
					GenerateASMFile();
				}

				HasCompileError = false;
			}
			catch (Exception e)
			{
				HasCompileError = true;
				AddOutput(e.ToString());
			}
			finally
			{
				compiler = null;
			}
		}

		private void CreateDiskImage(string compiledFile)
		{
			var bootImageOptions = new BootImageOptions();

			if (LauncherOptions.BootLoader == BootLoader.Syslinux_6_03)
			{
				bootImageOptions.MBRCode = GetResource(@"syslinux\6.03", "mbr.bin");
				bootImageOptions.FatBootCode = GetResource(@"syslinux\6.03", "ldlinux.bin");

				bootImageOptions.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\6.03", "ldlinux.sys")));
				bootImageOptions.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\6.03", "mboot.c32")));
			}
			else if (LauncherOptions.BootLoader == BootLoader.Syslinux_3_72)
			{
				bootImageOptions.MBRCode = GetResource(@"syslinux\3.72", "mbr.bin");
				bootImageOptions.FatBootCode = GetResource(@"syslinux\3.72", "ldlinux.bin");

				bootImageOptions.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\3.72", "ldlinux.sys")));
				bootImageOptions.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\3.72", "mboot.c32")));
			}

			bootImageOptions.IncludeFiles.Add(new IncludeFile("syslinux.cfg", GetResource("syslinux", "syslinux.cfg")));
			bootImageOptions.IncludeFiles.Add(new IncludeFile(compiledFile, "main.exe"));

			bootImageOptions.IncludeFiles.Add(new IncludeFile("TEST.TXT", Encoding.ASCII.GetBytes("This is a test file.")));

			foreach (var include in LauncherOptions.IncludeFiles)
			{
				bootImageOptions.IncludeFiles.Add(include);
			}

			bootImageOptions.VolumeLabel = "MOSABOOT";

			var vmext = ".img";
			switch (LauncherOptions.ImageFormat)
			{
				case ImageFormat.VHD: vmext = ".vhd"; break;
				case ImageFormat.VDI: vmext = ".vdi"; break;
				default: break;
			}

			ImageFile = Path.Combine(LauncherOptions.DestinationDirectory, Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile) + vmext);

			bootImageOptions.DiskImageFileName = ImageFile;
			bootImageOptions.PatchSyslinuxOption = true;
			bootImageOptions.FileSystem = LauncherOptions.FileSystem;
			bootImageOptions.ImageFormat = LauncherOptions.ImageFormat;
			bootImageOptions.BootLoader = LauncherOptions.BootLoader;

			Generator.Create(bootImageOptions);
		}

		private void CreateISOImageWithSyslinux(string compiledFile)
		{
			string isoDirectory = Path.Combine(LauncherOptions.DestinationDirectory, "iso");

			if (Directory.Exists(isoDirectory))
			{
				Directory.Delete(isoDirectory, true);
			}

			Directory.CreateDirectory(isoDirectory);

			if (LauncherOptions.BootLoader == BootLoader.Syslinux_6_03)
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.bin"), GetResource(@"syslinux\6.03", "isolinux.bin"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "mboot.c32"), GetResource(@"syslinux\6.03", "mboot.c32"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "ldlinux.c32"), GetResource(@"syslinux\6.03", "ldlinux.c32"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "libcom32.c32"), GetResource(@"syslinux\6.03", "libcom32.c32"));
			}
			else if (LauncherOptions.BootLoader == BootLoader.Syslinux_3_72)
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.bin"), GetResource(@"syslinux\3.72", "isolinux.bin"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "mboot.c32"), GetResource(@"syslinux\3.72", "mboot.c32"));
			}

			File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.cfg"), GetResource("syslinux", "syslinux.cfg"));

			foreach (var include in LauncherOptions.IncludeFiles)
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, include.Filename), include.Content);
			}

			File.Copy(compiledFile, Path.Combine(isoDirectory, "main.exe"));

			ImageFile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile)}.iso");

			string arg = $"-relaxed-filenames -J -R -o {Quote(ImageFile)} -b isolinux.bin -no-emul-boot -boot-load-size 4 -boot-info-table {Quote(isoDirectory)}";

			LaunchApplication(AppLocations.Mkisofs, arg, true);
		}

		private void CreateISOImageWithGrub(string compiledFile)
		{
			string isoDirectory = Path.Combine(LauncherOptions.DestinationDirectory, "iso");

			if (Directory.Exists(isoDirectory))
			{
				Directory.Delete(isoDirectory, true);
			}

			Directory.CreateDirectory(isoDirectory);
			Directory.CreateDirectory(Path.Combine(isoDirectory, "boot"));
			Directory.CreateDirectory(Path.Combine(isoDirectory, "boot", "grub"));
			Directory.CreateDirectory(isoDirectory);

			string loader = string.Empty;

			if (LauncherOptions.BootLoader == BootLoader.Grub_0_97)
			{
				loader = "boot/grub/stage2_eltorito";
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "stage2_eltorito"), GetResource(@"grub\0.97", "stage2_eltorito"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "menu.lst"), GetResource(@"grub\0.97", "menu.lst"));
			}
			else if (LauncherOptions.BootLoader == BootLoader.Grub_2_00)
			{
				loader = "boot/grub/i386-pc/eltorito.img";
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "grub.cfg"), GetResource(@"grub\2.00", "grub.cfg"));

				Directory.CreateDirectory(Path.Combine(isoDirectory, "boot", "grub", "i386-pc"));

				var data = GetResource(@"grub\2.00", "i386-pc.zip");
				var dataStream = new MemoryStream(data);

				var archive = new ZipArchive(dataStream);

				archive.ExtractToDirectory(Path.Combine(isoDirectory, "boot", "grub"));
			}

			foreach (var include in LauncherOptions.IncludeFiles)
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, include.Filename), include.Content);
			}

			File.Copy(compiledFile, Path.Combine(isoDirectory, "boot", "main.exe"));

			ImageFile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile)}.iso");

			string arg = $"-relaxed-filenames -J -R -o {Quote(ImageFile)} -b {Quote(loader)} -no-emul-boot -boot-load-size 4 -boot-info-table {Quote(isoDirectory)}";

			LaunchApplication(AppLocations.Mkisofs, arg, true);
		}

		private void CreateVMDK()
		{
			var vmdkFile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile)}.vmdk");

			string arg = $"convert -f raw -O vmdk {Quote(ImageFile)} {Quote(vmdkFile)}";

			ImageFile = vmdkFile;
			LaunchApplication(AppLocations.QEMUImg, arg, true);
		}

		private void LaunchNDISASM()
		{
			var textSection = Linker.Sections[(int)SectionKind.Text];

			const uint multibootHeaderLength = MultibootHeaderLength;
			ulong startingAddress = textSection.VirtualAddress + multibootHeaderLength;
			uint fileOffset = textSection.FileOffset + multibootHeaderLength;

			string arg = $"-b 32 -o0x{startingAddress.ToString("x")} -e0x{fileOffset.ToString("x")} {Quote(CompiledFile)}";

			var nasmfile = Path.Combine(LauncherOptions.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile)}.nasm");

			var process = LaunchApplication(AppLocations.NDISASM, arg);

			var output = GetOutput(process);

			File.WriteAllText(nasmfile, output);
		}

		private void GenerateASMFile()
		{
			var translator = new IntelTranslator()
			{
				IncludeAddress = true,
				IncludeBinary = true
			};

			var asmfile = Path.Combine(LauncherOptions.DestinationDirectory, Path.GetFileNameWithoutExtension(LauncherOptions.SourceFile) + ".asm");

			var textSection = Linker.Sections[(int)SectionKind.Text];

			var map = new Dictionary<ulong, string>();

			foreach (var symbol in Linker.Symbols)
			{
				if (map.ContainsKey(symbol.VirtualAddress))
					continue;

				map.Add(symbol.VirtualAddress, symbol.Name);
			}

			const uint multibootHeaderLength = MultibootHeaderLength;
			ulong startingAddress = textSection.VirtualAddress + multibootHeaderLength;
			uint fileOffset = textSection.FileOffset + multibootHeaderLength;
			uint length = textSection.Size;

			var code2 = File.ReadAllBytes(CompiledFile);

			var code = new byte[code2.Length];

			for (ulong i = fileOffset; i < (ulong)code2.Length; i++)
			{
				code[i - fileOffset] = code2[i];
			}

			using (var disasm = new Disassembler(code, ArchitectureMode.x86_32, startingAddress, true, Vendor.Any))
			{
				using (var dest = File.CreateText(asmfile))
				{
					if (map.ContainsKey(startingAddress))
					{
						dest.WriteLine("; " + map[startingAddress]);
					}

					foreach (var instruction in disasm.Disassemble())
					{
						var inst = translator.Translate(instruction);
						dest.WriteLine(inst);

						if (map.ContainsKey(instruction.PC))
						{
							dest.WriteLine("; " + map[instruction.PC]);
						}

						if (instruction.PC > startingAddress + length)
							break;
					}
				}
			}
		}

		/// <summary>
		/// Selects the architecture.
		/// </summary>
		/// <param name="platformType">Type of the platform.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementCompilerException">Unknown or unsupported Architecture</exception>
		private static BaseArchitecture SelectArchitecture(PlatformType platformType)
		{
			switch (platformType)
			{
				case PlatformType.x86: return Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect);
				case PlatformType.x64: return Mosa.Platform.x64.Architecture.CreateArchitecture(Mosa.Platform.x64.ArchitectureFeatureFlags.AutoDetect);
				default: throw new NotImplementCompilerException("Unknown or unsupported Architecture");
			}
		}

		private static bool Check(string directory, string filename)
		{
			return File.Exists(Path.GetFullPath(Path.Combine(directory, filename)));
		}

		private string Hunt(string filename)
		{
			// Only hunt if it's not in any of the path directories already, or the source directory

			// let's check the source directory
			string path = Path.GetDirectoryName(LauncherOptions.SourceFile);

			if (Check(path, filename))
				return path;

			// check current directory
			if (Check(Environment.CurrentDirectory, filename))
			{
				return Environment.CurrentDirectory;
			}

			// check within packages directory in 1 or 2 directories back
			// this is how VS organizes projects and packages

			var result = SearchSubdirectories(Path.Combine(Path.GetDirectoryName(LauncherOptions.SourceFile), "..", "packages"), filename);

			if (result != null)
				return result;

			result = SearchSubdirectories(Path.Combine(Path.GetDirectoryName(LauncherOptions.SourceFile), "..", "..", "packages"), filename);

			if (result != null)
				return result;

			result = SearchSubdirectories(Path.Combine(Environment.CurrentDirectory, "..", "packages"), filename);

			if (result != null)
				return result;

			result = SearchSubdirectories(Path.Combine(Environment.CurrentDirectory, "..", "..", "packages"), filename);

			return result;
		}

		private static string SearchSubdirectories(string path, string filename)
		{
			if (Directory.Exists(path))
			{
				var result = Directory.GetFiles(path, filename, SearchOption.AllDirectories);

				if (result?.Length >= 1)
				{
					return Path.GetDirectoryName(result[0]);
				}
			}

			return null;
		}
	}
}
