// Copyright (c) MOSA Project. Licensed under the New BSD License.
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Threading;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;

namespace Mosa.Tool.Launcher
{
	// TODO: Implement settings for FmtCmb and FrmCmb
	public partial class MainWindow : Window
	{
		private bool done;
		private Settings settings;
		private Builder builder;
		private OpenFileDialog source;
		private OpenFolderDialog destination;

		public MainWindow()
		{
			InitializeComponent();

			source = new OpenFileDialog
			{
				AllowMultiple = false,
				Title = "Select the file to compile"
			};

			destination = new OpenFolderDialog
			{
				Title = "Select the output directory"
			};

			DstLbl.Content = Path.Combine(Path.GetTempPath(), "MOSA");
		}

		private void AddOutput(string data)
		{
			if (string.IsNullOrEmpty(data))
				return;

			OutputTxt.Text += data + Environment.NewLine;
		}

		public void Initialize(string[] args)
		{
			// Get the app locations
			settings = AppLocationsSettings.GetAppLocations();

			// Set the default settings
			SetDefaultSettings();

			// Load the CLI arguments
			settings.Merge(SettingsLoader.RecursiveReader(args));

			var files = settings.GetValueList("Compiler.SourceFiles");
			if (files != null)
			{
				var src = files[0];
				if (src != null)
				{
					OsNameTxt.Text = Path.GetFileNameWithoutExtension(src);
					SrcLbl.Content = Path.GetFileName(src);
				}

				foreach (var file in files)
				{
					var path = Path.GetDirectoryName(Path.GetFullPath(file));

					if (!string.IsNullOrWhiteSpace(path))
						settings.AddPropertyListValue("SearchPaths", path);
				}
			} else settings.SetValue("Launcher.Start", false);

				// Output the current directory
			AddOutput("Current Directory: " + Environment.CurrentDirectory);

			// Set title label with the current compiler version
			TitleLbl.Content += CompilerVersion.VersionString;

			// Register all platforms supported by the compiler
			PlatformRegistry.Add(new Platform.x86.Architecture());
			PlatformRegistry.Add(new Platform.x64.Architecture());
			PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());

			// Initialize settings
			UpdateSettings();
			UpdateInterfaceAppLocations();

			if (settings.GetValue("Launcher.Start", false))
			{
				Tabs.SelectedIndex = 4;
				CompileBuildAndStart();
			}

			// Hacky method to center the screen
			this.GetObservable(IsVisibleProperty).Subscribe(value =>
			{
				if (!value || done)
					return;

				done = true;
				CenterWindow();
			});
		}

		private void AddCounters(string data)
		{
			CountersTxt.Text += data + Environment.NewLine;
		}

		private CompilerHooks CreateCompilerHook()
		{
			return new CompilerHooks
			{
				NotifyStatus = NotifyStatus
			};
		}

		private void NotifyStatus(string status) => Dispatcher.UIThread.Post(() => AddOutput(status));

		private void UpdateInterfaceAppLocations()
		{
			bochsPathLbl.Content = settings.GetValue("AppLocation.Bochs", "N/A");
			ndiasmPathLbl.Content = settings.GetValue("AppLocation.Ndisasm", "N/A");
			qemuPathLbl.Content = settings.GetValue("AppLocation.Qemu", "N/A");
			qemuBiosPathLbl.Content = settings.GetValue("AppLocation.QemuBIOS", "N/A");
			qemuImgPathLbl.Content = settings.GetValue("AppLocation.QemuImg", "N/A");
			vboxPathLbl.Content = settings.GetValue("AppLocation.VirtualBox", "N/A");
			mkisofsPathLbl.Content = settings.GetValue("AppLocation.Mkisofs", "N/A");

			var workstation = settings.GetValue("AppLocation.VmwareWorkstation", string.Empty);
			vmwarePathLbl.Content = string.IsNullOrEmpty(workstation) ? settings.GetValue("AppLocation.VmwarePlayer", "N/A") : workstation;
		}

		private void UpdateSettings()
		{
			settings.SetValue("Optimizations.SSA", SsaOpts.IsChecked.Value);
			settings.SetValue("Optimizations.Basic", BasicOpts.IsChecked.Value);
			settings.SetValue("Optimizations.SCCP", SccpOpts.IsChecked.Value);
			settings.SetValue("Optimizations.Devirtualization", DevirtOpts.IsChecked.Value);
			settings.SetValue("Optimizations.Inline", InlineOpts.IsChecked.Value);
			settings.SetValue("Optimizations.Inline.Explicit", InlineExplOpts.IsChecked.Value);
			settings.SetValue("Optimizations.LongExpansion", LongExpOpts.IsChecked.Value);
			settings.SetValue("Optimizations.TwoPass", TwoOptPass.IsChecked.Value);
			settings.SetValue("Optimizations.ValueNumbering", ValueNumOpts.IsChecked.Value);
			settings.SetValue("Optimizations.BitTracker", BtOpts.IsChecked.Value);
			settings.SetValue("Optimizations.Platform", PlatOpts.IsChecked.Value);
			settings.SetValue("Optimizations.LoopInvariantCodeMotion", LicmOpts.IsChecked.Value);

			settings.SetValue("Linker.Symbols", EmtSymbs.IsChecked.Value);
			settings.SetValue("Linker.StaticRelocations", EmtRelocs.IsChecked.Value);
			settings.SetValue("Linker.Drawf", EmtDwarf.IsChecked.Value);
			settings.SetValue("Compiler.BaseAddress", BaseAddrTxt.Text.StartsWith("0x") ? uint.Parse(BaseAddrTxt.Text[2..], NumberStyles.HexNumber) : uint.Parse(BaseAddrTxt.Text));

			settings.SetValue("CompilerDebug.NasmFile", NasmFile.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			settings.SetValue("CompilerDebug.AsmFile", AsmFile.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			settings.SetValue("CompilerDebug.MapFile", MapFile.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			settings.SetValue("CompilerDebug.DebugFile", DbgFile.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			settings.SetValue("CompilerDebug.InlinedFile", InlLstFile.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			settings.SetValue("CompilerDebug.PreLinkHashFile", HashFiles.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			settings.SetValue("CompilerDebug.PostLinkHashFile", HashFiles.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			settings.SetValue("CompilerDebug.CompileTimeFile", CompTimeFile.IsChecked.Value ? "%DEFAULT%" : string.Empty);

			settings.SetValue("Launcher.Exit", ExitOnLaunch.IsChecked.Value);

			settings.SetValue("Emulator.GDB", QemuGdb.IsChecked.Value);
			settings.SetValue("Launcher.LaunchGDB", LaunchGdb.IsChecked.Value);
			settings.SetValue("Launcher.LaunchDebugger", MosaDbger.IsChecked.Value);

			settings.SetValue("Compiler.Multithreading", MultiThreading.IsChecked.Value);
			settings.SetValue("Compiler.Multithreading.MaxThreads", 0);
			settings.SetValue("Compiler.MethodScanner", MethodScanner.IsChecked.Value);

			settings.SetValue("Emulator.Memory", (int)MemVal.Value);
			settings.SetValue("Emulator.Cores", (int)CpuVal.Value);

			settings.SetValue("Multiboot.Video", EnableVbe.IsChecked.Value);
			settings.SetValue("Multiboot.Width", (int)VbeWidth.Value);
			settings.SetValue("Multiboot.Height", (int)VbeHeight.Value);
			settings.SetValue("Multiboot.Depth", (int)VbeDepth.Value);

			settings.SetValue("Launcher.PlugKorlib", PlugKorlib.IsChecked.Value);
			settings.SetValue("OS.Name", OsNameTxt.Text);

			var src = SrcLbl.Content.ToString();

			settings.ClearProperty("Compiler.SourceFiles");
			settings.AddPropertyListValue("Compiler.SourceFiles", src);

			settings.ClearProperty("SearchPaths");
			settings.AddPropertyListValue("SearchPaths", Path.GetDirectoryName(src));

			settings.SetValue("Image.Folder", DstLbl.Content.ToString());

			switch (ImgCmb.SelectedIndex)
			{
				case 0: settings.SetValue("Image.Format", "ISO"); break;
				case 1: settings.SetValue("Image.Format", "IMG"); break;
				case 2: settings.SetValue("Image.Format", "VHD"); break;
				case 3: settings.SetValue("Image.Format", "VDI"); break;
				case 4: settings.SetValue("Image.Format", "VMDK"); break;
				default: settings.ClearProperty("Image.Format"); break;
			}

			switch (EmuCmb.SelectedIndex)
			{
				case 0: settings.SetValue("Emulator", "Qemu"); break;
				case 1: settings.SetValue("Emulator", "Bochs"); break;
				case 2: settings.SetValue("Emulator", "VMware"); break;
				case 3: settings.SetValue("Emulator", "VirtualBox"); break;
				default: settings.ClearProperty("Emulator"); break;
			}

			switch (CntCmb.SelectedIndex)
			{
				case 0: settings.SetValue("Emulator.Serial", "None"); break;
				case 1: settings.SetValue("Emulator.Serial", "Pipe"); break;
				case 2: settings.SetValue("Emulator.Serial", "TCPServer"); break;
				case 3: settings.SetValue("Emulator.Serial", "TCPClient"); break;
				default: settings.ClearProperty("Emulator.Serial"); break;
			}

			switch (FsCmb.SelectedIndex)
			{
				case 0: settings.SetValue("Image.FileSystem", "FAT12"); break;
				case 1: settings.SetValue("Image.FileSystem", "FAT16"); break;
				case 2: settings.SetValue("Image.FileSystem", "FAT32"); break;
				default: settings.ClearProperty("Image.FileSystem"); break;
			}

			switch (PltCmb.SelectedIndex)
			{
				case 0: settings.SetValue("Compiler.Platform", "x86"); break;
				case 1: settings.SetValue("Compiler.Platform", "x64"); break;
				case 2: settings.SetValue("Compiler.Platform", "ARMv8A32"); break;
				default: settings.SetValue("Compiler.Platform", "x86"); break;
			}

			switch (BldCmb.SelectedIndex)
			{
				case 0: settings.SetValue("Image.BootLoader", "grub2.00"); break;
				case 1: settings.SetValue("Image.BootLoader", "grub0.97"); break;
				case 2: settings.SetValue("Image.BootLoader", "syslinux6.03"); break;
				case 3: settings.SetValue("Image.BootLoader", "syslinux3.72"); break;
				case 4: settings.SetValue("Image.BootLoader", "limine"); break;
				default: settings.ClearProperty("Image.BootLoader"); break;
			}

			switch (FrmCmb.SelectedIndex)
			{
				case 0: settings.SetValue("Image.Firmware", "bios"); break;
				default: settings.ClearProperty("Image.Firmware"); break;
			}
		}

		private void SetDefaultSettings()
		{
			settings.SetValue("Compiler.Binary", true);
			settings.SetValue("Compiler.Multithreading.MaxThreads", Environment.ProcessorCount);
			settings.SetValue("Compiler.TraceLevel", 0);
			settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			settings.SetValue("Optimizations.Inline.Maximum", 12);
			settings.SetValue("Optimizations.Basic.Window", 5);
			settings.SetValue("Image.ImageFile", "%DEFAULT%");
			settings.SetValue("Multiboot.Version", "v1");
			settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			settings.SetValue("Emulator.Serial.Port", new Random().Next(11111, 22222));
			settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			settings.SetValue("Emulator.Display", true);
			settings.SetValue("Launcher.Start", false);
			settings.SetValue("Launcher.Launch", true);
			settings.SetValue("Launcher.HuntForCorLib", true);
		}

		private void CompileBuildAndStart()
		{
			OutputTxt.Clear();
			CountersTxt.Clear();

			ThreadPool.QueueUserWorkItem(_ =>
			{
				try
				{
					builder = new Builder(settings, CreateCompilerHook());
					builder.Build();
				}
				catch (Exception e)
				{
					Dispatcher.UIThread.Post(() => AddOutput(e.ToString()));
				}
				finally
				{
					if (builder.IsSucccessful)
						Dispatcher.UIThread.Post(CompileCompleted);
				}
			});
		}

		private void CompileCompleted()
		{
			if (!settings.GetValue("Launcher.Launch", false))
				return;

			foreach (var line in builder.Counters)
				AddCounters(line);

			var starter = new Starter(builder.Settings, CreateCompilerHook(), builder.Linker);
			if (!starter.Launch(true))
				return;

			if (settings.GetValue("Launcher.Exit", false))
				Close();
		}

		private async void CenterWindow()
		{
			Screen? screen = null;
			while (screen == null)
			{
				await Task.Delay(1);
				screen = Screens.ScreenFromVisual(this);
			}

			var x = (int)Math.Floor(screen.Bounds.Width / 2 - Bounds.Width / 2);
			var y = (int)Math.Floor(screen.Bounds.Height / 2 - (Bounds.Height + 30) / 2);

			Position = new PixelPoint(x, y);
		}

		private void LnchBtn_OnClick(object? _, RoutedEventArgs e)
		{
			Tabs.SelectedIndex = 4;
			UpdateSettings();

			var result = CheckOptions.Verify(settings);
			if (result == null)
				CompileBuildAndStart();
			else
				AddOutput("ERROR: " + result);
		}

		private async void SrcBtn_OnClick(object? _, RoutedEventArgs __)
		{
			var result = await source.ShowAsync(this);
			if (result == null)
				return;

			SrcLbl.Content = result[0];
		}

		private async void DstBtn_OnClick(object? _, RoutedEventArgs __)
		{
			var result = await destination.ShowAsync(this);
			if (result == null)
				return;

			DstLbl.Content = result;
		}
	}
}
