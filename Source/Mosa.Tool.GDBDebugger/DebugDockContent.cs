// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.DebugData;
using Mosa.Tool.GDBDebugger.GDB;
using Mosa.Utility.Launcher;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.GDBDebugger
{
	public partial class DebugDockContent : DockContent
	{
		protected MainForm MainForm;

		protected LauncherOptions Options { get { return MainForm.LauncherOptions; } }

		public DebugDockContent()
		{
			InitializeComponent();
		}

		public DebugDockContent(MainForm mainForm) : this()
		{
			MainForm = mainForm;
		}

		public string Status { set { MainForm.Status = value; } }

		public DebugSource DebugSource { get { return MainForm.DebugSource; } }

		public Connector GDBConnector { get { return MainForm.GDBConnector; } }
		public BasePlatform Platform { get { return GDBConnector?.Platform; } }
		public MemoryCache MemoryCache { get { return MainForm.MemoryCache; } }

		public bool IsConnected { get { return GDBConnector?.IsConnected ?? false; } }
		public bool IsRunning { get { return GDBConnector.IsRunning; } }
		public bool IsPaused { get { return GDBConnector.IsPaused; } }

		public virtual void OnPause()
		{
		}

		public virtual void OnRunning()
		{
		}

		public virtual void OnBreakpointChange()
		{
		}

		public virtual void OnWatchChange()
		{
		}
	}
}
