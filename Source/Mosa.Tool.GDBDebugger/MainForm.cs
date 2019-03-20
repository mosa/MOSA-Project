// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Framework;
using Mosa.Tool.GDBDebugger.DebugData;
using Mosa.Tool.GDBDebugger.GDB;
using Mosa.Tool.GDBDebugger.Views;
using Mosa.Utility.BootImage;
using Mosa.Utility.Launcher;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.GDBDebugger
{
	public partial class MainForm : Form, IStarterEvent
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

		//private ScriptView scriptView;

		public string Status { set { toolStripStatusLabel1.Text = value; toolStrip1.Refresh(); } }

		public Connector GDBConnector { get; private set; }
		public MemoryCache MemoryCache { get; private set; }

		public LauncherOptions LauncherOptions { get; } = new LauncherOptions();

		public AppLocations AppLocations { get; } = new AppLocations();

		public DebugSource DebugSource { get; set; } = new DebugSource();

		public List<BreakPoint> BreakPoints { get; } = new List<BreakPoint>();

		public List<Watch> Watchs { get; } = new List<Watch>();

		private Process VMProcess;

		public string VMHash;

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

			//scriptView = new ScriptView(this);

			sourceView = new SourceView(this);
			sourceDataView = new SourceDataView(this);

			AppLocations.FindApplications();
			LauncherOptions.EnableQemuGDB = true;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Text = "MOSA GDB Debugger v" + CompilerVersion.Version;

			dockPanel.SuspendLayout(true);
			dockPanel.DockTopPortion = 75;

			controlView.Show(dockPanel, DockState.DockTop);
			statusView.Show(controlView.PanelPane, DockAlignment.Right, 0.50);

			breakPointView.Show(dockPanel, DockState.DockBottom);
			watchView.Show(breakPointView.PanelPane, DockAlignment.Right, 0.50);

			displayView.Show(dockPanel, DockState.Document);
			outputView.Show(dockPanel, DockState.Document);

			//scriptView.Show(dockPanel, DockState.Document);
			registersView.Show(dockPanel, DockState.DockRight);
			stackFrameView.Show(registersView.Pane, DockAlignment.Bottom, 0.5);

			sourceView.Show(dockPanel, DockState.Document);
			sourceDataView.Show(dockPanel, DockState.Document);

			//stackFrameView.Show(dockPanel, DockState.DockRight);

			var memoryView = new MemoryView(this);
			memoryView.Show(dockPanel, DockState.Document);
			symbolView.Show(dockPanel, DockState.Document);

			instructionView.Show(symbolView.PanelPane, DockAlignment.Right, 0.35);
			methodView.Show(instructionView.PanelPane, instructionView);
			callStackView.Show(instructionView.PanelPane, DockAlignment.Bottom, 0.25);

			registersView.Show();

			dockPanel.ResumeLayout(true, true);

			CalculateVMHash();

			LauncherOptions.SerialConnectionOption = SerialConnectionOption.TCPServer;
			LauncherOptions.SerialConnectionPort = 1250;

			if (LauncherOptions.ImageFile != null)
			{
				VMProcess = StartQEMU();
			}
			LoadDebugFile();
			if (LauncherOptions.AutoStart)
			{
				System.Threading.Thread.Sleep(3000);
				Connect();
			}

			LoadBreakPoints();
			LoadWatches();
		}

		void IStarterEvent.NewStatus(string status)
		{
			MethodInvoker method = () => NewStatus(status);

			Invoke(method);
		}

		private void NewStatus(string info)
		{
			AddOutput(info);
		}

		private void LoadDebugFile()
		{
			if (LauncherOptions.DebugFile != null && File.Exists(LauncherOptions.DebugFile))
			{
				DebugSource = new DebugSource();
				LoadDebugData.LoadDebugInfo(LauncherOptions.DebugFile, DebugSource);
			}
		}

		public void AddOutput(string line)
		{
			outputView.AddOutput(line);
		}

		private void OnPause()
		{
			MethodInvoker method = NotifyPause;

			BeginInvoke(method);
		}

		private void OnRunning()
		{
			MethodInvoker method = NotifyRunning;

			BeginInvoke(method);
		}

		private void NotifyPause()
		{
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
			using (var connect = new ConnectWindow(LauncherOptions))
			{
				if (connect.ShowDialog(this) == DialogResult.OK)
				{
					Connect();
				}
			}
		}

		private void Connect()
		{
			if (GDBConnector != null)
			{
				GDBConnector.Disconnect();
				MemoryCache = null;
			}

			if (LauncherOptions.GDBPort == 0)
			{
				LauncherOptions.GDBPort = 1234;
			}

			GDBConnector = new Connector(new X86Platform(), LauncherOptions.GDBHost, LauncherOptions.GDBPort);

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

		public void AddBreakPoint(ulong address)
		{
			string name = CreateBreakPointName(address);
			var breakpoint = new BreakPoint(name, address);
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
			var cliParser = new Parser(config => config.HelpWriter = Console.Out);

			cliParser.ParseArguments(() => LauncherOptions, args);
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			using (var debug = new DebugAppLocationsWindow(AppLocations))
			{
				if (debug.ShowDialog(this) == DialogResult.OK)
				{
				}
			}
		}

		private static ImageFormat GetFormat(string fileName)
		{
			switch (Path.GetExtension(fileName).ToLower())
			{
				case ".bin": return ImageFormat.BIN;
				case ".img": return ImageFormat.IMG;
				case ".iso": return ImageFormat.ISO;
			}

			return ImageFormat.NotSpecified;
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			if (odfVMImage.ShowDialog() == DialogResult.OK)
			{
				LauncherOptions.ImageFile = odfVMImage.FileName;
				LauncherOptions.ImageFormat = GetFormat(LauncherOptions.ImageFile);

				var debugFile = Path.Combine(
					Path.GetDirectoryName(LauncherOptions.ImageFile),
					Path.GetFileNameWithoutExtension(LauncherOptions.ImageFile)) + ".debug";

				if (File.Exists(debugFile))
				{
					LauncherOptions.DebugFile = debugFile;
				}

				var breakpointFile = Path.Combine(
					Path.GetDirectoryName(LauncherOptions.ImageFile),
					Path.GetFileNameWithoutExtension(LauncherOptions.ImageFile)) + ".breakpoints";

				if (File.Exists(debugFile))
				{
					LauncherOptions.BreakpointFile = breakpointFile;
				}

				var watchFile = Path.Combine(
					Path.GetDirectoryName(LauncherOptions.ImageFile),
					Path.GetFileNameWithoutExtension(LauncherOptions.ImageFile)) + ".watches";

				if (File.Exists(watchFile))
				{
					LauncherOptions.WatchFile = watchFile;
				}

				LauncherOptions.GDBPort++;

				CalculateVMHash();

				VMProcess = StartQEMU();
				LoadDebugFile();
				Connect();
				LoadBreakPoints();
				LoadWatches();
			}
		}

		private Process StartQEMU()
		{
			var starter = new Starter(LauncherOptions, AppLocations, this);

			return starter.LaunchVM();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (GDBConnector != null)
			{
				GDBConnector.Disconnect();
				GDBConnector = null;
			}

			if (VMProcess?.HasExited == false)
			{
				VMProcess.Kill();
			}
		}

		public void LoadBreakPoints()
		{
			if (LauncherOptions.BreakpointFile == null || !File.Exists(LauncherOptions.BreakpointFile))
				return;

			bool remap = false;

			foreach (var line in File.ReadAllLines(LauncherOptions.BreakpointFile))
			{
				if (string.IsNullOrEmpty(line))
					continue;

				if (line.StartsWith("#HASH: "))
				{
					if (LauncherOptions.ImageFile != null && File.Exists(LauncherOptions.ImageFile))
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
			if (LauncherOptions.WatchFile == null || !File.Exists(LauncherOptions.WatchFile))
				return;

			bool remap = false;

			foreach (var line in File.ReadAllLines(LauncherOptions.WatchFile))
			{
				if (string.IsNullOrEmpty(line))
					continue;

				if (line.StartsWith("#HASH: "))
				{
					if (LauncherOptions.ImageFile != null && File.Exists(LauncherOptions.ImageFile))
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

		private void CalculateVMHash()
		{
			VMHash = null;

			if (LauncherOptions.ImageFile != null && File.Exists(LauncherOptions.ImageFile))
			{
				VMHash = CalculateFileHash(LauncherOptions.ImageFile);
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
	}
}
