﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.Views
{
	public partial class SourceDataView : DebugDockContent
	{
		public SourceDataView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		protected override void ClearDisplay()
		{
			lbMethodID.Text = string.Empty;
			lbOffset.Text = string.Empty;

			dataGridView1.Enabled = false;
			dataGridView2.Enabled = false;
		}

		protected override void UpdateDisplay()
		{
			ClearDisplay();

			var method = DebugSource.GetMethod(InstructionPointer);

			if (method == null)
				return;

			lbOffset.Text = (InstructionPointer - method.Address).ToString();
			lbMethodID.Text = method.ID.ToString();

			var sourceLabels = DebugSource.GetSourceLabels(method.ID);

			dataGridView1.Enabled = true;
			dataGridView1.DataSource = sourceLabels;
			dataGridView1.Columns["MethodID"].Visible = false;
			dataGridView1.AutoResizeColumns();

			var sourceInfos = DebugSource.GetSources(method.ID);

			if (sourceInfos == null)
			{
				dataGridView2.Enabled = false;
			}
			else
			{
				dataGridView2.Enabled = true;
				dataGridView2.DataSource = sourceInfos;
				dataGridView2.Columns["MethodID"].Visible = false;
				dataGridView2.Columns["Offset"].Visible = false;
				dataGridView2.AutoResizeColumns();
			}
		}
	}
}
