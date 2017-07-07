// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.DebugData;
using SharpDisasm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.View
{
	public partial class MethodView : DebugDockContent
	{
		private BindingList<MethodInstructionEntry> instructions = new BindingList<MethodInstructionEntry>();
		private Symbol methodSymbol;
		private byte[] buffer;
		private List<ulong> addresses = new List<ulong>();

		private class MethodInstructionEntry
		{
			[Browsable(false)]
			public ulong IP { get; set; }

			public string Address { get { return "0x" + IP.ToString((IP <= uint.MaxValue) ? "X4" : "X8"); } }

			public string Instruction { get; set; }

			[Browsable(false)]
			public int Length { get; set; }
		}

		public MethodView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			dataGridView1.DataSource = instructions;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[0].Width = 65;
			dataGridView1.Columns[1].Width = 250;
		}

		public override void OnPause()
		{
			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			//tbMethod.Text = Platform.InstructionPointer.ToHex();

			var symbol = DebugSource.GetFirstSymbol(Platform.InstructionPointer.Value);

			tbMethod.Text = symbol == null ? string.Empty : symbol.CommonName;

			if (symbol != null)
			{
				if (methodSymbol != symbol)
				{
					Query(symbol);
				}
				else
				{
					SelectRow();
				}
			}
		}

		private void SelectRow()
		{
			dataGridView1.ClearSelection();
			ulong ip = Platform.InstructionPointer.Value;

			for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				var methodInstruction = dataGridView1.Rows[i].DataBoundItem as MethodInstructionEntry;

				if (methodInstruction.IP == ip)
				{
					dataGridView1.Rows[i].Selected = true;

					int firstDisplayed = dataGridView1.FirstDisplayedScrollingRowIndex;
					int displayed = dataGridView1.DisplayedRowCount(true);
					int lastVisible = (firstDisplayed + displayed) - 1;

					if (i > lastVisible || i < firstDisplayed)
					{
						dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows[i].Index;
					}

					break;
				}
			}
		}

		private void Query(Symbol symbol)
		{
			if (!IsConnected || !IsPaused)
				return;

			addresses.Clear();
			instructions.Clear();
			methodSymbol = symbol;

			int MaxMemoryQuery = 1024;

			ulong at = symbol.Address;
			ulong end = symbol.Address + (ulong)symbol.Length;

			buffer = new byte[end - at];

			while (at < end)
			{
				int len = (int)(end - at);

				if (len > MaxMemoryQuery)
					len = MaxMemoryQuery;

				GDBConnector.ReadMemory(at, len, OnMemoryRead);

				addresses.Add(at);

				at += (ulong)len;
			}
		}

		private void OnMemoryRead(ulong address, byte[] bytes)
		{
			MethodInvoker method = delegate ()
			{
				UpdateBuffer(address, bytes);
			};

			BeginInvoke(method);
		}

		private void UpdateBuffer(ulong address, byte[] memory)
		{
			int offset = (int)(address - methodSymbol.Address);

			for (int i = 0; i < memory.Length; i++)
			{
				buffer[offset + i] = memory[i];
			}

			addresses.Remove(address);

			if (addresses.Count == 0)
			{
				UpdateDisplay(methodSymbol.Address, buffer);
				SelectRow();
			}
		}

		private void UpdateDisplay(ulong address, byte[] memory)
		{
			instructions.Clear();

			var mode = ArchitectureMode.x86_32; // todo:

			using (var disasm = new Disassembler(memory, mode, address, true, Vendor.Any))
			{
				var translator = new SharpDisasm.Translators.IntelTranslator()
				{
					IncludeAddress = false,
					IncludeBinary = false
				};

				foreach (var instruction in disasm.Disassemble())
				{
					var entry = new MethodInstructionEntry()
					{
						IP = instruction.Offset,
						Length = instruction.Length,
						Instruction = translator.Translate(instruction)
					};

					instructions.Add(entry);
				}
			}
		}

		private void MethodView_Resize(object sender, EventArgs e)
		{
			int width = this.Width - 80;

			if (width < 0)
				width = 50;

			tbMethod.Width = width;
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

			var clickedEntry = dataGridView1.Rows[e.RowIndex].DataBoundItem as MethodInstructionEntry;

			var menu = new MenuItem(clickedEntry.Address + " - " + clickedEntry.Instruction);
			menu.Enabled = false;
			var m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MainForm.OnCopyToClipboard)) { Tag = clickedEntry.Address });
			m.MenuItems.Add(new MenuItem("Set &Breakpoint", new EventHandler(MainForm.OnAddBreakPoint)) { Tag = new AddBreakPointArgs(null, clickedEntry.IP) });

			m.Show(dataGridView1, relativeMousePosition);
		}
	}
}
