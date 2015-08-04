// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.TinyCPUSimulator;

namespace Mosa.Tool.TinySimulator
{
	public partial class StatusView : SimulatorDockContent
	{
		public StatusView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public override void UpdateDock(BaseSimState simState)
		{
			var currentInstruction = simState.Instruction;
			var nextInstruction = SimCPU.GetOpcode(simState.NextIP);

			textBox1.Text = simState.Tick.ToString();
			textBox2.Text = currentInstruction != null ? currentInstruction.ToString() : "-BLANK-";
			textBox3.Text = nextInstruction != null ? nextInstruction.ToString() : "-BLANK-";

			textBox4.Text = "0x" + simState.IP.ToString("X8");
			textBox5.Text = "0x" + simState.NextIP.ToString("X8");

			textBox6.Text = simState.CPUException != null ? simState.CPUException.ToString() : "None";

			double speed = simState.Tick / simState.TotalElapsedSeconds;

			textBox7.Text = speed.ToString("0.00");

			Refresh();
		}
	}
}
