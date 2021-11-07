// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.Debugger.GDB;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	public partial class StackFrameView : DebugDockContent
	{
		private readonly BindingList<StackEntry> stackentries = new BindingList<StackEntry>();

		private class StackEntry
		{
			public string Offset { get; set; }

			public string HexValue { get; set; }

			public ulong Value { get; set; }

			public string Info { get; set; }

			[Browsable(false)]
			public string Address { get; set; }

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
			dataGridView1.Columns[3].Width = 200;
		}

		public override void OnRunning()
		{
			stackentries.Clear();
		}

		protected override void ClearDisplay()
		{
			lbAddress.Text = "";
			stackentries.Clear();
		}

		public override void OnPause()
		{
			ClearDisplay();

			if (StackFrame == 0 || StackPointer == 0)
				return;

			lbAddress.Text = ToHex(StackFrame);

			var span = StackFrame - StackPointer;

			if (span <= 0)
				span = 4;
			else if (span > 512)
				span = 512;

			if (span % 4 != 0)
				span += (span % 4);

			MemoryCache.ReadMemory(StackPointer, (uint)span + NativeIntegerSize, OnMemoryRead);
		}

		private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplay(address, bytes)));

		private void UpdateDisplay(ulong address, byte[] memory)
		{
			for (var i = memory.Length - NativeIntegerSize; i >= 0; i -= (int)NativeIntegerSize)
			{
				ulong value = MainForm.ToLong(memory, (uint)i, NativeIntegerSize);

				var at = address + (ulong)i;
				var offset = StackFrame - at;

				var entry = new StackEntry()
				{
					Address = BasePlatform.ToHex(at, NativeIntegerSize),
					HexValue = BasePlatform.ToHex(value, NativeIntegerSize),
					Value = value,
					Offset = Platform.StackFrame.Name.ToUpperInvariant() +
						(offset >= 0
						? "-" + BasePlatform.ToHex(offset, 1)
						: "+" + BasePlatform.ToHex(-(long)offset, 1)),
					Index = stackentries.Count,
					Info = MainForm.GetAddressInfo(value)
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

			var menu = new ToolStripMenuItem(clickedEntry.Offset + " - " + clickedEntry.HexValue);
			menu.Enabled = false;
			var m = new ContextMenuStrip();
			m.Items.Add(menu);
			m.Items.Add(new ToolStripMenuItem("Copy to &Clipboard", null, new EventHandler(MainForm.OnCopyToClipboard)) { Tag = clickedEntry.HexValue });
			m.Items.Add(new ToolStripMenuItem("Add to &Watch List", null, new EventHandler(MainForm.OnAddWatch)) { Tag = new AddWatchArgs(null, clickedEntry.Value, 4) });
			m.Items.Add(new ToolStripMenuItem("Set &Breakpoint", null, new EventHandler(MainForm.OnAddBreakPoint)) { Tag = new AddBreakPointArgs(null, clickedEntry.Value) });

			m.Show(dataGridView1, relativeMousePosition);
		}
	}
}
