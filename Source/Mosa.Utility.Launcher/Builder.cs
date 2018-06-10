﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Framework.Trace;
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
		public IList<string> Counters { get; }

		public DateTime CompileStartTime { get; private set; }

		public IBuilderEvent BuilderEvent { get; }

		public bool HasCompileError { get; private set; }

		public string CompiledFile { get; private set; }

		public string ImageFile { get; private set; }

		public BaseLinker Linker { get; private set; }

		public TypeSystem TypeSystem { get; private set; }

		public const uint MultibootHeaderLength = 3 * 16;

		protected ITraceListener traceListener;

		public Builder(Options options, AppLocations appLocations, IBuilderEvent builderEvent)
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

		public void Compile()
		{
			HasCompileError = true;
			Log.Clear();
			Counters.Clear();

			var compiler = new MosaCompiler();

			try
			{
				CompileStartTime = DateTime.Now;

				CompiledFile = Path.Combine(Options.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(Options.SourceFile)}.bin");

				compiler.CompilerOptions.EnableSSA = Options.EnableSSA;
				compiler.CompilerOptions.EnableIROptimizations = Options.EnableIROptimizations;
				compiler.CompilerOptions.EnableSparseConditionalConstantPropagation = Options.EnableSparseConditionalConstantPropagation;
				compiler.CompilerOptions.EnableInlinedMethods = Options.EnableInlinedMethods;
				compiler.CompilerOptions.InlinedIRMaximum = Options.InlinedIRMaximum;
				compiler.CompilerOptions.IRLongExpansion = Options.IRLongExpansion;
				compiler.CompilerOptions.TwoPassOptimizations = Options.TwoPassOptimizations;
				compiler.CompilerOptions.OutputFile = CompiledFile;

				compiler.CompilerOptions.Architecture = SelectArchitecture(Options.PlatformType);
				compiler.CompilerOptions.LinkerFormatType = Options.LinkerFormatType;
				compiler.CompilerOptions.BootStageFactory = GetBootStageFactory(Options.BootFormat);

				compiler.CompilerOptions.SetCustomOption("multiboot.video", Options.VBEVideo ? "true" : "false");
				compiler.CompilerOptions.SetCustomOption("multiboot.width", Options.Width.ToString());
				compiler.CompilerOptions.SetCustomOption("multiboot.height", Options.Height.ToString());
				compiler.CompilerOptions.SetCustomOption("multiboot.depth", Options.Depth.ToString());

				compiler.CompilerOptions.BaseAddress = Options.BaseAddress;
				compiler.CompilerOptions.EmitSymbols = Options.EmitSymbols;
				compiler.CompilerOptions.EmitRelocations = Options.EmitRelocations;

				compiler.CompilerOptions.SetCustomOption("x86.irq-methods", Options.Emitx86IRQMethods ? "true" : "false");

				if (Options.GenerateMapFile)
				{
					compiler.CompilerOptions.MapFile = Path.Combine(Options.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(Options.SourceFile)}.map");
				}

				if (Options.GenerateDebugFile)
				{
					var debugFile = Options.DebugFile ?? Path.GetFileNameWithoutExtension(Options.SourceFile) + ".debug";

					compiler.CompilerOptions.DebugFile = Path.Combine(Options.DestinationDirectory, debugFile);
				}

				if (!Directory.Exists(Options.DestinationDirectory))
				{
					Directory.CreateDirectory(Options.DestinationDirectory);
				}

				compiler.CompilerTrace.TraceListener = traceListener;

				if (string.IsNullOrEmpty(Options.SourceFile))
				{
					AddOutput("Please select a source file");
					return;
				}
				else if (!File.Exists(Options.SourceFile))
				{
					AddOutput($"File {Options.SourceFile} does not exists");
					return;
				}

				var inputFiles = new List<FileInfo>
				{
					new FileInfo(Options.SourceFile)
				};

				compiler.AddPath(Options.Paths);

				if (Options.HuntForCorLib)
				{
					var path = HuntForCorLibDirectory();

					if (path != null)
					{
						OutputEvent("Hunted and found Corlib here: " + path);
						compiler.AddPath(path);
					}
				}

				compiler.Load(inputFiles);

				if (Options.UseMultiThreadingCompiler)
				{
					compiler.ExecuteThreaded();
				}
				else
				{
					compiler.Execute();
				}

				Linker = compiler.Linker;
				TypeSystem = compiler.TypeSystem;

				if (Options.ImageFormat == ImageFormat.ISO)
				{
					if (Options.BootLoader == BootLoader.Grub_0_97 || Options.BootLoader == BootLoader.Grub_2_00)
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

					if (Options.ImageFormat == ImageFormat.VMDK)
					{
						CreateVMDK();
					}
				}

				HasCompileError = false;

				if (Options.GenerateNASMFile)
				{
					LaunchNDISASM();
				}

				if (Options.GenerateASMFile)
				{
					GenerateASMFile();
				}
			}
			catch (Exception e)
			{
				AddOutput(e.ToString());
			}
			finally
			{
				compiler.Dispose();
				compiler = null;
			}
		}

		private void CreateDiskImage(string compiledFile)
		{
			var bootImageOptions = new BootImageOptions();

			if (Options.BootLoader == BootLoader.Syslinux_6_03)
			{
				bootImageOptions.MBRCode = GetResource(@"syslinux\6.03", "mbr.bin");
				bootImageOptions.FatBootCode = GetResource(@"syslinux\6.03", "ldlinux.bin");

				bootImageOptions.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\6.03", "ldlinux.sys")));
				bootImageOptions.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\6.03", "mboot.c32")));
			}
			else if (Options.BootLoader == BootLoader.Syslinux_3_72)
			{
				bootImageOptions.MBRCode = GetResource(@"syslinux\3.72", "mbr.bin");
				bootImageOptions.FatBootCode = GetResource(@"syslinux\3.72", "ldlinux.bin");

				bootImageOptions.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\3.72", "ldlinux.sys")));
				bootImageOptions.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\3.72", "mboot.c32")));
			}

			bootImageOptions.IncludeFiles.Add(new IncludeFile("syslinux.cfg", GetResource("syslinux", "syslinux.cfg")));
			bootImageOptions.IncludeFiles.Add(new IncludeFile(compiledFile, "main.exe"));

			bootImageOptions.IncludeFiles.Add(new IncludeFile("TEST.TXT", Encoding.ASCII.GetBytes("This is a test file.")));

			foreach (var include in Options.IncludeFiles)
			{
				bootImageOptions.IncludeFiles.Add(include);
			}

			bootImageOptions.VolumeLabel = "MOSABOOT";

			var vmext = ".img";
			switch (Options.ImageFormat)
			{
				case ImageFormat.VHD: vmext = ".vhd"; break;
				case ImageFormat.VDI: vmext = ".vdi"; break;
				default: break;
			}

			ImageFile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + vmext);

			bootImageOptions.DiskImageFileName = ImageFile;
			bootImageOptions.PatchSyslinuxOption = true;
			bootImageOptions.FileSystem = Options.FileSystem;
			bootImageOptions.ImageFormat = Options.ImageFormat;
			bootImageOptions.BootLoader = Options.BootLoader;

			Generator.Create(bootImageOptions);
		}

		private void CreateISOImageWithSyslinux(string compiledFile)
		{
			string isoDirectory = Path.Combine(Options.DestinationDirectory, "iso");

			if (Directory.Exists(isoDirectory))
			{
				Directory.Delete(isoDirectory, true);
			}

			Directory.CreateDirectory(isoDirectory);

			if (Options.BootLoader == BootLoader.Syslinux_6_03)
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.bin"), GetResource(@"syslinux\6.03", "isolinux.bin"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "mboot.c32"), GetResource(@"syslinux\6.03", "mboot.c32"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "ldlinux.c32"), GetResource(@"syslinux\6.03", "ldlinux.c32"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "libcom32.c32"), GetResource(@"syslinux\6.03", "libcom32.c32"));
			}
			else if (Options.BootLoader == BootLoader.Syslinux_3_72)
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.bin"), GetResource(@"syslinux\3.72", "isolinux.bin"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "mboot.c32"), GetResource(@"syslinux\3.72", "mboot.c32"));
			}

			File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.cfg"), GetResource("syslinux", "syslinux.cfg"));

			foreach (var include in Options.IncludeFiles)
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, include.Filename), include.Content);
			}

			File.Copy(compiledFile, Path.Combine(isoDirectory, "main.exe"));

			ImageFile = Path.Combine(Options.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(Options.SourceFile)}.iso");

			string arg = $"-relaxed-filenames -J -R -o {Quote(ImageFile)} -b isolinux.bin -no-emul-boot -boot-load-size 4 -boot-info-table {Quote(isoDirectory)}";

			LaunchApplication(AppLocations.Mkisofs, arg, true);
		}

		private void CreateISOImageWithGrub(string compiledFile)
		{
			string isoDirectory = Path.Combine(Options.DestinationDirectory, "iso");

			if (Directory.Exists(isoDirectory))
			{
				Directory.Delete(isoDirectory, true);
			}

			Directory.CreateDirectory(isoDirectory);
			Directory.CreateDirectory(Path.Combine(isoDirectory, "boot"));
			Directory.CreateDirectory(Path.Combine(isoDirectory, "boot", "grub"));
			Directory.CreateDirectory(isoDirectory);

			string loader = string.Empty;

			if (Options.BootLoader == BootLoader.Grub_0_97)
			{
				loader = "boot/grub/stage2_eltorito";
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "stage2_eltorito"), GetResource(@"grub\0.97", "stage2_eltorito"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "menu.lst"), GetResource(@"grub\0.97", "menu.lst"));
			}
			else if (Options.BootLoader == BootLoader.Grub_2_00)
			{
				loader = "boot/grub/i386-pc/eltorito.img";
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "grub.cfg"), GetResource(@"grub\2.00", "grub.cfg"));

				Directory.CreateDirectory(Path.Combine(isoDirectory, "boot", "grub", "i386-pc"));

				var data = GetResource(@"grub\2.00", "i386-pc.zip");
				var dataStream = new MemoryStream(data);

				var archive = new ZipArchive(dataStream);

				archive.ExtractToDirectory(Path.Combine(isoDirectory, "boot", "grub"));
			}

			foreach (var include in Options.IncludeFiles)
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, include.Filename), include.Content);
			}

			File.Copy(compiledFile, Path.Combine(isoDirectory, "boot", "main.exe"));

			ImageFile = Path.Combine(Options.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(Options.SourceFile)}.iso");

			string arg = $"-relaxed-filenames -J -R -o {Quote(ImageFile)} -b {Quote(loader)} -no-emul-boot -boot-load-size 4 -boot-info-table {Quote(isoDirectory)}";

			LaunchApplication(AppLocations.Mkisofs, arg, true);
		}

		private void CreateVMDK()
		{
			var vmdkFile = Path.Combine(Options.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(Options.SourceFile)}.vmdk");

			string arg = $"convert -f raw -O vmdk {Quote(ImageFile)} {Quote(vmdkFile)}";

			ImageFile = vmdkFile;
			LaunchApplication(AppLocations.QEMUImg, arg, true);
		}

		private void LaunchNDISASM()
		{
			var textSection = Linker.LinkerSections[(int)SectionKind.Text];

			const uint multibootHeaderLength = MultibootHeaderLength;
			ulong startingAddress = textSection.VirtualAddress + multibootHeaderLength;
			uint fileOffset = textSection.FileOffset + multibootHeaderLength;

			string arg = $"-b 32 -o0x{startingAddress.ToString("x")} -e0x{fileOffset.ToString("x")} {Quote(CompiledFile)}";

			var nasmfile = Path.Combine(Options.DestinationDirectory, $"{Path.GetFileNameWithoutExtension(Options.SourceFile)}.nasm");

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

			var asmfile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".asm");

			var textSection = Linker.LinkerSections[(int)SectionKind.Text];

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
				case PlatformType.X86: return Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect);
				default: throw new NotImplementCompilerException("Unknown or unsupported Architecture");
			}
		}

		/// <summary>
		/// Gets the boot stage factory.
		/// </summary>
		/// <param name="bootFormat">The boot format.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementCompilerException"></exception>
		private static Func<BaseCompilerStage> GetBootStageFactory(BootFormat bootFormat)
		{
			switch (bootFormat)
			{
				case BootFormat.Multiboot_0_7: return delegate { return new Mosa.Platform.x86.CompilerStages.Multiboot0695Stage(); };
				default: return null;
			}
		}

		private string mscorlibFileName = "mscorlib.dll";

		private bool CheckForCorLib(string directory)
		{
			return File.Exists(Path.Combine(directory, mscorlibFileName));
		}

		private string HuntForCorLibDirectory()
		{
			// Only hunt if it's not in any of the path directories already, or the source directory

			// let's check the source directory
			string path = Path.GetDirectoryName(Options.SourceFile);

			if (CheckForCorLib(path))
				return null;

			foreach (var optPaths in Options.Paths)
			{
				if (CheckForCorLib(optPaths))
					return null;
			}

			// okay -- need to hunt for it

			// check current directory
			if (CheckForCorLib(Environment.CurrentDirectory))
				return Environment.CurrentDirectory;

			string result = null;

			// check within packages directory in 1 or 2 directories back
			// this is how VS organizes projects and packages

			result = SearchSubdirectories(Path.Combine(Path.GetDirectoryName(Options.SourceFile), "..", "packages"));

			if (result != null)
				return result;

			result = SearchSubdirectories(Path.Combine(Path.GetDirectoryName(Options.SourceFile), "..", "..", "packages"));

			if (result != null)
				return result;

			result = SearchSubdirectories(Path.Combine(Environment.CurrentDirectory, "..", "packages"));

			if (result != null)
				return result;

			result = SearchSubdirectories(Path.Combine(Environment.CurrentDirectory, "..", "..", "packages"));

			if (result != null)
				return result;

			return null;
		}

		private string SearchSubdirectories(string path)
		{
			if (Directory.Exists(path))
			{
				var result = Directory.GetFiles(path, mscorlibFileName, SearchOption.AllDirectories);

				if (result?.Length >= 1)
				{
					return Path.GetDirectoryName(result[0]);
				}
			}

			return null;
		}
	}
}
