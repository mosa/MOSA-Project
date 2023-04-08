// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Globalization;
using System.IO;
using System.Text;
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

namespace Mosa.Tool.Launcher;

// TODO: Implement settings for FmtCmb and FrmCmb
public partial class MainWindow : Window
{
	private bool done;
	private int totalMethods, completedMethods;
	private Settings settings;
	private Builder builder;
	private readonly OpenFileDialog source;
	private readonly OpenFolderDialog destination;
	private readonly OpenFolderDialog include;

	public MainWindow()
	{
		InitializeComponent();

		var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1) };
		timer.Tick += (_, _) =>
		{
			OutputProgress.Maximum = totalMethods;
			OutputProgress.Value = completedMethods;
		};
		timer.Start();

		source = new OpenFileDialog
		{
			AllowMultiple = false,
			Title = "Select the file to compile"
		};

		destination = new OpenFolderDialog
		{
			Title = "Select the output directory"
		};

		include = new OpenFolderDialog
		{
			Title = "Select the include directory"
		};

		DstLbl.Content = Path.Combine(Path.GetTempPath(), "MOSA");
	}

	private void NotifyProgress(int total, int at)
	{
		totalMethods = total;
		completedMethods = at;
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

		// Update the GUI settings
		UpdateGuiSettings();

		var sb = new StringBuilder();

		foreach (var arg in args)
		{
			sb.Append(arg);
			sb.Append(' ');
		}

		AddOutput($"Arguments: {sb}");

		// Output the current directory
		AddOutput($"Current Directory: {Environment.CurrentDirectory}");

		var files = settings.GetValueList("Compiler.SourceFiles");
		if (files != null)
		{
			var src = files[0];
			if (src != null)
			{
				OsNameTxt.Text = Path.GetFileNameWithoutExtension(src);
				SrcLbl.Content = Path.GetFullPath(src);
			}

			foreach (var file in files)
			{
				var path = Path.GetDirectoryName(Path.GetFullPath(file));

				if (!string.IsNullOrWhiteSpace(path))
					settings.AddPropertyListValue("SearchPaths", path);
			}
		}
		else settings.SetValue("Launcher.Start", false);

		IncDirTxt.Text = settings.GetValue("Image.FileSystem.RootInclude");

		// Set title label with the current compiler version
		TitleLbl.Content += CompilerVersion.VersionString;

		// Register all platforms supported by the compiler
		PlatformRegistry.Add(new Platform.x86.Architecture());
		PlatformRegistry.Add(new Platform.x64.Architecture());
		PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());

		// Initialize paths
		UpdatePaths();
		UpdateInterfaceAppLocations();

		if (settings.GetValue("Launcher.Start", false))
		{
			Tabs.SelectedIndex = 4;
			CompileBuildAndStart();
		}

		// Hacky method to center the window on the screen
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
			NotifyProgress = NotifyProgress,
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

	private void UpdateGuiSettings()
	{
		SsaOpts.IsChecked = settings.GetValue("Optimizations.SSA", SsaOpts.IsChecked!.Value);
		BasicOpts.IsChecked = settings.GetValue("Optimizations.Basic", BasicOpts.IsChecked!.Value);
		SccpOpts.IsChecked = settings.GetValue("Optimizations.SCCP", SccpOpts.IsChecked!.Value);
		DevirtOpts.IsChecked = settings.GetValue("Optimizations.Devirtualization", DevirtOpts.IsChecked!.Value);
		InlineOpts.IsChecked = settings.GetValue("Optimizations.Inline", InlineOpts.IsChecked!.Value);
		InlineExplOpts.IsChecked = settings.GetValue("Optimizations.Inline.Explicit", InlineExplOpts.IsChecked!.Value);
		LongExpOpts.IsChecked = settings.GetValue("Optimizations.LongExpansion", LongExpOpts.IsChecked!.Value);
		TwoOptPass.IsChecked = settings.GetValue("Optimizations.TwoPass", TwoOptPass.IsChecked!.Value);
		ValueNumOpts.IsChecked = settings.GetValue("Optimizations.ValueNumbering", ValueNumOpts.IsChecked!.Value);
		BtOpts.IsChecked = settings.GetValue("Optimizations.BitTracker", BtOpts.IsChecked!.Value);
		PlatOpts.IsChecked = settings.GetValue("Optimizations.Platform", PlatOpts.IsChecked!.Value);
		LicmOpts.IsChecked = settings.GetValue("Optimizations.LoopInvariantCodeMotion", LicmOpts.IsChecked!.Value);

		EmtSymbs.IsChecked = settings.GetValue("Linker.Symbols", EmtSymbs.IsChecked!.Value);
		EmtRelocs.IsChecked = settings.GetValue("Linker.StaticRelocations", EmtRelocs.IsChecked!.Value);
		EmtDwarf.IsChecked = settings.GetValue("Linker.Dwarf", EmtDwarf.IsChecked!.Value);
		BaseAddrTxt.Text = settings.GetValue("Compiler.BaseAddress");

		NasmFile.IsChecked = !string.IsNullOrEmpty(settings.GetValue("CompilerDebug.NasmFile"));
		AsmFile.IsChecked = !string.IsNullOrEmpty(settings.GetValue("CompilerDebug.AsmFile"));
		MapFile.IsChecked = !string.IsNullOrEmpty(settings.GetValue("CompilerDebug.MapFile"));
		DbgFile.IsChecked = !string.IsNullOrEmpty(settings.GetValue("CompilerDebug.DebugFile"));
		InlLstFile.IsChecked = !string.IsNullOrEmpty(settings.GetValue("CompilerDebug.InlinedFile"));
		HashFiles.IsChecked = !string.IsNullOrEmpty(settings.GetValue("CompilerDebug.PreLinkHashFile"));
		CompTimeFile.IsChecked = !string.IsNullOrEmpty(settings.GetValue("CompilerDebug.CompileTimeFile"));

		ExitOnLaunch.IsChecked = settings.GetValue("Launcher.Exit", ExitOnLaunch.IsChecked!.Value);

		QemuGdb.IsChecked = settings.GetValue("Emulator.GDB", QemuGdb.IsChecked!.Value);
		LaunchGdb.IsChecked = settings.GetValue("Launcher.LaunchGDB", LaunchGdb.IsChecked!.Value);
		MosaDbger.IsChecked = settings.GetValue("Launcher.LaunchDebugger", MosaDbger.IsChecked!.Value);

		MultiThreading.IsChecked = settings.GetValue("Compiler.Multithreading", MultiThreading.IsChecked!.Value);
		MethodScanner.IsChecked = settings.GetValue("Compiler.MethodScanner", MethodScanner.IsChecked!.Value);

		MemVal.Value = settings.GetValue("Emulator.Memory", (int)MemVal.Value);
		CpuVal.Value = settings.GetValue("Emulator.Cores", (int)CpuVal.Value);

		EnableVbe.IsChecked = settings.GetValue("Multiboot.Video", EnableVbe.IsChecked!.Value);
		VbeWidth.Value = settings.GetValue("Multiboot.Width", (int)VbeWidth.Value);
		VbeHeight.Value = settings.GetValue("Multiboot.Height", (int)VbeHeight.Value);
		VbeDepth.Value = settings.GetValue("Multiboot.Depth", (int)VbeDepth.Value);

		PlugKorlib.IsChecked = settings.GetValue("Launcher.PlugKorlib", PlugKorlib.IsChecked!.Value);
		OsNameTxt.Text = settings.GetValue("OS.Name");

		ImgCmb.SelectedIndex = settings.GetValue("Image.Format") switch
		{
			"IMG" => 0,
			"VHD" => 1,
			"VDI" => 2,
			"VMDK" => 3,
			_ => ImgCmb.SelectedIndex
		};

		EmuCmb.SelectedIndex = settings.GetValue("Emulator") switch
		{
			"Qemu" => 0,
			"Bochs" => 1,
			"VMware" => 2,
			"VirtualBox" => 3,
			_ => EmuCmb.SelectedIndex
		};

		CntCmb.SelectedIndex = settings.GetValue("Emulator.Serial") switch
		{
			"None" => 0,
			"Pipe" => 1,
			"TCPServer" => 2,
			"TCPClient" => 3,
			_ => CntCmb.SelectedIndex
		};

		FsCmb.SelectedIndex = settings.GetValue("Image.FileSystem") switch
		{
			"FAT12" => 0,
			"FAT16" => 1,
			"FAT32" => 2,
			_ => FsCmb.SelectedIndex
		};

		PltCmb.SelectedIndex = settings.GetValue("Compiler.Platform") switch
		{
			"x86" => 0,
			"x64" => 1,
			"ARMv8A32" => 2,
			_ => 0
		};

		FrmCmb.SelectedIndex = settings.GetValue("Image.Firmware") switch
		{
			"bios" => 0,
			_ => FrmCmb.SelectedIndex
		};
	}

	private void UpdateSettings()
	{
		settings.SetValue("Optimizations.SSA", SsaOpts.IsChecked!.Value);
		settings.SetValue("Optimizations.Basic", BasicOpts.IsChecked!.Value);
		settings.SetValue("Optimizations.SCCP", SccpOpts.IsChecked!.Value);
		settings.SetValue("Optimizations.Devirtualization", DevirtOpts.IsChecked!.Value);
		settings.SetValue("Optimizations.Inline", InlineOpts.IsChecked!.Value);
		settings.SetValue("Optimizations.Inline.Explicit", InlineExplOpts.IsChecked!.Value);
		settings.SetValue("Optimizations.LongExpansion", LongExpOpts.IsChecked!.Value);
		settings.SetValue("Optimizations.TwoPass", TwoOptPass.IsChecked!.Value);
		settings.SetValue("Optimizations.ValueNumbering", ValueNumOpts.IsChecked!.Value);
		settings.SetValue("Optimizations.BitTracker", BtOpts.IsChecked!.Value);
		settings.SetValue("Optimizations.Platform", PlatOpts.IsChecked!.Value);
		settings.SetValue("Optimizations.LoopInvariantCodeMotion", LicmOpts.IsChecked!.Value);

		settings.SetValue("Linker.Symbols", EmtSymbs.IsChecked!.Value);
		settings.SetValue("Linker.StaticRelocations", EmtRelocs.IsChecked!.Value);
		settings.SetValue("Linker.Dwarf", EmtDwarf.IsChecked!.Value);
		settings.SetValue("Compiler.BaseAddress", BaseAddrTxt.Text.StartsWith("0x") ? uint.Parse(BaseAddrTxt.Text[2..], NumberStyles.HexNumber) : uint.Parse(BaseAddrTxt.Text));

		settings.SetValue("CompilerDebug.NasmFile", NasmFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty);
		settings.SetValue("CompilerDebug.AsmFile", AsmFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty);
		settings.SetValue("CompilerDebug.MapFile", MapFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty);
		settings.SetValue("CompilerDebug.DebugFile", DbgFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty);
		settings.SetValue("CompilerDebug.InlinedFile", InlLstFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty);
		settings.SetValue("CompilerDebug.PreLinkHashFile", HashFiles.IsChecked!.Value ? "%DEFAULT%" : string.Empty);
		settings.SetValue("CompilerDebug.PostLinkHashFile", HashFiles.IsChecked.Value ? "%DEFAULT%" : string.Empty);
		settings.SetValue("CompilerDebug.CompileTimeFile", CompTimeFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty);

		settings.SetValue("Launcher.Exit", ExitOnLaunch.IsChecked!.Value);

		settings.SetValue("Emulator.GDB", QemuGdb.IsChecked!.Value);
		settings.SetValue("Launcher.LaunchGDB", LaunchGdb.IsChecked!.Value);
		settings.SetValue("Launcher.LaunchDebugger", MosaDbger.IsChecked!.Value);

		settings.SetValue("Compiler.Multithreading", MultiThreading.IsChecked!.Value);
		settings.SetValue("Compiler.Multithreading.MaxThreads", 0);
		settings.SetValue("Compiler.MethodScanner", MethodScanner.IsChecked!.Value);

		settings.SetValue("Emulator.Memory", (int)MemVal.Value);
		settings.SetValue("Emulator.Cores", (int)CpuVal.Value);

		settings.SetValue("Multiboot.Video", EnableVbe.IsChecked!.Value);
		settings.SetValue("Multiboot.Width", (int)VbeWidth.Value);
		settings.SetValue("Multiboot.Height", (int)VbeHeight.Value);
		settings.SetValue("Multiboot.Depth", (int)VbeDepth.Value);

		settings.SetValue("Launcher.PlugKorlib", PlugKorlib.IsChecked!.Value);
		settings.SetValue("OS.Name", OsNameTxt.Text);

		UpdatePaths();

		switch (ImgCmb.SelectedIndex)
		{
			case 0: settings.SetValue("Image.Format", "IMG"); break;
			case 1: settings.SetValue("Image.Format", "VHD"); break;
			case 2: settings.SetValue("Image.Format", "VDI"); break;
			case 3: settings.SetValue("Image.Format", "VMDK"); break;
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

		switch (FrmCmb.SelectedIndex)
		{
			case 0: settings.SetValue("Image.Firmware", "bios"); break;
			default: settings.ClearProperty("Image.Firmware"); break;
		}
	}

	private void UpdatePaths()
	{
		var src = SrcLbl.Content.ToString();

		settings.ClearProperty("Compiler.SourceFiles");
		settings.AddPropertyListValue("Compiler.SourceFiles", src);

		settings.ClearProperty("SearchPaths");
		settings.AddPropertyListValue("SearchPaths", Path.GetDirectoryName(src));

		settings.SetValue("Image.Folder", DstLbl.Content.ToString());
		settings.SetValue("Image.FileSystem.RootInclude", IncDirTxt.Text);
	}

	private void SetDefaultSettings()
	{
		settings.SetValue("Compiler.BaseAddress", 0x00400000);
		settings.SetValue("Compiler.Binary", true);
		settings.SetValue("Compiler.MethodScanner", false);
		settings.SetValue("Compiler.Multithreading", true);
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
		settings.SetValue("Optimizations.Inline.Explicit", true);
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
		settings.SetValue("Image.Folder", Path.Combine(Path.GetTempPath(), "MOSA"));
		settings.SetValue("Image.Format", "IMG");
		settings.SetValue("Image.FileSystem", "FAT16");
		settings.SetValue("Image.ImageFile", "%DEFAULT%");
		settings.SetValue("Image.Firmware", "bios");
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
		settings.SetValue("Launcher.Start", true);
		settings.SetValue("Launcher.Launch", true);
		settings.SetValue("Launcher.Exit", true);
		settings.SetValue("Launcher.PlugKorlib", true);
		settings.SetValue("Launcher.HuntForCorLib", true);
		settings.SetValue("Linker.Dwarf", false);
		settings.SetValue("OS.Name", OsNameTxt.Text);
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

	private void LnchBtn_OnClick(object? _, RoutedEventArgs __)
	{
		Tabs.SelectedIndex = 4;
		UpdateSettings();

		var result = CheckOptions.Verify(settings);
		if (result == null)
			CompileBuildAndStart();
		else
			AddOutput($"ERROR: {result}");
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

	private async void IncBtn_OnClick(object? _, RoutedEventArgs __)
	{
		var result = await include.ShowAsync(this);
		if (result == null)
			return;

		IncDirTxt.Text = result;
	}
}
