// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.TinyCPUSimulator;
using SharpDisasm.Udis86;
using SharpDisasm;

namespace Mosa.Tool.TinySimulator
{
	public partial class DisassemblyView : SimulatorDockContent
	{
		public DisassemblyView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public string GetInstruction(ulong location)
		{
			var raw = new byte[16];

			// read the 16 bytes (max instruction length on x86 is 15 bytes) from EIP
			for (int i = 0; i < 15; i = i + 4)
			{
				var value = SimCPU.DirectRead32(location + (ulong)i);

				raw[i] = (byte)(value & 0xFF);
				raw[i + 1] = (byte)(value >> 8 & 0xFF);
				raw[i + 2] = (byte)(value >> 16 & 0xFF);
				raw[i + 3] = (byte)(value >> 24 & 0xFF);
			}

			var disasm = new Disassembler(raw, ArchitectureMode.x86_32);
			var instruction = disasm.NextInstruction();

			return instruction.ToString();
		}

		public override void UpdateDock(BaseSimState simState)
		{
			tbLastInstruction.Text = GetInstruction(SimCPU.LastProgramCounter);
			tbNextInstruction.Text = GetInstruction(SimCPU.CurrentProgramCounter);

			Refresh();
		}
	}
}
