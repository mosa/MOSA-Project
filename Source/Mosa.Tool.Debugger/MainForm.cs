/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Mosa.Utility.DebugEngine;

namespace Mosa.Tool.Debugger
{
	public partial class MainForm : Form
	{
		public DebugServerEngine DebugEngine = new DebugServerEngine();

		ConnectionProperties connectionProperties = new ConnectionProperties();
		DispatchOutput dispatchOutput = new DispatchOutput();

		public string Status { set { this.toolStripStatusLabel1.Text = value; } }

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			dockPanel.SuspendLayout(true);
			connectionProperties.Show(dockPanel, DockState.DockLeft);
			dispatchOutput.Show(connectionProperties.Pane, DockAlignment.Bottom, 0.40);
			dockPanel.ResumeLayout(true, true);
			DebugEngine.SetDispatchMethod(this, Dispatch);
		}

		public void SignalConnect()
		{
			foreach (var content in this.dockPanel.Contents)
			{
				var debugContent = content as DebuggerDockContent;

				if (debugContent != null)
				{
					debugContent.Connect();
				}
			}
		}

		public void SignalDisconnect()
		{
			foreach (var content in this.dockPanel.Contents)
			{
				var debugContent = content as DebuggerDockContent;

				if (debugContent != null)
				{
					debugContent.Disconnect();
				}
			}
		}

		public void Dispatch(DebugMessage response)
		{
			if (response == null)
				return;

			dispatchOutput.BeginInvoke((SenderMesseageDelegate)dispatchOutput.ProcessResponses, new object[] { response });

			if (response.Sender is Form)
			{
				(response.Sender as Form).BeginInvoke(response.SenderMethod, new object[] { response });
			}
			else if (response.Sender is DockContent)
			{
				(response.Sender as DockContent).BeginInvoke(response.SenderMethod, new object[] { response });
			}

		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			connectionProperties.Show(dockPanel, DockState.DockLeft);
		}

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			toolStripStatusLabel1.Text = "Sending Ping...";
			DebugEngine.SendCommand(new DebugMessage(Codes.Ping, (byte[])null));
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{

			var memoryView = new MemoryView();
			memoryView.Show(dockPanel, DockState.Document);
		}
	}
}
