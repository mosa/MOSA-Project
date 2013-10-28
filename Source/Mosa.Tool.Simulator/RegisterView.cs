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
	public partial class RegisterView : SimulatorDockContent
	{
		private List<string> listNames;

		public RegisterView()
		{
			InitializeComponent();
		}

		private void GeneralPurposeRegistersView_Load(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
		}

		private void CreateList(SimState simState)
		{
			if (listNames != null)
				return;

			listNames = new List<string>();

			for (int i = 0; i < 64; i++)
			{
				foreach (var entry in simState.Values)
				{
					if (entry.Key.StartsWith("Register." + i.ToString() + "."))
					{
						listNames.Add(entry.Key);
						break;
					}
				}
			}
		}

		public override void Update(SimState simState)
		{
			dataGridView1.Rows.Clear();

			CreateList(simState);

			foreach (var register in listNames)
			{
				string name = register.Substring(register.LastIndexOf(".") + 1);
				string value = simState.Values[register];

				dataGridView1.Rows.Add(name, value);
			}

			this.Refresh();
		}

		public override void Update()
		{
			Update(SimCPU.GetState());
		}
	}
}