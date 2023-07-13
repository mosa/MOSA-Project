// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;
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
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;

namespace Mosa.Tool.Launcher;

// TODO: Implement settings for FmtCmb and FrmCmb
public partial class MainWindow : Window
{
	private readonly MosaSettings MosaSettings = new MosaSettings();

	private Builder Builder;
	private readonly OpenFileDialog Source;
	private readonly OpenFolderDialog Destination;
	private readonly OpenFolderDialog Include;

	private bool IsDone;
	private int TotalMethods;
	private int CompletedMethods;

	private Stopwatch Stopwatch = new Stopwatch();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	public MainWindow()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{
		InitializeComponent();

		var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1) };
		timer.Tick += (_, _) =>
		{
			OutputProgress.Maximum = TotalMethods;
			OutputProgress.Value = CompletedMethods;
		};
		timer.Start();

		Source = new OpenFileDialog
		{
			AllowMultiple = false,
			Title = "Select the file to compile"
		};

		Destination = new OpenFolderDialog
		{
			Title = "Select the output directory"
		};

		Include = new OpenFolderDialog
		{
			Title = "Select the include directory"
		};

		DstLbl.Content = Path.Combine(Path.GetTempPath(), "MOSA");

		Stopwatch.Reset();
	}

	private void NotifyProgress(int total, int at)
	{
		TotalMethods = total;
		CompletedMethods = at;
	}

	private void AddOutput(string status)
	{
		if (string.IsNullOrEmpty(status))
			return;

		OutputTxt.Text += $"{Stopwatch.Elapsed.TotalSeconds:00.00} | {status}{Environment.NewLine}";

		//<Setter Property="FontFamily" Value="Consolas"/>
	}

	public void Initialize(string[] args)
	{
		MosaSettings.LoadAppLocations();
		MosaSettings.SetDetfaultSettings();
		MosaSettings.LoadArguments(args);
		SetRequiredSettings();
		MosaSettings.ExpandSearchPaths();
		MosaSettings.NormalizeSettings();
		MosaSettings.UpdateFileAndPathSettings();

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

		if (MosaSettings.SourceFiles != null)
		{
			var src = MosaSettings.SourceFiles[0];

			if (src != null)
			{
				OsNameTxt.Text = Path.GetFileNameWithoutExtension(src);
				SrcLbl.Content = Path.GetFullPath(src);
			}

			foreach (var file in MosaSettings.SourceFiles)
			{
				var path = Path.GetDirectoryName(Path.GetFullPath(file));

				if (!string.IsNullOrWhiteSpace(path))
				{
					MosaSettings.SearchPaths.Add(path);
				}
			}
		}
		else
		{
			MosaSettings.ExplorerStart = false;
		}

		IncDirTxt.Text = MosaSettings.FileSystemRootInclude;

		// Set title label with the current compiler version
		TitleLbl.Content += CompilerVersion.VersionString;

		// Register all platforms supported by the compiler
		PlatformRegistry.Add(new Platform.x86.Architecture());
		PlatformRegistry.Add(new Platform.x64.Architecture());
		PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());

		// Initialize paths
		UpdatePaths();

		UpdateInterfaceAppLocations();

		if (MosaSettings.LauncherStart)
		{
			Tabs.SelectedIndex = 4;
			CompileBuildAndStart();
		}

		// Hacky method to center the window on the screen
		this.GetObservable(IsVisibleProperty).Subscribe(value =>
		{
			if (!value || IsDone)
				return;

			IsDone = true;
			CenterWindow();
		});
	}

	private void SetRequiredSettings()
	{
		MosaSettings.EmulatorDisplay = true;
		MosaSettings.LauncherStart = true;
		MosaSettings.Launcher = true;
		MosaSettings.LauncherExit = true;
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
		bochsPathLbl.Content = MosaSettings.BochsApp;
		ndiasmPathLbl.Content = MosaSettings.NdisasmApp;

		qemuPathLbl.Content = MosaSettings.QEMUApp;
		qemuBiosPathLbl.Content = MosaSettings.QEMUBios;
		qemuEdk2X86PathLbl.Content = MosaSettings.QEMUEdk2X86;
		qemuEdk2X64PathLbl.Content = MosaSettings.QEMUEdk2X64;
		qemuEdk2ARMPathLbl.Content = MosaSettings.QEMUEdk2ARM;
		qemuImgPathLbl.Content = MosaSettings.QemuImgApp;
		vboxPathLbl.Content = MosaSettings.VirtualBoxApp;
		mkisofsPathLbl.Content = MosaSettings.MkisofsApp;

		vmwarePathLbl.Content = string.IsNullOrEmpty(MosaSettings.VmwarePlayerApp) ? MosaSettings.VmwareWorkstationApp : MosaSettings.VmwarePlayerApp;
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

		EnableVbe.IsChecked = MosaSettings.MultibootVideo;
		VbeWidth.Value = MosaSettings.MultibootVideoWidth;
		VbeHeight.Value = MosaSettings.MultibootVideoHeight;
		VbeDepth.Value = MosaSettings.MultibootVideoDepth;

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

		PltCmb.SelectedIndex = MosaSettings.Platform.ToLowerInvariant() switch
		{
			"x86" => 0,
			"x64" => 1,
			"armv8a32" => 2,
			_ => 0
		};

		FrmCmb.SelectedIndex = MosaSettings.ImageFirmware.ToLowerInvariant() switch
		{
			"bios" => 0,
			_ => 0
		};
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

		MosaSettings.BaseAddress = BaseAddrTxt.Text.StartsWith("0x")
			? uint.Parse(BaseAddrTxt.Text[2..], NumberStyles.HexNumber)
			: uint.Parse(BaseAddrTxt.Text);

		MosaSettings.NasmFile = NasmFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		MosaSettings.AsmFile = AsmFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
		MosaSettings.MapFile = MapFile.IsChecked!.Value ? "%DEFAULT%" : string.Empty;
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

		MosaSettings.EmulatorMemory = (int)MemVal.Value;
		MosaSettings.EmulatorCores = (int)CpuVal.Value;

		MosaSettings.MultibootVideo = EnableVbe.IsChecked!.Value;
		MosaSettings.MultibootVideoWidth = (int)VbeWidth.Value;
		MosaSettings.MultibootVideoHeight = (int)VbeHeight.Value;
		MosaSettings.MultibootVideoDepth = (int)VbeDepth.Value;

		MosaSettings.PlugKorlib = PlugKorlib.IsChecked!.Value;
		MosaSettings.OSName = OsNameTxt.Text;

		UpdatePaths();

		MosaSettings.ImageFormat = ImgCmb.SelectedIndex switch
		{
			0 => "img",
			1 => "vhd",
			2 => "vdi",
			3 => "vmdk",
			_ => string.Empty,
		};

		MosaSettings.Emulator = EmuCmb.SelectedIndex switch
		{
			0 => "Qemu",
			1 => "Bochs",
			2 => "VMware",
			3 => "VirtualBox",
			_ => string.Empty,
		};

		MosaSettings.EmulatorSerial = CntCmb.SelectedIndex switch
		{
			0 => "None",
			1 => "Pipe",
			2 => "TCPServer",
			3 => "TCPClient",
			_ => string.Empty,
		};

		MosaSettings.FileSystem = FsCmb.SelectedIndex switch
		{
			0 => "FAT12",
			1 => "FAT16",
			2 => "FAT32",
			_ => string.Empty,
		};

		MosaSettings.Platform = PltCmb.SelectedIndex switch
		{
			0 => "x86",
			1 => "x64",
			2 => "ARMv8A32",
			_ => string.Empty,
		};

		MosaSettings.ImageFirmware = FrmCmb.SelectedIndex switch
		{
			0 => "bios",
			_ => string.Empty,
		};
	}

	private void UpdatePaths()
	{
		var src = SrcLbl.Content.ToString();

		MosaSettings.ClearSourceFiles();
		MosaSettings.AddSourceFile(src);

		MosaSettings.ClearSearchPaths();
		MosaSettings.AddSearchPath(Path.GetDirectoryName(src));

		MosaSettings.ImageFolder = DstLbl.Content.ToString();
		MosaSettings.FileSystemRootInclude = IncDirTxt.Text;
	}

	private void CompileBuildAndStart()
	{
		OutputTxt.Clear();
		CountersTxt.Clear();

		Stopwatch.Start();

		ThreadPool.QueueUserWorkItem(_ =>
		{
			try
			{
				Builder = new Builder(MosaSettings, CreateCompilerHook());
				Builder.Build();
			}
			catch (Exception e)
			{
				Dispatcher.UIThread.Post(() => AddOutput(e.ToString()));
			}
			finally
			{
				if (Builder.IsSucccessful)
					Dispatcher.UIThread.Post(CompileCompleted);
			}
		});
	}

	private void CompileCompleted()
	{
		if (!MosaSettings.LauncherStart)
			return;

		foreach (var line in Builder.Counters)
			AddCounters(line);

		var starter = new Starter(Builder.MosaSettings, CreateCompilerHook(), Builder.Linker);

		if (!starter.Launch(true))
			return;

		if (MosaSettings.LauncherExit)
			Close();

		Stopwatch.Stop();
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

		var result = CheckOptions.Verify(MosaSettings);

		if (result == null)
			CompileBuildAndStart();
		else
			AddOutput($"ERROR: {result}");
	}

	private async void SrcBtn_OnClick(object? _, RoutedEventArgs __)
	{
		var result = await Source.ShowAsync(this);
		if (result == null)
			return;

		SrcLbl.Content = result[0];
	}

	private async void DstBtn_OnClick(object? _, RoutedEventArgs __)
	{
		var result = await Destination.ShowAsync(this);
		if (result == null)
			return;

		DstLbl.Content = result;
	}

	private async void IncBtn_OnClick(object? _, RoutedEventArgs __)
	{
		var result = await Include.ShowAsync(this);
		if (result == null)
			return;

		IncDirTxt.Text = result;
	}
}
