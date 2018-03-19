// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Threading;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.Views
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
			MemoryCache.Clear();
			GDBConnector.ClearAllBreakPoints();
			GDBConnector.Step();
			MainForm.ResendBreakPoints();
		}

		private void btnStepN_Click(object sender, EventArgs e)
		{
			if (GDBConnector.IsRunning)
				return;

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

			MemoryCache.Clear();

			if (MainForm.BreakPoints.Count != 0)
			{
				GDBConnector.ClearAllBreakPoints();
				GDBConnector.Step(true);

				while (GDBConnector.IsRunning)
				{
					Thread.Sleep(1);
				}

				MainForm.ResendBreakPoints();

				steps--;
			}

			if (steps == 0)
				return;

			GDBConnector.StepN(steps);
		}

		private void btnRestart_Click(object sender, EventArgs e)
		{
			// ???
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			if (GDBConnector.IsRunning)
				return;

			MemoryCache.Clear();

			if (MainForm.BreakPoints.Count != 0)
			{
				GDBConnector.ClearAllBreakPoints();
				GDBConnector.Step(true);

				while (GDBConnector.IsRunning)
				{
					Thread.Sleep(1);
				}

				MainForm.ResendBreakPoints();
			}

			GDBConnector.Continue();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			GDBConnector.Break();
			GDBConnector.GetRegisters();
		}
	}
}
