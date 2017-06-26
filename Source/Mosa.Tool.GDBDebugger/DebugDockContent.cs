// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.GDB;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.GDBDebugger
{
	public partial class DebugDockContent : DockContent
	{
		protected MainForm MainForm;

		public DebugDockContent()
		{
			InitializeComponent();
		}

		public DebugDockContent(MainForm mainForm) : this()
		{
			MainForm = mainForm;
		}

		public string Status { set { MainForm.Status = value; } }
		public Connector GDBConnector { get { return MainForm.GDBConnector; } }
		public BasePlatform Platform { get { return (GDBConnector == null ? null : GDBConnector.Platform); } }

		public bool IsRunning { get { return GDBConnector.IsRunning; } }
		public bool IsPaused { get { return GDBConnector.IsPaused; } }

		public virtual void OnPause()
		{
		}

		public virtual void OnRunning()
		{
		}
	}
}
