// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	public partial class BreakpointView : DebugDockContent
	{
		private readonly BindingList<BreakPointEntry> Breakpoints = new BindingList<BreakPointEntry>();

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

		public BreakpointView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			dataGridView1.DataSource = Breakpoints;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[1].Width = 400;
		}

		public override void OnBreakpointChange()
		{
			Breakpoints.Clear();
			foreach (var breakpoint in MainForm.BreakPoints)
			{
				Breakpoints.Add(new BreakPointEntry(breakpoint));
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

			var clickedEntry = dataGridView1.CurrentCell.OwningRow.DataBoundItem as BreakPointEntry;

			MainForm.RemoveBreakPoint(clickedEntry.BreakPoint);
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

			var menu = new MenuItem(clickedEntry.Address + " - " + clickedEntry.Name)
			{
				Enabled = false
			};
			var m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MainForm.OnCopyToClipboardAsBreakPoint)) { Tag = clickedEntry.BreakPoint });
			m.MenuItems.Add(new MenuItem("&Delete breakpoint", new EventHandler(MainForm.OnRemoveBreakPoint)) { Tag = clickedEntry.BreakPoint });

			m.Show(dataGridView1, relativeMousePosition);
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			var address = MainForm.ParseHexAddress(tbAddress.Text);
			MainForm.AddBreakPoint(address);
		}

		private void btnDeleteAll_Click(object sender, EventArgs e)
		{
			MainForm.DeleteAllBreakPonts();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			if (MainForm.BreakpointFile == null)
			{
				if (MainForm.ImageFile != null)
				{
					MainForm.BreakpointFile = Path.Combine(
						Path.GetDirectoryName(MainForm.ImageFile),
						Path.GetFileNameWithoutExtension(MainForm.ImageFile)) + ".breakpoints";
				}
				else
				{
					MainForm.BreakpointFile = Path.Combine(Path.GetTempPath(), "default.breakpoints");
				}
			}

			SaveBreakPoints();
		}

		private void SaveBreakPoints()
		{
			var lines = new List<string>();

			if (MainForm.ImageFile != null)
			{
				lines.Add("#HASH: " + MainForm.VMHash);
			}

			foreach (var entry in Breakpoints)
			{
				lines.Add(entry.Address + '\t' + entry.Name);
			}

			File.WriteAllLines(MainForm.BreakpointFile, lines);
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				MainForm.BreakpointFile = openFileDialog1.FileName;
				MainForm.LoadBreakPoints();
			}
		}
	}
}
