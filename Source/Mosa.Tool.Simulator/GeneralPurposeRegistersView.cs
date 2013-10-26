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
	public partial class GeneralPurposeRegistersView : SimulatorDockContent
	{
		private List<string> registerNames;

		public GeneralPurposeRegistersView()
		{
			InitializeComponent();
		}

		private void GeneralPurposeRegistersView_Load(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
		}

		private void UpdateRegisters(SimState simState)
		{
			if (registerNames != null)
				return;

			registerNames = new List<string>();

			for (int i = 0; i < 64; i++)
			{
				foreach (var entry in simState.Values)
				{
					if (entry.Key.StartsWith("Register." + i.ToString() + "."))
					{
						registerNames.Add(entry.Key);
						break;
					}
				}
			}
		}

		private void Update(SimState simState)
		{
			dataGridView1.Rows.Clear();

			UpdateRegisters(simState);

			foreach (var register in registerNames)
			{
				string name = register.Substring(register.LastIndexOf(".") + 1);
				string value = simState.Values[register];

				dataGridView1.Rows.Add(name, value);
			}
		}

		public override void Update()
		{
			Update(SimAdapter.GetState());

			this.Refresh();
		}
	}
}