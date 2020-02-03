// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	public partial class RegisterView : DebugDockContent
	{
		private BindingList<RegisterEntry> registers = new BindingList<RegisterEntry>();

		private class RegisterEntry
		{
			public string Register { get; set; }

			public string HexValue { get; set; }

			public ulong Value { get; set; }

			[Browsable(false)]
			public uint Size { get; set; }

			//private Register Register { get; set; }

			//public RegisterEntry(Register register)
			//{
			//	Register = register;
			//}

			public RegisterEntry(string register, ulong value, string hexValue, uint size)
			{
				Register = register;
				Value = value;
				HexValue = hexValue;
				Size = size;
			}
		}

		public RegisterView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			dataGridView1.DataSource = registers;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[0].Width = 75;
			dataGridView1.Columns[1].Width = 75;
			dataGridView1.Columns[2].Width = 75;
		}

		public override void OnRunning()
		{
			registers.Clear();
		}

		public override void OnPause()
		{
			registers.Clear();

			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			foreach (var register in Platform.Registers)
			{
				registers.Add(new RegisterEntry(register.Name, register.Value, register.ToHex(), register.Size));
			}
		}

		private void dataGridView1_CellMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;

			if (e.RowIndex < 0 || e.ColumnIndex < 0)
				return;

			dataGridView1.ClearSelection();
			dataGridView1.Rows[e.RowIndex].Selected = true;
			var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);

			var clickedEntry = dataGridView1.Rows[e.RowIndex].DataBoundItem as RegisterEntry;

			var menu = new MenuItem(clickedEntry.Register + " - " + clickedEntry.HexValue);
			menu.Enabled = false;
			var m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MainForm.OnCopyToClipboard)) { Tag = clickedEntry.HexValue });
			m.MenuItems.Add(new MenuItem("Add to &Watch List", new EventHandler(MainForm.OnAddWatch)) { Tag = new AddWatchArgs(null, clickedEntry.Value, clickedEntry.Size) });
			m.MenuItems.Add(new MenuItem("Set &Breakpoint", new EventHandler(MainForm.OnAddBreakPoint)) { Tag = new AddBreakPointArgs(null, clickedEntry.Value) });

			m.Show(dataGridView1, relativeMousePosition);
		}
	}
}
