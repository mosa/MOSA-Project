// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Security.Cryptography;
using Mosa.Compiler.Framework;
using Mosa.Tool.Debugger.DebugData;
using Mosa.Tool.Debugger.GDB;
using Mosa.Tool.Debugger.Views;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.Debugger;

public partial class MainForm : Form
{
	public readonly OutputView outputView;

	private readonly RegisterView registersView;

	private readonly DisplayView displayView;
	private readonly ControlView controlView;

	private readonly CallStackView callStackView;
	private readonly StackFrameView stackFrameView;

	private readonly StatusView statusView;

	private readonly InstructionView instructionView;

	private readonly SymbolView symbolView;
	private readonly WatchView watchView;
	private readonly BreakpointView breakPointView;
	private readonly MethodView methodView;

	private readonly SourceView sourceView;
	private readonly SourceDataView sourceDataView;

	private readonly LaunchView launchView;
	private readonly MethodParametersView methodParametersView;

	private readonly TraceView traceView;

	//private ScriptView scriptView;

	public Connector GDBConnector { get; private set; }

	public MemoryCache MemoryCache { get; private set; }

	public MosaSettings MosaSettings { get; private set; } = new MosaSettings();

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public DebugSource DebugSource { get; set; } = new DebugSource();

	public List<BreakPoint> BreakPoints { get; } = new List<BreakPoint>();

	public List<Watch> Watchs { get; } = new List<Watch>();

	public BasePlatform Platform => GDBConnector?.Platform;

	public ulong InstructionPointer => Platform == null ? 0 : Platform.InstructionPointer.Value;

	public ulong StackFrame => Platform == null ? 0 : Platform.StackFrame.Value;

	public ulong StackPointer => Platform == null ? 0 : Platform.StackPointer.Value;

	public ulong StatusFlag => Platform == null ? 0 : Platform.StatusFlag.Value;

	private Process VMProcess;

	public string VMHash;

	private SimpleTCP SimpleTCP;

	private Stopwatch Stopwatch = new Stopwatch();

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public string Status
	{
		set { toolStripStatusLabel1.Text = value; toolStrip1.Refresh(); }
	}

	public MainForm()
	{
		InitializeComponent();

		MosaSettings.LoadAppLocations();
		MosaSettings.SetDefaultSettings();

		SetDefaultSettings();

		outputView = new OutputView(this);
		registersView = new RegisterView(this);
		displayView = new DisplayView(this);
		controlView = new ControlView(this);
		traceView = new TraceView(this);
		callStackView = new CallStackView(this);
		stackFrameView = new StackFrameView(this);
		statusView = new StatusView(this);
		symbolView = new SymbolView(this);
		watchView = new WatchView(this);
		breakPointView = new BreakpointView(this);
		instructionView = new InstructionView(this);
		methodView = new MethodView(this);
		methodParametersView = new MethodParametersView(this);
		//scriptView = new ScriptView(this);
		sourceView = new SourceView(this);
		sourceDataView = new SourceDataView(this);  // only useful when debugging this tool
		launchView = new LaunchView(this);

		AppDomain.CurrentDomain.DomainUnload += (s, e) => { TerminateAll(); };
		AppDomain.CurrentDomain.ProcessExit += (s, e) => { TerminateAll(); };
		AppDomain.CurrentDomain.UnhandledException += (s, e) => { TerminateAll(); };
	}

	private void SetDefaultSettings()
	{
		MosaSettings.ImageFile = null;
		MosaSettings.EmulatorGDB = true;
		MosaSettings.EmulatorSerial = "TCPServer";
		MosaSettings.EmulatorSerialPort = 1250;
		MosaSettings.EmulatorDisplay = true;
		MosaSettings.GDBPort = 1234;
	}

	private void SetRequiredSettings()
	{
		MosaSettings.EmulatorGDB = true;
		MosaSettings.LauncherSerial = true;
		MosaSettings.LauncherExit = false;
	}

	public void LoadArguments(string[] args)
	{
		MosaSettings.LoadArguments(args);
		MosaSettings.NormalizeSettings();
		SetRequiredSettings();
	}

	private void MainForm_Load(object sender, EventArgs e)
	{
		Text = "MOSA GDB Debugger v" + CompilerVersion.VersionString;

		dockPanel.SuspendLayout(true);
		dockPanel.Theme = new VS2015DarkTheme();
		dockPanel.DockTopPortion = 88;

		controlView.Show(dockPanel, DockState.DockTop);
		statusView.Show(controlView.PanelPane, DockAlignment.Right, 0.50);

		breakPointView.Show(dockPanel, DockState.DockBottom);
		watchView.Show(breakPointView.PanelPane, DockAlignment.Right, 0.50);

		launchView.Show(dockPanel, DockState.Document);
		displayView.Show(dockPanel, DockState.Document);
		traceView.Show(dockPanel, DockState.Document);
		outputView.Show(dockPanel, DockState.Document);

		//scriptView.Show(dockPanel, DockState.Document);
		registersView.Show(dockPanel, DockState.DockRight);
		stackFrameView.Show(registersView.Pane, DockAlignment.Bottom, 0.5);

		sourceView.Show(dockPanel, DockState.Document);

		sourceDataView.Show(dockPanel, DockState.Document);

		var memoryView = new MemoryView(this);
		memoryView.Show(dockPanel, DockState.Document);

		symbolView.Show(dockPanel, DockState.Document);

		methodParametersView.Show(symbolView.PanelPane, DockAlignment.Right, 0.35);

		instructionView.Show(methodParametersView.PanelPane, DockAlignment.Bottom, 0.85);
		methodView.Show(instructionView.PanelPane, instructionView);

		callStackView.Show(instructionView.PanelPane, DockAlignment.Bottom, 0.25);

		registersView.Show();
		launchView.Show();

		dockPanel.ResumeLayout(true, true);

		if (!string.IsNullOrEmpty(MosaSettings.ImageFile))
		{
			LaunchImage();
		}
	}

	private void InvokeNotifyStatus(string status)
	{
		Invoke((MethodInvoker)(() => NotifyStatus(status)));
	}

	private void NotifyStatus(string status)
	{
		outputView.LogEvent($"{Stopwatch.Elapsed.TotalSeconds:00.00} | {status}");
	}

	private void LoadDebugFile()
	{
		if (MosaSettings.DebugFile != null && File.Exists(MosaSettings.DebugFile))
		{
			DebugSource = new DebugSource();
			LoadDebugData.LoadDebugInfo(MosaSettings.DebugFile, DebugSource);
		}
	}

	private void OnPause() => Invoke((MethodInvoker)(() => NotifyPause()));

	private void OnRunning() => Invoke((MethodInvoker)(() => NotifyRunning()));

	private void NotifyPause()
	{
		DeleteTemporaryBreakPonts();

		foreach (var dock in dockPanel.Contents)
		{
			if (dock.DockHandler.Content is DebugDockContent debugdock)
			{
				debugdock.UpdateDockFocus();
				debugdock.OnPause();
			}
		}
	}

	private void NotifyRunning()
	{
		foreach (var dock in dockPanel.Contents)
		{
			if (dock.DockHandler.Content is DebugDockContent debugdock)
			{
				debugdock.OnRunning();
			}
		}
	}

	private void NotifyBreakPointChange()
	{
		foreach (var dock in dockPanel.Contents)
		{
			if (dock.DockHandler.Content is DebugDockContent debugdock)
			{
				debugdock.OnBreakpointChange();
			}
		}
	}

	private void NotifyWatchChange()
	{
		foreach (var dock in dockPanel.Contents)
		{
			if (dock.DockHandler.Content is DebugDockContent debugdock)
			{
				debugdock.OnWatchChange();
			}
		}
	}

	private static bool IsHexDigitsOnly(string str)
	{
		foreach (var c in str)
		{
			if (!((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f')))
				return false;
		}

		return true;
	}

	public static char[] separators = new char[] { '\t', ' ', ',', '[', ']' };

	public static ulong ParseAddress(string decode)
	{
		var parts = decode.Split(separators);

		foreach (var part in parts)
		{
			if (part.Length <= 6)
				continue;

			var address = ParseHexAddress(part);

			if (address > 0)
				return address;
		}

		return 0;
	}

	public static ulong ParseHexAddress(string input)
	{
		var nbr = input.ToLowerInvariant().Trim().Trim(',').Trim('[').Trim('[');

		var where = nbr.IndexOf('x');

		if (where >= 0)
		{
			nbr = nbr.Substring(where + 1);
		}

		if (nbr.EndsWith("h"))
		{
			nbr = nbr.Substring(0, nbr.Length - 1);
		}

		if (nbr.Length == 0)
			return 0;

		if (!IsHexDigitsOnly(nbr))
			return 0;

		return Convert.ToUInt64(nbr, 16);
	}

	private void btnConnect_Click(object sender, EventArgs e)
	{
		using (var connect = new ConnectWindow(MosaSettings))
		{
			if (connect.ShowDialog(this) == DialogResult.OK)
			{
				ConnectGDB();
			}
		}
	}

	private void ConnectGDB()
	{
		DisconnectGDB();

		if (MosaSettings.GDBPort == 0)
		{
			MosaSettings.GDBPort = 1234;
		}

		GDBConnector = new Connector(new X86Platform(), MosaSettings.GDBHost, MosaSettings.GDBPort);

		GDBConnector.Connect();
		GDBConnector.OnPause = OnPause;
		GDBConnector.OnRunning = OnRunning;

		if (!GDBConnector.IsConnected)
		{
			MessageBox.Show($"Could not connect to '{GDBConnector.ConnectionHost}' on port {GDBConnector.ConnectionPort}.");
			return;
		}

		GDBConnector.GDBClient.LogEvent = LogGDBEvent;

		GDBConnector.ExtendedMode();
		GDBConnector.ClearAllBreakPoints();
		ResendBreakPoints();
		MemoryCache = new MemoryCache(GDBConnector);
	}

	private void LogGDBEvent(string info)
	{
		//if (info.Length > 100)
		//	info = $"{info[..100]} ...";

		//LogEvent($"GDB >> {info}");
		//Debug.WriteLine($"GDB >> {info}");
	}

	private void DisconnectGDB()
	{
		if (GDBConnector != null)
		{
			GDBConnector.Disconnect();
			GDBConnector = null;
			MemoryCache = null;
		}
	}

	private void btnViewMemory_Click(object sender, EventArgs e)
	{
		var memoryView = new MemoryView(this);
		memoryView.Show(dockPanel, DockState.Document);
	}

	public void ResendBreakPoints()
	{
		foreach (var breakpoint in BreakPoints)
		{
			GDBConnector.AddBreakPoint(breakpoint.Address);
		}
	}

	public string CreateBreakPointName(ulong address)
	{
		var list = DebugSource.GetSymbolsStartingAt(address);

		if (list?.Count >= 1)
		{
			return list[0].CommonName;
		}

		return GetAddressInfo(address);
	}

	public string CreateWatchName(ulong address)
	{
		var list = DebugSource.GetSymbolsStartingAt(address);

		if (list?.Count >= 1)
		{
			return list[0].CommonName;
		}

		return string.Empty;
	}

	public void AddBreakPoint(BreakPoint breakpoint)
	{
		if (!BreakPoints.Any(b => b.Address == breakpoint.Address))
		{
			BreakPoints.Add(breakpoint);
			GDBConnector.AddBreakPoint(breakpoint.Address);
			NotifyBreakPointChange();
		}
	}

	public void AddBreakPoint(ulong address, string name, string description = null)
	{
		if (string.IsNullOrWhiteSpace(name))
			name = CreateBreakPointName(address);

		var breakpoint = new BreakPoint(name, address, description);
		AddBreakPoint(breakpoint);
	}

	public void AddBreakPoint(ulong address, bool temporary = false)
	{
		string name = CreateBreakPointName(address);
		var breakpoint = new BreakPoint(name, address, temporary);
		AddBreakPoint(breakpoint);
	}

	public void RemoveBreakPoint(BreakPoint breakpoint)
	{
		BreakPoints.Remove(breakpoint);
		GDBConnector.ClearBreakPoint(breakpoint.Address);
		NotifyBreakPointChange();
	}

	public void DeleteAllBreakPonts()
	{
		BreakPoints.Clear();
		GDBConnector.ClearAllBreakPoints();
		NotifyBreakPointChange();
	}

	public void DeleteTemporaryBreakPonts()
	{
		if (Platform == null)
			return;

		if (BreakPoints.Count == 0)
			return;

		var temps = new List<BreakPoint>();

		foreach (var breakpoint in BreakPoints)
		{
			if (breakpoint.Temporary && breakpoint.Address == InstructionPointer)
			{
				temps.Add(breakpoint);
			}
		}

		foreach (var breakpoint in temps)
		{
			BreakPoints.Remove(breakpoint);
			GDBConnector.ClearBreakPoint(breakpoint.Address);
		}

		if (temps.Count != 0)
		{
			NotifyBreakPointChange();
		}
	}

	public void AddWatch(Watch watch)
	{
		Watchs.Add(watch);
		NotifyWatchChange();
	}

	public void AddWatch(string name, ulong address, uint size, bool signed = false)
	{
		if (string.IsNullOrWhiteSpace(name))
			name = CreateWatchName(address);

		var watch = new Watch(name, address, size, signed);
		AddWatch(watch);
	}

	public void RemoveWatch(Watch watch)
	{
		Watchs.Remove(watch);
		NotifyWatchChange();
	}

	public void RemoveAllWatches()
	{
		Watchs.Clear();
		NotifyWatchChange();
	}

	public void OnAddBreakPoint(Object sender, EventArgs e)
	{
		var args = (sender as ToolStripMenuItem).Tag as AddBreakPointArgs;

		if (string.IsNullOrWhiteSpace(args.Name))
		{
			AddBreakPoint(args.Address);
		}
		else
		{
			AddBreakPoint(args.Address, args.Name);
		}
	}

	public void OnCopyToClipboardAsBreakPoint(Object sender, EventArgs e)
	{
		var text = (((sender as ToolStripMenuItem).Tag) as BreakPoint).Name;

		Clipboard.SetText(text);
	}

	public void OnCopyToClipboard(Object sender, EventArgs e)
	{
		var text = (((sender as ToolStripMenuItem).Tag) as string);

		Clipboard.SetText(text);
	}

	public void OnRemoveBreakPoint(Object sender, EventArgs e)
	{
		var breakpoint = (sender as ToolStripMenuItem).Tag as BreakPoint;

		RemoveBreakPoint(breakpoint);
	}

	public void OnAddWatch(Object sender, EventArgs e)
	{
		var args = (sender as ToolStripMenuItem).Tag as AddWatchArgs;

		AddWatch(args.Name, args.Address, args.Length);
	}

	public void OnRemoveWatch(Object sender, EventArgs e)
	{
		var watch = (sender as ToolStripMenuItem).Tag as Watch;

		RemoveWatch(watch);
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
	{
		using (var debug = new DebugAppLocationsWindow(this))
		{
			if (debug.ShowDialog(this) == DialogResult.OK)
			{
			}
		}
	}

	private static string GetFormat(string fileName)
	{
		return Path.GetExtension(fileName).ToLowerInvariant() switch
		{
			".bin" => "BIN",
			".img" => "IMG",
			".iso" => "ISO",
			_ => string.Empty,
		};
	}

	public void LaunchImage(string imageFile, string debugFile, string breakpointFile, string watchFile)
	{
		MosaSettings.ImageFile = imageFile;
		MosaSettings.ImageFormat = GetFormat(imageFile);

		MosaSettings.DebugFile = debugFile;
		MosaSettings.BreakpointFile = breakpointFile;
		MosaSettings.WatchFile = watchFile;

		LaunchImage();
	}

	public void LaunchImage(bool skipImports = false)
	{
		Stopwatch.Start();

		DisconnectGDB();
		TerminateAll();

		CalculateVMHash();

		StartVM();

		ConnectGDB();

		if (!skipImports)
		{
			LoadDebugFile();
			LoadBreakPoints();
			LoadWatches();
		}

		displayView.Show();
	}

	private void TerminateAll()
	{
		DisconnectGDB();

		if (SimpleTCP != null)
		{
			SimpleTCP.Disconnect();
			SimpleTCP = null;
		}

		if (VMProcess != null)
		{
			if (!VMProcess.HasExited)
			{
				VMProcess.Kill(true);
				VMProcess.WaitForExit();
			}

			VMProcess = null;
		}
	}

	private void StartVM()
	{
		TerminateAll();

		MosaSettings.ResolveDefaults();
		SetRequiredSettings();
		MosaSettings.ResolveFileAndPathSettings();
		MosaSettings.AddStandardPlugs();
		MosaSettings.ExpandSearchPaths();

		var compilerHooks = new CompilerHooks
		{
			NotifyStatus = InvokeNotifyStatus
		};

		var starter = new Starter(MosaSettings, compilerHooks);

		VMProcess = starter.LaunchVM();

		if (VMProcess == null)
			return;

		SimpleTCP = new SimpleTCP();

		SimpleTCP.OnStatusUpdate = InvokeNotifyStatus;

		SimpleTCP.OnDataAvailable = () =>
		{
			while (SimpleTCP != null && SimpleTCP.HasLine)
			{
				var line = SimpleTCP.GetLine();

				lock (this)
				{
					InvokeNotifyStatus(line);
				}

				if (line == "##KILL##")
				{
					TerminateAll();
				}
			}
		};

		try
		{
			VMProcess.Start();

			Thread.Sleep(50); // wait a bit for the process to start

			if (!SimpleTCP.Connect(MosaSettings.EmulatorSerialHost, MosaSettings.EmulatorSerialPort, 10000))
			{
				NotifyStatus("Error: Unable to connect to serial port");
				return;
			}

			NotifyStatus("VM Output");
			NotifyStatus("========================");
		}
		catch (Exception ex)
		{
			NotifyStatus($"Exception: {ex}");
		}
	}

	private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		TerminateAll();
	}

	public void LoadBreakPoints()
	{
		if (MosaSettings.BreakpointFile == null || !File.Exists(MosaSettings.BreakpointFile))
			return;

		var remap = false;

		foreach (var line in File.ReadAllLines(MosaSettings.BreakpointFile))
		{
			if (string.IsNullOrEmpty(line))
				continue;

			if (line.StartsWith("#HASH: "))
			{
				if (MosaSettings.ImageFile != null && File.Exists(MosaSettings.ImageFile))
				{
					var hash = line.Substring(7).Trim();

					remap = VMHash != hash;
				}
				continue;
			}

			if (line.StartsWith("#"))
				continue;

			var parts = line.Split('\t');

			if (parts.Length == 0)
				continue;

			var address = ParseHexAddress(parts[0]);
			var symbol = parts.Length >= 2 ? parts[1] : null;

			if (symbol != null && remap)
			{
				if (symbol.StartsWith("0x") && symbol.Contains('+'))
					continue;

				address = DebugSource.GetFirstSymbolByName(symbol);

				if (address == 0)
					continue;
			}

			AddBreakPoint(address, symbol);
		}
	}

	public void LoadWatches()
	{
		if (MosaSettings.WatchFile == null || !File.Exists(MosaSettings.WatchFile))
			return;

		var remap = false;

		foreach (var line in File.ReadAllLines(MosaSettings.WatchFile))
		{
			if (string.IsNullOrEmpty(line))
				continue;

			if (line.StartsWith("#HASH: "))
			{
				if (MosaSettings.ImageFile != null && File.Exists(MosaSettings.ImageFile))
				{
					var hash = line.Substring(7).Trim();

					remap = VMHash != hash;
				}
				continue;
			}

			if (line.StartsWith("#"))
				continue;

			var parts = line.Split('\t');

			if (parts.Length < 2)
				continue;

			var address = ParseHexAddress(parts[0]);
			var size = parts.Length >= 2 ? Convert.ToUInt32(parts[1]) : 0;
			var symbol = parts.Length >= 3 ? parts[2] : null;

			if (symbol != null && remap)
			{
				if (symbol.StartsWith("0x") && symbol.Contains('+'))
					continue;

				address = DebugSource.GetFirstSymbolByName(symbol);

				if (address == 0)
					continue;
			}

			AddWatch(symbol, address, size);
		}
	}

	public static ulong ToLong(byte[] bytes)
	{
		return ToLong(bytes, 0, (uint)bytes.Length);
	}

	public static ulong ToLong(byte[] bytes, uint size)
	{
		return ToLong(bytes, 0, size);
	}

	public static ulong ToLong(byte[] bytes, uint start, uint size)
	{
		var value = 0ul;

		for (var i = 0; i < size; i++)
		{
			var shifted = (ulong)(bytes[start + i] << (i * 8));
			value |= shifted;
		}

		return value;
	}

	private void CalculateVMHash()
	{
		VMHash = null;

		if (MosaSettings.ImageFile != null && File.Exists(MosaSettings.ImageFile))
		{
			VMHash = CalculateFileHash(MosaSettings.ImageFile);
		}
	}

	public static string CalculateFileHash(string filename)
	{
		using (var md5 = MD5.Create())
		{
			using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				var hash = md5.ComputeHash(stream);
				return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
			}
		}
	}

	public string GetAddressInfo(ulong address)
	{
		if (address < 4096)
			return null;

		var symbol = DebugSource.GetFirstSymbol(address);

		if (symbol != null)
		{
			int delta = (int)(address - symbol.Address);

			if (delta > 1024 * 16)
				return null;

			if (delta == 0)
				return symbol.CommonName;

			return $"0x{delta.ToString("X2")}+{symbol.CommonName}";
		}

		return null;
	}

	public void SetFocus(ulong instructionPointer, ulong stackFrame, ulong stackPointer)
	{
		sourceView.InstructionPointer = instructionPointer;
		sourceView.StackFrame = stackFrame;
		sourceView.StackPointer = stackPointer;

		methodView.InstructionPointer = instructionPointer;
		methodView.StackFrame = stackFrame;
		methodView.StackPointer = stackPointer;

		instructionView.InstructionPointer = instructionPointer;
		instructionView.StackFrame = stackFrame;
		instructionView.StackPointer = stackPointer;

		methodParametersView.InstructionPointer = instructionPointer;
		methodParametersView.StackFrame = stackFrame;
		methodParametersView.StackPointer = stackPointer;

		stackFrameView.InstructionPointer = instructionPointer;
		stackFrameView.StackFrame = stackFrame;
		stackFrameView.StackPointer = stackPointer;

		methodView.OnPause();
		instructionView.OnPause();
		sourceView.OnPause();
		methodParametersView.OnPause();
		stackFrameView.OnPause();
	}
}
