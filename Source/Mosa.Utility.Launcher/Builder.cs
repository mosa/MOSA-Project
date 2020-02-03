// Copyright (c) MOSA Project. Licensed under the New BSD License.

//using DiscUtils.Fat;
//using DiscUtils.Partitions;
//using DiscUtils.Raw;
//using DiscUtils.Streams;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
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

		public bool HasCompileError { get; private set; }

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
			Log.Clear();
			Counters.Clear();
			HasCompileError = true;

			CompileStartTime = DateTime.Now;

			try
			{
				if (!Directory.Exists(LauncherSettings.TemporaryFolder))
				{
					Directory.CreateDirectory(LauncherSettings.ImageFolder);
				}

				if (!Directory.Exists(LauncherSettings.ImageFolder))
				{
					Directory.CreateDirectory(LauncherSettings.ImageFolder);
				}

				if (string.IsNullOrEmpty(LauncherSettings.SourceFiles[0]))
				{
					AddOutput("ERROR: Missing source file");
					return;
				}
				else if (!File.Exists(LauncherSettings.SourceFiles[0]))
				{
					AddOutput($"File {LauncherSettings.SourceFiles[0]} does not exists");
					return;
				}

				Compile();

				BuildImage();

				if (!string.IsNullOrWhiteSpace(LauncherSettings.NasmFile))
				{
					LaunchNDISASM();
				}

				if (!string.IsNullOrWhiteSpace(LauncherSettings.AsmFile))
				{
					GenerateASMFile();
				}

				HasCompileError = false;
			}
			catch (Exception e)
			{
				HasCompileError = true;
				AddOutput($"Exception: {e.ToString()}");
			}
			finally
			{
				//compiler = null;
			}
		}

		private void Compile()
		{
			var fileHunter = new FileHunter(Path.GetDirectoryName(LauncherSettings.SourceFiles[0]));

			if (LauncherSettings.HuntForCorLib)
			{
				var fileCorlib = fileHunter.HuntFile("mscorlib.dll");

				if (fileCorlib != null)
				{
					Settings.AddPropertyListValue("Compiler.SourceFiles", fileCorlib.FullName);
				}
			}

			if (LauncherSettings.PlugKorlib)
			{
				var fileKorlib = fileHunter.HuntFile("Mosa.Plug.Korlib.dll");

				if (fileKorlib != null)
				{
					Settings.AddPropertyListValue("Compiler.SourceFiles", fileKorlib.FullName);
				}

				var platform = LauncherSettings.Platform;

				if (platform == "armv8a32")
				{
					platform = "ARMv8A32";
				}

				var fileKorlibPlatform = fileHunter.HuntFile($"Mosa.Plug.Korlib.{platform}.dll");

				if (fileKorlibPlatform != null)
				{
					Settings.AddPropertyListValue("Compiler.SourceFiles", fileKorlibPlatform.FullName);
				}
			}

			var compiler = new MosaCompiler(Settings, CompilerHooks);

			compiler.Load();
			compiler.Initialize();
			compiler.Setup();

			if (LauncherSettings.Multithreading)
			{
				compiler.ThreadedCompile();
			}
			else
			{
				compiler.Compile();
			}

			Linker = compiler.Linker;
			TypeSystem = compiler.TypeSystem;
		}

		private void BuildImage()
		{
			if (string.IsNullOrWhiteSpace(LauncherSettings.ImageFormat))
				return;

			AddOutput($"Generating Image: {LauncherSettings.ImageFormat}");

			if (LauncherSettings.ImageFormat == "iso")
			{
				if (LauncherSettings.ImageBootLoader == "grub0.97" || LauncherSettings.ImageBootLoader == "grub2.00")
				{
					CreateISOImageWithGrub();
				}
				else // assuming syslinux
				{
					CreateISOImageWithSyslinux();
				}
			}
			else if (LauncherSettings.ImageFormat == "vmdk")
			{
				var tmpimagefile = Path.Combine(LauncherSettings.TemporaryFolder, $"{Path.GetFileNameWithoutExtension(LauncherSettings.ImageFile)}.img");

				CreateDiskImage(tmpimagefile);

				CreateVMDK(tmpimagefile);
			}
			else if (LauncherSettings.ImageFormat == "img")
			{
				CreateDiskImage(LauncherSettings.ImageFile);
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

			if (LauncherSettings.ImageBootLoader == "syslinux3.72")
			{
				bootImageOptions.MBRCode = GetResource(@"syslinux\3.72", "mbr.bin");
				bootImageOptions.FatBootCode = GetResource(@"syslinux\3.72", "ldlinux.bin");

				bootImageOptions.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\3.72", "ldlinux.sys")));
				bootImageOptions.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\3.72", "mboot.c32")));
				bootImageOptions.PatchSyslinuxOption = true;
			}
			else if (LauncherSettings.ImageBootLoader == "syslinux6.03")
			{
				// NOT FULLY IMPLEMENTED YET!
				bootImageOptions.MBRCode = GetResource(@"syslinux\6.03", "mbr.bin");
				bootImageOptions.FatBootCode = GetResource(@"syslinux\6.03", "ldlinux.bin");

				bootImageOptions.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\6.03", "ldlinux.sys")));
				bootImageOptions.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\6.03", "mboot.c32")));
				bootImageOptions.PatchSyslinuxOption = false;
			}

			bootImageOptions.IncludeFiles.Add(new IncludeFile("syslinux.cfg", GetResource("syslinux", "syslinux.cfg")));
			bootImageOptions.IncludeFiles.Add(new IncludeFile(LauncherSettings.OutputFile, "main.exe"));

			bootImageOptions.IncludeFiles.Add(new IncludeFile("TEST.TXT", Encoding.ASCII.GetBytes("This is a test file.")));

			bootImageOptions.VolumeLabel = "MOSA";
			bootImageOptions.DiskImageFileName = imagefile;

			switch (LauncherSettings.ImageBootLoader)
			{
				case "syslinux3.72": bootImageOptions.BootLoader = BootLoader.Syslinux_3_72; break;
				case "syslinux6.03": bootImageOptions.BootLoader = BootLoader.Syslinux_6_03; break;
				case "grub0.97": bootImageOptions.BootLoader = BootLoader.Grub_0_97; break;
				case "grub2.00": bootImageOptions.BootLoader = BootLoader.Grub_2_00; break;
				default: break;
			}

			switch (LauncherSettings.ImageFormat)
			{
				case "img": bootImageOptions.ImageFormat = ImageFormat.IMG; break;
				case "iso": bootImageOptions.ImageFormat = ImageFormat.ISO; break;
				case "vhd": bootImageOptions.ImageFormat = ImageFormat.VHD; break;
				case "vdi": bootImageOptions.ImageFormat = ImageFormat.VDI; break;
				case "vmdk": bootImageOptions.ImageFormat = ImageFormat.VMDK; break;
				default: break;
			}

			switch (LauncherSettings.FileSystem)
			{
				case "fat12": bootImageOptions.FileSystem = BootImage.FileSystem.FAT12; break;
				case "fat16": bootImageOptions.FileSystem = BootImage.FileSystem.FAT16; break;
				case "fat32": bootImageOptions.FileSystem = BootImage.FileSystem.FAT32; break;
				default: throw new NotImplementCompilerException("unknown file system");
			}

			Generator.Create(bootImageOptions);
		}

		//private void CreateDiskImageV2(string compiledFile)
		//{
		//	var SectorSize = 512;
		//	var files = new List<IncludeFile>();
		//	byte[] mbr = null;
		//	byte[] fatBootCode = null;

		//	if (File.Exists(LauncherSettings.ImageFile))
		//	{
		//		File.Delete(LauncherSettings.ImageFile);
		//	}

		//	// Get Files
		//	if (LauncherSettings.ImageBootLoader == "syslinux6.03")
		//	{
		//		mbr = GetResource(@"syslinux\6.03", "mbr.bin");
		//		fatBootCode = GetResource(@"syslinux\6.03", "ldlinux.bin");

		//		files.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\6.03", "ldlinux.sys")));
		//		files.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\6.03", "mboot.c32")));
		//	}
		//	else if (LauncherSettings.ImageBootLoader == "syslinux3.72")
		//	{
		//		mbr = GetResource(@"syslinux\3.72", "mbr.bin");
		//		fatBootCode = GetResource(@"syslinux\3.72", "ldlinux.bin");

		//		files.Add(new IncludeFile("ldlinux.sys", GetResource(@"syslinux\3.72", "ldlinux.sys")));
		//		files.Add(new IncludeFile("mboot.c32", GetResource(@"syslinux\3.72", "mboot.c32")));
		//	}

		//	files.Add(new IncludeFile("syslinux.cfg", GetResource("syslinux", "syslinux.cfg")));
		//	files.Add(new IncludeFile(compiledFile, "main.exe"));

		//	files.Add(new IncludeFile("TEST.TXT", Encoding.ASCII.GetBytes("This is a test file.")));

		//	//foreach (var include in IncludeFiles)
		//	//{
		//	//	File.WriteAllBytes(Path.Combine(isoDirectory, include.Filename), include.Content);
		//	//}

		//	// Estimate file system size
		//	var blockCount = 8400 + 1;
		//	foreach (var file in files)
		//	{
		//		blockCount += (file.Content.Length / SectorSize) + 1;
		//	}

		//	using (var imageStream = new MemoryStream())
		//	{
		//		var disk = Disk.Initialize(imageStream, Ownership.Dispose, blockCount * SectorSize);

		//		BiosPartitionTable.Initialize(disk, WellKnownPartitionType.WindowsFat);

		//		disk.SetMasterBootRecord(mbr);

		//		using (var fs = FatFileSystem.FormatPartition(disk, 0, null))
		//		{
		//			foreach (var file in files)
		//			{
		//				var directory = Path.GetFullPath(file.Filename);

		//				if (!string.IsNullOrWhiteSpace(directory) && !fs.DirectoryExists(directory))
		//				{
		//					fs.CreateDirectory(directory);
		//				}

		//				using (var f = fs.OpenFile(file.Filename, FileMode.Create))
		//				{
		//					f.Write(file.Content);
		//					f.Close();
		//				}
		//			}
		//		}

		//		using (var partition = disk.Partitions.Partitions[0].Open())
		//		{
		//			partition.Seek(0x03, SeekOrigin.Begin);
		//			partition.Write(fatBootCode, 0, 3);
		//			partition.Seek(0x3E, SeekOrigin.Begin);
		//			partition.Write(fatBootCode, 0x3E, Math.Max(448, fatBootCode.Length - 0x3E));
		//			partition.Close();
		//		}

		//		imageStream.WriteTo(File.Create(LauncherSettings.ImageFile));
		//	}
		//}

		private void CreateISOImageWithSyslinux()
		{
			string isoDirectory = Path.Combine(LauncherSettings.ImageFolder, "iso");

			if (Directory.Exists(isoDirectory))
			{
				Directory.Delete(isoDirectory, true);
			}

			Directory.CreateDirectory(isoDirectory);

			if (LauncherSettings.ImageBootLoader == "syslinux6.03")
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.bin"), GetResource(@"syslinux\6.03", "isolinux.bin"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "mboot.c32"), GetResource(@"syslinux\6.03", "mboot.c32"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "ldlinux.c32"), GetResource(@"syslinux\6.03", "ldlinux.c32"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "libcom32.c32"), GetResource(@"syslinux\6.03", "libcom32.c32"));
			}
			else if (LauncherSettings.ImageBootLoader == "Syslinux3.72")
			{
				File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.bin"), GetResource(@"syslinux\3.72", "isolinux.bin"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "mboot.c32"), GetResource(@"syslinux\3.72", "mboot.c32"));
			}

			File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.cfg"), GetResource("syslinux", "syslinux.cfg"));

			//foreach (var include in IncludeFiles)
			//{
			//	File.WriteAllBytes(Path.Combine(isoDirectory, include.Filename), include.Content);
			//}

			File.Copy(LauncherSettings.OutputFile, Path.Combine(isoDirectory, "main.exe"));

			string arg = $"-relaxed-filenames -J -R -o {Quote(LauncherSettings.ImageFile)} -b isolinux.bin -no-emul-boot -boot-load-size 4 -boot-info-table {Quote(isoDirectory)}";

			LaunchApplication(LauncherSettings.Mkisofs, arg, true);
		}

		private void CreateISOImageWithGrub()
		{
			string isoDirectory = Path.Combine(LauncherSettings.ImageFolder, "iso");

			if (Directory.Exists(isoDirectory))
			{
				Directory.Delete(isoDirectory, true);
			}

			Directory.CreateDirectory(isoDirectory);
			Directory.CreateDirectory(Path.Combine(isoDirectory, "boot"));
			Directory.CreateDirectory(Path.Combine(isoDirectory, "boot", "grub"));
			Directory.CreateDirectory(isoDirectory);

			string loader = string.Empty;

			if (LauncherSettings.ImageBootLoader == "grub0.97")
			{
				loader = "boot/grub/stage2_eltorito";
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "stage2_eltorito"), GetResource(@"grub\0.97", "stage2_eltorito"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "menu.lst"), GetResource(@"grub\0.97", "menu.lst"));
			}
			else if (LauncherSettings.ImageBootLoader == "grub2.00")
			{
				loader = "boot/grub/i386-pc/eltorito.img";
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "grub.cfg"), GetResource(@"grub\2.00", "grub.cfg"));

				Directory.CreateDirectory(Path.Combine(isoDirectory, "boot", "grub", "i386-pc"));

				var data = GetResource(@"grub\2.00", "i386-pc.zip");
				var dataStream = new MemoryStream(data);

				var archive = new ZipArchive(dataStream);

				archive.ExtractToDirectory(Path.Combine(isoDirectory, "boot", "grub"));
			}

			//foreach (var include in IncludeFiles)
			//{
			//	File.WriteAllBytes(Path.Combine(isoDirectory, include.Filename), include.Content);
			//}

			File.Copy(LauncherSettings.OutputFile, Path.Combine(isoDirectory, "boot", "main.exe"));

			string arg = $"-relaxed-filenames -J -R -o {Quote(LauncherSettings.ImageFile)} -b {Quote(loader)} -no-emul-boot -boot-load-size 4 -boot-info-table {Quote(isoDirectory)}";

			LaunchApplication(LauncherSettings.Mkisofs, arg, true);
		}

		private void CreateVMDK(string source)
		{
			string arg = $"convert -f raw -O vmdk {Quote(source)} {Quote(LauncherSettings.ImageFile)}";

			LaunchApplication(LauncherSettings.QemuImg, arg, true);
		}

		private void LaunchNDISASM()
		{
			//var textSection = Linker.Sections[(int)SectionKind.Text];
			var startingAddress = LauncherSettings.BaseAddress + MultibootHeaderLength;
			var fileOffset = Linker.BaseFileOffset + MultibootHeaderLength;

			string arg = $"-b 32 -o0x{startingAddress.ToString("x")} -e0x{fileOffset.ToString("x")} {Quote(LauncherSettings.OutputFile)}";

			//var nasmfile = Path.Combine(LauncherSettings.ImageFolder, $"{Path.GetFileNameWithoutExtension(LauncherSettings.SourceFiles[0])}.nasm");

			var process = LaunchApplication(LauncherSettings.Ndisasm, arg);

			var processoutput = GetOutput(process);

			File.WriteAllText(LauncherSettings.NasmFile, processoutput);
		}

		private void GenerateASMFile()
		{
			var translator = new IntelTranslator()
			{
				IncludeAddress = true,
				IncludeBinary = true
			};

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

			var code2 = File.ReadAllBytes(LauncherSettings.OutputFile);

			var code = new byte[code2.Length];

			for (ulong i = fileOffset; i < (ulong)code2.Length; i++)
			{
				code[i - fileOffset] = code2[i];
			}

			var mode = ArchitectureMode.x86_32;

			switch (LauncherSettings.Platform)
			{
				case "x86": mode = ArchitectureMode.x86_32; break;
				case "x64": mode = ArchitectureMode.x86_64; break;
			}

			using (var disasm = new Disassembler(code, mode, startingAddress, true, Vendor.Any))
			{
				using (var dest = File.CreateText(LauncherSettings.AsmFile))
				{
					if (map.TryGetValue(startingAddress, out List<string> list))
					{
						foreach (var entry in list)
						{
							dest.WriteLine($"; {entry}");
						}
					}

					foreach (var instruction in disasm.Disassemble())
					{
						var inst = translator.Translate(instruction);
						dest.WriteLine(inst);

						if (map.TryGetValue(instruction.PC, out List<string> list2))
						{
							foreach (var entry in list2)
							{
								dest.WriteLine($"; {entry}");
							}
						}

						if (instruction.PC > startingAddress + length)
							break;
					}
				}
			}
		}

		private void NotifyEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			if (compilerEvent == CompilerEvent.CompileStart
				|| compilerEvent == CompilerEvent.CompileEnd
				|| compilerEvent == CompilerEvent.CompilingMethods
				|| compilerEvent == CompilerEvent.CompilingMethodsCompleted
				|| compilerEvent == CompilerEvent.InlineMethodsScheduled
				|| compilerEvent == CompilerEvent.LinkingStart
				|| compilerEvent == CompilerEvent.LinkingEnd
				|| compilerEvent == CompilerEvent.Warning
				|| compilerEvent == CompilerEvent.Error
				|| compilerEvent == CompilerEvent.Exception)
			{
				string status = $"Compiling: {$"{(DateTime.Now - CompileStartTime).TotalSeconds:0.00}"} secs: {compilerEvent.ToText()}";

				if (!string.IsNullOrEmpty(message))
					status += $"- { message}";

				AddOutput(status);
			}
			else if (compilerEvent == CompilerEvent.Counter)
			{
				AddCounters(message);
			}
		}
	}
}
