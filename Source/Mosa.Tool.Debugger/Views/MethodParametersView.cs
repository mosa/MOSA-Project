// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.Debugger.DebugData;
using Mosa.Tool.Debugger.GDB;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	public partial class MethodParametersView : DebugDockContent
	{
		private readonly BindingList<StackEntry> stackentries = new BindingList<StackEntry>();

		private MethodInfo method;

		private class StackEntry
		{
			public string Offset { get; set; }

			public string HexValue { get; set; }

			public ulong Value { get; set; }

			public string Name { get; set; }

			public uint Size { get; set; }

			public string Type { get; set; }

			public string Info { get; set; }

			[Browsable(false)]
			public string Address { get; set; }

			[Browsable(false)]
			public int Index { get; set; }
		}

		public MethodParametersView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			dataGridView1.DataSource = stackentries;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[0].Width = 75;
			dataGridView1.Columns[1].Width = 75;
			dataGridView1.Columns[2].Width = 75;
			dataGridView1.Columns[3].Width = 200;
			dataGridView1.Columns[4].Width = 40;
			dataGridView1.Columns[5].Width = 200;
			dataGridView1.Columns[6].Width = 200;
		}

		public override void OnRunning()
		{
			ClearDisplay();
		}

		protected override void ClearDisplay()
		{
			stackentries.Clear();
		}

		protected override void UpdateDisplay()
		{
			method = null;
			ClearDisplay();

			if (StackFrame == 0 || StackPointer == 0)
				return;

			method = DebugSource.GetMethod(InstructionPointer);

			if (method == null)
				return;

			if (method.ParameterStackSize == 0)
				return;

			var symbol = DebugSource.GetFirstSymbolsStartingAt(InstructionPointer);

			ulong paramStart = StackFrame + (NativeIntegerSize * 2);

			if (symbol != null)
			{
				// new stack frame has not been setup
				paramStart = StackPointer + NativeIntegerSize;
			}
			else
			{
				symbol = DebugSource.GetFirstSymbolsStartingAt(InstructionPointer - Platform.FirstPrologueInstructionSize);

				if (symbol != null)
				{
					// new stack frame has not been setup
					paramStart = StackPointer + (NativeIntegerSize * 2);
				}
			}

			MemoryCache.ReadMemory(paramStart, method.ParameterStackSize, OnMemoryRead);
		}

		private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplay(address, bytes)));

		private void UpdateDisplay(ulong address, byte[] memory)
		{
			var parameters = DebugSource.GetMethodParameters(method);

			if (parameters == null || parameters.Count == 0)
				return;

			long offset = (long)(address - StackFrame);

			foreach (var parameter in parameters)
			{
				var type = DebugSource.GetTypeInfo(parameter.ParameterTypeID);

				uint size = (parameter.Size == NativeIntegerSize || parameter.Size == NativeIntegerSize * 2) ? parameter.Size : 0;

				if (size == 0 && parameter.Size <= NativeIntegerSize)
				{
					size = NativeIntegerSize;
				}

				ulong value = (size != 0) ? MainForm.ToLong(memory, parameter.Offset, size) : 0;

				var entry = new StackEntry()
				{
					Index = (int)parameter.Index,
					Name = parameter.Name,
					Offset = Platform.StackFrame.Name.ToUpperInvariant() +
						(offset >= 0
						? "+" + BasePlatform.ToHex(offset + parameter.Offset, 1)
						: "-" + BasePlatform.ToHex(-offset + parameter.Offset, 1)),
					Address = BasePlatform.ToHex(StackFrame + parameter.Offset, size),
					Size = size,
					Value = value,
					HexValue = BasePlatform.ToHex(value, size),
					Type = type.FullName,
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
