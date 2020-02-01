// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Tool.Debugger.DebugData;
using Mosa.Tool.Debugger.GDB;
using Mosa.Tool.Debugger.Views;
using Mosa.Utility.BootImage;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.Debugger
{
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

		//private ScriptView scriptView;

		public string Status { set { toolStripStatusLabel1.Text = value; toolStrip1.Refresh(); } }

		public Connector GDBConnector { get; private set; }

		public MemoryCache MemoryCache { get; private set; }

		public Settings Settings { get; }

		public DebugSource DebugSource { get; set; } = new DebugSource();

		public List<BreakPoint> BreakPoints { get; } = new List<BreakPoint>();

		public List<Watch> Watchs { get; } = new List<Watch>();

		public BasePlatform Platform { get { return GDBConnector?.Platform; } }

		public ulong InstructionPointer { get { return Platform == null ? 0 : Platform.InstructionPointer.Value; } }
		public ulong StackFrame { get { return Platform == null ? 0 : Platform.StackFrame.Value; } }
		public ulong StackPointer { get { return Platform == null ? 0 : Platform.StackPointer.Value; } }
		public ulong StatusFlag { get { return Platform == null ? 0 : Platform.StatusFlag.Value; } }

		private Process VMProcess;

		public string VMHash;

		#region Settings

		public string BreakpointFile
		{
			get { return Settings.GetValue("Debugger.BreakpointFile", null); }
			set { Settings.SetValue("Debugger.BreakpointFile", value); }
		}

		public string WatchFile
		{
			get { return Settings.GetValue("Debugger.WatchFile", null); }
			set { Settings.SetValue("Debugger.WatchFile", value); }
		}

		public string DebugFile
		{
			get { return Settings.GetValue("CompilerDebug.DebugFile", "%DEFAULT%"); }
			set { Settings.SetValue("CompilerDebug.DebugFile", value); }
		}

		public string ImageFile
		{
			get { return Settings.GetValue("Image.ImageFile", null); }
			set { Settings.SetValue("Image.ImageFile", value); }
		}

		public int GDBPort
		{
			get { return Settings.GetValue("GDB.Port", 0); }
			set { Settings.SetValue("GDB.Port", value); }
		}

		public string GDBHost
		{
			get { return Settings.GetValue("GDB.Host", "localhost"); }
			set { Settings.SetValue("GDB.Host", value); }
		}

		public string ImageFormat
		{
			get { return Settings.GetValue("Image.Format", null); }
			set { Settings.SetValue("Image.Format", value); }
		}

		public bool LauncherStart
		{
			get { return Settings.GetValue("Launcher.Start", false); }
			set { Settings.SetValue("Launcher.Start", value); }
		}

		public bool EmulatorDisplay
		{
			get { return Settings.GetValue("LEmulator.Display", false); }
			set { Settings.SetValue("Emulator.Display", value); }
		}

		public string QEMU
		{
			get { return Settings.GetValue("AppLocation.Qemu", null); }
			set { Settings.SetValue("AppLocation.Qemu", value); }
		}

		public string QEMUBios
		{
			get { return Settings.GetValue("AppLocation.QemuBIOS", null); }
			set { Settings.SetValue("AppLocation.QemuBIOS", value); }
		}

		#endregion Settings

		public MainForm()
		{
			InitializeComponent();

			outputView = new OutputView(this);

			registersView = new RegisterView(this);

			displayView = new DisplayView(this);
			controlView = new ControlView(this);

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
			sourceDataView = new SourceDataView(this);

			launchView = new LaunchView(this);

			Settings = AppLocationsSettings.GetAppLocations();

			Settings.SetValue("Emulator.GDB", true);
			Settings.SetValue("Emulator.Serial", "TCPServer");
			Settings.SetValue("Emulator.Serial.Port", 1250);
			Settings.SetValue("Emulator.Display", false);

			GDBPort = 1234;

			AppDomain.CurrentDomain.DomainUnload += (s, e) => { KillVMProcess(); };
			AppDomain.CurrentDomain.ProcessExit += (s, e) => { KillVMProcess(); };
			AppDomain.CurrentDomain.UnhandledException += (s, e) => { KillVMProcess(); };
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Text = "MOSA GDB Debugger v" + CompilerVersion.VersionString;

			dockPanel.SuspendLayout(true);
			dockPanel.DockTopPortion = 54;

			controlView.Show(dockPanel, DockState.DockTop);
			statusView.Show(controlView.PanelPane, DockAlignment.Right, 0.50);

			breakPointView.Show(dockPanel, DockState.DockBottom);
			watchView.Show(breakPointView.PanelPane, DockAlignment.Right, 0.50);

			launchView.Show(dockPanel, DockState.Document);
			displayView.Show(dockPanel, DockState.Document);
			outputView.Show(dockPanel, DockState.Document);

			//scriptView.Show(dockPanel, DockState.Document);
			registersView.Show(dockPanel, DockState.DockRight);
			stackFrameView.Show(registersView.Pane, DockAlignment.Bottom, 0.5);

			sourceView.Show(dockPanel, DockState.Document);
			sourceDataView.Show(dockPanel, DockState.Document);

			var memoryView = new MemoryView(this);
			memoryView.Show(dockPanel, DockState.Document);

			symbolView.Show(dockPanel, DockState.Document);

			instructionView.Show(symbolView.PanelPane, DockAlignment.Right, 0.35);
			methodView.Show(instructionView.PanelPane, instructionView);
			callStackView.Show(instructionView.PanelPane, DockAlignment.Bottom, 0.25);
			methodParametersView.Show(callStackView.Pane, callStackView);

			registersView.Show();
			launchView.Show();

			dockPanel.ResumeLayout(true, true);

			if (ImageFile != null)
			{
				LaunchImage();
			}
		}

		private void NotifyStatus(string status) => Invoke((MethodInvoker)(() => NewStatus(status)));

		private void NewStatus(string info)
		{
			outputView.AddOutput(info);
		}

		private void LoadDebugFile()
		{
			if (DebugFile != null && File.Exists(DebugFile))
			{
				DebugSource = new DebugSource();
				LoadDebugData.LoadDebugInfo(DebugFile, DebugSource);
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

		public ulong ParseHexAddress(string input)
		{
			string nbr = input.ToUpper().Trim();
			int digits = 10;
			int where = nbr.IndexOf('X');

			if (where >= 0)
			{
				digits = 16;
				nbr = nbr.Substring(where + 1);
			}

			return Convert.ToUInt64(nbr, digits);
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			using (var connect = new ConnectWindow(Settings))
			{
				if (connect.ShowDialog(this) == DialogResult.OK)
				{
					Connect();
				}
			}
		}

		private void Connect()
		{
			Disconnect();

			if (GDBPort == 0)
			{
				GDBPort = 1234;
			}

			GDBConnector = new Connector(new X86Platform(), GDBHost, GDBPort);

			GDBConnector.Connect();
			GDBConnector.OnPause = OnPause;
			GDBConnector.OnRunning = OnRunning;

			if (!GDBConnector.IsConnected)
			{
				MessageBox.Show($"Could not connect to '{GDBConnector.ConnectionHost}' on port {GDBConnector.ConnectionPort.ToString()}.");
				return;
			}

			GDBConnector.ExtendedMode();
			GDBConnector.ClearAllBreakPoints();
			ResendBreakPoints();
			MemoryCache = new MemoryCache(GDBConnector);
		}

		private void Disconnect()
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
			else
			{
				var first = DebugSource.GetFirstSymbol(address);

				if (first != null)
				{
					int delta = (int)(address - first.Address);
					return "0x" + delta.ToString("X2") + "+" + first.CommonName;
				}
			}

			return string.Empty;
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
			var args = (sender as Menu).Tag as AddBreakPointArgs;

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
			var text = (((sender as Menu).Tag) as BreakPoint).Name;

			Clipboard.SetText(text);
		}

		public void OnCopyToClipboard(Object sender, EventArgs e)
		{
			var text = (((sender as Menu).Tag) as string);

			Clipboard.SetText(text);
		}

		public void OnRemoveBreakPoint(Object sender, EventArgs e)
		{
			var breakpoint = (sender as Menu).Tag as BreakPoint;

			RemoveBreakPoint(breakpoint);
		}

		public void OnAddWatch(Object sender, EventArgs e)
		{
			var args = (sender as Menu).Tag as AddWatchArgs;

			AddWatch(args.Name, args.Address, args.Length);
		}

		public void OnRemoveWatch(Object sender, EventArgs e)
		{
			var watch = (sender as Menu).Tag as Watch;

			RemoveWatch(watch);
		}

		public void LoadArguments(string[] args)
		{
			var arguments = Reader.ParseArguments(args, CommandLineArguments.Map);

			Settings.Merge(arguments);

			//UpdateDisplay(Settings);
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
			switch (Path.GetExtension(fileName).ToLower())
			{
				case ".bin": return "BIN";
				case ".img": return "IMG";
				case ".iso": return "ISO";
				default: return string.Empty;
			}
		}

		public void LaunchImage(string imageFile, string debugFile, string breakpointFile, string watchFile)
		{
			ImageFile = imageFile;
			ImageFormat = GetFormat(imageFile);

			DebugFile = debugFile;
			BreakpointFile = breakpointFile;
			WatchFile = watchFile;

			LaunchImage();
		}

		public void LaunchImage(bool skipImports = false)
		{
			Disconnect();
			KillVMProcess();

			CalculateVMHash();

			StartQEMU();

			Connect();

			if (!skipImports)
			{
				LoadDebugFile();
				LoadBreakPoints();
				LoadWatches();
			}

			displayView.Show();
		}

		private void KillVMProcess()
		{
			if (VMProcess == null)
				return;

			if (!VMProcess.HasExited)
			{
				VMProcess.Kill();
				VMProcess.WaitForExit();
			}

			VMProcess = null;
		}

		private CompilerHooks CreateCompilerHooks()
		{
			var compilerHooks = new CompilerHooks
			{
				NotifyStatus = NotifyStatus
			};

			return compilerHooks;
		}

		private void StartQEMU()
		{
			var compilerHook = CreateCompilerHooks();

			var starter = new Starter(Settings, compilerHook);

			VMProcess = starter.LaunchVM();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Disconnect();

			KillVMProcess();
		}

		public void LoadBreakPoints()
		{
			if (BreakpointFile == null || !File.Exists(BreakpointFile))
				return;

			bool remap = false;

			foreach (var line in File.ReadAllLines(BreakpointFile))
			{
				if (string.IsNullOrEmpty(line))
					continue;

				if (line.StartsWith("#HASH: "))
				{
					if (ImageFile != null && File.Exists(ImageFile))
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
			if (WatchFile == null || !File.Exists(WatchFile))
				return;

			bool remap = false;

			foreach (var line in File.ReadAllLines(WatchFile))
			{
				if (string.IsNullOrEmpty(line))
					continue;

				if (line.StartsWith("#HASH: "))
				{
					if (ImageFile != null && File.Exists(ImageFile))
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
			ulong value = 0;

			for (int i = 0; i < size; i++)
			{
				ulong shifted = (ulong)(bytes[start + i] << (i * 8));
				value |= shifted;
			}

			return value;
		}

		private void CalculateVMHash()
		{
			VMHash = null;

			if (ImageFile != null && File.Exists(ImageFile))
			{
				VMHash = CalculateFileHash(ImageFile);
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

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
		}
	}
}
