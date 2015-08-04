// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Roundsd : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			var a = LoadFloatValue(cpu, instruction.Operand1, instruction.Size);
			uint p = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Size;

			double r;
			if ((p & 0x3) == 0x3)
			{
				r = Math.Truncate(a.Low);
			}
			else if ((p & 0x2) == 0x2)
			{
				r = Math.Ceiling(a.Low);
			}
			else if ((p & 0x1) == 0x1)
			{
				r = Math.Floor(a.Low);
			}
			else
			{
				r = Math.Round(a.Low);
			}

			StoreFloatValue(cpu, instruction.Operand1, r, size);
		}
	}
}
