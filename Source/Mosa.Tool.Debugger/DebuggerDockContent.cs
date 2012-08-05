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

		public virtual void OnConnect() { }
		public virtual void OnDisconnect() { }

		public DebugServerEngine DebugEngine { get { return ((this.ParentForm) as MainForm).DebugEngine; } }

		public bool SendCommand(DebugMessage message)
		{
			return DebugEngine.SendCommand(message);
		}
	}
}
