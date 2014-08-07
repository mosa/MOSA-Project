/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Linker.Elf32;
using Mosa.Compiler.Linker.PE;
using Mosa.Utility.Aot;
using Mosa.Utility.BootImage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Mosa.Tool.Launcher
{
	public partial class MainForm : Form, ICompilerEventListener
	{
		public enum VMEmulator { Qemu, WMware, Boches };

		public enum VMDiskFormat { NotSpecified, IMG, VHD, VDI, ISO };

		public string SourceFile { get; set; }

		public string DestinationDirectory { get; set; }

		public bool ExitOnLaunch { get; set; }

		public bool AutoLaunch { get; set; }

		public bool GenerateMap { get; set; }

		public bool GenerateASM { get; set; }

		public VMEmulator Emulator { get; set; }

		public bool MOSADebugger { get; set; }

		public VMDiskFormat DiskImage { get; set; }

		protected DateTime compileStartTime;
		protected string compiledFile;
		protected string imageFile;

		public MainForm()
		{
			InitializeComponent();
			AutoLaunch = false;
			ExitOnLaunch = false;
			GenerateASM = false;
			GenerateMap = false;
			DiskImage = VMDiskFormat.NotSpecified;
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			lbSource.Text = Path.GetFileName(SourceFile);
			lbSourceDirectory.Text = Path.GetDirectoryName(SourceFile);
			cbExitOnLaunch.Checked = ExitOnLaunch;
			cbGenerateASMFile.Checked = GenerateASM;
			cbGenerateMapFile.Checked = GenerateMap;

			switch (Emulator)
			{
				case VMEmulator.Qemu: cbEmulator.SelectedIndex = 0; cbImageFormat.SelectedIndex = 0; break;
				case VMEmulator.Boches: cbEmulator.SelectedIndex = 1; cbImageFormat.SelectedIndex = 0; break;
				case VMEmulator.WMware: cbEmulator.SelectedIndex = 2; cbImageFormat.SelectedIndex = 3; break;
				default: break;
			}

			switch (DiskImage)
			{
				case VMDiskFormat.IMG: cbImageFormat.SelectedIndex = 0; break;
				case VMDiskFormat.VHD: cbImageFormat.SelectedIndex = 1; break;
				case VMDiskFormat.VDI: cbImageFormat.SelectedIndex = 2; break;
				case VMDiskFormat.ISO: cbImageFormat.SelectedIndex = 3; break;
				default: break;
			}

			cbMOSADebugger.Checked = MOSADebugger;

			this.Refresh();

			if (AutoLaunch)
				CompilerAndLaunch();
		}

		public void AddOutput(string data)
		{
			if (data == null)
				return;

			richTextBox1.AppendText(data);
			richTextBox1.AppendText("\n");
			richTextBox1.Update();
		}

		public void AddCounters(string data)
		{
			richTextBox2.AppendText(data);
			richTextBox2.AppendText("\n");
			richTextBox2.Update();
		}

		void ICompilerEventListener.SubmitTraceEvent(CompilerEvent compilerStage, string info)
		{
			if (compilerStage == CompilerEvent.CompilerStageStart || compilerStage == CompilerEvent.CompilerStageEnd)
			{
				string status = "Compiling: " + String.Format("{0:0.00}", (DateTime.Now - compileStartTime).TotalSeconds) + " secs: " + compilerStage.ToText() + ": " + info;

				AddOutput(status);
			}
			else if (compilerStage == CompilerEvent.Counter)
			{
				AddCounters(info);
			}
		}

		void ICompilerEventListener.SubmitMethodStatus(int totalMethods, int queuedMethods)
		{
			progressBar1.Maximum = totalMethods;
			progressBar1.Value = totalMethods - queuedMethods;
		}

		private void btnSource_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				SourceFile = openFileDialog1.FileName;

				lbSource.Text = Path.GetFileName(SourceFile);
				lbSourceDirectory.Text = Path.GetDirectoryName(SourceFile);
			}
		}

		private string CombineParameterAndDirectory(string parameter, string subdirectory)
		{
			var variable = Environment.GetEnvironmentVariable(parameter);

			if (variable == null)
				return null;

			return Path.Combine(variable, subdirectory);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			cbPlatform.SelectedIndex = 0;
			cbImageFormat.SelectedIndex = 0;
			cbLinkerFormat.SelectedIndex = 0;
			cbEmulator.SelectedIndex = 0;
			cbBootFormat.SelectedIndex = 0;
			cbBootFileSystem.SelectedIndex = 0;
			tabControl1.SelectedTab = tabPage1;

			if (DestinationDirectory == null)
			{
				DestinationDirectory = Path.Combine(Path.GetTempPath(), "MOSA");
			}

			lbDestinationDirectory.Text = DestinationDirectory;

			// find QEMU executable
			lbQEMUExecutable.Text = TryFind(
				"qemu-system-i386.exe",
				new string[] {
					CombineParameterAndDirectory("MOSA",@"Tools\QEMU"),
					CombineParameterAndDirectory("MOSA",@"QEMU"),
					@"..\Tools\QEMU",
					@"Tools\QEMU",
					CombineParameterAndDirectory("ProgramFiles",@"qemu"),
					CombineParameterAndDirectory("ProgramFiles(x86)",@"qemu")
				}
			);

			lbQEMUBIOSDirectory.Text = Path.GetDirectoryName(
				TryFind(
					"bios.bin",
					new string[] {
						Path.GetDirectoryName(lbQEMUExecutable.Text),
						Path.Combine(Path.GetDirectoryName(lbQEMUExecutable.Text),"bios")
					}
				)
			);

			// find NDISMASM
			lbNDISASMExecutable.Text = TryFind(
				   "ndisasm.exe",
				   new string[] {
					CombineParameterAndDirectory("MOSA",@"Tools\ndisasm"),
					CombineParameterAndDirectory("MOSA",@"ndisasm"),
					@"..\Tools\ndisasm",
					@"Tools\ndisasm"
				}
			);

			// find BOCHS
			lbBOCHSExecutable.Text = TryFind(
				"bochs.exe",
				new string[] {
					CombineParameterAndDirectory("ProgramFiles",@"Bochs-2.6.5"),
					CombineParameterAndDirectory("ProgramFiles(x86)",@"Bochs-2.6.5"),
					CombineParameterAndDirectory("ProgramFiles",@"Bochs-2.6.2"),
					CombineParameterAndDirectory("ProgramFiles(x86)",@"Bochs-2.6.2"),
					CombineParameterAndDirectory("MOSA",@"Tools\Bochs"),
					CombineParameterAndDirectory("MOSA",@"Bochs"),
					@"..\Tools\Bochs",
					@"Tools\Bochs"
				}
			);

			// find vmware player
			lbVMwarePlayerExecutable.Text = TryFind(
				"vmplayer.exe",
				new string[] {
					Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"),@"VMware\VMware Player"),
					Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"),@"VMware\VMware Player")
				}
			);
		}

		private string TryFind(string file, IList<string> directories)
		{
			string location;

			foreach (var directory in directories)
			{
				if (TryFind(file, directory, out location))
					return location;
			}

			return string.Empty;
		}

		private bool TryFind(string file, string directory, out string location)
		{
			location = string.Empty;

			if (directory == null)
				return false;

			string combine = Path.Combine(directory, file);

			if (File.Exists(combine))
			{
				location = combine;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Selects the architecture.
		/// </summary>
		/// <param name="architecture">The architecture.</param>
		/// <returns></returns>
		private static BaseArchitecture SelectArchitecture(string architecture)
		{
			switch (architecture.ToLower())
			{
				case "x86": return Mosa.Platform.x86.Architecture.CreateArchitecture(Mosa.Platform.x86.ArchitectureFeatureFlags.AutoDetect);
				default: throw new NotImplementCompilerException(String.Format("Unknown or unsupported Architecture {0}.", architecture));
			}
		}

		/// <summary>
		/// Gets the boot stage factory.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementCompilerException"></exception>
		private static Func<ICompilerStage> GetBootStageFactory(string format)
		{
			switch (format.ToLower())
			{
				case "multibootHeader-0.7":
				case "multibootheader v0.7":
				case "mb0.7": return delegate { return new Mosa.Platform.x86.Stages.Multiboot0695Stage(); };
				default: throw new NotImplementCompilerException(String.Format("Unknown or unsupported boot format {0}.", format));
			}
		}

		/// <summary>
		/// Gets the linker factory.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		private static Func<BaseLinker> GetLinkerFactory(string format)
		{
			switch (format.ToLower())
			{
				case "pe": return delegate { return new PELinker(); };
				case "pe32": return delegate { return new PELinker(); };
				case "elf": return delegate { return new Elf32(); };
				case "elf32": return delegate { return new Elf32(); };
				//case "elf64": return delegate { return new Elf64Linker(); };
				default: return null;
			}
		}

		private void btnDestination_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				DestinationDirectory = folderBrowserDialog1.SelectedPath;
				lbDestinationDirectory.Text = DestinationDirectory;
			}
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			CompilerAndLaunch();
		}

		private void CompilerAndLaunch()
		{
			if (CheckKeyPressed())
				return;

			richTextBox1.Clear();
			richTextBox2.Clear();
			tabControl1.SelectedTab = tabPage2;

			Compile();

			if (CheckKeyPressed())
				return;

			Launch(cbExitOnLaunch.Checked);

			if (cbExitOnLaunch.Checked)
			{
				Application.Exit();
			}
		}

		protected byte[] GetResource(string name)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var stream = assembly.GetManifestResourceStream("Mosa.Tool.Launcher.Resources." + name);
			var binary = new BinaryReader(stream);
			return binary.ReadBytes((int)stream.Length);
		}

		private void Compile()
		{
			compileStartTime = DateTime.Now;

			compiledFile = Path.Combine(DestinationDirectory, Path.GetFileNameWithoutExtension(SourceFile) + ".bin");

			var compilerOptions = new CompilerOptions();
			compilerOptions.EnableSSA = cbEnableSSA.Checked;
			compilerOptions.EnableSSAOptimizations = cbEnableSSAOptimizations.Checked;
			compilerOptions.OutputFile = compiledFile;

			compilerOptions.Architecture = SelectArchitecture(cbPlatform.SelectedItem.ToString());
			compilerOptions.LinkerFactory = GetLinkerFactory(cbLinkerFormat.SelectedItem.ToString());
			compilerOptions.BootStageFactory = GetBootStageFactory(cbBootFormat.SelectedItem.ToString());

			if (cbGenerateMapFile.Checked)
			{
				compilerOptions.MapFile = Path.Combine(DestinationDirectory, Path.GetFileNameWithoutExtension(SourceFile) + ".map");
			}

			if (!Directory.Exists(DestinationDirectory))
			{
				Directory.CreateDirectory(DestinationDirectory);
			}

			var compilerTrace = new CompilerTrace();
			compilerTrace.CompilerEventListener = this;

			var inputFiles = new List<FileInfo>();
			inputFiles.Add(new FileInfo(SourceFile));

			AotCompiler.Compile(compilerOptions, inputFiles, compilerTrace);

			if (cbImageFormat.SelectedIndex != 3)
			{
				CreateISOImage(compiledFile);
			}
			else
			{
				CreateDiskImage(compiledFile);
			}

			if (cbGenerateASMFile.Checked)
			{
				LaunchNDISASM();
			}
		}

		private void CreateDiskImage(string compiledFile)
		{
			var options = new Options();

			options.MBRCode = GetResource("mbr.bin");
			options.FatBootCode = GetResource("boot.bin");

			options.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource("ldlinux.sys")));
			options.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource("mboot.c32")));
			options.IncludeFiles.Add(new IncludeFile("syslinux.cfg", GetResource("syslinux.cfg")));
			options.IncludeFiles.Add(new IncludeFile(compiledFile, "main.exe"));

			options.VolumeLabel = "MOSABOOT";
			options.PatchSyslinuxOption = true;

			string vmext = ".img";

			switch (cbImageFormat.SelectedIndex)
			{
				case 0: options.ImageFormat = ImageFormatType.IMG; break;
				case 1: options.ImageFormat = ImageFormatType.VHD; vmext = ".vhd"; break;
				case 2: options.ImageFormat = ImageFormatType.VDI; vmext = ".vdi"; break;
				default: break;
			}

			imageFile = Path.Combine(DestinationDirectory, Path.GetFileNameWithoutExtension(SourceFile) + vmext);
			options.DiskImageFileName = imageFile;

			Generator.Create(options);
		}

		private void CreateISOImage(string compiledFile)
		{
			var options = new Utility.IsoImage.Options();

			//options.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource("ldlinux.sys")));
			//options.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource("mboot.c32")));
			//options.IncludeFiles.Add(new IncludeFile("syslinux.cfg", GetResource("syslinux.cfg")));
			//options.IncludeFiles.Add(new IncludeFile(compiledFile, "main.exe"));

			// mboot.c32	isolinux.bin	isolinux.cfg	main.exe

			options.BootInfoTable = true;
			options.BootLoadSize = 4;
			options.VolumeLabel = "MOSABOOT";
			options.BootFileName = "main.exe";

			var iso = new Utility.IsoImage.Iso9660Generator(options);
			iso.Generate();
		}

		private void Launch(bool exit)
		{
			switch (cbEmulator.SelectedIndex)
			{
				case 0: LaunchQemu(exit); break;
				case 1: LaunchBochs(exit); break;
				case 2: LaunchVMwarePlayer(exit); break;
				default: break;
			}
		}

		private static string Quote(string location)
		{
			return '"' + location + '"';
		}

		private string LaunchApplication(string app, string args, bool waitForExit)
		{
			AddOutput("Launching Application: " + app);
			AddOutput("Arguments: " + args);

			ProcessStartInfo start = new ProcessStartInfo();
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

			var output = LaunchApplication(lbNDISASMExecutable.Text, arg, true);

			var asmfile = Path.Combine(DestinationDirectory, Path.GetFileNameWithoutExtension(SourceFile) + ".asm");

			File.WriteAllText(asmfile, output);
		}

		private void LaunchQemu(bool exit)
		{
			string arg =
				"-hda " + Quote(imageFile) +
				" -L " + Quote(lbQEMUBIOSDirectory.Text);

			var output = LaunchApplication(lbQEMUExecutable.Text, arg, !exit);

			AddOutput(output);
		}

		private void LaunchBochs(bool exit)
		{
			var logfile = Path.Combine(DestinationDirectory, Path.GetFileNameWithoutExtension(SourceFile) + "-bochs.log");
			var configfile = Path.Combine(DestinationDirectory, Path.GetFileNameWithoutExtension(SourceFile) + ".bxrc");
			var exeDir = Path.GetDirectoryName(lbBOCHSExecutable.Text);

			var fileVersionInfo = FileVersionInfo.GetVersionInfo(lbBOCHSExecutable.Text);

			// simd or sse
			var simd = "simd";

			if (!(fileVersionInfo.FileMajorPart >= 2 && fileVersionInfo.FileMinorPart >= 6 && fileVersionInfo.FileBuildPart >= 5))
				simd = "sse";

			var sb = new StringBuilder();

			sb.AppendLine("megs: " + nmMemory.Value.ToString());
			sb.AppendLine("ata0: enabled=1,ioaddr1=0x1f0,ioaddr2=0x3f0,irq=14");
			sb.AppendLine("cpuid: mmx=1,sep=1," + simd + "=sse4_2,apic=xapic,aes=1,movbe=1,xsave=1");
			sb.AppendLine("boot: c");
			sb.AppendLine("log: " + Quote(logfile));
			sb.AppendLine("ata0-master: type=disk,path=" + Quote(imageFile) + ",biosdetect=none,cylinders=0,heads=0,spt=0");
			sb.AppendLine("romimage: file=" + Quote(Path.Combine(exeDir, "BIOS-bochs-latest")));
			sb.AppendLine("vgaromimage: file=" + Quote(Path.Combine(exeDir, "VGABIOS-lgpl-latest")));

			//sb.AppendLine("com1: enabled=1, mode=pipe-server, dev=\\.\pipe\MOSA");

			File.WriteAllText(configfile, sb.ToString());

			string arg =
				"-q " +
				"-f " + Quote(configfile);

			var output = LaunchApplication(lbBOCHSExecutable.Text, arg, !exit);

			AddOutput(output);
		}

		private void LaunchVMwarePlayer(bool exit)
		{
			var logfile = Path.Combine(DestinationDirectory, Path.GetFileNameWithoutExtension(SourceFile) + "-vmx.log");
			var configfile = Path.Combine(DestinationDirectory, Path.GetFileNameWithoutExtension(SourceFile) + ".vmx");

			var sb = new StringBuilder();

			sb.AppendLine(".encoding = \"windows-1252\"");
			sb.AppendLine("config.version = \"8\"");
			sb.AppendLine("virtualHW.version = \"4\"");
			sb.AppendLine("memsize = " + Quote(nmMemory.Value.ToString()));
			sb.AppendLine("ide0:0.present = \"FALSE\"");
			sb.AppendLine("ide0:0.fileName = \"auto detect\"");
			sb.AppendLine("ide1:0.present = \"FALSE\"");
			sb.AppendLine("ide1:0.fileName = \"auto detect\"");
			sb.AppendLine("ide1:0.deviceType = \"cdrom-raw\"");
			sb.AppendLine("floppy0.present = \"FALSE\"");
			sb.AppendLine("displayName = \"MOSA - " + Path.GetFileNameWithoutExtension(SourceFile) + "\"");
			sb.AppendLine("guestOS = \"other\"");
			sb.AppendLine("priority.grabbed = \"normal\"");
			sb.AppendLine("priority.ungrabbed = \"normal\"");
			sb.AppendLine("scsi0:0.present = \"TRUE\"");
			sb.AppendLine("scsi0:0.fileName = " + Quote(imageFile));
			sb.AppendLine("scsi0:0.redo = \"\"");

			//sb.AppendLine("ide0:0.fileName = " + Quote(imageFile));
			//sb.AppendLine("ide0:0.deviceType = \"cdrom-image\"");

			//sb.AppendLine("serial0.present = \"TRUE\"");
			//sb.AppendLine("serial0.yieldOnMsrRead = \"FALSE\"");
			//sb.AppendLine("serial0.fileType = \"pipe\"");
			//sb.AppendLine("serial0.fileName = \"\\\\.\\pipe\\MOSA\"");
			//sb.AppendLine("serial0.pipe.endPoint = \"server\"");
			//sb.AppendLine("serial0.tryNoRxLoss = \"FALSE\"");

			File.WriteAllText(configfile, sb.ToString());

			string arg = Quote(configfile);

			var output = LaunchApplication(lbVMwarePlayerExecutable.Text, arg, !exit);

			AddOutput(output);
		}

		private bool CheckKeyPressed()
		{
			return ((Control.ModifierKeys & Keys.Shift) != 0) || ((Control.ModifierKeys & Keys.Control) != 0);
		}
	}
}