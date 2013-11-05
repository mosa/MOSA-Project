/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.TinyCPUSimulator;
using System;

namespace Mosa.Tool.Simulator
{
	public partial class HistoryView : SimulatorDockContent
	{
		private bool enable = true;

		public HistoryView()
		{
			InitializeComponent();
		}

		public override void UpdateDock(SimState simState)
		{
			if (!enable)
				return;

			if (simState == null)
				return;

			if (!MainForm.Record)
				return;

			lbHistory.Items.Clear();

			foreach (var entry in MainForm.history)
			{
				lbHistory.Items.Add(entry);
			}

			lbHistory.SelectedIndex = lbHistory.Items.Count - 1;

			this.Refresh();
		}

		private void Select(SimState simState)
		{
			if (simState == null)
				return;

			enable = false;

			MainForm.UpdateAllDocks(simState);

			enable = true;
		}

		private void lbHistory_DoubleClick(object sender, EventArgs e)
		{
			Select(lbHistory.SelectedItem as SimState);
		}

		private void lbHistory_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			Select(lbHistory.SelectedItem as SimState);
		}

		private void lbHistory_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			Select(lbHistory.SelectedItem as SimState);
		}

		private void toolStripTextBox1_Leave(object sender, EventArgs e)
		{
			int max = 1000;

			if (Int32.TryParse(toolStripTextBox1.Text, out max))
			{
				if (max < 1)
					max = 1;

				MainForm.MaxHistory = max;
			}
		}
	}
}