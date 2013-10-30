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
	public partial class StackFrameView : SimulatorDockContent
	{
		public StackFrameView()
		{
			InitializeComponent();
		}

		public override void UpdateDock(SimState simState)
		{
			listBox1.Items.Clear();

			int count = Convert.ToInt32(simState.Values["StackFrame.Index.Count"]);

			for (int index = 0; index < count; index++)
			{
				listBox1.Items.Add(index.ToString("D2") + ": " + simState.Values["StackFrame.Index." + index.ToString()]);
			}

			this.Refresh();
		}

		public override void UpdateDock()
		{
			UpdateDock(SimCPU.GetState());
		}
	}
}