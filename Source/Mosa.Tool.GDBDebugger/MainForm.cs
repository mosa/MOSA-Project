// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.GDBDebugger
{
	public partial class MainForm : Form
	{
		//private AssembliesView assembliesView;
		//private RegisterView registersView;
		//private DisplayView displayView;
		//private ControlView controlView;
		//private CallStackView callStackView;
		//private StackFrameView stackFrameView;
		//private StackView stackView;
		//private FlagView flagView;
		//private StatusView statusView;
		//private HistoryView historyView;
		//private SymbolView symbolView;
		//private WatchView watchView;
		//private BreakPointView breakPointView;
		//private OutputView outputView;
		//private ScriptView scriptView;
		//private DisassemblyView disassemblyView;

		public string Status { set { toolStripStatusLabel1.Text = value; toolStrip1.Refresh(); } }

		public bool Display32 { get; private set; }

		public MainForm()
		{
			InitializeComponent();

			//assembliesView = new AssembliesView(this);
			//registersView = new RegisterView(this);
			//displayView = new DisplayView(this);
			//controlView = new ControlView(this);
			//callStackView = new CallStackView(this);
			//stackFrameView = new StackFrameView(this);
			//stackView = new StackView(this);
			//flagView = new FlagView(this);
			//statusView = new StatusView(this);
			//historyView = new HistoryView(this);
			//symbolView = new SymbolView(this);
			//watchView = new WatchView(this);
			//breakPointView = new BreakPointView(this);
			//outputView = new OutputView(this);
			//scriptView = new ScriptView(this);
			//disassemblyView = new DisassemblyView(this);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			dockPanel.SuspendLayout(true);
			dockPanel.DockTopPortion = 150;

			//statusView.Show(dockPanel, DockState.DockTop);
			//controlView.Show(statusView.PanelPane, DockAlignment.Right, 0.50);
			//callStackView.Show(controlView.PanelPane, DockAlignment.Bottom, 0.50);
			//disassemblyView.Show(statusView.PanelPane, DockAlignment.Right, 0.50);

			//breakPointView.Show(dockPanel, DockState.DockBottom);
			//watchView.Show(breakPointView.PanelPane, DockAlignment.Right, 0.50);

			//displayView.Show(dockPanel, DockState.Document);
			//historyView.Show(dockPanel, DockState.Document);
			//assembliesView.Show(dockPanel, DockState.Document);
			//outputView.Show(dockPanel, DockState.Document);
			//scriptView.Show(dockPanel, DockState.Document);
			//symbolView.Show(dockPanel, DockState.Document);

			//registersView.Show(dockPanel, DockState.DockRight);
			//flagView.Show(dockPanel, DockState.DockRight);
			//stackView.Show(dockPanel, DockState.DockRight);
			//stackFrameView.Show(dockPanel, DockState.DockRight);

			//registersView.Show();

			dockPanel.ResumeLayout(true, true);
		}

		public static string Format(object value)
		{
			if (value is string)
				return value as string;
			else if (value is uint)
				return "0x" + ((uint)value).ToString("X8");
			else if (value is int)
				return "0x" + ((int)value).ToString("X8");
			else if (value is ulong)
				return "0x" + ((ulong)value).ToString("X16");
			else if (value is long)
				return "0x" + ((long)value).ToString("X16");
			else if (value is double)
				return ((double)value).ToString();
			else if (value is float)
				return ((float)value).ToString();
			else if (value is bool)
				return (bool)value ? "TRUE" : "FALSE";

			return value.ToString();
		}

		public static string Format(object value, bool display32)
		{
			if (display32)
			{
				if (value is ulong)
					return "0x" + ((ulong)value).ToString("X8");
				else if (value is long)
					return "0x" + ((long)value).ToString("X8");
			}

			return Format(value);
		}

		public void UpdateAllDocks()
		{
			foreach (var dock in dockPanel.Contents)
			{
				if (dock.DockHandler.Content is DebugDockContent)
				{
					(dock.DockHandler.Content as DebugDockContent).UpdateDock();
				}
			}
		}
	}
}
