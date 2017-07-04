// Copyright (c) MOSA Project. Licensed under the New BSD License.

using SharpDisasm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.View
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

			var address = MainForm.ParseMemoryAddress(tbAddress.Text);
			var bytes = 512;

			GDBConnector.ReadMemory(address, bytes, OnMemoryRead);
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
			instructions.Clear();

			var mode = ArchitectureMode.x86_32; // todo:

			using (var disasm = new Disassembler(memory, mode, address, true))
			{
				var translator = new SharpDisasm.Translators.IntelTranslator();

				translator.IncludeAddress = false;
				translator.IncludeBinary = false;

				foreach (var instruction in disasm.Disassemble())
				{
					var entry = new InstructionEntry()
					{
						IP = instruction.PC,
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
	}
}
