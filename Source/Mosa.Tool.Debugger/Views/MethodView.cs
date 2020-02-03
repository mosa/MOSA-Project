// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.Debugger.DebugData;
using SharpDisasm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	public partial class MethodView : DebugDockContent
	{
		private BindingList<MethodInstructionEntry> instructions = new BindingList<MethodInstructionEntry>();
		private SymbolInfo methodSymbol;
		private byte[] buffer;
		private List<ulong> addresses = new List<ulong>();

		private class MethodInstructionEntry
		{
			[Browsable(false)]
			public ulong IP { get; set; }

			public string Address { get { return "0x" + IP.ToString((IP <= uint.MaxValue) ? "X4" : "X8"); } }

			public string Instruction { get; set; }

			public string Info { get; set; }

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
			dataGridView1.Columns[1].Width = 170;
			dataGridView1.Columns[2].Width = 400;
		}

		public override void OnPause()
		{
			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			var symbol = DebugSource.GetFirstSymbol(InstructionPointer);

			tbMethod.Text = symbol == null ? string.Empty : symbol.CommonName;

			if (symbol != null)
			{
				if (methodSymbol != symbol || instructions.Count == 0)
				{
					Query(symbol);
				}
				else
				{
					SelectRow();
				}
			}
			else
			{
				instructions.Clear();
			}
		}

		private void SelectRow()
		{
			dataGridView1.ClearSelection();

			for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				var methodInstruction = dataGridView1.Rows[i].DataBoundItem as MethodInstructionEntry;

				if (methodInstruction.IP == InstructionPointer)
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

		private void Query(SymbolInfo symbol)
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

				MemoryCache.ReadMemory(at, (uint)len, OnMemoryRead);

				addresses.Add(at);

				at += (ulong)len;
			}
		}

		private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateBuffer(address, bytes)));

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
					var text = translator.Translate(instruction);

					var info = string.Empty;

					var value = ParseAddress(text);

					if (value != 0)
					{
						var symbol = DebugSource.GetFirstSymbolsStartingAt(value);

						if (symbol != null)
						{
							info = symbol.Name;
						}
					}

					var entry = new MethodInstructionEntry()
					{
						IP = instruction.Offset,
						Length = instruction.Length,
						Instruction = text,
						Info = info
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

		public ulong ParseAddress(string decode)
		{
			if (String.IsNullOrWhiteSpace(decode))
				return 0;

			if (!decode.Contains("call"))
				return 0;

			try
			{
				int space = decode.IndexOf(' ');

				if (space <= 0)
					return 0;

				var value = decode.Substring(space + 1).Trim();

				var address = MainForm.ParseHexAddress(value);

				return address;
			}
			catch
			{
				return 0;
			}
		}
	}
}
