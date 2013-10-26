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
	public partial class CallStackView : SimulatorDockContent
	{

		public CallStackView()
		{
			InitializeComponent();
		}

		private void AddSymbol(ulong ip)
		{
			SimSymbol symbol = SimCPU.FindSymbol(ip);

			textBox1.AppendText("0x" + ip.ToString("X") + " ");

			if (symbol != null)
				textBox1.AppendText(symbol.Name);
			else
				textBox1.AppendText("Unknown");

			textBox1.AppendText("\n");
		}

		public override void Update()
		{
			textBox1.Clear();

			ulong ip = SimCPU.CurrentInstructionPointer;

			AddSymbol(ip);

			ulong ebp = SimCPU.FramePointer;

			//for (int i = 0; i < 10; i++)
			//{
			//	if (ebp == 0)
			//		break;

			//	ip = SimCPU.DirectRead32((ulong)((long)ebp + SimCPU.PreviousFrameOffset));

			//	if (ip == 0)
			//		break;

			//	AddSymbol(ip);

			//	ebp = SimCPU.DirectRead32(ebp);
			//}

			this.Refresh();
		}
	}
}