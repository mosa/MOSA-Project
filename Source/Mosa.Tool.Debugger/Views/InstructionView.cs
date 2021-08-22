// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.Disassembler;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	public partial class InstructionView : DebugDockContent
	{
		private readonly BindingList<InstructionEntry> instructions = new BindingList<InstructionEntry>();

		private class InstructionEntry
		{
			[Browsable(false)]
			public ulong IP { get; set; }

			public string Address { get { return DebugDockContent.ToHex(IP); } }

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

		protected override void ClearDisplay()
		{
			tbAddress.Text = string.Empty;
		}

		protected override void UpdateDisplay()
		{
			tbAddress.Text = ToHex(InstructionPointer);

			//var address = MainForm.ParseHexAddress(tbAddress.Text);
			//uint bytes = 512;

			MemoryCache.ReadMemory((uint)InstructionPointer, 512, OnMemoryRead);
		}

		private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplay(address, bytes)));

		private void UpdateDisplay(ulong address, byte[] memory)
		{
			instructions.Clear();

			var disassembler = new Disassembler("x86");
			disassembler.SetMemory(memory, address);

			foreach (var instruction in disassembler.Decode())
			{
				var entry = new InstructionEntry()
				{
					IP = instruction.Address,
					Length = instruction.Length,
					Instruction = instruction.Instruction.ToString()
				};

				instructions.Add(entry);
			}
		}

		private void toolStripButton1_Click(object sender, System.EventArgs e)
		{
			if (IsReady)
				UpdateDisplay();
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

			var menu = new ToolStripMenuItem(clickedEntry.Address + " - " + clickedEntry.Instruction);
			menu.Enabled = false;
			var m = new ContextMenuStrip();
			m.Items.Add(menu);
			m.Items.Add(new ToolStripMenuItem("Copy to &Clipboard", null, new EventHandler(MainForm.OnCopyToClipboard)) { Tag = clickedEntry.Address });
			m.Items.Add(new ToolStripMenuItem("Set &Breakpoint", null, new EventHandler(MainForm.OnAddBreakPoint)) { Tag = new AddBreakPointArgs(null, clickedEntry.IP, clickedEntry.Instruction) });

			m.Show(dataGridView1, relativeMousePosition);
		}
	}
}
