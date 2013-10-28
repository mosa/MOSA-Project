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

		public override void Update(SimState simState)
		{
			while (MainForm.SimStates.Count != lbHistory.Items.Count)
			{
				lbHistory.Items.Add(MainForm.SimStates[lbHistory.Items.Count]);
			}

			lbHistory.SelectedIndex = lbHistory.Items.Count - 1;

			this.Refresh();
		}

		public override void Update()
		{
			if (!enable)
				return;

			Update(SimCPU.GetState());
		}

		private void lbHistory_DoubleClick(object sender, EventArgs e)
		{
			enable = false;
			
			MainForm.UpdateAll(lbHistory.SelectedItem as SimState);
			
			enable = true;
		}
	}
}