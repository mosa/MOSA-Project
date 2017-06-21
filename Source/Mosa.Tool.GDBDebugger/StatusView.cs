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
			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			tbIP.Text = Platform.InstructionPointer.ToHex();

			//var nextInstruction = SimCPU.GetOpcode(simState.NextIP);
			//textBox3.Text = nextInstruction != null ? nextInstruction.ToString() : "-BLANK-";

			Refresh();
		}
	}
}
