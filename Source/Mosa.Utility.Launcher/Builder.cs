// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Linker.Elf32;
using Mosa.Compiler.Trace;
using Mosa.Utility.Aot;
using Mosa.Utility.BootImage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.IO.Compression;

namespace Mosa.Utility.Launcher
{
	public class Builder
	{
		private MosaCompiler Compiler { get; set; }

		public Options Options { get; set; }

		public AppLocations AppLocations { get; set; }

		public IList<string> Log { get; private set; }

		public IList<string> Counters { get; private set; }

		public DateTime CompileStartTime { get; private set; }

		public IBuilderEvent BuilderEvent { get; private set; }

		protected string compiledFile;
		protected string imageFile;
		protected ITraceListener traceListener;

		public Builder(Options options, AppLocations appLocations, IBuilderEvent builderEvent)
		{
			Log = new List<string>();
			Counters = new List<string>();
			traceListener = new BuilderEventListener(this);
			Options = options;
			AppLocations = appLocations;
			BuilderEvent = builderEvent;
		}

		public void AddOutput(string status)
		{
			if (status == null)
				return;

			Log.Add(status);

			if (BuilderEvent != null)
				BuilderEvent.NewStatus(status);
		}

		public void AddCounters(string data)
		{
			if (data == null)
				return;

			Counters.Add(data);
		}

		public bool HasCompileError { get; private set; }

		public void Compile()
		{
			HasCompileError = false;
			try
			{
				Compiler = new MosaCompiler();
				CompileStartTime = DateTime.Now;

				compiledFile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".bin");

				Compiler.CompilerFactory = delegate { return new AotCompiler(); };

				Compiler.CompilerOptions.EnableSSA = Options.EnableSSA;
				Compiler.CompilerOptions.EnableOptimizations = Options.EnableIROptimizations;
				Compiler.CompilerOptions.EnableSparseConditionalConstantPropagation = Options.EnableSparseConditionalConstantPropagation;
				Compiler.CompilerOptions.EnableInlinedMethods = Options.EnableInlinedMethods;
				Compiler.CompilerOptions.InlinedIRMaximum = Options.InlinedIRMaximum;
				Compiler.CompilerOptions.EnableVariablePromotion = Options.EnableVariablePromotion;
				Compiler.CompilerOptions.OutputFile = compiledFile;

				Compiler.CompilerOptions.Architecture = SelectArchitecture(Options.PlatformType);
				Compiler.CompilerOptions.LinkerFactory = GetLinkerFactory(Options.LinkerFormat);
				Compiler.CompilerOptions.BootStageFactory = GetBootStageFactory(Options.BootFormat);

				Compiler.CompilerOptions.SetCustomOption("multiboot.video", Options.VBEVideo ? "true" : "false");
				Compiler.CompilerOptions.SetCustomOption("multiboot.width", Options.Width.ToString());
				Compiler.CompilerOptions.SetCustomOption("multiboot.height", Options.Height.ToString());
				Compiler.CompilerOptions.SetCustomOption("multiboot.depth", Options.Depth.ToString());

				if (Options.GenerateMapFile)
				{
					Compiler.CompilerOptions.MapFile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".map");
				}

				if (!Directory.Exists(Options.DestinationDirectory))
				{
					Directory.CreateDirectory(Options.DestinationDirectory);
				}

				Compiler.CompilerTrace.TraceListener = traceListener;

				if (string.IsNullOrEmpty(Options.SourceFile))
				{
					AddOutput("Please select a source file");
					HasCompileError = true;
					return;
				}
				else if (!File.Exists(Options.SourceFile))
				{
					AddOutput(string.Format("File {0} does not exists", Options.SourceFile));
					HasCompileError = true;
					return;
				}

				var inputFiles = new List<FileInfo>();
				inputFiles.Add(new FileInfo(Options.SourceFile));

				Compiler.Load(inputFiles);

				var threads = Options.UseMultipleThreadCompiler ? Environment.ProcessorCount : 1;
				Compiler.Execute(threads);

				if (Options.ImageFormat == ImageFormat.ISO)
				{
					if (Options.BootLoader == BootLoader.Grub_0_97 || Options.BootLoader == BootLoader.Grub_2_00)
					{
						CreateISOImageWithGrub(compiledFile);
					}
					else // assuming syslinux
					{
						CreateISOImageWithSyslinux(compiledFile);
					}
				}
				else
				{
					CreateDiskImage(compiledFile);

					if (Options.ImageFormat == ImageFormat.VMDK)
					{
						CreateVMDK(imageFile);
					}
				}

				if (Options.GenerateASMFile)
				{
					LaunchNDISASM();
				}
			}
			finally
			{
				Compiler.Dispose();
				Compiler = null;
			}
		}

		protected byte[] GetResource(string path, string name)
		{
			var newname = path.Replace(".", "._").Replace(@"\", "._").Replace(@"/", "._").Replace(@"-", "_") + "." + name;
			return GetResource(newname);
		}

		protected byte[] GetResource(string name)
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

			bootImageOptions.IncludeFiles.Add(new IncludeFile("syslinux.cfg", GetResource(@"syslinux", "syslinux.cfg")));
			bootImageOptions.IncludeFiles.Add(new IncludeFile(compiledFile, "main.exe"));

			bootImageOptions.VolumeLabel = "MOSABOOT";

			var vmext = ".img";
			switch (Options.ImageFormat)
			{
				case ImageFormat.VHD: vmext = ".vhd"; break;
				case ImageFormat.VDI: vmext = ".vdi"; break;
				default: break;
			}

			imageFile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + vmext);

			bootImageOptions.DiskImageFileName = imageFile;
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

			File.WriteAllBytes(Path.Combine(isoDirectory, "isolinux.cfg"), GetResource(@"syslinux", "syslinux.cfg"));
			File.Copy(compiledFile, Path.Combine(isoDirectory, "main.exe"));

			imageFile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".iso");

			string arg =
				"-relaxed-filenames" +
				" -J -R" +
				" -o " + Quote(imageFile) +
				" -b isolinux.bin" +
				" -no-emul-boot" +
				" -boot-load-size 4" +
				" -boot-info-table " +
				Quote(isoDirectory);

			var output = LaunchApplication(AppLocations.mkisofs, arg, true);

			AddOutput(output);
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
				loader = @"boot/grub/stage2_eltorito";
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "stage2_eltorito"), GetResource(@"grub\0.97", "stage2_eltorito"));
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "menu.lst"), GetResource(@"grub\0.97", "menu.lst"));
			}
			else if (Options.BootLoader == BootLoader.Grub_2_00)
			{
				loader = @"boot/grub/i386-pc/eltorito.img";
				File.WriteAllBytes(Path.Combine(isoDirectory, "boot", "grub", "grub.cfg"), GetResource(@"grub\2.00", "grub.cfg"));

				Directory.CreateDirectory(Path.Combine(isoDirectory, "boot", "grub", "i386-pc"));

				var data = GetResource(@"grub\2.00", "i386-pc.zip");
				var dataStream = new MemoryStream(data);

				var archive = new ZipArchive(dataStream);

				archive.ExtractToDirectory(Path.Combine(isoDirectory, "boot", "grub"));
			}

			File.Copy(compiledFile, Path.Combine(isoDirectory, "boot", "main.exe"));

			imageFile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".iso");

			string arg =
				"-relaxed-filenames" +
				" -J -R" +
				" -o " + Quote(imageFile) +
				" -b " + Quote(loader) +
				" -no-emul-boot" +
				" -boot-load-size 4" +
				" -boot-info-table " +
				Quote(isoDirectory);

			var output = LaunchApplication(AppLocations.mkisofs, arg, true);

			AddOutput(output);
		}

		private void CreateVMDK(string compiledFile)
		{
			var vmdkFile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".vmdk");

			string arg =
				"convert" +
				" -f" +
				" raw" +
				" -O" +
				" vmdk " +
				Quote(imageFile) + " " +
				Quote(vmdkFile);

			var output = LaunchApplication(AppLocations.QEMUImg, arg, true);

			AddOutput(output);

			imageFile = vmdkFile;
		}

		private string LaunchApplication(string app, string args, bool waitForExit)
		{
			AddOutput("Launching Application: " + app);
			AddOutput("Arguments: " + args);

			var start = new ProcessStartInfo();
			start.FileName = app;
			start.Arguments = args;
			start.UseShellExecute = false;
			start.CreateNoWindow = true;
			start.RedirectStandardOutput = true;
			start.RedirectStandardError = true;

			var process = Process.Start(start);

			if (waitForExit)
			{
				var output = process.StandardOutput.ReadToEnd();

				process.WaitForExit();

				var error = process.StandardError.ReadToEnd();
				return output + error;
			}

			return string.Empty;
		}

		private void LaunchNDISASM()
		{
			string arg =
				"-b 32 -o0x400030 -e 0x1030 " + Quote(compiledFile);

			var output = LaunchApplication(AppLocations.NDISASM, arg, true);

			var asmfile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".asm");

			File.WriteAllText(asmfile, output);
		}

		public void Launch()
		{
			switch (Options.Emulator)
			{
				case EmulatorType.Qemu: LaunchQemu(Options.ExitOnLaunch); break;
				case EmulatorType.Bochs: LaunchBochs(Options.ExitOnLaunch); break;
				case EmulatorType.VMware: LaunchVMwarePlayer(Options.ExitOnLaunch); break;
				default: throw new InvalidOperationException();
			}
		}

		private void LaunchQemu(bool exit)
		{
			string arg =
				" -L " + Quote(AppLocations.QEMUBIOSDirectory);

			if (Options.ImageFormat == ImageFormat.ISO)
			{
				arg = arg +
					" -cdrom " + Quote(imageFile);
			}
			else
			{
				arg = arg +
					" -hda " + Quote(imageFile);
			}

			if (Options.PlatformType == PlatformType.X86)
			{
				arg = arg + " -cpu qemu32,+sse4.1";
			}

			//arg = arg + " -vga vmware";

			if (Options.DebugConnectionOption == DebugConnectionOption.Pipe)
			{
				arg = arg + " -serial pipe:MOSA";
			}
			else if (Options.DebugConnectionOption == DebugConnectionOption.TCPServer)
			{
				arg = arg + " -serial tcp:127.0.0.1:9999,server,nowait,nodelay";
			}
			else if (Options.DebugConnectionOption == DebugConnectionOption.TCPClient)
			{
				arg = arg + " -serial tcp:127.0.0.1:9999,client,nowait";
			}

			var output = LaunchApplication(AppLocations.QEMU, arg, !exit);

			AddOutput(output);
		}

		private void LaunchBochs(bool exit)
		{
			var logfile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + "-bochs.log");
			var configfile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".bxrc");
			var exeDir = Path.GetDirectoryName(AppLocations.BOCHS);

			var fileVersionInfo = FileVersionInfo.GetVersionInfo(AppLocations.BOCHS);

			// simd or sse
			var simd = "simd";

			if (!(fileVersionInfo.FileMajorPart >= 2 && fileVersionInfo.FileMinorPart >= 6 && fileVersionInfo.FileBuildPart >= 5))
				simd = "sse";

			var sb = new StringBuilder();

			sb.AppendLine("megs: " + Options.MemoryInMB.ToString());
			sb.AppendLine("ata0: enabled=1,ioaddr1=0x1f0,ioaddr2=0x3f0,irq=14");
			sb.AppendLine("cpuid: mmx=1,sep=1," + simd + "=sse4_2,apic=xapic,aes=1,movbe=1,xsave=1");
			sb.AppendLine("boot: cdrom,disk");
			sb.AppendLine("log: " + Quote(logfile));
			sb.AppendLine("romimage: file=" + Quote(Path.Combine(exeDir, "BIOS-bochs-latest")));
			sb.AppendLine("vgaromimage: file=" + Quote(Path.Combine(exeDir, "VGABIOS-lgpl-latest")));

			if (Options.ImageFormat == ImageFormat.ISO)
			{
				sb.AppendLine("ata0-master: type=cdrom,path=" + Quote(imageFile) + ",status=inserted");
			}
			else
			{
				sb.AppendLine("ata0-master: type=disk,path=" + Quote(imageFile) + ",biosdetect=none,cylinders=0,heads=0,spt=0");
			}

			if (Options.DebugConnectionOption == DebugConnectionOption.Pipe)
			{
				sb.AppendLine(@"com1: enabled=1, mode=pipe-server, dev=\\.\pipe\MOSA");
			}

			File.WriteAllText(configfile, sb.ToString());

			string arg =
				"-q " +
				"-f " + Quote(configfile);

			var output = LaunchApplication(AppLocations.BOCHS, arg, !exit);

			AddOutput(output);
		}

		private void LaunchVMwarePlayer(bool exit)
		{
			var logfile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + "-vmx.log");
			var configfile = Path.Combine(Options.DestinationDirectory, Path.GetFileNameWithoutExtension(Options.SourceFile) + ".vmx");

			var sb = new StringBuilder();

			sb.AppendLine(".encoding = \"windows-1252\"");
			sb.AppendLine("config.version = \"8\"");
			sb.AppendLine("virtualHW.version = \"4\"");
			sb.AppendLine("memsize = " + Quote(Options.MemoryInMB.ToString()));

			sb.AppendLine("displayName = \"MOSA - " + Path.GetFileNameWithoutExtension(Options.SourceFile) + "\"");
			sb.AppendLine("guestOS = \"other\"");
			sb.AppendLine("priority.grabbed = \"normal\"");
			sb.AppendLine("priority.ungrabbed = \"normal\"");
			sb.AppendLine("virtualHW.productCompatibility = \"hosted\"");
			sb.AppendLine("ide0:0.present = \"TRUE\"");
			sb.AppendLine("ide0:0.fileName = " + Quote(imageFile));

			if (Options.ImageFormat == ImageFormat.ISO)
			{
				sb.AppendLine("ide0:0.deviceType = \"cdrom-image\"");
			}

			sb.AppendLine("floppy0.present = \"FALSE\"");

			if (Options.DebugConnectionOption == DebugConnectionOption.Pipe)
			{
				sb.AppendLine("serial0.present = \"TRUE\"");
				sb.AppendLine("serial0.yieldOnMsrRead = \"FALSE\"");
				sb.AppendLine("serial0.fileType = \"pipe\"");
				sb.AppendLine("serial0.fileName = \"\\\\.\\pipe\\MOSA\"");
				sb.AppendLine("serial0.pipe.endPoint = \"server\"");
				sb.AppendLine("serial0.tryNoRxLoss = \"FALSE\"");
			}

			File.WriteAllText(configfile, sb.ToString());

			string arg = Quote(configfile);

			var output = LaunchApplication(AppLocations.VMwarePlayer, arg, !exit);

			AddOutput(output);
		}

		/// <summary>
		/// Selects the architecture.
		/// </summary>
		/// <param name="platformType">Type of the platform.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementCompilerException">Unknown or unsupported Architecture</exception>
		/// <exception cref="Mosa.Compiler.Common.NotImplementCompilerException">Unknown or unsupported Architecture</exception>
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
		/// <exception cref="Mosa.Compiler.Common.NotImplementCompilerException"></exception>
		/// <exception cref="NotImplementCompilerException"></exception>
		private static Func<ICompilerStage> GetBootStageFactory(BootFormat bootFormat)
		{
			switch (bootFormat)
			{
				case BootFormat.Multiboot_0_7: return delegate { return new Mosa.Platform.x86.Stages.Multiboot0695Stage(); };
				default: return null;
			}
		}

		/// <summary>
		/// Gets the linker factory.
		/// </summary>
		/// <param name="linkerType">Type of the linker.</param>
		/// <returns></returns>
		private static Func<BaseLinker> GetLinkerFactory(LinkerFormat linkerType)
		{
			switch (linkerType)
			{
				case LinkerFormat.Elf32: return delegate { return new Elf32(); };

				//case LinkerType.Elf64: return delegate { return new Elf64(); };
				default: return null;
			}
		}
	}
}
