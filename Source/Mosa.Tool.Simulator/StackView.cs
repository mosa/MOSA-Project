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
	public partial class StackView : SimulatorDockContent
	{
		public StackView()
		{
			InitializeComponent();
		}

		public override void Update(SimState simState)
		{
			listBox1.Items.Clear();

			int count = Convert.ToInt32(simState.Values["Stack.Index.Count"]);

			for (int index = count - 1; index >= 0; index--)
			{
				listBox1.Items.Add(index.ToString("D2") + ": " + simState.Values["Stack.Index." + index.ToString()]);
			}

			listBox1.SelectedIndex = listBox1.Items.Count - 1;

			this.Refresh();
		}

		public override void Update()
		{
			Update(SimCPU.GetState());
		}
	}
}