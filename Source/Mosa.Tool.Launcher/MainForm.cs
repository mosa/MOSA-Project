// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using MetroFramework.Forms;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Utility.BootImage;
using Mosa.Utility.Launcher;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Mosa.Tool.Launcher
{
	public partial class MainForm : MetroForm, IBuilderEvent, IStarterEvent
	{
		public Builder Builder { get; }

		public Starter Starter { get; private set; }

		public LauncherOptions Options { get; }

		public AppLocations AppLocations { get; set; }

		private int TotalMethods = 0;
		private int CompletedMethods = 0;

		public string ConfigFile
		{
			get
			{
				return Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, ".config.xml");
			}
		}

		private readonly BindingList<IncludedEntry> includedEntries = new BindingList<IncludedEntry>();

		private class IncludedEntry
		{
			[Browsable(false)]
			public IncludeFile IncludeFile { get; }

			public int Size { get { return IncludeFile.Length; } }
			public string Destination { get { return IncludeFile.Filename; } }
			public string Source { get { return IncludeFile.SourceFileName; } }

			public IncludedEntry(IncludeFile file)
			{
				IncludeFile = file;
			}
		}

		public MainForm()
		{
			InitializeComponent();

			Options = new LauncherOptions();
			AppLocations = new AppLocations();

			AppLocations.FindApplications();

			Builder = new Builder(Options, AppLocations, this);

			dataGridView1.DataSource = includedEntries;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[1].Width = 175;
			dataGridView1.Columns[2].Width = 500;

			AddOutput("Current Directory: " + Environment.CurrentDirectory);
		}

		private void UpdateInterfaceAppLocations()
		{
			lbBOCHSExecutable.Text = AppLocations.BOCHS;
			lbNDISASMExecutable.Text = AppLocations.NDISASM;
			lbQEMUExecutable.Text = AppLocations.QEMU;
			lbQEMUBIOSDirectory.Text = AppLocations.QEMUBIOSDirectory;
			lbQEMUImgApplication.Text = AppLocations.QEMUImg;
			lbVMwarePlayerExecutable.Text = AppLocations.VMwarePlayer;
			lbmkisofsExecutable.Text = AppLocations.Mkisofs;
		}

		private void UpdateBuilderOptions()
		{
			Options.EnableSSA = cbEnableSSA.Checked;
			Options.EnableIROptimizations = cbEnableIROptimizations.Checked;
			Options.EnableSparseConditionalConstantPropagation = cbEnableSparseConditionalConstantPropagation.Checked;
			Options.GenerateNASMFile = cbGenerateNASMFile.Checked;
			Options.GenerateASMFile = cbGenerateASMFile.Checked;
			Options.GenerateMapFile = cbGenerateMapFile.Checked;
			Options.GenerateDebugFile = cbGenerateDebugInfoFile.Checked;
			Options.ExitOnLaunch = cbExitOnLaunch.Checked;
			Options.EnableQemuGDB = cbEnableQemuGDB.Checked;
			Options.LaunchGDB = cbLaunchGDB.Checked;
			Options.LaunchGDBDebugger = cbLaunchMosaDebugger.Checked;
			Options.EnableMultiThreading = cbCompilerUsesMultipleThreads.Checked;
			Options.EmulatorMemoryInMB = (uint)nmMemory.Value;
			Options.EnableInlinedMethods = cbInlinedMethods.Checked;
			Options.VBEVideo = cbVBEVideo.Checked;
			Options.EnableIRLongExpansion = cbIRLongExpansion.Checked;
			Options.TwoPassOptimizations = cbTwoPassOptimizations.Checked;
			Options.EnableIRLongExpansion = cbIRLongExpansion.Checked;
			Options.EnableValueNumbering = cbValueNumbering.Checked;
			Options.GenerateDebugFile = cbGenerateDebugInfoFile.Checked;
			Options.BaseAddress = tbBaseAddress.Text.ParseHexOrInteger();
			Options.EmitAllSymbols = cbEmitSymbolTable.Checked;
			Options.EmitStaticRelocations = cbRelocationTable.Checked;
			Options.EnableMethodScanner = cbEnableMethodScanner.Checked;
			Options.GenerateCompileTimeFile = cbGenerateCompilerTime.Checked;
			Options.EnableBitTracker = cbBitTracker.Checked;

			if (Options.VBEVideo)
			{
				var Mode = tbMode.Text.Split('x');

				if (Mode.Length == 3)
				{
					try
					{
						int ModeWidth = int.Parse(Mode[0]); //Get Mode Width
						int ModeHeight = int.Parse(Mode[1]); //Get Mode Height
						int ModeDepth = int.Parse(Mode[2]); //Get Mode Depth

						Options.Width = ModeWidth;
						Options.Height = ModeHeight;
						Options.Depth = ModeDepth;
					}
					catch (Exception e)
					{
						throw new Exception("An error occurred while parsing VBE Mode: " + e.Message);
					}
				}
				else
				{
					throw new Exception("An error occurred while parsing VBE Mode: There wasn't 3 arguments");
				}
			}

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
				case 0: Options.SerialConnectionOption = SerialConnectionOption.None; break;
				case 1: Options.SerialConnectionOption = SerialConnectionOption.Pipe; break;
				case 2: Options.SerialConnectionOption = SerialConnectionOption.TCPServer; break;
				case 3: Options.SerialConnectionOption = SerialConnectionOption.TCPClient; break;
				default: break;
			}

			switch (cbBootFormat.SelectedIndex)
			{
				case 0: Options.MultibootSpecification = MultibootSpecification.V1; break;
				case 1: Options.MultibootSpecification = MultibootSpecification.V2; break;
				default: Options.MultibootSpecification = MultibootSpecification.None; break;
			}

			switch (cbBootFileSystem.SelectedIndex)
			{
				case 0: Options.FileSystem = FileSystem.FAT12; break;
				case 1: Options.FileSystem = FileSystem.FAT16; break;
				default: break;
			}

			switch (cbPlatform.SelectedIndex)
			{
				case 0: Options.PlatformType = PlatformType.x86; break;
				case 1: Options.PlatformType = PlatformType.x64; break;
				default: Options.PlatformType = PlatformType.NotSpecified; break;
			}

			switch (cbBootLoader.SelectedIndex)
			{
				case 0: Options.BootLoader = BootLoader.Syslinux_3_72; break;
				case 1: Options.BootLoader = BootLoader.Syslinux_6_03; break;
				case 2: Options.BootLoader = BootLoader.Grub_0_97; break;
				case 3: Options.BootLoader = BootLoader.Grub_2_00; break;
				default: break;
			}

			Options.IncludeFiles.Clear();

			foreach (var entry in includedEntries)
			{
				Options.IncludeFiles.Add(entry.IncludeFile);
			}

			//lbSource.Text = Path.GetFileName(Options.SourceFile);
			//lbSourceDirectory.Text = Path.GetDirectoryName(Options.SourceFile);
		}

		private void UpdateInterfaceOptions()
		{
			cbEnableSSA.Checked = Options.EnableSSA;
			cbEnableIROptimizations.Checked = Options.EnableIROptimizations;
			cbEnableSparseConditionalConstantPropagation.Checked = Options.EnableSparseConditionalConstantPropagation;
			cbGenerateNASMFile.Checked = Options.GenerateNASMFile;
			cbGenerateASMFile.Checked = Options.GenerateASMFile;
			cbGenerateMapFile.Checked = Options.GenerateMapFile;
			cbGenerateDebugInfoFile.Checked = Options.GenerateDebugFile;
			cbExitOnLaunch.Checked = Options.ExitOnLaunch;
			cbEnableQemuGDB.Checked = Options.EnableQemuGDB;
			cbLaunchGDB.Checked = Options.LaunchGDB;
			cbLaunchMosaDebugger.Checked = Options.LaunchGDBDebugger;
			cbInlinedMethods.Checked = Options.EnableInlinedMethods;
			cbCompilerUsesMultipleThreads.Checked = Options.EnableMultiThreading;
			nmMemory.Value = Options.EmulatorMemoryInMB;
			cbVBEVideo.Checked = Options.VBEVideo;
			tbBaseAddress.Text = "0x" + Options.BaseAddress.ToString("x8");
			cbRelocationTable.Checked = Options.EmitStaticRelocations;
			cbEmitSymbolTable.Checked = Options.EmitAllSymbols;
			tbMode.Text = Options.Width + "x" + Options.Height + "x" + Options.Depth;
			cbIRLongExpansion.Checked = Options.EnableIRLongExpansion;
			cbTwoPassOptimizations.Checked = Options.TwoPassOptimizations;
			cbValueNumbering.Checked = Options.EnableValueNumbering;
			cbEnableMethodScanner.Checked = Options.EnableMethodScanner;
			cbGenerateCompilerTime.Checked = Options.GenerateCompileTimeFile;
			cbBitTracker.Checked = Options.EnableBitTracker;

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

			switch (Options.FileSystem)
			{
				case FileSystem.FAT12: cbBootFileSystem.SelectedIndex = 0; break;
				case FileSystem.FAT16: cbBootFileSystem.SelectedIndex = 1; break;
				default: break;
			}

			switch (Options.BootLoader)
			{
				case BootLoader.Syslinux_3_72: cbBootLoader.SelectedIndex = 0; break;
				case BootLoader.Syslinux_6_03: cbBootLoader.SelectedIndex = 1; break;
				case BootLoader.Grub_0_97: cbBootLoader.SelectedIndex = 2; break;
				case BootLoader.Grub_2_00: cbBootLoader.SelectedIndex = 3; break;
				default: break;
			}

			switch (Options.PlatformType)
			{
				case PlatformType.x86: cbPlatform.SelectedIndex = 0; break;
				case PlatformType.x64: cbPlatform.SelectedIndex = 1; break;
				default: cbPlatform.SelectedIndex = 0; break;
			}

			switch (Options.MultibootSpecification)
			{
				case MultibootSpecification.V1: cbBootFormat.SelectedIndex = 0; break;

				//case MultibootSpecification.V2: cbBootFormat.SelectedIndex = 1; break;
				default: cbBootFormat.SelectedIndex = 0; break;
			}

			switch (Options.SerialConnectionOption)
			{
				case SerialConnectionOption.None: cbDebugConnectionOption.SelectedIndex = 0; break;
				case SerialConnectionOption.Pipe: cbDebugConnectionOption.SelectedIndex = 1; break;
				case SerialConnectionOption.TCPServer: cbDebugConnectionOption.SelectedIndex = 2; break;
				case SerialConnectionOption.TCPClient: cbDebugConnectionOption.SelectedIndex = 3; break;
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
			Invoke((MethodInvoker)(() => NewStatus(status)));
		}

		void IStarterEvent.NewStatus(string status)
		{
			Invoke((MethodInvoker)(() => NewStatus(status)));
		}

		private void UpdateProgress()
		{
			progressBar1.Maximum = TotalMethods;
			progressBar1.Value = CompletedMethods;
		}

		void IBuilderEvent.UpdateProgress(int total, int at)
		{
			TotalMethods = total;
			CompletedMethods = at;
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			UpdateInterfaceOptions();
			UpdateInterfaceAppLocations();

			Refresh();

			if (Options.AutoStart)
			{
				CompileBuildAndStart();
			}
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

		private void BtnSource_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				var filename = openFileDialog1.FileName;

				Options.SourceFile = filename;
				Options.Paths.Clear();
				Options.Paths.Add(Path.GetDirectoryName(filename));

				lbSource.Text = Path.GetFileName(Options.SourceFile);
				lbSourceDirectory.Text = Path.GetDirectoryName(Options.SourceFile);
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Text = "MOSA Launcher v" + CompilerVersion.Version;
			tbApplicationLocations.SelectedTab = tabOptions;

			foreach (var includeFile in Options.IncludeFiles)
			{
				includedEntries.Add(new IncludedEntry(includeFile));
			}
		}

		private void BtnDestination_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				Options.DestinationDirectory = folderBrowserDialog1.SelectedPath;

				// UpdateBuilderOptions();
				lbDestinationDirectory.Text = Options.DestinationDirectory;
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.F5)
				CompileBuildAndStart();

			//if (keyData == Keys.F6)
			//	Builder.Launch();

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void CompileBuildAndStart()
		{
			//Options.SaveFile(ConfigFile);
			rtbOutput.Clear();
			rtbCounters.Clear();

			if (CheckKeyPressed())
				return;

			tbApplicationLocations.SelectedTab = tabOutput;

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
					{
						OnCompileCompleted();
					}
				}
			}));
		}

		private void OnException(string data)
		{
			MethodInvoker method = () => AddOutput(data);

			Invoke(method);
		}

		private void OnCompileCompleted()
		{
			MethodInvoker method = CompileCompleted;

			Invoke(method);
		}

		private void CompileCompleted()
		{
			if (Builder.LauncherOptions.LaunchVM)
			{
				foreach (var line in Builder.Counters)
				{
					AddCounters(line);
				}

				if (CheckKeyPressed())
					return;

				Options.ImageFile = Options.BootLoaderImage ?? Builder.ImageFile;
				Starter = new Starter(Options, AppLocations, this, Builder.Linker);

				Starter.Launch();
			}

			if (Options.ExitOnLaunch)
			{
				Application.Exit();
			}
		}

		private bool CheckKeyPressed()
		{
			return ((ModifierKeys & Keys.Shift) != 0) || ((ModifierKeys & Keys.Control) != 0);
		}

		private void Btn1_Click(object sender, EventArgs e)
		{
			UpdateBuilderOptions();

			var result = CheckOptions.Verify(Options);

			if (result == null)
			{
				CompileBuildAndStart();
			}
			else
			{
				UpdateStatusLabel("ERROR: " + result);
				AddOutput(result);
			}
		}

		private void CbVBEVideo_CheckedChanged(object sender, EventArgs e)
		{
			tbMode.Enabled = cbVBEVideo.Checked;
		}

		private void BtnAddFiles_Click(object sender, EventArgs e)
		{
			using (var open = new OpenFileDialog())
			{
				open.Multiselect = true;
				open.Filter = "All files (*.*)|*.*";

				if (open.ShowDialog(this) == DialogResult.OK)
				{
					foreach (var file in open.FileNames)
					{
						var includeFile = new IncludeFile(file, Path.GetFileName(file));

						includedEntries.Add(new IncludedEntry(includeFile));
					}
				}
			}
		}

		private void BtnRemoveFiles_Click(object sender, EventArgs e)
		{
			while (dataGridView1.SelectedRows.Count > 0)
			{
				dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
			}
		}

		public void LoadArguments(string[] args)
		{
			var cliParser = new Parser(config => config.HelpWriter = Console.Out);

			cliParser.ParseArguments(() => Options, args);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			UpdateProgress();
		}
	}
}
