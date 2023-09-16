// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Globalization;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;

namespace Mosa.Tool.Launcher;

public partial class MainWindow : Window
{
	private readonly MosaSettings mosaSettings = new();
	private readonly Stopwatch stopwatch = new();
	private readonly CompilerHooks compilerHooks;
	private readonly DispatcherTimer timer;

	private Builder? builder;
	private int totalMethods;
	private int completedMethods;

	public MainWindow()
	{
		InitializeComponent();

		timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
		timer.Tick += (_, _) =>
		{
			OutputProgress.Maximum = totalMethods;
			OutputProgress.Value = completedMethods;
		};

		compilerHooks = new CompilerHooks
		{
			NotifyProgress = NotifyProgress,
			NotifyStatus = NotifyStatus
		};

		DstLbl.Content = Path.Combine(Path.GetTempPath(), "MOSA");
	}

	private void NotifyProgress(int total, int at)
	{
		totalMethods = total;
		completedMethods = at;
	}

	private void AddOutput(string status)
	{
		if (string.IsNullOrEmpty(status)) return;

		OutputTxt.Text += $"{stopwatch.Elapsed.TotalSeconds:00.00} | {status}{Environment.NewLine}";
	}

	public void Initialize(string[] args)
	{
		mosaSettings.LoadAppLocations();
		mosaSettings.SetDetfaultSettings();
		mosaSettings.LoadArguments(args);
		SetRequiredSettings();
		mosaSettings.ExpandSearchPaths();
		mosaSettings.NormalizeSettings();
		mosaSettings.UpdateFileAndPathSettings();

		UpdateGuiSettings();

		var sb = new StringBuilder();

		foreach (var arg in args)
		{
			sb.Append(arg);
			sb.Append(' ');
		}

		AddOutput($"Arguments: {sb}");
		AddOutput($"Current Directory: {Environment.CurrentDirectory}");

		if (mosaSettings.SourceFiles is { Count: > 0 })
		{
			var src = mosaSettings.SourceFiles[0];
			if (!string.IsNullOrEmpty(src))
			{
				OsNameTxt.Text = Path.GetFileNameWithoutExtension(src);
				SrcLbl.Content = Path.GetFullPath(src);
			}

			foreach (var file in mosaSettings.SourceFiles)
			{
				var path = Path.GetDirectoryName(Path.GetFullPath(file));
				if (!string.IsNullOrWhiteSpace(path)) mosaSettings.AddSearchPath(path);
			}
		} else mosaSettings.ExplorerStart = false;

		IncDirTxt.Text = mosaSettings.FileSystemRootInclude;
		TitleLbl.Content += CompilerVersion.VersionString;

		PlatformRegistry.Add(new Compiler.x86.Architecture());
		PlatformRegistry.Add(new Compiler.x64.Architecture());
		PlatformRegistry.Add(new Compiler.ARM32.Architecture());

		UpdatePaths();
		UpdateInterfaceAppLocations();

		if (!mosaSettings.LauncherStart) return;

		Tabs.SelectedIndex = 4;

		var result = CheckOptions.Verify(mosaSettings);

		if (string.IsNullOrEmpty(result)) CompileBuildAndStart();
		else AddOutput($"ERROR: {result}");
	}

	private void SetRequiredSettings()
	{
		mosaSettings.EmulatorDisplay = true;
		if (mosaSettings is { SourceFiles.Count: > 0, LauncherStart: true }) mosaSettings.LauncherStart = true;
		mosaSettings.Launcher = true;
		mosaSettings.LauncherExit = true;
	}

	private void AddCounters(string data) => CountersTxt.Text += data + Environment.NewLine;

	private void NotifyStatus(string status) => Dispatcher.UIThread.Post(() => AddOutput(status));

	private void UpdateInterfaceAppLocations()
	{
		qemuPathLbl.Content = mosaSettings.QemuX86App;
		qemuBiosPathLbl.Content = mosaSettings.QemuBIOS;
		qemuEdk2X86PathLbl.Content = mosaSettings.QemuEdk2X86;
		qemuEdk2X64PathLbl.Content = mosaSettings.QemuEdk2X64;
		qemuEdk2ARMPathLbl.Content = mosaSettings.QemuEdk2ARM32;
		qemuImgPathLbl.Content = mosaSettings.QemuImgApp;
		bochsPathLbl.Content = mosaSettings.BochsApp;
		vmwarePathLbl.Content = string.IsNullOrEmpty(mosaSettings.VmwarePlayerApp) ? mosaSettings.VmwareWorkstationApp : mosaSettings.VmwarePlayerApp;
		vboxPathLbl.Content = mosaSettings.VirtualBoxApp;
		mkisofsPathLbl.Content = mosaSettings.MkisofsApp;
		ndiasmPathLbl.Content = mosaSettings.NdisasmApp;
	}

	private void UpdateGuiSettings()
	{
		SsaOpts.IsChecked = mosaSettings.SSA;
		BasicOpts.IsChecked = mosaSettings.BasicOptimizations;
		SccpOpts.IsChecked = mosaSettings.SparseConditionalConstantPropagation;
		DevirtOpts.IsChecked = mosaSettings.Devirtualization;
		InlineOpts.IsChecked = mosaSettings.InlineMethods;
		InlineExplOpts.IsChecked = mosaSettings.InlineExplicit;
		LongExpOpts.IsChecked = mosaSettings.LongExpansion;
		TwoOptPass.IsChecked = mosaSettings.TwoPassOptimization;
		ValueNumOpts.IsChecked = mosaSettings.ValueNumbering;
		BtOpts.IsChecked = mosaSettings.BitTracker;
		PlatOpts.IsChecked = mosaSettings.PlatformOptimizations;
		LicmOpts.IsChecked = mosaSettings.LoopInvariantCodeMotion;

		EmtSymbs.IsChecked = mosaSettings.EmitSumbols;
		EmtRelocs.IsChecked = mosaSettings.EmitStaticRelocations;
		EmtDwarf.IsChecked = mosaSettings.EmitDwarf;
		BaseAddrTxt.Text = mosaSettings.BaseAddress.ToHex();

		NasmFile.IsChecked = !string.IsNullOrEmpty(mosaSettings.NasmFile);
		AsmFile.IsChecked = !string.IsNullOrEmpty(mosaSettings.AsmFile);
		MapFile.IsChecked = !string.IsNullOrEmpty(mosaSettings.MapFile);
		DbgFile.IsChecked = !string.IsNullOrEmpty(mosaSettings.DebugFile);
		InlLstFile.IsChecked = !string.IsNullOrEmpty(mosaSettings.InlinedFile);
		HashFiles.IsChecked = !string.IsNullOrEmpty(mosaSettings.PreLinkHashFile);
		CompTimeFile.IsChecked = !string.IsNullOrEmpty(mosaSettings.CompileTimeFile);

		ExitOnLaunch.IsChecked = mosaSettings.LauncherExit;

		QemuGdb.IsChecked = mosaSettings.LaunchGDB;
		LaunchGdb.IsChecked = mosaSettings.LaunchGDB;
		MosaDbger.IsChecked = mosaSettings.LaunchDebugger;

		MultiThreading.IsChecked = mosaSettings.Multithreading;
		MethodScanner.IsChecked = mosaSettings.MethodScanner;

		MemVal.Value = mosaSettings.EmulatorMemory;
		CpuVal.Value = mosaSettings.EmulatorCores;

		EnableMbGraphics.IsChecked = mosaSettings.MultibootVideo;
		GraphicsWidth.Value = mosaSettings.MultibootVideoWidth;
		GraphicsHeight.Value = mosaSettings.MultibootVideoHeight;

		PlugKorlib.IsChecked = mosaSettings.PlugKorlib;
		OsNameTxt.Text = mosaSettings.OSName;

		ImgCmb.SelectedIndex = mosaSettings.ImageFormat.ToUpperInvariant() switch
		{
			"IMG" => 0,
			"VHD" => 1,
			"VDI" => 2,
			"VMDK" => 3,
			_ => ImgCmb.SelectedIndex
		};

		EmuCmb.SelectedIndex = mosaSettings.Emulator switch
		{
			"Qemu" => 0,
			"Bochs" => 1,
			"VMware" => 2,
			"VirtualBox" => 3,
			_ => EmuCmb.SelectedIndex
		};

		CntCmb.SelectedIndex = mosaSettings.EmulatorSerial switch
		{
			"None" => 0,
			"Pipe" => 1,
			"TCPServer" => 2,
			"TCPClient" => 3,
			_ => CntCmb.SelectedIndex
		};

		FsCmb.SelectedIndex = mosaSettings.FileSystem.ToUpperInvariant() switch
		{
			"FAT12" => 0,
			"FAT16" => 1,
			"FAT32" => 2,
			_ => FsCmb.SelectedIndex
		};

		FmtCmb.SelectedIndex = mosaSettings.MultibootVersion.ToLowerInvariant() switch
		{
			"" => 0,
			"v2" => 1,
			_ => FmtCmb.SelectedIndex
		};

		PltCmb.SelectedIndex = mosaSettings.Platform.ToLowerInvariant() switch
		{
			"x86" => 0,
			"x64" => 1,
			"arm32" => 2,
			_ => PltCmb.SelectedIndex
		};

		FrmCmb.SelectedIndex = mosaSettings.ImageFirmware.ToLowerInvariant() switch
		{
			"bios" => 0,
			_ => FrmCmb.SelectedIndex
		};
	}

	private void UpdateSettings()
	{
		mosaSettings.SSA = SsaOpts.IsChecked!.Value;
		mosaSettings.BasicOptimizations = BasicOpts.IsChecked!.Value;
		mosaSettings.SparseConditionalConstantPropagation = SccpOpts.IsChecked!.Value;
		mosaSettings.Devirtualization = DevirtOpts.IsChecked!.Value;
		mosaSettings.InlineMethods = InlineOpts.IsChecked!.Value;
		mosaSettings.InlineExplicit = InlineExplOpts.IsChecked!.Value;
		mosaSettings.LongExpansion = LongExpOpts.IsChecked!.Value;
		mosaSettings.TwoPassOptimization = TwoOptPass.IsChecked!.Value;
		mosaSettings.ValueNumbering = ValueNumOpts.IsChecked!.Value;
		mosaSettings.BitTracker = BtOpts.IsChecked!.Value;
		mosaSettings.PlatformOptimizations = PlatOpts.IsChecked!.Value;
		mosaSettings.LoopInvariantCodeMotion = LicmOpts.IsChecked!.Value;

		mosaSettings.EmitSumbols = EmtSymbs.IsChecked!.Value;
		mosaSettings.EmitStaticRelocations = EmtRelocs.IsChecked!.Value;
		mosaSettings.EmitDwarf = EmtDwarf.IsChecked!.Value;

		mosaSettings.BaseAddress = BaseAddrTxt.Text!.StartsWith("0x")
			? uint.Parse(BaseAddrTxt.Text[2..], NumberStyles.HexNumber)
			: uint.Parse(BaseAddrTxt.Text);

		mosaSettings.NasmFile = NasmFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		mosaSettings.AsmFile = AsmFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		mosaSettings.MapFile = MapFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		mosaSettings.DebugFile = DbgFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		mosaSettings.InlinedFile = InlLstFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		mosaSettings.PreLinkHashFile = HashFiles.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		mosaSettings.PostLinkHashFile = HashFiles.IsChecked.Value ? "%DEFAULT%" : string.Empty;
		mosaSettings.CompileTimeFile = CompTimeFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;

		mosaSettings.LauncherExit = ExitOnLaunch.IsChecked!.Value;

		mosaSettings.EmulatorGDB = QemuGdb.IsChecked!.Value;
		mosaSettings.LaunchGDB = LaunchGdb.IsChecked!.Value;
		mosaSettings.LaunchDebugger = MosaDbger.IsChecked!.Value;

		mosaSettings.Multithreading = MultiThreading.IsChecked!.Value;
		mosaSettings.MaxThreads = 0;
		mosaSettings.MethodScanner = MethodScanner.IsChecked!.Value;

		mosaSettings.EmulatorMemory = (int)MemVal.Value!;
		mosaSettings.EmulatorCores = (int)CpuVal.Value!;

		mosaSettings.MultibootVideo = EnableMbGraphics.IsChecked!.Value;
		mosaSettings.MultibootVideoWidth = (int)GraphicsWidth.Value!;
		mosaSettings.MultibootVideoHeight = (int)GraphicsHeight.Value!;

		mosaSettings.PlugKorlib = PlugKorlib.IsChecked!.Value;
		mosaSettings.OSName = OsNameTxt.Text!;

		UpdatePaths();

		mosaSettings.ImageFormat = ImgCmb.SelectedIndex switch
		{
			0 => "img",
			1 => "vhd",
			2 => "vdi",
			3 => "vmdk",
			_ => mosaSettings.ImageFormat
		};

		mosaSettings.Emulator = EmuCmb.SelectedIndex switch
		{
			0 => "Qemu",
			1 => "Bochs",
			2 => "VMware",
			3 => "VirtualBox",
			_ => mosaSettings.Emulator
		};

		mosaSettings.EmulatorSerial = CntCmb.SelectedIndex switch
		{
			0 => "None",
			1 => "Pipe",
			2 => "TCPServer",
			3 => "TCPClient",
			_ => mosaSettings.EmulatorSerial
		};

		mosaSettings.FileSystem = FsCmb.SelectedIndex switch
		{
			0 => "FAT12",
			1 => "FAT16",
			2 => "FAT32",
			_ => mosaSettings.FileSystem
		};

		mosaSettings.MultibootVersion = FmtCmb.SelectedIndex switch
		{
			0 => string.Empty,
			1 => "v2",
			_ => mosaSettings.MultibootVersion
		};

		mosaSettings.Platform = PltCmb.SelectedIndex switch
		{
			0 => "x86",
			1 => "x64",
			2 => "ARM32",
			_ => mosaSettings.Platform
		};

		mosaSettings.ImageFirmware = FrmCmb.SelectedIndex switch
		{
			0 => "bios",
			_ => mosaSettings.ImageFirmware
		};
	}

	private void UpdatePaths()
	{
		var src = SrcLbl.Content!.ToString()!;
		mosaSettings.ClearSourceFiles();
		mosaSettings.AddSourceFile(src);

		var path = Path.GetDirectoryName(src);
		if (!string.IsNullOrEmpty(path))
		{
			mosaSettings.ClearSearchPaths();
			mosaSettings.AddSearchPath(path);
		}

		mosaSettings.ImageFolder = DstLbl.Content!.ToString()!;
		mosaSettings.FileSystemRootInclude = IncDirTxt.Text!;
	}

	private void CompileBuildAndStart()
	{
		OutputTxt.Clear();
		CountersTxt.Clear();

		stopwatch.Start();
		timer.Start();

		ThreadPool.QueueUserWorkItem(_ =>
		{
			try
			{
				builder = new Builder(mosaSettings, compilerHooks);
				builder.Build();
			}
			catch (Exception e)
			{
				Dispatcher.UIThread.Post(() => AddOutput(e.ToString()));
			}
			finally
			{
				if (builder!.IsSucccessful) Dispatcher.UIThread.Post(CompileCompleted);
			}
		});
	}

	private void CompileCompleted()
	{
		if (!mosaSettings.Launcher) return;

		foreach (var line in builder!.Counters) AddCounters(line);

		var starter = new Starter(builder.MosaSettings, compilerHooks, builder.Linker);
		if (!starter.Launch(true)) return;

		if (mosaSettings.LauncherExit) Close();

		timer.Stop();
		stopwatch.Stop();
	}

	private void LnchBtn_OnClick(object? sender, RoutedEventArgs e)
	{
		Tabs.SelectedIndex = 4;
		UpdateSettings();

		var result = CheckOptions.Verify(mosaSettings);

		if (string.IsNullOrEmpty(result)) CompileBuildAndStart();
		else AddOutput($"ERROR: {result}");
	}

	private async void SrcBtn_OnClick(object? sender, RoutedEventArgs e)
	{
		var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
		{
			Title = "Select the file to compile",
			AllowMultiple = false
		});

		if (files.Count == 0) return;

		SrcLbl.Content = files[0].Path.LocalPath;
	}

	private async void DstBtn_OnClick(object? sender, RoutedEventArgs e)
	{
		var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
		{
			Title = "Select the output directory",
			AllowMultiple = false
		});

		if (folders.Count == 0) return;

		DstLbl.Content = folders[0].Path.LocalPath;
	}

	private async void IncBtn_OnClick(object? sender, RoutedEventArgs e)
	{
		var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
		{
			Title = "Select the include directory",
			AllowMultiple = false
		});

		if (folders.Count == 0) return;

		IncDirTxt.Text = folders[0].Path.LocalPath;
	}
}
