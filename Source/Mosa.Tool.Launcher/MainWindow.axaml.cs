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
		private bool _done;
		private Settings _settings;
		private Builder _builder;
		private OpenFileDialog _source;
		private OpenFolderDialog _destination;

		public MainWindow()
		{
			InitializeComponent();

			_source = new OpenFileDialog
			{
				AllowMultiple = false,
				Title = "Select the file to compile"
			};

			_destination = new OpenFolderDialog
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
			_settings = AppLocationsSettings.GetAppLocations();

			// Set the default settings
			SetDefaultSettings();

			// Load the CLI arguments
			_settings.Merge(SettingsLoader.RecursiveReader(args));

			var files = _settings.GetValueList("Compiler.SourceFiles");
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
						_settings.AddPropertyListValue("SearchPaths", path);
				}
			} else _settings.SetValue("Launcher.Start", false);

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

			if (_settings.GetValue("Launcher.Start", false))
			{
				Tabs.SelectedIndex = 4;
				CompileBuildAndStart();
			}

			// Hacky method to center the screen
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
			_settings.SetValue("Compiler.BaseAddress", uint.Parse(BaseAddrTxt.Text, NumberStyles.Integer | NumberStyles.HexNumber));

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

			var src = SrcLbl.Content.ToString();

			_settings.ClearProperty("Compiler.SourceFiles");
			_settings.AddPropertyListValue("Compiler.SourceFiles", src);

			_settings.ClearProperty("SearchPaths");
			_settings.AddPropertyListValue("SearchPaths", Path.GetDirectoryName(src));

			_settings.SetValue("Image.Folder", DstLbl.Content.ToString());

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

		private void SetDefaultSettings()
		{
			_settings.SetValue("Compiler.Binary", true);
			_settings.SetValue("Compiler.Multithreading.MaxThreads", Environment.ProcessorCount);
			_settings.SetValue("Compiler.TraceLevel", 0);
			_settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			_settings.SetValue("Optimizations.Inline.Maximum", 12);
			_settings.SetValue("Optimizations.Basic.Window", 5);
			_settings.SetValue("Image.ImageFile", "%DEFAULT%");
			_settings.SetValue("Multiboot.Version", "v1");
			_settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			_settings.SetValue("Emulator.Serial.Port", new Random().Next(11111, 22222));
			_settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			_settings.SetValue("Emulator.Display", true);
			_settings.SetValue("Launcher.Start", false);
			_settings.SetValue("Launcher.Launch", true);
			_settings.SetValue("Launcher.HuntForCorLib", true);
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

			SrcLbl.Content = result[0];
		}

		private async void DstBtn_OnClick(object? _, RoutedEventArgs __)
		{
			var result = await _destination.ShowAsync(this);
			if (result == null)
				return;

			DstLbl.Content = result;
		}
	}
}
