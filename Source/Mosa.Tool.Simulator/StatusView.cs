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
	public partial class StatusView : SimulatorDockContent
	{
		public StatusView()
		{
			InitializeComponent();
		}

		public override void UpdateDock(SimState simState)
		{
			var currentInstruction = SimCPU.DecodeOpcode(simState.IP);
			var nextInstruction = SimCPU.DecodeOpcode(simState.NextIP);

			this.textBox1.Text = simState.Tick.ToString();
			this.textBox2.Text = currentInstruction != null ? currentInstruction.ToString() : "-BLANK-";
			this.textBox3.Text = nextInstruction != null ? nextInstruction.ToString() : "-BLANK-";

			this.textBox4.Text = "0x" + simState.IP.ToString("X8");
			this.textBox5.Text = "0x" + simState.NextIP.ToString("X8");

			this.textBox6.Text = simState.CPUException != null ? simState.CPUException.ToString() : "None";
		
			this.Refresh();
		}

	}
}