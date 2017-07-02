// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.DebugData;
using Mosa.Tool.GDBDebugger.GDB;
using Mosa.Tool.GDBDebugger.View;
using System;
using System.Collections.Generic;
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

		private SymbolView symbolView;
<<<<<<< HEAD

=======
>>>>>>> b9cbaa3d13afc4e3398a9d152a78f5b4da65829e
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
<<<<<<< HEAD

=======
>>>>>>> b9cbaa3d13afc4e3398a9d152a78f5b4da65829e
			watchView = new WatchView(this);
			breakPointView = new BreakPointView(this);

			//scriptView = new ScriptView(this);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			dockPanel.SuspendLayout(true);
			dockPanel.DockTopPortion = 150;

			statusView.Show(dockPanel, DockState.DockTop);
			controlView.Show(statusView.PanelPane, DockAlignment.Right, 0.50);

			//callStackView.Show(controlView.PanelPane, DockAlignment.Bottom, 0.50);
			breakPointView.Show(dockPanel, DockState.DockBottom);
<<<<<<< HEAD

=======
>>>>>>> b9cbaa3d13afc4e3398a9d152a78f5b4da65829e
			watchView.Show(breakPointView.PanelPane, DockAlignment.Right, 0.50);

			//displayView.Show(dockPanel, DockState.Document);
			outputView.Show(dockPanel, DockState.Document);

			//scriptView.Show(dockPanel, DockState.Document);
			symbolView.Show(dockPanel, DockState.Document);
			registersView.Show(dockPanel, DockState.DockRight);

			//flagView.Show(dockPanel, DockState.DockRight);
			//stackView.Show(dockPanel, DockState.DockRight);
			stackFrameView.Show(dockPanel, DockState.DockRight);

			registersView.Show();

			var memoryView = new MemoryView(this);
			memoryView.Show(dockPanel, DockState.Document);

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

<<<<<<< HEAD
        private void btnConnect_Click(object sender, EventArgs e)
=======
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

		private void btnConnect_Click(object sender, EventArgs e)
>>>>>>> b9cbaa3d13afc4e3398a9d152a78f5b4da65829e
		{
            using (ConnectWindow connect = new ConnectWindow())
            {
                if (connect.ShowDialog(this) == DialogResult.OK)
                {
                    ConnectDebugger(connect.Debugger);
                }
            }
		}

<<<<<<< HEAD
        private void btnDebugQemu_Click(object sender, EventArgs e)
        {
            using (DebugQemuWindow debug = new DebugQemuWindow(Options))
            {
                if(debug.ShowDialog(this) == DialogResult.OK)
                {
                    //TODO: Put this somewhere + add event handler for user exit.
                    //debug.QEMUProcess

                    ConnectDebugger(debug.Debugger);
                }
            }
        }

        private void Connect()
        {
            ConnectDebugger(new Connector(new X86Platform()));
        }

        private void ConnectDebugger(Connector connector)
        {
            //Cursor?
            Cursor = Cursors.WaitCursor;

            if (GDBConnector != null)
                GDBConnector.Disconnect();

            if (connector.Connect())
            {
                GDBConnector = connector;

                GDBConnector.OnPause = OnPause;
                GDBConnector.OnRunning = OnRunning;

                Status = "Connected";
            }
            else
            {
                MessageBox.Show(this, "Failed to connect to remote!", "Connection error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor = Cursors.Default;
        }

        private void btnViewMemory_Click(object sender, EventArgs e)
=======
		private void Connect()
		{
			if (GDBConnector != null)
				GDBConnector.Disconnect();

			GDBConnector = new Connector(new X86Platform());

			GDBConnector.Connect(Options.GDBPort);

			GDBConnector.OnPause = OnPause;
			GDBConnector.OnRunning = OnRunning;

			if (!GDBConnector.IsConnected)
			{
				MessageBox.Show($"Could not connect to 'localhost' on port {Options.GDBPort}.");
			}
		}

		private void btnViewMemory_Click(object sender, EventArgs e)
>>>>>>> b9cbaa3d13afc4e3398a9d152a78f5b4da65829e
		{
			var memoryView = new MemoryView(this);
			memoryView.Show(dockPanel, DockState.Document);
		}

		public void AddBreakPoint(BreakPoint breakpoint)
		{
			BreakPoints.Add(breakpoint);
<<<<<<< HEAD
=======
			GDBConnector.AddBreakPoint(breakpoint.Address);
>>>>>>> b9cbaa3d13afc4e3398a9d152a78f5b4da65829e
			NotifyBreakPointChange();
		}

		public void AddBreakPoint(ulong address, string name = null)
		{
			var breakpoint = new BreakPoint(name, address);
			AddBreakPoint(breakpoint);
		}

		public void RemoveBreakPoint(BreakPoint breakpoint)
		{
			BreakPoints.Remove(breakpoint);
<<<<<<< HEAD
=======
			GDBConnector.ClearBreakPoint(breakpoint.Address);
>>>>>>> b9cbaa3d13afc4e3398a9d152a78f5b4da65829e
			NotifyBreakPointChange();
		}

		public void AddWatch(Watch watch)
		{
			Watchs.Add(watch);
<<<<<<< HEAD

=======
>>>>>>> b9cbaa3d13afc4e3398a9d152a78f5b4da65829e
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
<<<<<<< HEAD

=======
>>>>>>> b9cbaa3d13afc4e3398a9d152a78f5b4da65829e
			NotifyWatchChange();
		}
    }
}
