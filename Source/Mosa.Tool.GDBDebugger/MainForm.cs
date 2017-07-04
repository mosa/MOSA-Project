// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.DebugData;
using Mosa.Tool.GDBDebugger.GDB;
using Mosa.Tool.GDBDebugger.View;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.GDBDebugger
{
	public partial class MainForm : Form
	{
		private OutputView outputView;

		private RegisterView registersView;

		//private DisplayView displayView;
		private ControlView controlView;

		//private CallStackView callStackView;
		private StackFrameView stackFrameView;

		//private StackView stackView;
		//private FlagView flagView;
		private StatusView statusView;

		private InstructionView instructionView;

		private SymbolView symbolView;
		private WatchView watchView;
		private BreakPointView breakPointView;

		//private ScriptView scriptView;

		public string Status { set { toolStripStatusLabel1.Text = value; toolStrip1.Refresh(); } }

		public Connector GDBConnector { get; private set; }

		public Options Options { get; private set; } = new Options();

		public DebugSource DebugSource { get; private set; } = new DebugSource();

		public List<BreakPoint> BreakPoints { get; private set; } = new List<BreakPoint>();

		public List<Watch> Watchs { get; private set; } = new List<Watch>();

		public MainForm()
		{
			InitializeComponent();

			outputView = new OutputView(this);

			registersView = new RegisterView(this);

			//displayView = new DisplayView(this);
			controlView = new ControlView(this);

			//callStackView = new CallStackView(this);
			stackFrameView = new StackFrameView(this);

			//stackView = new StackView(this);
			//flagView = new FlagView(this);
			statusView = new StatusView(this);
			symbolView = new SymbolView(this);
			watchView = new WatchView(this);
			breakPointView = new BreakPointView(this);
			instructionView = new InstructionView(this);

			//scriptView = new ScriptView(this);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			dockPanel.SuspendLayout(true);
			dockPanel.DockTopPortion = 75;

			controlView.Show(dockPanel, DockState.DockTop);
			statusView.Show(controlView.PanelPane, DockAlignment.Right, 0.50);

			//callStackView.Show(controlView.PanelPane, DockAlignment.Bottom, 0.50);
			breakPointView.Show(dockPanel, DockState.DockBottom);
			watchView.Show(breakPointView.PanelPane, DockAlignment.Right, 0.50);

			//displayView.Show(dockPanel, DockState.Document);
			outputView.Show(dockPanel, DockState.Document);

			//scriptView.Show(dockPanel, DockState.Document);
			registersView.Show(dockPanel, DockState.DockRight);

			//flagView.Show(dockPanel, DockState.DockRight);
			//stackView.Show(dockPanel, DockState.DockRight);
			stackFrameView.Show(dockPanel, DockState.DockRight);

			var memoryView = new MemoryView(this);
			memoryView.Show(dockPanel, DockState.Document);
			symbolView.Show(dockPanel, DockState.Document);

			instructionView.Show(symbolView.PanelPane, DockAlignment.Right, 0.35);

			registersView.Show();

			dockPanel.ResumeLayout(true, true);

			if (Options.AutoConnect)
			{
				Connect();
			}

			if (Options.DebugInfoFile != null)
			{
				LoadDebugData.LoadDebugInfo(Options.DebugInfoFile, DebugSource);
			}
		}

		public void AddOutput(string line)
		{
			outputView.AddOutput(line);
		}

		private void OnPause()
		{
			MethodInvoker method = delegate ()
			{
				NotifyPause();
			};

			BeginInvoke(method);
		}

		private void OnRunning()
		{
			MethodInvoker method = delegate ()
			{
				NotifyRunning();
			};

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

		public ulong ParseMemoryAddress(string input)
		{
			string nbr = input.ToUpper().Trim();
			int digits = 10;
			int where = nbr.IndexOf('X');

			if (where >= 0)
			{
				digits = 16;
				nbr = nbr.Substring(where + 1);
			}

			var value = Convert.ToUInt64(nbr, digits);

			return value;
		}

		private void btnDebugQEMU_Click(object sender, EventArgs e)
		{
			using (DebugQemuWindow debug = new DebugQemuWindow(Options))
			{
				if (debug.ShowDialog(this) == DialogResult.OK)
				{
					Thread.Sleep(1000); //HACK: Wait for QEMU

					Connect(debug.Debugger);
				}
			}
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			using (ConnectWindow connect = new ConnectWindow())
			{
				if (connect.ShowDialog(this) == DialogResult.OK)
				{
					Connect(connect.Debugger);
				}
			}
		}

		private void Connect()
		{
			Connect(new Connector(new X86Platform(), "localhost", Options.GDBPort));
		}

		private void Connect(Connector connector)
		{
			if (GDBConnector != null)
				GDBConnector.Disconnect();

			GDBConnector = connector;

			GDBConnector.Connect();

			GDBConnector.OnPause = OnPause;
			GDBConnector.OnRunning = OnRunning;

			if (!GDBConnector.IsConnected)
			{
				MessageBox.Show($"Could not connect to '{GDBConnector.ConnectionHost}' on port {GDBConnector.ConnectionPort}.");
				return;
			}

			GDBConnector.ExtendedMode();
		}

		private void btnViewMemory_Click(object sender, EventArgs e)
		{
			var memoryView = new MemoryView(this);
			memoryView.Show(dockPanel, DockState.Document);
		}

		public void AddBreakPoint(BreakPoint breakpoint)
		{
			if(!BreakPoints.Any(b => b.Address == breakpoint.Address))
			{
				BreakPoints.Add(breakpoint);
				GDBConnector.AddBreakPoint(breakpoint.Address);
				NotifyBreakPointChange();
			}
		}

		public void AddBreakPoint(ulong address, string name = null)
		{
			var breakpoint = new BreakPoint(name, address);
			AddBreakPoint(breakpoint);
		}

		public void RemoveBreakPoint(BreakPoint breakpoint)
		{
			BreakPoints.Remove(breakpoint);
			GDBConnector.ClearBreakPoint(breakpoint.Address);
			NotifyBreakPointChange();
		}

		public void AddWatch(Watch watch)
		{
			Watchs.Add(watch);
			NotifyWatchChange();
		}

		public void AddWatch(string name, ulong address, int size, bool signed = false)
		{
			var watch = new Watch(name, address, size, signed);
			AddWatch(watch);
		}

		public void RemoveWatch(Watch watch)
		{
			Watchs.Remove(watch);
			NotifyWatchChange();
		}
	}
}
