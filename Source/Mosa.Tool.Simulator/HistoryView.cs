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
using System.Collections.Generic;

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
			if (simState == null)
				return;

			//lbHistory.Items.Clear();

			//foreach (var entry in history)
			//{
			//	lbHistory.Items.Add(entry);
			//}

			//lbHistory.SelectedIndex = lbHistory.Items.Count - 1;

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
	}
}