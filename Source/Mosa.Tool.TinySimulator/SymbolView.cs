/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Linker;
using Mosa.TinyCPUSimulator;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Mosa.Tool.TinySimulator
{
	public partial class SymbolView : SimulatorDockContent
	{
		private List<SymbolEntry> symbols;

		private class SymbolEntry
		{
			public string Name { get { return LinkerSymbol.Name; } }

			public long Length { get { return LinkerSymbol.Length; } }

			public string VirtualAddress { get { return MainForm.Format(LinkerSymbol.VirtualAddress, force32); } }

			public long Offset { get { return LinkerSymbol.Offset; } }

			public string SectionKind { get; set; }

			public string SectionAddress { get { return MainForm.Format(LinkerSymbol.SectionAddress, force32); } }

			public LinkerSymbol LinkerSymbol;

			private bool force32;

			public SymbolEntry(LinkerSymbol linkerSymbol, bool force32)
			{
				this.LinkerSymbol = linkerSymbol;
				this.force32 = force32;
				this.SectionKind = linkerSymbol.SectionKind.ToString();
			}
		}

		public SymbolView()
		{
			InitializeComponent();
		}

		private void SymbolView_Load(object sender, EventArgs e)
		{
			toolStripComboBox1.SelectedIndex = 0;
			toolStripComboBox2.SelectedIndex = 0;
		}

		public void CreateEntries()
		{
			if (MainForm.Linker == null)
				return;

			symbols = new List<SymbolEntry>(MainForm.Linker.Symbols.Count);

			string filter = toolStripTextBox1.Text.Trim();
			string kind = toolStripComboBox1.SelectedIndex < 1 ? string.Empty : toolStripComboBox1.SelectedItem.ToString().Trim();

			int start = 0;
			int end = Int32.MaxValue;

			switch (toolStripComboBox2.SelectedIndex)
			{
				case 1: start = 1; end = 1; break;
				case 2: start = 2; end = 2; break;
				case 3: start = 4; end = 4; break;
				case 4: start = 8; end = 8; break;
				default: break;
			}

			foreach (var symbol in MainForm.Linker.Symbols)
			{
				if (!(filter.Length == 0 || symbol.Name.Contains(filter)))
					continue;

				if (kind != string.Empty && symbol.SectionKind.ToString() != kind)
					continue;

				if (symbol.Length > end || symbol.Length < start)
					continue;

				symbols.Add(new SymbolEntry(symbol, true)); // true == forces 32 bit
			}

			dataGridView1.DataSource = symbols;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[0].Width = 500;
		}

		public override void UpdateDock(SimState simState)
		{
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			CreateEntries();
		}

		private SymbolEntry clickedSymbolEntry;

		private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;

			if (e.RowIndex < 0 && e.ColumnIndex < 0)
				return;

			var row = dataGridView1.Rows[e.RowIndex].DataBoundItem as SymbolEntry;

			clickedSymbolEntry = row;

			var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);

			ContextMenu m = new ContextMenu();
			m.MenuItems.Add(new MenuItem(string.Format("&Add to Watch List: {0}", row.Name), new EventHandler(MenuItem1_Click)));
			m.Show(dataGridView1, relativeMousePosition);
		}

		private void MenuItem1_Click(Object sender, EventArgs e)
		{
			if (clickedSymbolEntry == null)
				return;

			MainForm.AddWatch(clickedSymbolEntry.Name, (ulong)clickedSymbolEntry.LinkerSymbol.VirtualAddress, (int)clickedSymbolEntry.LinkerSymbol.Length * 8);

			return;
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