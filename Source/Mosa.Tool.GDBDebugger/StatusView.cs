// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger
{
	public partial class StatusView : DebugDockContent
	{
		public StatusView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public override void UpdateDock()
		{
			//var currentInstruction = simState.Instruction;
			//var nextInstruction = SimCPU.GetOpcode(simState.NextIP);

			//textBox3.Text = nextInstruction != null ? nextInstruction.ToString() : "-BLANK-";

			//textBox4.Text = "0x" + simState.IP.ToString("X8");

			Refresh();
		}
	}
}
