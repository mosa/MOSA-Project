// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.DebugEngine;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.Debugger
{
	public partial class DebuggerDockContent : DockContent
	{
		public DebuggerDockContent()
		{
			InitializeComponent();
		}

		public virtual void Connect()
		{
		}

		public virtual void Disconnect()
		{
		}

		public MainForm MainForm { get { return ((this.ParentForm) as MainForm); } }

		public DebugServerEngine DebugEngine { get { return MainForm.DebugEngine; } }

		public string Status { set { MainForm.Status = value; } }

		public bool SendCommand(DebugMessage message)
		{
			return DebugEngine.SendCommand(message);
		}
	}
}
