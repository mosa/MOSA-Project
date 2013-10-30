/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.TinyCPUSimulator;

namespace Mosa.Tool.Simulator
{
	public partial class CallStackView : SimulatorDockContent
	{
		public CallStackView()
		{
			InitializeComponent();
		}

		private void AddSymbol(ulong ip)
		{
			SimSymbol symbol = SimCPU.FindSymbol(ip);

			string node = "[0x" + ip.ToString("X8") + "] " + (symbol != null ? symbol.Name : "Unknown");

			treeView1.Nodes.Add(node);
		}

		public override void Update(SimState simState)
		{
			treeView1.Nodes.Clear();

			ulong ip = simState.IP;

			AddSymbol(ip);

			this.Refresh();
		}

		public override void Update()
		{
			Update(SimCPU.GetState());

			ulong ip = SimCPU.CurrentInstructionPointer;
			ulong ebp = SimCPU.FramePointer;

			try
			{
				for (int i = 0; i < 10; i++)
				{
					if (ebp == 0)
						break;

					ip = SimCPU.DirectRead32((ulong)((long)ebp + SimCPU.PreviousFrameOffset));

					if (ip == 0)
						break;

					AddSymbol(ip);

					ebp = SimCPU.DirectRead32((ulong)((long)ebp));
				}
			}
			catch (Exception e)
			{
			}

			this.Refresh();
		}
	}
}