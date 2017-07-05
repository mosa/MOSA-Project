// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.DebugData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.View
{
	public partial class SymbolView : DebugDockContent
	{
		private BindingList<SymbolEntry> symbols = new BindingList<SymbolEntry>();

		private class SymbolEntry
		{
			public string Address { get { return "0x" + Symbol.Address.ToString((Symbol.Address <= uint.MaxValue) ? "X4" : "X8"); } }

			public string Name { get { return Symbol.Name; } }

			public string Kind { get { return Symbol.Kind; } }

			public int Length { get { return Symbol.Length; } }

			[Browsable(false)]
			public Symbol Symbol { get; }

			public SymbolEntry(Symbol symbol)
			{
				Symbol = symbol;
			}
		}

		public SymbolView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			dataGridView1.DataSource = symbols;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[0].Width = 65;
			dataGridView1.Columns[1].Width = 450;
		}

		private void SymbolView_Load(object sender, EventArgs e)
		{
			cbKind.SelectedIndex = 0;
			cbLength.SelectedIndex = 0;
		}

		public void CreateEntries()
		{
			symbols.Clear();

			string filter = toolStripTextBox1.Text.Trim();

			if (filter.Length < 3)
				return;

			string kind = cbKind.SelectedIndex < 1 ? string.Empty : cbKind.SelectedItem.ToString().Trim();

			uint start = 0;
			uint end = UInt32.MaxValue;

			switch (cbLength.SelectedIndex)
			{
				case 1: start = 1; end = 1; break;
				case 2: start = 2; end = 2; break;
				case 3: start = 4; end = 4; break;
				case 4: start = 8; end = 8; break;
				default: break;
			}

			foreach (var symbol in MainForm.DebugSource.Symbols)
			{
				if (!(filter.Length == 0 || symbol.Name.Contains(filter)))
					continue;

				if (kind != string.Empty && symbol.Kind != kind)
					continue;

				if (symbol.Length > end || symbol.Length < start)
					continue;

				symbols.Add(new SymbolEntry(symbol));
			}
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			CreateEntries();
		}

		private SymbolEntry clickedEntry;

		private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;

			if (e.RowIndex < 0 || e.ColumnIndex < 0)
				return;

			clickedEntry = dataGridView1.Rows[e.RowIndex].DataBoundItem as SymbolEntry;

			var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);

			var menu = new MenuItem(clickedEntry.Name);
			menu.Enabled = false;
			var m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MenuItem3_Click)));
			m.MenuItems.Add(new MenuItem("Add to &Watch List", new EventHandler(MenuItem1_Click)));
			m.MenuItems.Add(new MenuItem("Set &Breakpoint", new EventHandler(MenuItem2_Click)));
			m.Show(dataGridView1, relativeMousePosition);
		}

		private void MenuItem1_Click(Object sender, EventArgs e)
		{
			if (clickedEntry == null)
				return;

			MainForm.AddWatch(clickedEntry.Name, clickedEntry.Symbol.Address, clickedEntry.Length);
		}

		private void MenuItem2_Click(Object sender, EventArgs e)
		{
			if (clickedEntry == null)
				return;

			MainForm.AddBreakPoint(clickedEntry.Symbol.Address, clickedEntry.Name);
		}

		private void MenuItem3_Click(Object sender, EventArgs e)
		{
			if (clickedEntry == null)
				return;

			Clipboard.SetText(clickedEntry.Address + " : " + clickedEntry.Name);
		}

		private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			CreateEntries();
		}

		private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			CreateEntries();
		}
	}
}
