using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Threading;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;

namespace Mosa.Tool.Launcher
{
	public partial class MainWindow : Window
	{
		private bool _done;
		private Settings _settings;
		private Builder _builder;
		private int _totalMethods, _completedMethods;
		private readonly OpenFileDialog _source;
		private readonly OpenFolderDialog _destination;

		public MainWindow()
		{
			InitializeComponent();

			var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1) };
			timer.Tick += (_, _) =>
			{
				OutputProgress.Maximum = _totalMethods;
				OutputProgress.Value = _completedMethods;
			};
			timer.Start();

			_source = new OpenFileDialog
			{
				AllowMultiple = false,
				Title = "Select the file to compile"
			};

			_destination = new OpenFolderDialog
			{
				Title = "Select the output directory"
			};

			// TODO: Implement settings for that option
			FmtCmb.SelectedIndex = 0;
		}

		private void NotifyProgress(int total, int at)
		{
			_totalMethods = total;
			_completedMethods = at;
		}

		private void AddOutput(string data)
		{
			if (string.IsNullOrEmpty(data))
				return;

			OutputTxt.Text += $"{data}\n";
		}

		public void Initialize(string[] args)
		{
			// Get the app locations
			_settings = AppLocationsSettings.GetAppLocations();

			// Set the default settings
			SetDefaultSettings();

			// Load the CLI arguments
			_settings.Merge(SettingsLoader.RecursiveReader(args));

			var files = _settings.GetValueList("Compiler.SourceFiles");
			if (files != null)
				foreach (var path in files.Select(Path.GetFullPath).Select(Path.GetDirectoryName).Where(path => !string.IsNullOrWhiteSpace(path)))
					_settings.AddPropertyListValue("SearchPaths", path);
			else
				_settings.SetValue("Launcher.Start", false);

			// Output the current directory
			AddOutput($"Current Directory: {Environment.CurrentDirectory}");

			// Set title label with the current compiler version
			TitleLbl.Content += CompilerVersion.VersionString;

			// Register all platforms supported by the compiler
			PlatformRegistry.Add(new Platform.x86.Architecture());
			PlatformRegistry.Add(new Platform.x64.Architecture());
			PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());

			// Initialize settings
			UpdateDisplay();
			UpdateInterfaceAppLocations();

			if (_settings.GetValue("Launcher.Start", false))
			{
				Tabs.SelectedIndex = 4;
				CompileBuildAndStart();
			}

			// Hacky method to center the window on the screen
			this.GetObservable(IsVisibleProperty).Subscribe(value =>
			{
				if (!value || _done)
					return;

				_done = true;
				CenterWindow();
			});
		}

		private void AddCounters(string data)
		{
			CountersTxt.Text += $"{data}\n";
		}

		private CompilerHooks CreateCompilerHook()
		{
			return new CompilerHooks
			{
				NotifyProgress = NotifyProgress,
				NotifyStatus = NotifyStatus
			};
		}

		private void NotifyStatus(string status) => Dispatcher.UIThread.Post(() => AddOutput(status));

		private void UpdateInterfaceAppLocations()
		{
			bochsPathLbl.Content = _settings.GetValue("AppLocation.Bochs", "N/A");
			ndiasmPathLbl.Content = _settings.GetValue("AppLocation.Ndisasm", "N/A");
			qemuPathLbl.Content = _settings.GetValue("AppLocation.Qemu", "N/A");
			qemuBiosPathLbl.Content = _settings.GetValue("AppLocation.QemuBIOS", "N/A");
			qemuImgPathLbl.Content = _settings.GetValue("AppLocation.QemuImg", "N/A");
			vboxPathLbl.Content = _settings.GetValue("AppLocation.VirtualBox", "N/A");
			mkisofsPathLbl.Content = _settings.GetValue("AppLocation.Mkisofs", "N/A");

			var workstation = _settings.GetValue("AppLocation.VmwareWorkstation", string.Empty);
			vmwarePathLbl.Content = string.IsNullOrEmpty(workstation) ? _settings.GetValue("AppLocation.VmwarePlayer", "N/A") : workstation;
		}

		private void UpdateSettings()
		{
			_settings.SetValue("Optimizations.SSA", SsaOpts.IsChecked.Value);
			_settings.SetValue("Optimizations.Basic", BasicOpts.IsChecked.Value);
			_settings.SetValue("Optimizations.SCCP", SccpOpts.IsChecked.Value);
			_settings.SetValue("Optimizations.Devirtualization", DevirtOpts.IsChecked.Value);
			_settings.SetValue("Optimizations.Inline", InlineOpts.IsChecked.Value);
			_settings.SetValue("Optimizations.Inline.Explicit", InlineExplOpts.IsChecked.Value);
			_settings.SetValue("Optimizations.LongExpansion", LongExpOpts.IsChecked.Value);
			_settings.SetValue("Optimizations.TwoPass", TwoOptPass.IsChecked.Value);
			_settings.SetValue("Optimizations.ValueNumbering", ValueNumOpts.IsChecked.Value);
			_settings.SetValue("Optimizations.BitTracker", BtOpts.IsChecked.Value);
			_settings.SetValue("Optimizations.Platform", PlatOpts.IsChecked.Value);
			_settings.SetValue("Optimizations.LoopInvariantCodeMotion", LicmOpts.IsChecked.Value);

			_settings.SetValue("Linker.Symbols", EmtSymbs.IsChecked.Value);
			_settings.SetValue("Linker.StaticRelocations", EmtRelocs.IsChecked.Value);
			_settings.SetValue("Linker.Drawf", EmtDwarf.IsChecked.Value);
			_settings.SetValue("Compiler.BaseAddress", (uint)BaseAddrTxt.Text.ParseHexOrInteger());

			_settings.SetValue("CompilerDebug.NasmFile", NasmFile.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			_settings.SetValue("CompilerDebug.AsmFile", AsmFile.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			_settings.SetValue("CompilerDebug.MapFile", MapFile.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			_settings.SetValue("CompilerDebug.DebugFile", DbgFile.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			_settings.SetValue("CompilerDebug.InlinedFile", InlLstFile.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			_settings.SetValue("CompilerDebug.PreLinkHashFile", HashFiles.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			_settings.SetValue("CompilerDebug.PostLinkHashFile", HashFiles.IsChecked.Value ? "%DEFAULT%" : string.Empty);
			_settings.SetValue("CompilerDebug.CompileTimeFile", CompTimeFile.IsChecked.Value ? "%DEFAULT%" : string.Empty);

			_settings.SetValue("Launcher.Exit", ExitOnLaunch.IsChecked.Value);

			_settings.SetValue("Emulator.GDB", QemuGdb.IsChecked.Value);
			_settings.SetValue("Launcher.LaunchGDB", LaunchGdb.IsChecked.Value);
			_settings.SetValue("Launcher.LaunchDebugger", MosaDbger.IsChecked.Value);

			_settings.SetValue("Compiler.Multithreading", MultiThreading.IsChecked.Value);
			_settings.SetValue("Compiler.Multithreading.MaxThreads", 0);
			_settings.SetValue("Compiler.MethodScanner", MethodScanner.IsChecked.Value);

			_settings.SetValue("Emulator.Memory", (int)MemVal.Value);
			_settings.SetValue("Emulator.Cores", (int)CpuVal.Value);

			_settings.SetValue("Multiboot.Video", EnableVbe.IsChecked.Value);
			_settings.SetValue("Multiboot.Width", (int)VbeWidth.Value);
			_settings.SetValue("Multiboot.Height", (int)VbeHeight.Value);
			_settings.SetValue("Multiboot.Depth", (int)VbeDepth.Value);

			_settings.SetValue("Launcher.PlugKorlib", PlugKorlib.IsChecked.Value);
			_settings.SetValue("OS.Name", OsNameTxt.Text);

			switch (ImgCmb.SelectedIndex)
			{
				case 0: _settings.SetValue("Image.Format", "ISO"); break;
				case 1: _settings.SetValue("Image.Format", "IMG"); break;
				case 2: _settings.SetValue("Image.Format", "VHD"); break;
				case 3: _settings.SetValue("Image.Format", "VDI"); break;
				case 4: _settings.SetValue("Image.Format", "VMDK"); break;
				default: _settings.ClearProperty("Image.Format"); break;
			}

			switch (EmuCmb.SelectedIndex)
			{
				case 0: _settings.SetValue("Emulator", "Qemu"); break;
				case 1: _settings.SetValue("Emulator", "Bochs"); break;
				case 2: _settings.SetValue("Emulator", "VMware"); break;
				case 3: _settings.SetValue("Emulator", "VirtualBox"); break;
				default: _settings.ClearProperty("Emulator"); break;
			}

			switch (CntCmb.SelectedIndex)
			{
				case 0: _settings.SetValue("Emulator.Serial", "None"); break;
				case 1: _settings.SetValue("Emulator.Serial", "Pipe"); break;
				case 2: _settings.SetValue("Emulator.Serial", "TCPServer"); break;
				case 3: _settings.SetValue("Emulator.Serial", "TCPClient"); break;
				default: _settings.ClearProperty("Emulator.Serial"); break;
			}

			switch (FsCmb.SelectedIndex)
			{
				case 0: _settings.SetValue("Image.FileSystem", "FAT12"); break;
				case 1: _settings.SetValue("Image.FileSystem", "FAT16"); break;
				case 2: _settings.SetValue("Image.FileSystem", "FAT32"); break;
				default: _settings.ClearProperty("Image.FileSystem"); break;
			}

			switch (PltCmb.SelectedIndex)
			{
				case 0: _settings.SetValue("Compiler.Platform", "x86"); break;
				case 1: _settings.SetValue("Compiler.Platform", "x64"); break;
				case 2: _settings.SetValue("Compiler.Platform", "ARMv8A32"); break;
				default: _settings.SetValue("Compiler.Platform", "x86"); break;
			}

			switch (BldCmb.SelectedIndex)
			{
				case 0: _settings.SetValue("Image.BootLoader", "grub2.00"); break;
				case 1: _settings.SetValue("Image.BootLoader", "grub0.97"); break;
				case 2: _settings.SetValue("Image.BootLoader", "syslinux6.03"); break;
				case 3: _settings.SetValue("Image.BootLoader", "syslinux3.72"); break;
				case 4: _settings.SetValue("Image.BootLoader", "limine"); break;
				default: _settings.ClearProperty("Image.BootLoader"); break;
			}
		}

		private void UpdateDisplay()
		{
			SsaOpts.IsChecked = _settings.GetValue("Optimizations.SSA", true);
			BasicOpts.IsChecked = _settings.GetValue("Optimizations.Basic", true);
			SccpOpts.IsChecked = _settings.GetValue("Optimizations.SCCP", true);
			InlineOpts.IsChecked = _settings.GetValue("Optimizations.Inline", true);
			InlineExplOpts.IsChecked = _settings.GetValue("Optimizations.Inline.Explicit", false);
			LongExpOpts.IsChecked = _settings.GetValue("Optimizations.LongExpansion", true);
			TwoOptPass.IsChecked = _settings.GetValue("Optimizations.TwoPass", true);
			ValueNumOpts.IsChecked = _settings.GetValue("Optimizations.ValueNumbering", true);
			BtOpts.IsChecked = _settings.GetValue("Optimizations.BitTracker", true);
			PlatOpts.IsChecked = _settings.GetValue("Optimizations.Platform", true);
			LicmOpts.IsChecked = _settings.GetValue("Optimizations.LoopInvariantCodeMotion", true);
			DevirtOpts.IsChecked = _settings.GetValue("Optimizations.Devirtualization", true);

			EmtDwarf.IsChecked = _settings.GetValue("Linker.Drawf", false);
			EmtRelocs.IsChecked = _settings.GetValue("Linker.StaticRelocations", false);
			EmtSymbs.IsChecked = _settings.GetValue("Linker.Symbols", false);
			BaseAddrTxt.Text = "0x" + _settings.GetValue("Compiler.BaseAddress", 0x00400000).ToString("x8");

			NasmFile.IsChecked = _settings.GetValue("CompilerDebug.NasmFile", string.Empty) == "%DEFAULT%";
			AsmFile.IsChecked = _settings.GetValue("CompilerDebug.AsmFile", string.Empty) == "%DEFAULT%";
			MapFile.IsChecked = _settings.GetValue("CompilerDebug.MapFile", string.Empty) == "%DEFAULT%";
			DbgFile.IsChecked = _settings.GetValue("CompilerDebug.DebugFile", string.Empty) == "%DEFAULT%";
			InlLstFile.IsChecked = _settings.GetValue("CompilerDebug.InlinedFile", string.Empty) == "%DEFAULT%";
			HashFiles.IsChecked = _settings.GetValue("CompilerDebug.PreLinkHashFile", string.Empty) == "%DEFAULT%";
			CompTimeFile.IsChecked = _settings.GetValue("CompilerDebug.CompileTimeFile", string.Empty) == "%DEFAULT%";

			ExitOnLaunch.IsChecked = _settings.GetValue("Launcher.Exit", true);

			QemuGdb.IsChecked = _settings.GetValue("Emulator.GDB", false);
			LaunchGdb.IsChecked = _settings.GetValue("Launcher.LaunchGDB", false);
			MosaDbger.IsChecked = _settings.GetValue("Launcher.LaunchDebugger", false);

			MultiThreading.IsChecked = _settings.GetValue("Compiler.Multithreading", false);
			MethodScanner.IsChecked = _settings.GetValue("Compiler.MethodScanner", false);

			MemVal.Value = _settings.GetValue("Emulator.Memory", 128);
			CpuVal.Value = _settings.GetValue("Emulator.Cores", 1);

			EnableVbe.IsChecked = _settings.GetValue("Multiboot.Video", false);
			VbeWidth.Value = _settings.GetValue("Multiboot.Width", 640);
			VbeHeight.Value = _settings.GetValue("Multiboot.Height", 480);
			VbeDepth.Value = _settings.GetValue("Multiboot.Depth", 32);

			PlugKorlib.IsChecked = _settings.GetValue("Launcher.PlugKorlib", true);
			OsNameTxt.Text = _settings.GetValue("OS.Name", "MOSA");

			ImgCmb.SelectedIndex = _settings.GetValue("Image.Format", string.Empty).ToUpperInvariant() switch
			{
				"ISO" => 0,
				"IMG" => 1,
				"VHD" => 2,
				"VDI" => 3,
				"VMDK" => 4,
				_ => ImgCmb.SelectedIndex
			};

			EmuCmb.SelectedIndex = _settings.GetValue("Emulator", string.Empty).ToLowerInvariant() switch
			{
				"qemu" => 0,
				"bochs" => 1,
				"vmware" => 2,
				"virtualbox" => 3,
				_ => EmuCmb.SelectedIndex
			};

			FsCmb.SelectedIndex = _settings.GetValue("Image.FileSystem", string.Empty).ToLowerInvariant() switch
			{
				"fat12" => 0,
				"fat16" => 1,
				"fat32" => 2,
				_ => FsCmb.SelectedIndex
			};

			BldCmb.SelectedIndex = _settings.GetValue("Image.BootLoader", string.Empty).ToLowerInvariant() switch
			{
				"grub2.00" => 0,
				"grub0.97" => 1,
				"syslinux6.03" => 2,
				"syslinux3.72" => 3,
				"limine" => 4,
				_ => BldCmb.SelectedIndex
			};

			PltCmb.SelectedIndex = _settings.GetValue("Compiler.Platform", string.Empty).ToLowerInvariant() switch
			{
				"x86" => 0,
				"x64" => 1,
				"armv8a32" => 2,
				_ => PltCmb.SelectedIndex
			};

			CntCmb.SelectedIndex = _settings.GetValue("Emulator.Serial", string.Empty).ToLowerInvariant() switch
			{
				"none" => 0,
				"pipe" => 1,
				"tcpserver" => 2,
				"tcpclient" => 3,
				_ => CntCmb.SelectedIndex
			};

			DstLbl.Content = _settings.GetValue("Image.Folder", "No path specified");

			var files = _settings.GetValueList("Compiler.SourceFiles");
			var name = files is { Count: >= 1 } ? files[0] : null;

			SrcLbl.Content = name != null ? Path.GetFileName(name) : "No path specified";
		}

		private void SetDefaultSettings()
		{
			_settings.SetValue("Compiler.BaseAddress", 0x00400000);
			_settings.SetValue("Compiler.Binary", true);
			_settings.SetValue("Compiler.MethodScanner", false);
			_settings.SetValue("Compiler.Multithreading", true);
			_settings.SetValue("Compiler.Multithreading.MaxThreads", Environment.ProcessorCount);
			_settings.SetValue("Compiler.Platform", "x86");
			_settings.SetValue("Compiler.TraceLevel", 0);
			_settings.SetValue("CompilerDebug.DebugFile", string.Empty);
			_settings.SetValue("CompilerDebug.AsmFile", string.Empty);
			_settings.SetValue("CompilerDebug.MapFile", string.Empty);
			_settings.SetValue("CompilerDebug.NasmFile", string.Empty);
			_settings.SetValue("CompilerDebug.InlineFile", string.Empty);
			_settings.SetValue("Optimizations.Basic", true);
			_settings.SetValue("Optimizations.BitTracker", true);
			_settings.SetValue("Optimizations.Inline", true);
			_settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			_settings.SetValue("Optimizations.Inline.Explicit", false);
			_settings.SetValue("Optimizations.Inline.Maximum", 12);
			_settings.SetValue("Optimizations.Basic.Window", 5);
			_settings.SetValue("Optimizations.LongExpansion", true);
			_settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
			_settings.SetValue("Optimizations.Platform", true);
			_settings.SetValue("Optimizations.SCCP", true);
			_settings.SetValue("Optimizations.Devirtualization", true);
			_settings.SetValue("Optimizations.SSA", true);
			_settings.SetValue("Optimizations.TwoPass", true);
			_settings.SetValue("Optimizations.ValueNumbering", true);
			_settings.SetValue("Image.BootLoader", "limine");
			_settings.SetValue("Image.Folder", Path.Combine(Path.GetTempPath(), "MOSA"));
			_settings.SetValue("Image.Format", "IMG");
			_settings.SetValue("Image.FileSystem", "FAT16");
			_settings.SetValue("Image.ImageFile", "%DEFAULT%");
			_settings.SetValue("Multiboot.Version", "v1");
			_settings.SetValue("Multiboot.Video", false);
			_settings.SetValue("Multiboot.Video.Width", 640);
			_settings.SetValue("Multiboot.Video.Height", 480);
			_settings.SetValue("Multiboot.Video.Depth", 32);
			_settings.SetValue("Emulator", "Qemu");
			_settings.SetValue("Emulator.Memory", 128);
			_settings.SetValue("Emulator.Cores", 1);
			_settings.SetValue("Emulator.Serial", "none");
			_settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			_settings.SetValue("Emulator.Serial.Port", new Random().Next(11111, 22222));
			_settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			_settings.SetValue("Emulator.Display", true);
			_settings.SetValue("Launcher.Start", false);
			_settings.SetValue("Launcher.Launch", true);
			_settings.SetValue("Launcher.Exit", true);
			_settings.SetValue("Launcher.HuntForCorLib", true);
			_settings.SetValue("Launcher.PlugKorlib", true);
			_settings.SetValue("Linker.Drawf", false);
			_settings.SetValue("OS.Name", "MOSA");
		}

		private void CompileBuildAndStart()
		{
			OutputTxt.Clear();
			CountersTxt.Clear();

			ThreadPool.QueueUserWorkItem(_ =>
			{
				try
				{
					_builder = new Builder(_settings, CreateCompilerHook());
					_builder.Build();
				}
				catch (Exception e)
				{
					Dispatcher.UIThread.Post(() => AddOutput(e.ToString()));
				}
				finally
				{
					if (_builder.IsSucccessful)
						Dispatcher.UIThread.Post(CompileCompleted);
				}
			});
		}

		private void CompileCompleted()
		{
			if (!_settings.GetValue("Launcher.Launch", false))
				return;

			foreach (var line in _builder.Counters)
				AddCounters(line);

			var starter = new Starter(_builder.Settings, CreateCompilerHook(), _builder.Linker);
			if (!starter.Launch(true))
				return;

			if (_settings.GetValue("Launcher.Exit", false))
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

			var result = CheckOptions.Verify(_settings);
			if (result == null)
				CompileBuildAndStart();
			else
				AddOutput("ERROR: " + result);
		}

		private async void SrcBtn_OnClick(object? _, RoutedEventArgs __)
		{
			var result = await _source.ShowAsync(this);
			if (result == null)
				return;

			var file = result[0];

			_settings.ClearProperty("Compiler.SourceFiles");
			_settings.AddPropertyListValue("Compiler.SourceFiles", file);

			_settings.ClearProperty("SearchPaths");
			_settings.AddPropertyListValue("SearchPaths", Path.GetDirectoryName(file));

			SrcLbl.Content = file;
		}

		private async void DstBtn_OnClick(object? _, RoutedEventArgs __)
		{
			var result = await _destination.ShowAsync(this);
			if (result == null)
				return;

			_settings.SetValue("Image.Folder", result);
			DstLbl.Content = result;
		}
	}
}
