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
using System.Diagnostics;

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

			textBox1.Text = simState.Tick.ToString();
			textBox2.Text = currentInstruction != null ? currentInstruction.ToString() : "-BLANK-";
			textBox3.Text = nextInstruction != null ? nextInstruction.ToString() : "-BLANK-";

			textBox4.Text = "0x" + simState.IP.ToString("X8");
			textBox5.Text = "0x" + simState.NextIP.ToString("X8");

			textBox6.Text = simState.CPUException != null ? simState.CPUException.ToString() : "None";

			double speed = simState.Tick / (double)simState.Values["TotalElapsed"];

			textBox7.Text = speed.ToString("0.00");

			Refresh();
		}

	}
}