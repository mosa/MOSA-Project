// Copyright (c) MOSA Project. Licensed under the New BSD License.

using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.GDBDebugger
{
	public partial class DebugDockContent : DockContent
	{
		protected MainForm MainForm;

		public DebugDockContent()
		{ }

		public DebugDockContent(MainForm mainForm)
		{
			InitializeComponent();
			MainForm = mainForm;
		}

		public string Status { set { MainForm.Status = value; } }

		public virtual void UpdateDock()
		{ }
	}
}
