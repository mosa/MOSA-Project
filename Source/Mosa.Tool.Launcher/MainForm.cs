// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.Launcher;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Mosa.Tool.Launcher
{
	public partial class MainForm : Form, IBuilderEvent
	{
		public Builder Builder { get; private set; }

		public Options Options { get; private set; }

		public AppLocations AppLocations { get; set; }

		public string ConfigFile
		{
			get
			{
				return Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, ".config.xml");
			}
		}

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
			Options.EnableSSA = cbEnableSSA.Checked;
			Options.EnableIROptimizations = cbEnableIROptimizations.Checked;
			Options.EnableSparseConditionalConstantPropagation = cbEnableSparseConditionalConstantPropagation.Checked;
			Options.GenerateASMFile = cbGenerateASMFile.Checked;
			Options.GenerateMapFile = cbGenerateMapFile.Checked;
			Options.ExitOnLaunch = cbExitOnLaunch.Checked;
			Options.MOSADebugger = cbMOSADebugger.Checked;
			Options.CompilerUsesMultipleThreads = cbCompilerUsesMultipleThreads.Checked;
			Options.MemoryInMB = (uint)nmMemory.Value;
			Options.EnableInlinedMethods = cbInlinedMethods.Checked;

			switch (cbImageFormat.SelectedIndex)
			{
				case 0: Options.ImageFormat = ImageFormat.IMG; break;
				case 1: Options.ImageFormat = ImageFormat.ISO; break;
				case 2: Options.ImageFormat = ImageFormat.VHD; break;
				case 3: Options.ImageFormat = ImageFormat.VDI; break;
				case 4: Options.ImageFormat = ImageFormat.VMDK; break;
				default: break;
			}

			switch (cbEmulator.SelectedIndex)
			{
				case 0: Options.Emulator = EmulatorType.Qemu; break;
				case 1: Options.Emulator = EmulatorType.Bochs; break;
				case 2: Options.Emulator = EmulatorType.VMware; break;
				default: break;
			}

			switch (cbDebugConnectionOption.SelectedIndex)
			{
				case 0: Options.DebugConnectionOption = DebugConnectionOption.None; break;
				case 1: Options.DebugConnectionOption = DebugConnectionOption.Pipe; break;
				case 2: Options.DebugConnectionOption = DebugConnectionOption.TCPServer; break;
				case 3: Options.DebugConnectionOption = DebugConnectionOption.TCPClient; break;
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
				case 0: Options.FileSystemFormat = FileSystemFormat.FAT12; ; break;
				case 1: Options.FileSystemFormat = FileSystemFormat.FAT16; ; break;
				default: Options.FileSystemFormat = FileSystemFormat.FAT16; ; break;
			}

			switch (cbLinkerFormat.SelectedIndex)
			{
				case 0: Options.LinkerFormat = LinkerFormat.Elf32; break;
				case 1: Options.LinkerFormat = LinkerFormat.Elf64; break;
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
			cbEnableSSA.Checked = Options.EnableSSA;
			cbEnableIROptimizations.Checked = Options.EnableIROptimizations;
			cbEnableSparseConditionalConstantPropagation.Checked = Options.EnableSparseConditionalConstantPropagation;
			cbGenerateASMFile.Checked = Options.GenerateASMFile;
			cbGenerateMapFile.Checked = Options.GenerateMapFile;
			cbExitOnLaunch.Checked = Options.ExitOnLaunch;
			cbMOSADebugger.Checked = Options.MOSADebugger;
			cbInlinedMethods.Checked = Options.EnableInlinedMethods;
			cbCompilerUsesMultipleThreads.Checked = Options.CompilerUsesMultipleThreads;
			nmMemory.Value = Options.MemoryInMB;

			switch (Options.ImageFormat)
			{
				case ImageFormat.IMG: cbImageFormat.SelectedIndex = 0; break;
				case ImageFormat.ISO: cbImageFormat.SelectedIndex = 1; break;
				case ImageFormat.VHD: cbImageFormat.SelectedIndex = 2; break;
				case ImageFormat.VDI: cbImageFormat.SelectedIndex = 3; break;
				case ImageFormat.VMDK: cbImageFormat.SelectedIndex = 4; break;
				default: break;
			}

			switch (Options.Emulator)
			{
				case EmulatorType.Qemu: cbEmulator.SelectedIndex = 0; break;
				case EmulatorType.Bochs: cbEmulator.SelectedIndex = 1; break;
				case EmulatorType.VMware: cbEmulator.SelectedIndex = 2; break;
				default: break;
			}

			switch (Options.FileSystemFormat)
			{
				case FileSystemFormat.FAT12: cbBootFileSystem.SelectedIndex = 0; break;
				case FileSystemFormat.FAT16: cbBootFileSystem.SelectedIndex = 1; break;
				default: break;
			}

			switch (Options.LinkerFormat)
			{
				case LinkerFormat.Elf32: cbLinkerFormat.SelectedIndex = 0; break;
				case LinkerFormat.Elf64: cbLinkerFormat.SelectedIndex = 1; break; // Not supported yet
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

			switch (Options.DebugConnectionOption)
			{
				case DebugConnectionOption.None: cbDebugConnectionOption.SelectedIndex = 0; break;
				case DebugConnectionOption.Pipe: cbDebugConnectionOption.SelectedIndex = 1; break;
				case DebugConnectionOption.TCPServer: cbDebugConnectionOption.SelectedIndex = 2; break;
				case DebugConnectionOption.TCPClient: cbDebugConnectionOption.SelectedIndex = 3; break;
				default: break;
			}

			lbDestinationDirectory.Text = Options.DestinationDirectory;
			lbSource.Text = Options.SourceFile;
			lbSourceDirectory.Text = Path.GetDirectoryName(Options.SourceFile);
		}

		public void UpdateStatusLabel(string msg)
		{
			tsStatusLabel.Text = msg;
		}

		private void NewStatus(string info)
		{
			AddOutput(info);
		}

		void IBuilderEvent.NewStatus(string status)
		{
			MethodInvoker method = delegate ()
			{
				NewStatus(status);
			};

			Invoke(method);
		}

		private void UpdateProgress(int total, int at)
		{
			progressBar1.Maximum = total;
			progressBar1.Value = at;
		}

		void IBuilderEvent.UpdateProgress(int total, int at)
		{
			MethodInvoker method = delegate ()
			{
				UpdateProgress(total, at);
			};

			Invoke(method);
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			UpdateInterfaceOptions();
			UpdateeInterfaceAppLocations();

			this.Refresh();

			if (Options.AutoLaunch)
				CompileAndLaunch();
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

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.F5)
				CompileAndLaunch();
			if (keyData == Keys.F6)
				Builder.Launch();

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void CompileAndLaunch()
		{
			//Options.SaveFile(ConfigFile);
			rtbOutput.Clear();
			rtbCounters.Clear();

			if (CheckKeyPressed())
				return;

			tabControl1.SelectedTab = tpOutput;

			ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
				{
					try
					{
						Builder.Compile();
					}
					catch (Exception e)
					{
						OnException(e.ToString());
					}
					finally
					{
						if (!Builder.HasCompileError)
							OnCompileCompleted();
					}
				}
			));
		}

		private void OnException(string data)
		{
			MethodInvoker method = delegate ()
			{
				AddOutput(data);
			};

			Invoke(method);
		}

		private void OnCompileCompleted()
		{
			MethodInvoker method = delegate ()
			{
				CompileCompleted();
			};

			Invoke(method);
		}

		private void CompileCompleted()
		{
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

		private void button1_Click(object sender, EventArgs e)
		{
			UpdateBuilderOptions();

			var result = CheckOptions.Verify(Options);

			if (result == null)
			{
				CompileAndLaunch();
			}
			else
			{
				UpdateStatusLabel("ERROR: " + result);
				AddOutput(result);
			}
		}
	}
}
