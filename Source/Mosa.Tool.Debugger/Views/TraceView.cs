// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.Debugger.GDB;
using Mosa.Utility.Disassembler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	public partial class TraceView : DebugDockContent
	{
		public TraceView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();

			dataGridView1.ColumnCount = 2;
			dataGridView1.Columns[0].Name = "Address";
			dataGridView1.Columns[1].Name = "Instruction";
			dataGridView1.Columns[0].Width = 75;
			dataGridView1.Columns[1].Width = 200;
		}

		private void SetupGrid()
		{
			if (dataGridView1.ColumnCount != 2)
				return;

			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			dataGridView1.ColumnCount = 2 + Platform.Registers.Count;

			int index = 2;

			foreach (var register in Platform.Registers)
			{
				dataGridView1.Columns[index].Name = register.Name;
				dataGridView1.Columns[index].Width = 75;
				index++;
			}
		}

		public override void OnRunning()
		{
		}

		public override void OnPause()
		{
			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			MemoryCache.ReadMemory(InstructionPointer, 16, OnMemoryRead);
		}

		private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplay(address, bytes)));

		private void UpdateDisplay(ulong address, byte[] memory)
		{
			SetupGrid();

			string opinstruction = "N/A";

			if (address == InstructionPointer)
			{
				var disassembler = new Disassembler("x86");
				disassembler.SetMemory(memory, address);

				foreach (var instruction in disassembler.Decode())
				{
					opinstruction = instruction.Instruction;
					break;
				}
			}

			var elements = new object[dataGridView1.ColumnCount];

			elements[0] = Platform.InstructionPointer.ToHex();
			elements[1] = opinstruction;

			int index = 2;
			foreach (var register in Platform.Registers)
			{
				elements[index++] = register.ToHex();
			}

			dataGridView1.Rows.Add(elements);
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				var filename = saveFileDialog.FileName;

				if (File.Exists(filename))
					File.Delete(filename);

				int columnCount = dataGridView1.Columns.Count;
				string columnNames = "";
				string[] outputCsv = new string[dataGridView1.Rows.Count + 1];
				for (int i = 0; i < columnCount; i++)
				{
					columnNames += dataGridView1.Columns[i].HeaderText.ToString() + "\t";
				}
				outputCsv[0] += columnNames;

				for (int i = 1; (i - 1) < dataGridView1.Rows.Count; i++)
				{
					for (int j = 0; j < columnCount; j++)
					{
						outputCsv[i] += dataGridView1.Rows[i - 1].Cells[j].Value.ToString() + "\t";
					}
				}

				File.WriteAllLines(filename, outputCsv, Encoding.UTF8);
			}
		}
	}
}
