// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.View
{
	public partial class ControlView : DebugDockContent
	{
		public ControlView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		private void btnStep_Click(object sender, EventArgs e)
		{
			GDBConnector.Step();
		}

		private void btnStepN_Click(object sender, EventArgs e)
		{
			uint steps = 0;
			try
			{
				steps = Convert.ToUInt32(tbSteps.Text);
			}
			catch
			{
				MessageBox.Show($"Invalid input, '{tbSteps.Text}' is not a valid number.");
				return;
			}
			GDBConnector.StepN(steps);
		}

		private void btnRestart_Click(object sender, EventArgs e)
		{
			GDBConnector.Restart();
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			GDBConnector.Continue();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			GDBConnector.Break();
		}
	}
}
