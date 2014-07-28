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

namespace Mosa.Tool.Launcher
{
	public partial class MainForm : Form
	{
		public string SourceFile { get; set; }

		public string DestinationDirectory { get; set; }

		public MainForm()
		{
			InitializeComponent();
		}

		private void btnSource_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				SourceFile = openFileDialog1.FileName;
				lbSource.Text = Path.GetFileName(SourceFile);
				lbSourceDirectory.Text = Path.GetDirectoryName(SourceFile);

				if (String.IsNullOrWhiteSpace(DestinationDirectory))
				{
					DestinationDirectory = Path.GetDirectoryName(SourceFile) + Path.DirectorySeparatorChar + "build";
					lbDestinationDirectory.Text = DestinationDirectory;
				}
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			cbPlatform.SelectedIndex = 0;
			cbImageFormat.SelectedIndex = 0;
			cbLinkerFormat.SelectedIndex = 0;
			cbEmulator.SelectedIndex = 0;
			cbBootFormat.SelectedIndex = 0;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var compilerOptions = new CompilerOptions();
			compilerOptions.EnableSSA = cbEnableSSA.Checked;
			compilerOptions.EnableSSAOptimizations = cbEnableSSAOptimizations.Checked;
			compilerOptions.OutputFile = this.DestinationDirectory + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(SourceFile) + ".bin";

			compilerOptions.Architecture = SelectArchitecture(cbPlatform.SelectedItem.ToString());
			compilerOptions.LinkerFactory = GetLinkerFactory(cbLinkerFormat.SelectedItem.ToString());
			compilerOptions.BootStageFactory = GetBootStageFactory(cbBootFormat.SelectedItem.ToString());

			CompilerTrace compilerTrace = new CompilerTrace();

			var inputFiles = new List<FileInfo>();
			inputFiles.Add(new FileInfo(SourceFile));

			AotCompiler.Compile(compilerOptions, inputFiles, compilerTrace);
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
	}
}