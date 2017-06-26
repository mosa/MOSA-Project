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

		public override void OnRunning()
		{
			txtInstruction.Text = "Running...";
			tbIP.Text = string.Empty;
		}

		public override void OnPause()
		{
			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			// todo: get memory and decode instruction

			tbIP.Text = Platform.InstructionPointer.ToHex();
			txtInstruction.Text = string.Empty; // todo

			Refresh();
		}
	}
}
