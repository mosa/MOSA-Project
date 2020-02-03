// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.Debugger.DebugData;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	public partial class SymbolView : DebugDockContent
	{
		private BindingList<SymbolEntry> symbols = new BindingList<SymbolEntry>();

		private class SymbolEntry
		{
			public string Address { get { return "0x" + Symbol.Address.ToString((Symbol.Address <= uint.MaxValue) ? "X4" : "X8"); } }

			public string Name { get { return Symbol.CommonName; } }

			public string Kind { get { return Symbol.Kind; } }

			public uint Length { get { return Symbol.Length; } }

			//public string FullName { get { return Symbol.Name; } }

			[Browsable(false)]
			public SymbolInfo Symbol { get; }

			public SymbolEntry(SymbolInfo symbol)
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
				if (!(filter.Length == 0 || symbol.CommonName.Contains(filter)))
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

		private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;

			if (e.RowIndex < 0 || e.ColumnIndex < 0)
				return;

			dataGridView1.ClearSelection();
			dataGridView1.Rows[e.RowIndex].Selected = true;
			var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);

			var clickedEntry = dataGridView1.Rows[e.RowIndex].DataBoundItem as SymbolEntry;

			var menu = new MenuItem(clickedEntry.Name);
			menu.Enabled = false;
			var m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MainForm.OnCopyToClipboard)) { Tag = clickedEntry.Address });
			m.MenuItems.Add(new MenuItem("Add to &Watch List", new EventHandler(MainForm.OnAddWatch)) { Tag = new AddWatchArgs(clickedEntry.Name, clickedEntry.Symbol.Address, clickedEntry.Length) });
			m.MenuItems.Add(new MenuItem("Set &Breakpoint", new EventHandler(MainForm.OnAddBreakPoint)) { Tag = new AddBreakPointArgs(clickedEntry.Name, clickedEntry.Symbol.Address) });

			m.Show(dataGridView1, relativeMousePosition);
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
