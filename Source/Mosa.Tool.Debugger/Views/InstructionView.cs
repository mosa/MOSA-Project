// Copyright (c) MOSA Project. Licensed under the New BSD License.

using SharpDisasm;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	public partial class InstructionView : DebugDockContent
	{
		private BindingList<InstructionEntry> instructions = new BindingList<InstructionEntry>();

		private class InstructionEntry
		{
			[Browsable(false)]
			public ulong IP { get; set; }

			public string Address { get { return "0x" + IP.ToString((IP <= uint.MaxValue) ? "X4" : "X8"); } }

			public string Instruction { get; set; }

			public int Length { get; set; }
		}

		public InstructionView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			dataGridView1.DataSource = instructions;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[0].Width = 65;
			dataGridView1.Columns[1].Width = 250;
		}

		public override void OnRunning()
		{
			instructions.Clear();
		}

		public override void OnPause()
		{
			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			tbAddress.Text = Platform.InstructionPointer.ToHex();
			Query();
		}

		private void Query()
		{
			if (!IsConnected || !IsPaused)
				return;

			var address = MainForm.ParseHexAddress(tbAddress.Text);
			uint bytes = 512;

			MemoryCache.ReadMemory(address, bytes, OnMemoryRead);
		}

		private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplay(address, bytes)));

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
					var entry = new InstructionEntry()
					{
						IP = instruction.Offset,
						Length = instruction.Length,
						Instruction = translator.Translate(instruction)
					};

					instructions.Add(entry);
				}
			}
		}

		private void toolStripButton1_Click(object sender, System.EventArgs e)
		{
			Query();
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

			var clickedEntry = dataGridView1.Rows[e.RowIndex].DataBoundItem as InstructionEntry;

			var menu = new MenuItem(clickedEntry.Address + " - " + clickedEntry.Instruction);
			menu.Enabled = false;
			var m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MainForm.OnCopyToClipboard)) { Tag = clickedEntry.Address });
			m.MenuItems.Add(new MenuItem("Set &Breakpoint", new EventHandler(MainForm.OnAddBreakPoint)) { Tag = new AddBreakPointArgs(null, clickedEntry.IP, clickedEntry.Instruction) });

			m.Show(dataGridView1, relativeMousePosition);
		}
	}
}
