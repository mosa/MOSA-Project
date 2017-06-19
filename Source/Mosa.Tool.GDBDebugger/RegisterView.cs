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

			//foreach (var name in simState.RegisterList)
			//{
			//	dataGridView1.Rows.Add(name, MainForm.Format(simState.GetRegister(name)));
			//}
		}
	}
}
