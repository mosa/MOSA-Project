// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.GDB;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.Views
{
	public partial class StackFrameView : DebugDockContent
	{
		private BindingList<StackEntry> stackentries = new BindingList<StackEntry>();

		private class StackEntry
		{
			public string Offset { get; set; }

			public string Address { get; set; }

			public string HexValue { get; set; }

			[Browsable(false)]
			public ulong Value { get; set; }

			[Browsable(false)]
			public int Index { get; set; }
		}

		public StackFrameView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			dataGridView1.DataSource = stackentries;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[0].Width = 75;
			dataGridView1.Columns[1].Width = 75;
			dataGridView1.Columns[2].Width = 75;
		}

		public override void OnRunning()
		{
			stackentries.Clear();
		}

		public override void OnPause()
		{
			stackentries.Clear();

			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			if (Platform.StackFrame.Value == 0 || Platform.StackPointer.Value == 0)
				return;

			var span = Platform.StackFrame.Value - Platform.StackPointer.Value;

			if (span <= 0)
				span = 4;
			else if (span > 512)
				span = 512;

			if (span % 4 != 0)
				span = span + (span % 4);

			MemoryCache.ReadMemory(Platform.StackFrame.Value - span, (uint)span + 4, OnMemoryRead);
		}

		private void OnMemoryRead(ulong address, byte[] bytes)
		{
			MethodInvoker method = delegate ()
			{
				UpdateDisplay(address, bytes);
			};

			BeginInvoke(method);
		}

		private void UpdateDisplay(ulong address, byte[] memory)
		{
			uint size = 4; // fixme

			for (int i = memory.Length - 4; i > 0; i = i - (int)size)
			{
				ulong value = (ulong)(memory[i] | (memory[i + 1] << 8) | (memory[i + 2] << 16) | (memory[i + 3] << 24));

				var at = address + (ulong)i;

				var entry = new StackEntry()
				{
					Address = BasePlatform.ToHex(at, size),
					HexValue = BasePlatform.ToHex(value, size),
					Value = value,
					Offset = Platform.StackFrame.Name.ToUpper() + "-" + (Platform.StackFrame.Value - at).ToString().PadLeft(2, '0'),
					Index = stackentries.Count,
				};

				stackentries.Add(entry);
			}
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

			var clickedEntry = dataGridView1.Rows[e.RowIndex].DataBoundItem as StackEntry;

			var menu = new MenuItem(clickedEntry.Offset + " - " + clickedEntry.HexValue);
			menu.Enabled = false;
			var m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MainForm.OnCopyToClipboard)) { Tag = clickedEntry.HexValue });
			m.MenuItems.Add(new MenuItem("Add to &Watch List", new EventHandler(MainForm.OnAddWatch)) { Tag = new AddWatchArgs(null, clickedEntry.Value, 4) });
			m.MenuItems.Add(new MenuItem("Set &Breakpoint", new EventHandler(MainForm.OnAddBreakPoint)) { Tag = new AddBreakPointArgs(null, clickedEntry.Value) });

			m.Show(dataGridView1, relativeMousePosition);
		}
	}
}
