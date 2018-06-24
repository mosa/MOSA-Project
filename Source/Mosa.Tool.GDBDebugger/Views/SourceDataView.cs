// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger.Views
{
	public partial class SourceDataView : DebugDockContent
	{
		public SourceDataView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public override void OnRunning()
		{
			// Clear
		}

		public override void OnPause()
		{
			Query();
		}

		private void Query()
		{
			lbMethodID.Text = string.Empty;
			lbOffset.Text = string.Empty;

			dataGridView1.Enabled = false;
			dataGridView2.Enabled = false;

			if (!IsConnected || !IsPaused)
				return;

			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			var address = Platform.InstructionPointer.Value;

			var method = DebugSource.GetMethod(address);

			if (method == null)
				return;

			lbOffset.Text = (address - method.Address).ToString();
			lbMethodID.Text = method.ID.ToString();

			var sourceLabels = DebugSource.GetSourceLabels(method.ID);

			dataGridView1.Enabled = true;
			dataGridView1.DataSource = sourceLabels;
			dataGridView1.Columns["MethodID"].Visible = false;
			dataGridView1.AutoResizeColumns();

			var sourceInfos = DebugSource.GetSources(method.ID);

			dataGridView2.Enabled = true;
			dataGridView2.DataSource = sourceInfos;
			dataGridView2.Columns["MethodID"].Visible = false;
			dataGridView2.Columns["Offset"].Visible = false;
			dataGridView2.AutoResizeColumns();
		}
	}
}
