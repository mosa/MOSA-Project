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
	public partial class ControlView : SimulatorDockContent
	{
		public ControlView()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			MainForm.ExecuteSteps(1);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			uint steps = Convert.ToUInt32(tbSteps.Text);
			MainForm.ExecuteSteps(steps);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			MainForm.Restart();
		}

		private void cbRecord_CheckedChanged(object sender, EventArgs e)
		{
			MainForm.Record = cbRecord.Checked;
		}
	}
}