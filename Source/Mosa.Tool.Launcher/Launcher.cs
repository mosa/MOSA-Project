// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Mosa.Tool.Launcher
{
	public partial class Launcher : Form
	{
		private readonly Settings Settings;

		public Builder Builder;

		private int TotalMethods = 0;
		private int CompletedMethods = 0;

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

		public Launcher()
		{
			InitializeComponent();

			Settings = AppLocationsSettings.GetAppLocations();

			AddOutput($"Current Directory: {Environment.CurrentDirectory}");

			lbName.Text = "MOSA Launcher v" + CompilerVersion.VersionString;

			cmbFormat.SelectedIndex = 0;

			RegisterPlatforms();
		}

		private void RegisterPlatforms()
		{
			PlatformRegistry.Add(new Platform.x86.Architecture());
			PlatformRegistry.Add(new Platform.x64.Architecture());
			PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());
		}

		public void UpdateStatusLabel(string msg)
		{
			toolStripStatusLabel1.Text = msg;
		}

		private void NewStatus(string info)
		{
			UpdateStatusLabel(info);
			AddOutput(info);
		}

		private void NotifyStatus(string status) => Invoke((MethodInvoker)(() => NewStatus(status)));

		private void UpdateProgress()
		{
			progressBar1.Maximum = TotalMethods;
			progressBar1.Value = CompletedMethods;
		}

		private void NotifyProgress(int total, int at)
		{
			TotalMethods = total;
			CompletedMethods = at;
		}

		private void Launcher_Shown(object sender, EventArgs e)
		{
			UpdateInterfaceAppLocations();

			Refresh();

			if (Settings.GetValue("Launcher.Start", false))
				CompileBuildAndStart();
		}

		public void AddOutput(string data)
		{
			if (data == null)
				return;

			txtOutput.AppendText(data);
			txtOutput.AppendText(Environment.NewLine);
			txtOutput.Update();
		}

		public void AddCounters(string data)
		{
			txtCounters.AppendText(data);
			txtCounters.AppendText(Environment.NewLine);
			txtCounters.Update();
		}

		private void btnSource_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				var filename = openFileDialog1.FileName;

				UpdateSettings();

				Settings.ClearProperty("Compiler.SourceFiles");
				Settings.AddPropertyListValue("Compiler.SourceFiles", filename);

				Settings.ClearProperty("SearchPaths");
				Settings.AddPropertyListValue("SearchPaths", Path.GetDirectoryName(filename));

				UpdateDisplay();
			}
		}

		private void Launcher_Load(object sender, EventArgs e)
		{
			Text = "MOSA Launcher GUI v" + CompilerVersion.VersionString;
		}

		private void btnDest_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				Settings.SetValue("Image.Folder", folderBrowserDialog1.SelectedPath);

				lblDest.Text = Settings.GetValue("Image.Folder", string.Empty);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.F5)
			{
				CompileBuildAndStart();
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private CompilerHooks CreateCompilerHook()
		{
			var compilerHooks = new CompilerHooks
			{
				NotifyProgress = NotifyProgress,
				NotifyStatus = NotifyStatus
			};

			return compilerHooks;
		}

		private void CompileBuildAndStart()
		{
			txtOutput.Clear();
			txtCounters.Clear();

			if (CheckKeyPressed())
				return;

			tabControl1.SelectTab(4);

			ThreadPool.QueueUserWorkItem(state =>
			{
				try
				{
					var compilerHook = CreateCompilerHook();

					Builder = new Builder(Settings, compilerHook);

					Builder.Build();
				}
				catch (Exception e)
				{
					OnException(e.ToString());
				}
				finally
				{
					if (Builder != null && Builder.IsSucccessful)
					{
						OnCompileCompleted();
					}
				}
			});
		}

		private void OnException(string data) => Invoke((MethodInvoker)(() => AddOutput(data)));

		private void OnCompileCompleted() => Invoke((MethodInvoker)(() => CompileCompleted()));

		private void CompileCompleted()
		{
			if (Settings.GetValue("Launcher.Launch", false))
			{
				foreach (var line in Builder.Counters)
				{
					AddCounters(line);
				}

				if (CheckKeyPressed())
					return;

				var compilerHooks = CreateCompilerHook();

				var starter = new Starter(Builder.Settings, compilerHooks, Builder.Linker);

				starter.Launch();

				if (starter.IsSucccessful)
				{
					if (Settings.GetValue("Launcher.Exit", false))
					{
						Application.Exit();
					}
				}
			}
		}

		private bool CheckKeyPressed()
		{
			return ((ModifierKeys & Keys.Shift) != 0) || ((ModifierKeys & Keys.Control) != 0);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			UpdateProgress();
		}

		public void LoadArguments(string[] args)
		{
			SetDefaultSettings();

			var arguments = SettingsLoader.RecursiveReader(args);

			Settings.Merge(arguments);

			UpdateDisplay();

			var sourcefiles = Settings.GetValueList("Compiler.SourceFiles");

			if (sourcefiles != null)
			{
				foreach (var sourcefile in sourcefiles)
				{
					var full = Path.GetFullPath(sourcefile);
					var path = Path.GetDirectoryName(full);

					if (!string.IsNullOrWhiteSpace(path))
					{
						Settings.AddPropertyListValue("SearchPaths", path);
					}
				}
			}
			else
			{
				Settings.SetValue("Launcher.Start", false);
			}
		}

		private void SetDefaultSettings()
		{
			Settings.SetValue("Compiler.BaseAddress", 0x00400000);
			Settings.SetValue("Compiler.Binary", true);
			Settings.SetValue("Compiler.MethodScanner", false);
			Settings.SetValue("Compiler.Multithreading", true);
			Settings.SetValue("Compiler.Multithreading.MaxThreads", Environment.ProcessorCount);
			Settings.SetValue("Compiler.Platform", "x86");
			Settings.SetValue("Compiler.TraceLevel", 0);
			Settings.SetValue("CompilerDebug.DebugFile", string.Empty);
			Settings.SetValue("CompilerDebug.AsmFile", string.Empty);
			Settings.SetValue("CompilerDebug.MapFile", string.Empty);
			Settings.SetValue("CompilerDebug.NasmFile", string.Empty);
			Settings.SetValue("CompilerDebug.InlineFile", string.Empty);
			Settings.SetValue("Optimizations.Basic", true);
			Settings.SetValue("Optimizations.BitTracker", true);
			Settings.SetValue("Optimizations.Inline", true);
			Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			Settings.SetValue("Optimizations.Inline.Explicit", false);
			Settings.SetValue("Optimizations.Inline.Maximum", 12);
			Settings.SetValue("Optimizations.Basic.Window", 5);
			Settings.SetValue("Optimizations.LongExpansion", true);
			Settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
			Settings.SetValue("Optimizations.Platform", true);
			Settings.SetValue("Optimizations.SCCP", true);
			Settings.SetValue("Optimizations.Devirtualization", true);
			Settings.SetValue("Optimizations.SSA", true);
			Settings.SetValue("Optimizations.TwoPass", true);
			Settings.SetValue("Optimizations.ValueNumbering", true);
			Settings.SetValue("Image.BootLoader", "syslinux3.72");
			Settings.SetValue("Image.Folder", Path.Combine(Path.GetTempPath(), "MOSA"));
			Settings.SetValue("Image.Format", "IMG");
			Settings.SetValue("Image.FileSystem", "FAT16");
			Settings.SetValue("Image.ImageFile", "%DEFAULT%");
			Settings.SetValue("Multiboot.Version", "v1");
			Settings.SetValue("Multiboot.Video", false);
			Settings.SetValue("Multiboot.Video.Width", 640);
			Settings.SetValue("Multiboot.Video.Height", 480);
			Settings.SetValue("Multiboot.Video.Depth", 32);
			Settings.SetValue("Emulator", "Qemu");
			Settings.SetValue("Emulator.Memory", 128);
			Settings.SetValue("Emulator.Cores", 1);
			Settings.SetValue("Emulator.Serial", "none");
			Settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			Settings.SetValue("Emulator.Serial.Port", new Random().Next(11111, 22222));
			Settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			Settings.SetValue("Emulator.Display", true);
			Settings.SetValue("Launcher.Start", false);
			Settings.SetValue("Launcher.Launch", true);
			Settings.SetValue("Launcher.Exit", true);
			Settings.SetValue("Launcher.PlugKorlib", true);
			Settings.SetValue("Launcher.HuntForCorLib", true);
			Settings.SetValue("Linker.Drawf", false);
			Settings.SetValue("OS.Name", "MOSA");
		}

		private void UpdateInterfaceAppLocations()
		{
			lblBochs.Text = Settings.GetValue("AppLocation.Bochs", string.Empty);
			lblNdisasm.Text = Settings.GetValue("AppLocation.Ndisasm", string.Empty);
			lblQemu.Text = Settings.GetValue("AppLocation.Qemu", string.Empty);
			lblBios.Text = Settings.GetValue("AppLocation.QemuBIOS", string.Empty);
			lblImage.Text = Settings.GetValue("AppLocation.QemuImg", string.Empty);

			string VMWareWorkstation = Settings.GetValue("AppLocation.VmwareWorkstation", string.Empty);
			string VMWarePlayer = Settings.GetValue("AppLocation.VmwarePlayer", string.Empty);
			lblVmware.Text = String.IsNullOrWhiteSpace(VMWareWorkstation) ? VMWarePlayer : VMWareWorkstation;

			lblIso.Text = Settings.GetValue("AppLocation.Mkisofs", string.Empty);
		}

		private void UpdateSettings()
		{
			Settings.SetValue("Optimizations.SSA", chkSSA.Checked);
			Settings.SetValue("Optimizations.Basic", chkBasicOptimizations.Checked);
			Settings.SetValue("Optimizations.SCCP", chkSCCP.Checked);
			Settings.SetValue("Optimizations.Devirtualization", chkDevirtualization.Checked);
			Settings.SetValue("CompilerDebug.NasmFile", chkNasm.Checked ? "%DEFAULT%" : string.Empty);
			Settings.SetValue("CompilerDebug.AsmFile", chkAsm.Checked ? "%DEFAULT%" : string.Empty);
			Settings.SetValue("CompilerDebug.MapFile", chkMap.Checked ? "%DEFAULT%" : string.Empty);
			Settings.SetValue("CompilerDebug.DebugFile", chkDebug.Checked ? "%DEFAULT%" : string.Empty);
			Settings.SetValue("CompilerDebug.InlinedFile", chkInlined.Checked ? "%DEFAULT%" : string.Empty);
			Settings.SetValue("CompilerDebug.PreLinkHashFile", chkHash.Checked ? "%DEFAULT%" : string.Empty);
			Settings.SetValue("CompilerDebug.PostLinkHashFile", chkHash.Checked ? "%DEFAULT%" : string.Empty);
			Settings.SetValue("Launcher.Exit", chkExit.Checked);
			Settings.SetValue("Emulator.GDB", chkQemu.Checked);
			Settings.SetValue("Launcher.LaunchGDB", chkGdb.Checked);
			Settings.SetValue("Launcher.LaunchDebugger", chkMosa.Checked);
			Settings.SetValue("Compiler.Multithreading", chkThreads.Checked);
			Settings.SetValue("Compiler.Multithreading.MaxThreads", 0);
			Settings.SetValue("Emulator.Memory", Settings.GetValue("Emulator.Memory", 128));
			Settings.SetValue("Emulator.Cores", Settings.GetValue("Emulator.Cores", 1));
			Settings.SetValue("Optimizations.Inline", chkInline.Checked);
			Settings.SetValue("Optimizations.Inline.Explicit", chkInlineExplicitOnly.Checked);
			Settings.SetValue("Multiboot.Video", chkVbe.Checked);
			Settings.SetValue("Optimizations.LongExpansion", chkLongExpansion.Checked);
			Settings.SetValue("Optimizations.TwoPass", chkTwoPass.Checked);
			Settings.SetValue("Optimizations.ValueNumbering", cbValueNumbering.Checked);
			Settings.SetValue("Compiler.BaseAddress", (uint)txtBase.Text.ParseHexOrInteger());
			Settings.SetValue("Linker.Symbols", chkSymbols.Checked);
			Settings.SetValue("Linker.StaticRelocations", chkStaticRelocations.Checked);
			Settings.SetValue("Linker.Drawf", chkDwarf.Checked);
			Settings.SetValue("Compiler.MethodScanner", chkScanner.Checked);
			Settings.SetValue("CompilerDebug.CompileTimeFile", chkCompile.Checked ? "%DEFAULT%" : string.Empty);
			Settings.SetValue("Optimizations.BitTracker", chkBitTracker.Checked);
			Settings.SetValue("Optimizations.Platform", chkPlatformOptimizations.Checked);
			Settings.SetValue("Optimizations.LoopInvariantCodeMotion", checkBox6.Checked);
			Settings.SetValue("Launcher.Launch", true);
			Settings.SetValue("OS.Name", txtOS.Text);

			try
			{
				Settings.SetValue("Multiboot.Width", (int)numWidth.Value);
				Settings.SetValue("Multiboot.Height", (int)numHeight.Value);
				Settings.SetValue("Multiboot.Depth", (int)numDepth.Value);
			}
			catch (Exception e)
			{
				throw new Exception("An error occurred while parsing VBE Mode: " + e.Message);
			}

			switch (cmbImage.SelectedIndex)
			{
				case 0: Settings.SetValue("Image.Format", "ISO"); break;
				case 1: Settings.SetValue("Image.Format", "IMG"); break;
				case 2: Settings.SetValue("Image.Format", "VHD"); break;
				case 3: Settings.SetValue("Image.Format", "VDI"); break;
				case 4: Settings.SetValue("Image.Format", "VMDK"); break;
				default: Settings.ClearProperty("Image.Format"); break;
			}

			switch (cmbEmulator.SelectedIndex)
			{
				case 0: Settings.SetValue("Emulator", "Qemu"); break;
				case 1: Settings.SetValue("Emulator", "Bochs"); break;
				case 2: Settings.SetValue("Emulator", "VMware"); break;
				case 3: Settings.SetValue("Emulator", "VirtualBox"); break;
				default: Settings.ClearProperty("Emulator"); break;
			}

			switch (cmbConnection.SelectedIndex)
			{
				case 0: Settings.SetValue("Emulator.Serial", "None"); break;
				case 1: Settings.SetValue("Emulator.Serial", "Pipe"); break;
				case 2: Settings.SetValue("Emulator.Serial", "TCPServer"); break;
				case 3: Settings.SetValue("Emulator.Serial", "TCPClient"); break;
				default: Settings.ClearProperty("Emulator.Serial"); break;
			}

			switch (cmbFile.SelectedIndex)
			{
				case 0: Settings.SetValue("Image.FileSystem", "FAT12"); break;
				case 1: Settings.SetValue("Image.FileSystem", "FAT16"); break;
				case 2: Settings.SetValue("Image.FileSystem", "FAT32"); break;
				default: Settings.ClearProperty("Image.FileSystem"); break;
			}

			switch (cmbPlatform.SelectedIndex)
			{
				case 0: Settings.SetValue("Compiler.Platform", "x86"); break;
				case 1: Settings.SetValue("Compiler.Platform", "x64"); break;
				case 2: Settings.SetValue("Compiler.Platform", "ARMv8A32"); break;
				default: Settings.SetValue("Compiler.Platform", "x86"); break;
			}

			switch (cmbLoader.SelectedIndex)
			{
				case 0: Settings.SetValue("Image.BootLoader", "grub2.00"); break;
				case 1: Settings.SetValue("Image.BootLoader", "grub0.97"); break;
				case 2: Settings.SetValue("Image.BootLoader", "syslinux6.03"); break;
				case 3: Settings.SetValue("Image.BootLoader", "syslinux3.72"); break;
				default: Settings.ClearProperty("Image.BootLoader"); break;
			}
		}

		private void UpdateDisplay()
		{
			chkSSA.Checked = Settings.GetValue("Optimizations.SSA", true);
			chkBasicOptimizations.Checked = Settings.GetValue("Optimizations.Basic", true);
			chkSCCP.Checked = Settings.GetValue("Optimizations.SCCP", true);
			chkDwarf.Checked = Settings.GetValue("Linker.Drawf", chkDwarf.Checked);
			chkDevirtualization.Checked = Settings.GetValue("Optimizations.Devirtualization", chkDevirtualization.Checked);
			chkNasm.Checked = Settings.GetValue("CompilerDebug.NasmFile", string.Empty) == "%DEFAULT%";
			chkAsm.Checked = Settings.GetValue("CompilerDebug.AsmFile", string.Empty) == "%DEFAULT%";
			chkMap.Checked = Settings.GetValue("CompilerDebug.MapFile", string.Empty) == "%DEFAULT%";
			chkDebug.Checked = Settings.GetValue("CompilerDebug.DebugFile", string.Empty) == "%DEFAULT%";
			chkInlined.Checked = Settings.GetValue("CompilerDebug.InlinedFile", string.Empty) == "%DEFAULT%";
			chkHash.Checked = Settings.GetValue("CompilerDebug.PreLinkHashFile", string.Empty) == "%DEFAULT%";
			chkExit.Checked = Settings.GetValue("Launcher.Exit", true);
			chkQemu.Checked = Settings.GetValue("Emulator.GDB", false);
			chkGdb.Checked = Settings.GetValue("Launcher.LaunchGDB", false);
			chkMosa.Checked = Settings.GetValue("Launcher.LaunchDebugger", false);
			chkInline.Checked = Settings.GetValue("Optimizations.Inline", true);
			chkInlineExplicitOnly.Checked = Settings.GetValue("Optimizations.Inline.Explicit", false);
			chkThreads.Checked = Settings.GetValue("Compiler.Multithreading", false);
			numMemory.Value = Settings.GetValue("Emulator.Memory", 128);
			numCores.Value = Settings.GetValue("Emulator.Cores", 1);
			chkVbe.Checked = Settings.GetValue("Multiboot.Video", false);
			txtBase.Text = "0x" + Settings.GetValue("Compiler.BaseAddress", 0x00400000).ToString("x8");
			chkStaticRelocations.Checked = Settings.GetValue("Linker.StaticRelocations", false);
			chkSymbols.Checked = Settings.GetValue("Linker.Symbols", false);
			chkLongExpansion.Checked = Settings.GetValue("Optimizations.LongExpansion", true);
			chkTwoPass.Checked = Settings.GetValue("Optimizations.TwoPass", true);
			cbValueNumbering.Checked = Settings.GetValue("Optimizations.ValueNumbering", true);
			chkScanner.Checked = Settings.GetValue("Compiler.MethodScanner", false);
			chkCompile.Checked = Settings.GetValue("CompilerDebug.CompileTimeFile", string.Empty) == "%DEFAULT%";
			chkBitTracker.Checked = Settings.GetValue("Optimizations.BitTracker", false);
			chkPlatformOptimizations.Checked = Settings.GetValue("Optimizations.Platform", false);
			checkBox6.Checked = Settings.GetValue("Optimizations.LoopInvariantCodeMotion", false);
			numWidth.Value = Settings.GetValue("Multiboot.Width", 640);
			numHeight.Value = Settings.GetValue("Multiboot.Height", 480);
			numDepth.Value = Settings.GetValue("Multiboot.Depth", 32);
			txtOS.Text = Settings.GetValue("OS.Name", "MOSA");

			switch (Settings.GetValue("Image.Format", string.Empty).ToUpperInvariant())
			{
				case "ISO": cmbImage.SelectedIndex = 0; break;
				case "IMG": cmbImage.SelectedIndex = 1; break;
				case "VHD": cmbImage.SelectedIndex = 2; break;
				case "VDI": cmbImage.SelectedIndex = 3; break;
				case "VMDK": cmbImage.SelectedIndex = 4; break;
				default: break;
			}

			switch (Settings.GetValue("Emulator", string.Empty).ToLowerInvariant())
			{
				case "qemu": cmbEmulator.SelectedIndex = 0; break;
				case "bochs": cmbEmulator.SelectedIndex = 1; break;
				case "vmware": cmbEmulator.SelectedIndex = 2; break;
				case "virtualbox": cmbEmulator.SelectedIndex = 3; break;
				default: cmbEmulator.SelectedIndex = -1; break;
			}

			switch (Settings.GetValue("Image.FileSystem", string.Empty).ToLowerInvariant())
			{
				case "fat12": cmbFile.SelectedIndex = 0; break;
				case "fat16": cmbFile.SelectedIndex = 1; break;
				case "fat32": cmbFile.SelectedIndex = 2; break;
				default: break;
			}

			switch (Settings.GetValue("Image.BootLoader", string.Empty).ToLowerInvariant())
			{
				case "grub2.00": cmbLoader.SelectedIndex = 0; break;
				case "grub0.97": cmbLoader.SelectedIndex = 1; break;
				case "syslinux6.03": cmbLoader.SelectedIndex = 2; break;
				case "syslinux3.72": cmbLoader.SelectedIndex = 3; break;
				default: break;
			}

			switch (Settings.GetValue("Compiler.Platform", string.Empty).ToLowerInvariant())
			{
				case "x86": cmbPlatform.SelectedIndex = 0; break;
				case "x64": cmbPlatform.SelectedIndex = 1; break;
				case "armv8a32": cmbPlatform.SelectedIndex = 2; break;
				default: cmbPlatform.SelectedIndex = 0; break;
			}

			switch (Settings.GetValue("Emulator.Serial", string.Empty).ToLowerInvariant())
			{
				case "none": cmbConnection.SelectedIndex = 0; break;
				case "pipe": cmbConnection.SelectedIndex = 1; break;
				case "tcpserver": cmbConnection.SelectedIndex = 2; break;
				case "tcpclient": cmbConnection.SelectedIndex = 3; break;
				default: cmbConnection.SelectedIndex = 0; break;
			}

			lblDest.Text = Settings.GetValue("Image.Folder", string.Empty);

			var sourcefiles = Settings.GetValueList("Compiler.SourceFiles");

			var sourcefile = sourcefiles == null ? null : sourcefiles[0];
			string filename = sourcefiles != null && sourcefiles.Count >= 1 ? sourcefile : null;

			lblSource.Text = (filename != null) ? Path.GetFileName(filename) : string.Empty;
		}

		private void btnCompile_Click(object sender, EventArgs e)
		{
			UpdateStatusLabel("Starting...");
			UpdateSettings();

			var result = CheckOptions.Verify(Settings);

			if (result == null)
			{
				CompileBuildAndStart();
				UpdateStatusLabel("Completed!");
			}
			else
			{
				UpdateStatusLabel("ERROR: " + result);
				AddOutput(result);
			}
		}
	}
}
