// Copyright (c) MOSA Project. Licensed under the New BSD License.

using SharpDisasm;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.View
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

			tbIP.Text = Platform.InstructionPointer.ToHex();
			txtInstruction.Text = string.Empty;

			GDBConnector.ReadMemory(Platform.InstructionPointer.Value, 16, OnMemoryRead);
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
			if (address != Platform.InstructionPointer.Value)
				return;

			// Determine the architecture mode
			ArchitectureMode mode = ArchitectureMode.x86_64; // todo:

			try
			{
				// Create the disassembler
				using (var disasm = new Disassembler(memory, mode, address, true))
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
				}
			}
			catch
			{
				txtInstruction.Text = "Unable to decode!";
			}
		}
	}
}
