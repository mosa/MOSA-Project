/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Utility.Launcher;

//using Mosa.Utility.BootImage;
using System;
using System.IO;
using System.Windows.Forms;

namespace Mosa.Tool.Launcher
{
	public partial class MainForm : Form, IBuilderEvent
	{
		public Builder Builder { get; private set; }

		public Options Options { get; private set; }

		public AppLocations AppLocations { get; set; }

		public MainForm()
		{
			InitializeComponent();

			Options = new Options();
			AppLocations = new AppLocations();

			AppLocations.FindApplications();

			Builder = new Builder(Options, AppLocations, this);
		}

		private void UpdateeInterfaceAppLocations()
		{
			lbBOCHSExecutable.Text = AppLocations.BOCHS;
			lbNDISASMExecutable.Text = AppLocations.NDISASM;
			lbQEMUExecutable.Text = AppLocations.QEMU;
			lbQEMUBIOSDirectory.Text = AppLocations.QEMUBIOSDirectory;
			lbQEMUImgApplication.Text = AppLocations.QEMUImg;
			lbVMwarePlayerExecutable.Text = AppLocations.VMwarePlayer;
			lbmkisofsExecutable.Text = AppLocations.mkisofs;
		}

		private void UpdateBuilderOptions()
		{
			//Builder.Options.AutoLaunch = cbExitOnLaunch;
			Options.EnableSSA = cbEnableSSA.Checked;
			Options.EnableIROptimizations = cbEnableIROptimizations.Checked;
			Options.EnableSparseConditionalConstantPropagation = cbEnableSparseConditionalConstantPropagation.Checked;
			Options.GenerateASMFile = cbGenerateASMFile.Checked;
			Options.GenerateMapFile = cbGenerateMapFile.Checked;
			Options.ExitOnLaunch = cbExitOnLaunch.Checked;
			Options.MOSADebugger = cbMOSADebugger.Checked;
			Options.MemoryInMB = (uint)nmMemory.Value;

			switch (cbImageFormat.SelectedIndex)
			{
				case 0: Options.ImageFormat = ImageFormat.IMG; break;
				case 1: Options.ImageFormat = ImageFormat.VHD; break;
				case 2: Options.ImageFormat = ImageFormat.VDI; break;
				case 3: Options.ImageFormat = ImageFormat.ISO; break;
				case 4: Options.ImageFormat = ImageFormat.VMDK; break;
				default: break;
			}

			switch (cbEmulator.SelectedIndex)
			{
				case 0: Options.Emulator = EmulatorType.Qemu; break;
				case 1: Options.Emulator = EmulatorType.Boches; break;
				case 2: Options.Emulator = EmulatorType.WMware; break;
				default: break;
			}

			lbSource.Text = Path.GetFileName(Options.SourceFile);
			lbSourceDirectory.Text = Path.GetDirectoryName(Options.SourceFile);

			switch (cbBootFormat.SelectedIndex)
			{
				case 0: Options.BootFormat = BootFormat.Multiboot_0_7; break;
				default: Options.BootFormat = BootFormat.NotSpecified; break;
			}

			switch (cbBootFileSystem.SelectedIndex)
			{
				case 0: Options.FileSystemFormat = FileSystemFormat.FAT16; ; break;
				case 1: Options.FileSystemFormat = FileSystemFormat.FAT24; ; break;
				default: Options.FileSystemFormat = FileSystemFormat.FAT16; ; break;
			}

			switch (cbLinkerFormat.SelectedIndex)
			{
				case 0: Options.LinkerFormat = LinkerFormat.Elf32; break;
				case 1: Options.LinkerFormat = LinkerFormat.PE32; break;
				case 2: Options.LinkerFormat = LinkerFormat.Elf64; break;
				default: break;
			}

			switch (cbPlatform.SelectedIndex)
			{
				case 0: Options.PlatformType = PlatformType.X86; break;
				default: break;
			}

			switch (cbPlatform.SelectedIndex)
			{
				case 0: Options.BootFormat = BootFormat.Multiboot_0_7; break;
				default: Options.BootFormat = BootFormat.NotSpecified; break;
			}
		}

		private void UpdateInterfaceOptions()
		{
			//Builder.Options.AutoLaunch = cbExitOnLaunch;
			cbEnableSSA.Checked = Options.EnableSSA;
			cbEnableIROptimizations.Checked = Options.EnableIROptimizations;
			cbEnableSparseConditionalConstantPropagation.Checked = Options.EnableSparseConditionalConstantPropagation;
			cbGenerateASMFile.Checked = Options.GenerateASMFile;
			cbGenerateMapFile.Checked = Options.GenerateMapFile;
			cbExitOnLaunch.Checked = Options.ExitOnLaunch;
			cbMOSADebugger.Checked = Options.MOSADebugger;
			nmMemory.Value = Options.MemoryInMB;

			switch (Options.ImageFormat)
			{
				case ImageFormat.IMG: cbImageFormat.SelectedIndex = 0; break;
				case ImageFormat.VHD: cbImageFormat.SelectedIndex = 1; break;
				case ImageFormat.VDI: cbImageFormat.SelectedIndex = 2; break;
				case ImageFormat.ISO: cbImageFormat.SelectedIndex = 3; break;
				case ImageFormat.VMDK: cbImageFormat.SelectedIndex = 4; break;
				default: break;
			}

			switch (Options.Emulator)
			{
				case EmulatorType.Qemu: cbEmulator.SelectedIndex = 0; break;
				case EmulatorType.Boches: cbEmulator.SelectedIndex = 1; break;
				case EmulatorType.WMware: cbEmulator.SelectedIndex = 2; break;
				default: break;
			}

			switch (Options.FileSystemFormat)
			{
				case FileSystemFormat.FAT16: cbBootFileSystem.SelectedIndex = 0; break;
				case FileSystemFormat.FAT24: cbBootFileSystem.SelectedIndex = 1; break;
				default: break;
			}

			switch (Options.LinkerFormat)
			{
				case LinkerFormat.Elf32: cbLinkerFormat.SelectedIndex = 0; break;
				case LinkerFormat.PE32: cbLinkerFormat.SelectedIndex = 1; break;
				case LinkerFormat.Elf64: cbLinkerFormat.SelectedIndex = 0; break; // Not supported yet
				default: break;
			}

			switch (Options.PlatformType)
			{
				case PlatformType.X86: cbPlatform.SelectedIndex = 0; break;
				default: break;
			}

			switch (Options.BootFormat)
			{
				case BootFormat.Multiboot_0_7: cbBootFormat.SelectedIndex = 0; break;
				default: cbBootFormat.SelectedIndex = 0; break;
			}

			lbDestinationDirectory.Text = Options.DestinationDirectory;
			lbSource.Text = Options.SourceFile;
			lbSourceDirectory.Text = Path.GetDirectoryName(Options.SourceFile);
		}

		void IBuilderEvent.NewStatus(string info)
		{
			AddOutput(info);
		}

		void IBuilderEvent.UpdateProgress(int total, int at)
		{
			progressBar1.Maximum = total;
			progressBar1.Value = at;

			//progressBar1.Refresh();
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			UpdateInterfaceOptions();
			UpdateeInterfaceAppLocations();

			this.Refresh();

			if (Options.AutoLaunch)
				CompilerAndLaunch();
		}

		public void AddOutput(string data)
		{
			if (data == null)
				return;

			rtbOutput.AppendText(data);
			rtbOutput.AppendText("\n");
			rtbOutput.Update();
		}

		public void AddCounters(string data)
		{
			rtbCounters.AppendText(data);
			rtbCounters.AppendText("\n");
			rtbCounters.Update();
		}

		private void btnSource_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Options.SourceFile = openFileDialog1.FileName;

				// UpdateBuilderOptions();
				lbSource.Text = Path.GetFileName(Options.SourceFile);
				lbSourceDirectory.Text = Path.GetDirectoryName(Options.SourceFile);
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			tabControl1.SelectedTab = tbOptions;
		}

		private void btnDestination_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Options.DestinationDirectory = folderBrowserDialog1.SelectedPath;

				// UpdateBuilderOptions();
				lbDestinationDirectory.Text = Options.DestinationDirectory;
			}
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			UpdateBuilderOptions();

			CompilerAndLaunch();
		}

		private void CompilerAndLaunch()
		{
			rtbOutput.Clear();
			rtbCounters.Clear();

			if (CheckKeyPressed())
				return;

			tabControl1.SelectedTab = tpOutput;

			Builder.Compile();

			foreach (var line in Builder.Counters)
			{
				AddCounters(line);
			}

			if (CheckKeyPressed())
				return;

			Builder.Launch();

			if (Options.ExitOnLaunch)
			{
				Application.Exit();
			}
		}

		private bool CheckKeyPressed()
		{
			return ((Control.ModifierKeys & Keys.Shift) != 0) || ((Control.ModifierKeys & Keys.Control) != 0);
		}
	}
}