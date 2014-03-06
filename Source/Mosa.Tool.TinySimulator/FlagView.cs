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

namespace Mosa.Tool.TinySimulator
{
	public partial class FlagView : SimulatorDockContent
	{
		public FlagView()
		{
			InitializeComponent();
		}

		private void GeneralPurposeRegistersView_Load(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
		}

		public override void UpdateDock(BaseSimState simState)
		{
			dataGridView1.Rows.Clear();

			foreach (var name in simState.FlagList)
			{
				dataGridView1.Rows.Add(name, MainForm.Format(simState.GetRegister(name)));
			}

			this.Refresh();
		}

	}
}