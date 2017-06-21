// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Tool.GDBDebugger
{
	public partial class RegisterView : DebugDockContent
	{
		public RegisterView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		private void GeneralPurposeRegistersView_Load(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
		}

		public override void UpdateDock()
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
