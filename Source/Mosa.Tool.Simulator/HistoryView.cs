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

			if (MainForm.SimStates.Count < lbHistory.Items.Count)
				lbHistory.Items.Clear();

			while (MainForm.SimStates.Count != lbHistory.Items.Count)
			{
				lbHistory.Items.Add(MainForm.SimStates[lbHistory.Items.Count]);
			}

			lbHistory.SelectedIndex = lbHistory.Items.Count - 1;

			this.Refresh();
		}

		private void lbHistory_DoubleClick(object sender, EventArgs e)
		{
			enable = false;

			MainForm.UpdateAllDocks(lbHistory.SelectedItem as SimState);

			enable = true;
		}
	}
}