// Copyright (c) MOSA Project. Licensed under the New BSD License.

using SharpDisasm;
using System;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger
{
	public partial class StatusView : DebugDockContent
	{
		public StatusView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public override void OnRunning()
		{
			txtInstruction.Text = "Running...";
			tbIP.Text = string.Empty;
		}

		public override void OnPause()
		{
			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			// todo: get memory and decode instruction

			tbIP.Text = Platform.InstructionPointer.ToHex();
			txtInstruction.Text = string.Empty; // todo

			GDBConnector.ReadMemory(Platform.InstructionPointer.Value, 16, OnMemoryRead);

			//Refresh();
		}

		private void OnMemoryRead(byte[] bytes)
		{
			MethodInvoker method = delegate ()
			{
				UpdateDisplay(bytes);
			};

			BeginInvoke(method);
		}

		private void UpdateDisplay(byte[] memory)
		{
			// Determine the architecture mode
			ArchitectureMode mode = ArchitectureMode.x86_64; // todo:

			try
			{
				// Create the disassembler
				using (var disasm = new Disassembler(memory, mode, Platform.InstructionPointer.Value, true))
				{
					// Need a new instance of translator every time as they aren't thread safe
					var translator = new SharpDisasm.Translators.IntelTranslator();

					// Configure the translator to output instruction addresses and instruction binary as hex
					translator.IncludeAddress = false;
					translator.IncludeBinary = false;

					// Disassemble instruction
					foreach (var instruction in disasm.Disassemble())
					{
						var asm = translator.Translate(instruction);
						txtInstruction.Text = asm;
						break;
					}

					//var disassembled = disasm.Disassemble();
					//var instruction = disassembled.GetEnumerator().Current;
					//var asm = translator.Translate(instruction);
					//txtInstruction.Text = asm;
				}
			}
			catch
			{
				txtInstruction.Text = "Unable to decode!";
			}
		}
	}
}
