// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.Disassembler;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
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
			tbInstruction.Text = "Running...";
			tbIP.Text = string.Empty;
			tbMethod.Text = string.Empty;
		}

		public override void OnPause()
		{
			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			tbIP.Text = Platform.InstructionPointer.ToHex();
			tbInstruction.Text = string.Empty;

			MemoryCache.ReadMemory(InstructionPointer, 16, OnMemoryRead);

			var symbol = DebugSource.GetFirstSymbol(InstructionPointer);

			tbMethod.Text = symbol == null ? string.Empty : symbol.CommonName;
		}

		private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplay(address, bytes)));

		private void UpdateDisplay(ulong address, byte[] memory)
		{
			if (address != InstructionPointer)
				return;

			var disassembler = new Disassembler("x86");
			disassembler.SetMemory(memory, address);

			tbInstruction.Text = "Unable to decode!";

			foreach (var instruction in disassembler.Decode())
			{
				tbInstruction.Text = instruction.Instruction;
				break;
			}
		}
	}
}
