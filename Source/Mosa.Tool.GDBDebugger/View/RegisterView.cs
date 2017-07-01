// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger.View
{
	public partial class RegisterView : DebugDockContent
	{
		public RegisterView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			dataGridView1.Rows.Clear();
		}

		public override void OnRunning()
		{
			dataGridView1.Rows.Clear();
		}

		public override void OnPause()
		{
			dataGridView1.Rows.Clear();

			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			foreach (var register in MainForm.GDBConnector.Platform.Registers)
			{
				dataGridView1.Rows.Add(register.Name, register.ToHex());
			}
		}
	}
}
