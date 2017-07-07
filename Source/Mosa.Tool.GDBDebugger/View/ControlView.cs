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
			GDBConnector.ClearAllBreakPoints();
			GDBConnector.Step();
			MainForm.ResendBreakPoints();
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

			if (steps == 0)
				return;

			GDBConnector.ClearAllBreakPoints();
			GDBConnector.Step();
			MainForm.ResendBreakPoints();

			if (steps <= 1)
				return;

			GDBConnector.StepN(steps - 1);
		}

		private void btnRestart_Click(object sender, EventArgs e)
		{
			// ???
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			GDBConnector.ClearAllBreakPoints();
			GDBConnector.Step();
			MainForm.ResendBreakPoints();

			GDBConnector.Continue();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			GDBConnector.Break();
		}
	}
}
