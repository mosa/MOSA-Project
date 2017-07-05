// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.View
{
	public partial class BreakPointView : DebugDockContent
	{
		private BindingList<BreakPointEntry> breakpoints = new BindingList<BreakPointEntry>();

		private class BreakPointEntry
		{
			public string Address { get { return "0x" + BreakPoint.Address.ToString((BreakPoint.Address <= uint.MaxValue) ? "X4" : "X8"); } }

			public string Name { get { return BreakPoint.Name; } }

			[Browsable(false)]
			public BreakPoint BreakPoint { get; }

			public BreakPointEntry(BreakPoint breakPoint)
			{
				BreakPoint = breakPoint;
			}
		}

		public BreakPointView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			dataGridView1.DataSource = breakpoints;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[1].Width = 400;
		}

		public override void OnBreakpointChange()
		{
			breakpoints.Clear();
			foreach (var breakpoint in MainForm.BreakPoints)
			{
				breakpoints.Add(new BreakPointEntry(breakpoint));
			}
		}

		public void AddBreakPoint(string name, ulong address)
		{
			var breakpoint = new BreakPoint(name, address);

			MainForm.AddBreakPoint(breakpoint);
		}

		private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Delete)
				return;

			if (dataGridView1.CurrentCell == null)
				return;

			var row = dataGridView1.CurrentCell.OwningRow.DataBoundItem;

			var breakPointEntry = row as BreakPointEntry;

			MainForm.RemoveBreakPoint(breakPointEntry.BreakPoint);
		}

		private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;

			if (e.RowIndex < 0 || e.ColumnIndex < 0)
				return;

			dataGridView1.ClearSelection();
			dataGridView1.Rows[e.RowIndex].Selected = true;
			var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);

			var clickedEntry = dataGridView1.Rows[e.RowIndex].DataBoundItem as BreakPointEntry;

			var menu = new MenuItem(clickedEntry.Name);
			menu.Enabled = false;
			var m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MainForm.OnCopyToClipboard)) { Tag = clickedEntry.Name });
			m.MenuItems.Add(new MenuItem("&Delete breakpoint", new EventHandler(MainForm.OnRemoveBreakPoint)) { Tag = clickedEntry.BreakPoint });

			m.Show(dataGridView1, relativeMousePosition);
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			var address = MainForm.ParseMemoryAddress(tbAddress.Text);
			MainForm.AddBreakPoint(address);
		}
	}
}
