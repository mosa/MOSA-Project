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
	public partial class RegisterView : SimulatorDockContent
	{
		public RegisterView()
		{
			InitializeComponent();
		}

		private void GeneralPurposeRegistersView_Load(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
		}

		public override void UpdateDock(SimState simState)
		{
			dataGridView1.Rows.Clear();

			string[] entries = simState.Values["Register.List"] as string[];

			foreach (var name in entries)
			{
				dataGridView1.Rows.Add(name, MainForm.Format(simState.Values["Register." + name]));
			}

			this.Refresh();
		}

	}
}