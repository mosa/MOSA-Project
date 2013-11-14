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
	public partial class ControlView : SimulatorDockContent
	{
		public ControlView()
		{
			InitializeComponent();
		}

		public bool Record { get { return cbRecord.Checked; } set { cbRecord.Checked = value; } }

		private void btnStep_Click(object sender, EventArgs e)
		{
			MainForm.ExecuteSteps(1);
		}

		private void btnStepN_Click(object sender, EventArgs e)
		{
			uint steps = Convert.ToUInt32(tbSteps.Text);
			MainForm.ExecuteSteps(steps);
		}

		private void btnRestart_Click(object sender, EventArgs e)
		{
			MainForm.Restart();
		}

		private void cbRecord_CheckedChanged(object sender, EventArgs e)
		{
			MainForm.Record = cbRecord.Checked;
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			MainForm.Start();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			MainForm.Stop();
		}

		private void cbBreakOnJump_CheckedChanged(object sender, EventArgs e)
		{
			SimCPU.Monitor.BreakAfterJump = cbBreakAfterJump.Checked;
		}

		private void cbBreakOnCall_CheckedChanged(object sender, EventArgs e)
		{
			SimCPU.Monitor.BreakAfterCall = cbBreakAfterCall.Checked;
		}

		private void cbBreakOnReturn_CheckedChanged(object sender, EventArgs e)
		{
			SimCPU.Monitor.BreakAfterReturn = cbBreakAfterReturn.Checked;
		}

		private void cbBreakAfterBranch_CheckedChanged(object sender, EventArgs e)
		{
			SimCPU.Monitor.BreakAfterBranch = cbBreakAfterBranch.Checked;
		}

		private void btnStepOver_Click(object sender, EventArgs e)
		{
			if (SimCPU.LastInstruction.Opcode.FlowType == OpcodeFlowType.Call || SimCPU.LastInstruction.Opcode.FlowType == OpcodeFlowType.Normal)
			{
				ulong ip = SimCPU.LastProgramCounter + SimCPU.CurrentInstruction.OpcodeSize;

				SimCPU.Monitor.StepOverBreakPoint = ip;
				MainForm.Start();
			}
			else
			{
				SimCPU.Monitor.StepOverBreakPoint = 0;
				MainForm.ExecuteSteps(1);
			}
		}
	}
}