/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.TinyCPUSimulator;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System;

namespace Mosa.Tool.TinySimulator
{
	public partial class BreakPointView : SimulatorDockContent
	{
		private BindingList<BreakPointEntry> breakpoints = new BindingList<BreakPointEntry>();

		private class BreakPointEntry
		{
			public string Name { get; private set; }

			public string Hex { get; private set; }

			public ulong Address { get; private set; }

			private bool display32;

			public BreakPointEntry(ulong address, bool display32)
			{
				this.Address = address;
				this.Hex = MainForm.Format(address, display32);
				this.display32 = display32;
			}

			public BreakPointEntry(string name, ulong address, bool display32)
				: this(address, display32)
			{
				this.Name = name;
			}
		}

		public BreakPointView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			dataGridView1.DataSource = breakpoints;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[0].Width = 250;
		}

		public void AddBreakPoint(string name, ulong address)
		{
			var breakpoint = new BreakPointEntry(name, address, true);

			breakpoints.Add(breakpoint);

			SimCPU.Monitor.AddBreakPoint(address);
		}

		private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Delete)
				return;

			if (dataGridView1.CurrentCell == null)
				return;

			var row = dataGridView1.CurrentCell.OwningRow.DataBoundItem;

			breakpoints.Remove(row as BreakPointEntry);

			SimCPU.Monitor.RemoveBreakPoint((row as BreakPointEntry).Address);
		}

		private BreakPointEntry breakPointEntry = null;

		private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;

			if (e.RowIndex < 0 || e.ColumnIndex < 0)
				return;

			var row = dataGridView1.Rows[e.RowIndex].DataBoundItem as BreakPointEntry;

			breakPointEntry = row;

			var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);

			MenuItem menu = new MenuItem(row.Name);
			menu.Enabled = false;
			ContextMenu m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MenuItem3_Click)));
			m.MenuItems.Add(new MenuItem("&Delete breakpoint", new EventHandler(MenuItem1_Click)));
			m.Show(dataGridView1, relativeMousePosition);
		}

		private void MenuItem1_Click(Object sender, EventArgs e)
		{
			if (breakPointEntry == null)
				return;

			breakpoints.Remove(breakPointEntry);

			SimCPU.Monitor.RemoveBreakPoint(breakPointEntry.Address);
		}

		private void MenuItem3_Click(Object sender, EventArgs e)
		{
			if (breakPointEntry == null)
				return;

			Clipboard.SetText(breakPointEntry.Name);
		}

	}
}