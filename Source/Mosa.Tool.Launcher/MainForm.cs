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
using System.Windows.Forms;

namespace Mosa.Tool.Launcher
{
	public partial class MainForm : Form, ICompilerEventListener
	{
		public string SourceFile { get; set; }
		public string DestinationDirectory { get; set; }

		protected DateTime compileStartTime;
		protected string compiledFile;
		protected string imageFile;

		public MainForm()
		{
			InitializeComponent();
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
				SetSource(openFileDialog1.FileName);
			}
		}

		public void SetSource(string filename)
		{
			SourceFile = filename;
			lbSource.Text = Path.GetFileName(SourceFile);
			lbSourceDirectory.Text = Path.GetDirectoryName(SourceFile);
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
					@"%MOSA%\Tools\QEMU\",
					@"%MOSA%\QEMU\",
					@"..\Tools\QEMU\",
					@"\Tools\QEMU\",
					@"%ProgramFiles%\qemu\",
					@"%ProgramFiles(x86)%\qemu\",
					//@"C:\Program Files (x86)\qemu\",
					//@"C:\Program Files\qemu\"
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
					@"%MOSA%\Tools\ndisasm\",
					@"%MOSA%\ndisasm\",
					@"..\Tools\ndisasm\",
					@"\Tools\ndisasm\"
				}
			);

			lbBOCHSExecutable.Text = TryFind(
				"bochs.exe",
				new string[] {
					@"%MOSA%\Tools\Bochs\",
					@"%MOSA%\Bochs\",
					@"..\Tools\Bochs\",
					@"\Tools\Bochs\",
					@"%ProgramFiles%\Bochs-2.6.5\",
					@"%ProgramFiles(x86)%\Bochs-2.6.5\",
					//@"C:\Program Files (x86)\Bochs-2.6.5\",
					//@"C:\Program Files\Bochs-2.6.5\",
					@"%ProgramFiles%\Bochs-2.6.2\",
					@"%ProgramFiles(x86)%\Bochs-2.6.2\",
					//@"C:\Program Files (x86)\Bochs-2.6.2\",
					//@"C:\Program Files\Bochs-2.6.2\"
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
			string combine = Path.Combine(directory, file);

			if (File.Exists(combine))
			{
				location = combine;
				return true;
			}

			location = string.Empty;

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
			richTextBox1.Clear();
			richTextBox2.Clear();
			tabControl1.SelectedTab = tabPage2;
			Compile();

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

			CompilerTrace compilerTrace = new CompilerTrace();
			compilerTrace.CompilerEventListener = this;

			var inputFiles = new List<FileInfo>();
			inputFiles.Add(new FileInfo(SourceFile));

			AotCompiler.Compile(compilerOptions, inputFiles, compilerTrace);

			Options options = new Options();

			options.MBRCode = GetResource("mbr.bin");
			options.FatBootCode = GetResource("boot.bin");

			options.IncludeFiles.Add(new IncludeFile("ldlinux.sys", GetResource("ldlinux.sys")));
			options.IncludeFiles.Add(new IncludeFile("mboot.c32", GetResource("mboot.c32")));
			options.IncludeFiles.Add(new IncludeFile("syslinux.cfg", GetResource("syslinux.cfg")));
			options.IncludeFiles.Add(new IncludeFile(compilerOptions.OutputFile, "main.exe"));

			options.VolumeLabel = "MOSABOOT";
			options.PatchSyslinuxOption = true;

			switch (cbImageFormat.SelectedIndex)
			{
				case 0: options.ImageFormat = ImageFormatType.IMG; break;
				case 1: options.ImageFormat = ImageFormatType.VHD; break;
				case 2: options.ImageFormat = ImageFormatType.VDI; break;
				default: break;
			}

			imageFile = Path.Combine(DestinationDirectory, Path.GetFileNameWithoutExtension(SourceFile) + ".img");
			options.DiskImageFileName = imageFile;

			Generator.Create(options);

			if (cbGenerateASMFile.Checked)
			{
				LaunchNDISASM();
			}
		}

		private void Launch(bool exit)
		{
			if (cbEmulator.SelectedIndex == 0)
				LaunchQemu(exit);
			else if (cbEmulator.SelectedIndex == 1)
				LaunchBochs(exit);
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
			var output = process.StandardOutput.ReadToEnd();

			if (waitForExit)
			{
				process.WaitForExit();
			}

			//return output;
			var error = process.StandardError.ReadToEnd();

			return output + error;
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
			var exeDir = Path.GetDirectoryName(lbBOCHSExecutable.Text);

			string arg =
				//"-q" +
				"-n" +
				//" -noconsole" +
				" megs:64" +
				" ata0:enabled=1,ioaddr1=0x1f0,ioaddr2=0x3f0,irq=14" +
				" cpuid:mmx=1,sep=1,sse=sse4_2,apic=xapic,aes=1,movbe=1,xsave=1" +
				" boot:c" +	
				//" config_interface:win32config" +
				//" display_library:win32" +
				" log:" + Quote(logfile) +
				" ata0-master:type=disk,path=" + Quote(imageFile) + ",biosdetect=none,cylinders=0,heads=0,spt=0" +
				" romimage:file=" + Quote(Path.Combine(exeDir, "BIOS-bochs-latest")) +
				" vgaromimage:file=" + Quote(Path.Combine(exeDir, "VGABIOS-lgpl-latest"));

			var output = LaunchApplication(lbBOCHSExecutable.Text, arg, !exit);

			AddOutput(output);
		}
	}
}