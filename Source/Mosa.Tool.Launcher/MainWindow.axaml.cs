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
	private readonly MosaSettings MosaSettings = new();
	private readonly Stopwatch Stopwatch = new();
	private readonly CompilerHooks CompilerHooks;
	private readonly DispatcherTimer Timer;

	private Builder Builder;
	private int TotalMethods;
	private int CompletedMethods;

	public MainWindow()
	{
		InitializeComponent();

		Timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
		Timer.Tick += (_, _) =>
		{
			OutputProgress.Maximum = TotalMethods;
			OutputProgress.Value = CompletedMethods;
		};

		CompilerHooks = CreateCompilerHooks();
	}

	private CompilerHooks CreateCompilerHooks()
	{
		var compilerHooks = new CompilerHooks
		{
			NotifyProgress = NotifyProgress,
			NotifyStatus = NotifyStatus
		};

		return compilerHooks;
	}

	private void NotifyProgress(int total, int at)
	{
		TotalMethods = total;
		CompletedMethods = at;
	}

	private void OutputStatus(string status)
	{
		if (string.IsNullOrEmpty(status)) return;

		OutputTxt.Text += $"{Stopwatch.Elapsed.TotalSeconds:00.00} | {status}{Environment.NewLine}";
	}

	public void Initialize(string[] args)
	{
		MosaSettings.LoadAppLocations();
		MosaSettings.SetDefaultSettings();
		MosaSettings.LoadArguments(args);
		MosaSettings.NormalizeSettings();
		MosaSettings.ResolveDefaults();
		SetRequiredSettings();
		MosaSettings.ResolveFileAndPathSettings();
		MosaSettings.AddStandardPlugs();
		MosaSettings.ExpandSearchPaths();

		UpdateGuiSettings();

		var sb = new StringBuilder();

		foreach (var arg in args)
		{
			sb.Append(arg);
			sb.Append(' ');
		}

		OutputStatus($"Arguments: {sb}");
		//OutputStatus($"Current Directory: {Environment.CurrentDirectory}");

		if (MosaSettings.SourceFiles is { Count: > 0 })
		{
			var src = MosaSettings.SourceFiles[0];

			if (!string.IsNullOrEmpty(src))
			{
				OsNameTxt.Text = Path.GetFileNameWithoutExtension(src);
				SrcLbl.Content = Path.GetFullPath(src);
			}

			foreach (var file in MosaSettings.SourceFiles)
			{
				var path = Path.GetDirectoryName(Path.GetFullPath(file));
				if (!string.IsNullOrWhiteSpace(path))
				{
					MosaSettings.AddSearchPath(path);
				}
			}
		}
		else MosaSettings.ExplorerStart = false;

		IncDirTxt.Text = MosaSettings.FileSystemRootInclude;
		TitleLbl.Content += CompilerVersion.VersionString;

		PlatformRegistry.Add(new Compiler.x86.Architecture());
		PlatformRegistry.Add(new Compiler.x64.Architecture());
		PlatformRegistry.Add(new Compiler.ARM32.Architecture());

		UpdatePaths();
		UpdateInterfaceAppLocations();

		if (!MosaSettings.LauncherStart)
			return;

		Tabs.SelectedIndex = 4;

		var result = CheckOptions.Verify(MosaSettings);

		if (string.IsNullOrEmpty(result)) CompileBuildAndStart();
		else OutputStatus($"ERROR: {result}");
	}

	private void SetRequiredSettings()
	{
		MosaSettings.EmulatorDisplay = true;
		MosaSettings.Launcher = true;
		MosaSettings.LauncherExit = true;

		if (MosaSettings is { SourceFiles.Count: > 0, LauncherStart: true })
			MosaSettings.LauncherStart = true;
	}

	private void AddCounters(string data) => CountersTxt.Text += data + Environment.NewLine;

	private void NotifyStatus(string status) => Dispatcher.UIThread.Post(() => OutputStatus(status));

	private void UpdateInterfaceAppLocations()
	{
		qemuPathLbl.Content = MosaSettings.QemuX86App;
		qemuBiosPathLbl.Content = MosaSettings.QemuBIOS;
		qemuEdk2X86PathLbl.Content = MosaSettings.QemuEdk2X86;
		qemuEdk2X64PathLbl.Content = MosaSettings.QemuEdk2X64;
		qemuEdk2ARMPathLbl.Content = MosaSettings.QemuEdk2ARM32;
		qemuImgPathLbl.Content = MosaSettings.QemuImgApp;
		bochsPathLbl.Content = MosaSettings.BochsApp;
		vmwarePathLbl.Content = string.IsNullOrEmpty(MosaSettings.VmwarePlayerApp) ? MosaSettings.VmwareWorkstationApp : MosaSettings.VmwarePlayerApp;
		vboxPathLbl.Content = MosaSettings.VirtualBoxApp;
		mkisofsPathLbl.Content = MosaSettings.MkisofsApp;
		ndiasmPathLbl.Content = MosaSettings.NdisasmApp;
	}

	private void UpdateGuiSettings()
	{
		SsaOpts.IsChecked = MosaSettings.SSA;
		BasicOpts.IsChecked = MosaSettings.BasicOptimizations;
		SccpOpts.IsChecked = MosaSettings.SparseConditionalConstantPropagation;
		DevirtOpts.IsChecked = MosaSettings.Devirtualization;
		InlineOpts.IsChecked = MosaSettings.InlineMethods;
		InlineExplOpts.IsChecked = MosaSettings.InlineExplicit;
		LongExpOpts.IsChecked = MosaSettings.LongExpansion;
		TwoOptPass.IsChecked = MosaSettings.TwoPassOptimization;
		ValueNumOpts.IsChecked = MosaSettings.ValueNumbering;
		BtOpts.IsChecked = MosaSettings.BitTracker;
		PlatOpts.IsChecked = MosaSettings.PlatformOptimizations;
		LicmOpts.IsChecked = MosaSettings.LoopInvariantCodeMotion;

		EmtSymbs.IsChecked = MosaSettings.EmitSumbols;
		EmtRelocs.IsChecked = MosaSettings.EmitStaticRelocations;
		EmtDwarf.IsChecked = MosaSettings.EmitDwarf;
		BaseAddrTxt.Text = MosaSettings.BaseAddress.ToHex();

		NasmFile.IsChecked = !string.IsNullOrEmpty(MosaSettings.NasmFile);
		AsmFile.IsChecked = !string.IsNullOrEmpty(MosaSettings.AsmFile);
		MapFile.IsChecked = !string.IsNullOrEmpty(MosaSettings.MapFile);
		DbgFile.IsChecked = !string.IsNullOrEmpty(MosaSettings.DebugFile);
		InlLstFile.IsChecked = !string.IsNullOrEmpty(MosaSettings.InlinedFile);
		HashFiles.IsChecked = !string.IsNullOrEmpty(MosaSettings.PreLinkHashFile);
		CompTimeFile.IsChecked = !string.IsNullOrEmpty(MosaSettings.CompileTimeFile);

		ExitOnLaunch.IsChecked = MosaSettings.LauncherExit;

		QemuGdb.IsChecked = MosaSettings.LaunchGDB;
		LaunchGdb.IsChecked = MosaSettings.LaunchGDB;
		MosaDbger.IsChecked = MosaSettings.LaunchDebugger;

		MultiThreading.IsChecked = MosaSettings.Multithreading;
		MethodScanner.IsChecked = MosaSettings.MethodScanner;

		MemVal.Value = MosaSettings.EmulatorMemory;
		CpuVal.Value = MosaSettings.EmulatorCores;

		EnableMbGraphics.IsChecked = MosaSettings.MultibootVideo;
		GraphicsWidth.Value = MosaSettings.MultibootVideoWidth;
		GraphicsHeight.Value = MosaSettings.MultibootVideoHeight;

		PlugKorlib.IsChecked = MosaSettings.PlugKorlib;
		OsNameTxt.Text = MosaSettings.OSName;

		ImgCmb.SelectedIndex = MosaSettings.ImageFormat.ToUpperInvariant() switch
		{
			"IMG" => 0,
			"VHD" => 1,
			"VDI" => 2,
			"VMDK" => 3,
			_ => ImgCmb.SelectedIndex
		};

		EmuCmb.SelectedIndex = MosaSettings.Emulator switch
		{
			"Qemu" => 0,
			"Bochs" => 1,
			"VMware" => 2,
			"VirtualBox" => 3,
			_ => EmuCmb.SelectedIndex
		};

		CntCmb.SelectedIndex = MosaSettings.EmulatorSerial switch
		{
			"None" => 0,
			"Pipe" => 1,
			"TCPServer" => 2,
			"TCPClient" => 3,
			_ => CntCmb.SelectedIndex
		};

		FsCmb.SelectedIndex = MosaSettings.FileSystem.ToUpperInvariant() switch
		{
			"FAT12" => 0,
			"FAT16" => 1,
			"FAT32" => 2,
			_ => FsCmb.SelectedIndex
		};

		FmtCmb.SelectedIndex = MosaSettings.MultibootVersion.ToLowerInvariant() switch
		{
			"" => 0,
			"v2" => 1,
			_ => FmtCmb.SelectedIndex
		};

		PltCmb.SelectedIndex = MosaSettings.Platform.ToLowerInvariant() switch
		{
			"x86" => 0,
			"x64" => 1,
			"arm32" => 2,
			//"arm64" => 3,
			_ => PltCmb.SelectedIndex
		};

		FrmCmb.SelectedIndex = MosaSettings.ImageFirmware.ToLowerInvariant() switch
		{
			"bios" => 0,
			_ => FrmCmb.SelectedIndex
		};

		DstLbl.Content = MosaSettings.TemporaryFolder;
	}

	private void UpdateSettings()
	{
		MosaSettings.SSA = SsaOpts.IsChecked!.Value;
		MosaSettings.BasicOptimizations = BasicOpts.IsChecked!.Value;
		MosaSettings.SparseConditionalConstantPropagation = SccpOpts.IsChecked!.Value;
		MosaSettings.Devirtualization = DevirtOpts.IsChecked!.Value;
		MosaSettings.InlineMethods = InlineOpts.IsChecked!.Value;
		MosaSettings.InlineExplicit = InlineExplOpts.IsChecked!.Value;
		MosaSettings.LongExpansion = LongExpOpts.IsChecked!.Value;
		MosaSettings.TwoPassOptimization = TwoOptPass.IsChecked!.Value;
		MosaSettings.ValueNumbering = ValueNumOpts.IsChecked!.Value;
		MosaSettings.BitTracker = BtOpts.IsChecked!.Value;
		MosaSettings.PlatformOptimizations = PlatOpts.IsChecked!.Value;
		MosaSettings.LoopInvariantCodeMotion = LicmOpts.IsChecked!.Value;

		MosaSettings.EmitSumbols = EmtSymbs.IsChecked!.Value;
		MosaSettings.EmitStaticRelocations = EmtRelocs.IsChecked!.Value;
		MosaSettings.EmitDwarf = EmtDwarf.IsChecked!.Value;

		MosaSettings.BaseAddress = BaseAddrTxt.Text!.StartsWith("0x")
			? uint.Parse(BaseAddrTxt.Text[2..], NumberStyles.HexNumber)
			: uint.Parse(BaseAddrTxt.Text);

		MosaSettings.NasmFile = NasmFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		MosaSettings.AsmFile = AsmFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		MosaSettings.MapFile = MapFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		//mosaSettings.CounterFile = CounterFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		MosaSettings.DebugFile = DbgFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		MosaSettings.InlinedFile = InlLstFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		MosaSettings.PreLinkHashFile = HashFiles.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		MosaSettings.PostLinkHashFile = HashFiles.IsChecked.Value ? "%DEFAULT%" : string.Empty;
		MosaSettings.CompileTimeFile = CompTimeFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;

		MosaSettings.LauncherExit = ExitOnLaunch.IsChecked!.Value;

		MosaSettings.EmulatorGDB = QemuGdb.IsChecked!.Value;
		MosaSettings.LaunchGDB = LaunchGdb.IsChecked!.Value;
		MosaSettings.LaunchDebugger = MosaDbger.IsChecked!.Value;

		MosaSettings.Multithreading = MultiThreading.IsChecked!.Value;
		MosaSettings.MaxThreads = 0;
		MosaSettings.MethodScanner = MethodScanner.IsChecked!.Value;

		MosaSettings.EmulatorMemory = (int)MemVal.Value!;
		MosaSettings.EmulatorCores = (int)CpuVal.Value!;

		MosaSettings.MultibootVideo = EnableMbGraphics.IsChecked!.Value;
		MosaSettings.MultibootVideoWidth = (int)GraphicsWidth.Value!;
		MosaSettings.MultibootVideoHeight = (int)GraphicsHeight.Value!;

		MosaSettings.PlugKorlib = PlugKorlib.IsChecked!.Value;
		MosaSettings.OSName = OsNameTxt.Text!;

		UpdatePaths();

		MosaSettings.ImageFormat = ImgCmb.SelectedIndex switch
		{
			0 => "img",
			1 => "vhd",
			2 => "vdi",
			3 => "vmdk",
			_ => MosaSettings.ImageFormat
		};

		MosaSettings.Emulator = EmuCmb.SelectedIndex switch
		{
			0 => "Qemu",
			1 => "Bochs",
			2 => "VMware",
			3 => "VirtualBox",
			_ => MosaSettings.Emulator
		};

		MosaSettings.EmulatorSerial = CntCmb.SelectedIndex switch
		{
			0 => "None",
			1 => "Pipe",
			2 => "TCPServer",
			3 => "TCPClient",
			_ => MosaSettings.EmulatorSerial
		};

		MosaSettings.FileSystem = FsCmb.SelectedIndex switch
		{
			0 => "FAT12",
			1 => "FAT16",
			2 => "FAT32",
			_ => MosaSettings.FileSystem
		};

		MosaSettings.MultibootVersion = FmtCmb.SelectedIndex switch
		{
			0 => string.Empty,
			1 => "v2",
			_ => MosaSettings.MultibootVersion
		};

		MosaSettings.Platform = PltCmb.SelectedIndex switch
		{
			0 => "x86",
			1 => "x64",
			2 => "ARM32",
			_ => MosaSettings.Platform
		};

		MosaSettings.ImageFirmware = FrmCmb.SelectedIndex switch
		{
			0 => "bios",
			_ => MosaSettings.ImageFirmware
		};
	}

	private void UpdatePaths()
	{
		var src = SrcLbl.Content!.ToString()!;
		MosaSettings.ClearSourceFiles();
		MosaSettings.AddSourceFile(src);

		var path = Path.GetDirectoryName(src);
		if (!string.IsNullOrEmpty(path))
		{
			MosaSettings.ClearSearchPaths();
			MosaSettings.AddSearchPath(path);
		}

		MosaSettings.ImageFolder = DstLbl.Content!.ToString()!;
		MosaSettings.FileSystemRootInclude = IncDirTxt.Text!;
	}

	private void CompileBuildAndStart()
	{
		OutputTxt.Clear();
		CountersTxt.Clear();

		Stopwatch.Start();
		Timer.Start();

		ThreadPool.QueueUserWorkItem(_ =>
		{
			try
			{
				Builder = new Builder(MosaSettings, CompilerHooks);
				Builder.Build();
			}
			catch (Exception e)
			{
				Dispatcher.UIThread.Post(() => OutputStatus(e.ToString()));
			}
			finally
			{
				if (Builder!.IsSucccessful) Dispatcher.UIThread.Post(CompileCompleted);
			}
		});
	}

	private void CompileCompleted()
	{
		if (!MosaSettings.Launcher)
			return;

		foreach (var line in Builder!.Counters) AddCounters(line);

		var starter = new Starter(Builder.MosaSettings, CompilerHooks, Builder.Linker);

		if (!starter.Launch(true))
			return;

		if (MosaSettings.LauncherExit) Close();

		Timer.Stop();
		Stopwatch.Stop();
	}

	private void LnchBtn_OnClick(object sender, RoutedEventArgs e)
	{
		Tabs.SelectedIndex = 4;
		UpdateSettings();

		var result = CheckOptions.Verify(MosaSettings);

		if (string.IsNullOrEmpty(result))
		{
			CompileBuildAndStart();
		}
		else
		{
			OutputStatus($"ERROR: {result}");
		}
	}

	private async void SrcBtn_OnClick(object sender, RoutedEventArgs e)
	{
		var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
		{
			Title = "Select the file to compile",
			AllowMultiple = false
		});

		if (files.Count == 0) return;

		SrcLbl.Content = files[0].Path.LocalPath;
	}

	private async void DstBtn_OnClick(object sender, RoutedEventArgs e)
	{
		var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
		{
			Title = "Select the output directory",
			AllowMultiple = false
		});

		if (folders.Count == 0) return;

		DstLbl.Content = folders[0].Path.LocalPath;
	}

	private async void IncBtn_OnClick(object sender, RoutedEventArgs e)
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
