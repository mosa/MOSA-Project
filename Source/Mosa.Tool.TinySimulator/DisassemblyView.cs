// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.TinyCPUSimulator;
using SharpDisasm;

namespace Mosa.Tool.TinySimulator
{
	public partial class DisassemblyView : SimulatorDockContent
	{
		public SimAssemblyCode simAssemblyCode;

		public DisassemblyView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public string GetInstruction(ulong location)
		{
			//if (simAssemblyCode == null || simAssemblyCode.simCPU != SimCPU)
			//{
			//	simAssemblyCode = new SimAssemblyCode(SimCPU);
			//}

			//var disasm = new Disassembler(simAssemblyCode, ArchitectureMode.x86_32, location, location);

			var disasm = new Disassembler(new SimAssemblyCode(SimCPU, location), ArchitectureMode.x86_32, location);

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
