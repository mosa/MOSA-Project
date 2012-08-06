using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Mosa.Utility.DebugEngine;

namespace Mosa.Tool.Debugger
{
	public partial class DebuggerDockContent : DockContent
	{
		public DebuggerDockContent()
		{
			InitializeComponent();
		}

		public virtual void Connect() { }
		public virtual void Disconnect() { }

		public MainForm MainForm { get { return ((this.ParentForm) as MainForm); } }

		public DebugServerEngine DebugEngine { get { return MainForm.DebugEngine; } }

		public string Status { set { MainForm.Status = value; } }

		public bool SendCommand(DebugMessage message)
		{
			return DebugEngine.SendCommand(message);
		}

	}
}
