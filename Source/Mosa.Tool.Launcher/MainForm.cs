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
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Mosa.Utility.BootImage;
using System.Reflection;

namespace Mosa.Tool.Launcher
{
	public partial class MainForm : Form, ICompilerEventListener
	{
		public string SourceFile { get; set; }

		public string DestinationDirectory { get; set; }

		protected DateTime compileStartTime;

		public MainForm()
		{
			InitializeComponent();
		}

		public void AddOutput(string data)
		{
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

			if (String.IsNullOrWhiteSpace(DestinationDirectory))
			{
				DestinationDirectory = Path.GetDirectoryName(SourceFile) + Path.DirectorySeparatorChar + "build";
				lbDestinationDirectory.Text = DestinationDirectory;
			}
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
			CompileAndLaunch();
		}

		protected byte[] GetResource(string name)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var stream = assembly.GetManifestResourceStream("Mosa.Tool.Launcher.Resources." + name);
			var binary = new BinaryReader(stream);
			return binary.ReadBytes((int)stream.Length);
		}

		private void CompileAndLaunch()
		{
			compileStartTime = DateTime.Now;

			var destinationDirectory = DestinationDirectory + Path.DirectorySeparatorChar;

			var compilerOptions = new CompilerOptions();
			compilerOptions.EnableSSA = cbEnableSSA.Checked;
			compilerOptions.EnableSSAOptimizations = cbEnableSSAOptimizations.Checked;
			compilerOptions.OutputFile = destinationDirectory + Path.GetFileNameWithoutExtension(SourceFile) + ".bin";

			compilerOptions.Architecture = SelectArchitecture(cbPlatform.SelectedItem.ToString());
			compilerOptions.LinkerFactory = GetLinkerFactory(cbLinkerFormat.SelectedItem.ToString());
			compilerOptions.BootStageFactory = GetBootStageFactory(cbBootFormat.SelectedItem.ToString());

			if (cbGenerateMapFile.Checked)
			{
				compilerOptions.MapFile = destinationDirectory + Path.GetFileNameWithoutExtension(SourceFile) + ".map";
			}

			if (!Directory.Exists(destinationDirectory))
			{
				Directory.CreateDirectory(destinationDirectory);
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

			options.DiskImageFileName = destinationDirectory + "bootimage.img";

			Generator.Create(options);
		}

	}
}