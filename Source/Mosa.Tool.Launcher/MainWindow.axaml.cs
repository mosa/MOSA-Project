// Copyright (c) MOSA Project. Licensed under the New BSD License.
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

			// TODO: Implement settings for that option
			FmtCmb.SelectedIndex = 0;
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
			settings = AppLocationsSettings.GetAppLocations();

			// Set the default settings
			SetDefaultSettings();

			// Load the CLI arguments
			settings.Merge(SettingsLoader.RecursiveReader(args));

			var files = settings.GetValueList("Compiler.SourceFiles");
			if (files != null)
				foreach (var path in files.Select(Path.GetFullPath).Select(Path.GetDirectoryName).Where(path => !string.IsNullOrWhiteSpace(path)))
					settings.AddPropertyListValue("SearchPaths", path);
			else
				settings.SetValue("Launcher.Start", false);

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
			CountersTxt.Text += $"{data}\n";
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
			settings.SetValue("Compiler.BaseAddress", (uint)BaseAddrTxt.Text.ParseHexOrInteger());

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
		}

		private void UpdateDisplay()
		{
			SsaOpts.IsChecked = settings.GetValue("Optimizations.SSA", true);
			BasicOpts.IsChecked = settings.GetValue("Optimizations.Basic", true);
			SccpOpts.IsChecked = settings.GetValue("Optimizations.SCCP", true);
			InlineOpts.IsChecked = settings.GetValue("Optimizations.Inline", true);
			InlineExplOpts.IsChecked = settings.GetValue("Optimizations.Inline.Explicit", false);
			LongExpOpts.IsChecked = settings.GetValue("Optimizations.LongExpansion", true);
			TwoOptPass.IsChecked = settings.GetValue("Optimizations.TwoPass", true);
			ValueNumOpts.IsChecked = settings.GetValue("Optimizations.ValueNumbering", true);
			BtOpts.IsChecked = settings.GetValue("Optimizations.BitTracker", true);
			PlatOpts.IsChecked = settings.GetValue("Optimizations.Platform", true);
			LicmOpts.IsChecked = settings.GetValue("Optimizations.LoopInvariantCodeMotion", true);
			DevirtOpts.IsChecked = settings.GetValue("Optimizations.Devirtualization", true);

			EmtDwarf.IsChecked = settings.GetValue("Linker.Drawf", false);
			EmtRelocs.IsChecked = settings.GetValue("Linker.StaticRelocations", false);
			EmtSymbs.IsChecked = settings.GetValue("Linker.Symbols", false);
			BaseAddrTxt.Text = "0x" + settings.GetValue("Compiler.BaseAddress", 0x00400000).ToString("x8");

			NasmFile.IsChecked = settings.GetValue("CompilerDebug.NasmFile", string.Empty) == "%DEFAULT%";
			AsmFile.IsChecked = settings.GetValue("CompilerDebug.AsmFile", string.Empty) == "%DEFAULT%";
			MapFile.IsChecked = settings.GetValue("CompilerDebug.MapFile", string.Empty) == "%DEFAULT%";
			DbgFile.IsChecked = settings.GetValue("CompilerDebug.DebugFile", string.Empty) == "%DEFAULT%";
			InlLstFile.IsChecked = settings.GetValue("CompilerDebug.InlinedFile", string.Empty) == "%DEFAULT%";
			HashFiles.IsChecked = settings.GetValue("CompilerDebug.PreLinkHashFile", string.Empty) == "%DEFAULT%";
			CompTimeFile.IsChecked = settings.GetValue("CompilerDebug.CompileTimeFile", string.Empty) == "%DEFAULT%";

			ExitOnLaunch.IsChecked = settings.GetValue("Launcher.Exit", true);

			QemuGdb.IsChecked = settings.GetValue("Emulator.GDB", false);
			LaunchGdb.IsChecked = settings.GetValue("Launcher.LaunchGDB", false);
			MosaDbger.IsChecked = settings.GetValue("Launcher.LaunchDebugger", false);

			MultiThreading.IsChecked = settings.GetValue("Compiler.Multithreading", false);
			MethodScanner.IsChecked = settings.GetValue("Compiler.MethodScanner", false);

			MemVal.Value = settings.GetValue("Emulator.Memory", 128);
			CpuVal.Value = settings.GetValue("Emulator.Cores", 1);

			EnableVbe.IsChecked = settings.GetValue("Multiboot.Video", false);
			VbeWidth.Value = settings.GetValue("Multiboot.Width", 640);
			VbeHeight.Value = settings.GetValue("Multiboot.Height", 480);
			VbeDepth.Value = settings.GetValue("Multiboot.Depth", 32);

			PlugKorlib.IsChecked = settings.GetValue("Launcher.PlugKorlib", true);
			OsNameTxt.Text = settings.GetValue("OS.Name", "MOSA");

			ImgCmb.SelectedIndex = settings.GetValue("Image.Format", string.Empty).ToUpperInvariant() switch
			{
				"ISO" => 0,
				"IMG" => 1,
				"VHD" => 2,
				"VDI" => 3,
				"VMDK" => 4,
				_ => ImgCmb.SelectedIndex
			};

			EmuCmb.SelectedIndex = settings.GetValue("Emulator", string.Empty).ToLowerInvariant() switch
			{
				"qemu" => 0,
				"bochs" => 1,
				"vmware" => 2,
				"virtualbox" => 3,
				_ => EmuCmb.SelectedIndex
			};

			FsCmb.SelectedIndex = settings.GetValue("Image.FileSystem", string.Empty).ToLowerInvariant() switch
			{
				"fat12" => 0,
				"fat16" => 1,
				"fat32" => 2,
				_ => FsCmb.SelectedIndex
			};

			BldCmb.SelectedIndex = settings.GetValue("Image.BootLoader", string.Empty).ToLowerInvariant() switch
			{
				"grub2.00" => 0,
				"grub0.97" => 1,
				"syslinux6.03" => 2,
				"syslinux3.72" => 3,
				"limine" => 4,
				_ => BldCmb.SelectedIndex
			};

			PltCmb.SelectedIndex = settings.GetValue("Compiler.Platform", string.Empty).ToLowerInvariant() switch
			{
				"x86" => 0,
				"x64" => 1,
				"armv8a32" => 2,
				_ => PltCmb.SelectedIndex
			};

			CntCmb.SelectedIndex = settings.GetValue("Emulator.Serial", string.Empty).ToLowerInvariant() switch
			{
				"none" => 0,
				"pipe" => 1,
				"tcpserver" => 2,
				"tcpclient" => 3,
				_ => CntCmb.SelectedIndex
			};

			DstLbl.Content = settings.GetValue("Image.Folder", "No path specified");

			var files = settings.GetValueList("Compiler.SourceFiles");
			var name = files is { Count: >= 1 } ? files[0] : null;

			SrcLbl.Content = name != null ? Path.GetFileName(name) : "No path specified";
		}

		private void SetDefaultSettings()
		{
			settings.SetValue("Compiler.BaseAddress", 0x00400000);
			settings.SetValue("Compiler.Binary", true);
			settings.SetValue("Compiler.MethodScanner", false);
			settings.SetValue("Compiler.Multithreading", true);
			settings.SetValue("Compiler.Multithreading.MaxThreads", Environment.ProcessorCount);
			settings.SetValue("Compiler.Platform", "x86");
			settings.SetValue("Compiler.TraceLevel", 0);
			settings.SetValue("CompilerDebug.DebugFile", string.Empty);
			settings.SetValue("CompilerDebug.AsmFile", string.Empty);
			settings.SetValue("CompilerDebug.MapFile", string.Empty);
			settings.SetValue("CompilerDebug.NasmFile", string.Empty);
			settings.SetValue("CompilerDebug.InlineFile", string.Empty);
			settings.SetValue("Optimizations.Basic", true);
			settings.SetValue("Optimizations.BitTracker", true);
			settings.SetValue("Optimizations.Inline", true);
			settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			settings.SetValue("Optimizations.Inline.Explicit", false);
			settings.SetValue("Optimizations.Inline.Maximum", 12);
			settings.SetValue("Optimizations.Basic.Window", 5);
			settings.SetValue("Optimizations.LongExpansion", true);
			settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
			settings.SetValue("Optimizations.Platform", true);
			settings.SetValue("Optimizations.SCCP", true);
			settings.SetValue("Optimizations.Devirtualization", true);
			settings.SetValue("Optimizations.SSA", true);
			settings.SetValue("Optimizations.TwoPass", true);
			settings.SetValue("Optimizations.ValueNumbering", true);
			settings.SetValue("Image.BootLoader", "limine");
			settings.SetValue("Image.Folder", Path.Combine(Path.GetTempPath(), "MOSA"));
			settings.SetValue("Image.Format", "IMG");
			settings.SetValue("Image.FileSystem", "FAT16");
			settings.SetValue("Image.ImageFile", "%DEFAULT%");
			settings.SetValue("Multiboot.Version", "v1");
			settings.SetValue("Multiboot.Video", false);
			settings.SetValue("Multiboot.Video.Width", 640);
			settings.SetValue("Multiboot.Video.Height", 480);
			settings.SetValue("Multiboot.Video.Depth", 32);
			settings.SetValue("Emulator", "Qemu");
			settings.SetValue("Emulator.Memory", 128);
			settings.SetValue("Emulator.Cores", 1);
			settings.SetValue("Emulator.Serial", "none");
			settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			settings.SetValue("Emulator.Serial.Port", new Random().Next(11111, 22222));
			settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			settings.SetValue("Emulator.Display", true);
			settings.SetValue("Launcher.Start", false);
			settings.SetValue("Launcher.Launch", true);
			settings.SetValue("Launcher.Exit", true);
			settings.SetValue("Launcher.HuntForCorLib", true);
			settings.SetValue("Launcher.PlugKorlib", true);
			settings.SetValue("Linker.Drawf", false);
			settings.SetValue("OS.Name", "MOSA");
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

			var file = result[0];

			settings.ClearProperty("Compiler.SourceFiles");
			settings.AddPropertyListValue("Compiler.SourceFiles", file);

			settings.ClearProperty("SearchPaths");
			settings.AddPropertyListValue("SearchPaths", Path.GetDirectoryName(file));

			SrcLbl.Content = file;
		}

		private async void DstBtn_OnClick(object? _, RoutedEventArgs __)
		{
			var result = await destination.ShowAsync(this);
			if (result == null)
				return;

			settings.SetValue("Image.Folder", result);
			DstLbl.Content = result;
		}
	}
}
