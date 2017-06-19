// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Tool.GDBDebugger
{
	public partial class ControlView : DebugDockContent
	{
		public ControlView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		private void btnStep_Click(object sender, EventArgs e)
		{
			//MainForm.ExecuteSteps(1);
		}

		private void btnStepN_Click(object sender, EventArgs e)
		{
			uint steps = Convert.ToUInt32(tbSteps.Text);

			//MainForm.ExecuteSteps(steps);
		}

		private void btnRestart_Click(object sender, EventArgs e)
		{
			//MainForm.Restart();
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			//MainForm.Start();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			//MainForm.Stop();
		}

		private void btnStepOver_Click(object sender, EventArgs e)
		{
			//if (SimCPU == null)
			//	return;

			//if (SimCPU.LastInstruction.Opcode.FlowType == OpcodeFlowType.Call || SimCPU.LastInstruction.Opcode.FlowType == OpcodeFlowType.Normal)
			//{
			//	ulong ip = SimCPU.LastProgramCounter + SimCPU.CurrentInstruction.OpcodeSize;

			//	SimCPU.Monitor.StepOverBreakPoint = ip;
			//	MainForm.Start();
			//}
			//else
			//{
			//	SimCPU.Monitor.StepOverBreakPoint = 0;
			//	MainForm.ExecuteSteps(1);
			//}
		}
	}
}
