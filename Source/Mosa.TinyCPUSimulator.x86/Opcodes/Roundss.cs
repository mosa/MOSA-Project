// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Roundss : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			var a = LoadFloatValue(cpu, instruction.Operand1, instruction.Size);
			uint p = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Size;

			float r;
			if ((p & 0x3) == 0x3)
			{
				r = (float)Math.Truncate(a.LowF);
			}
			else if ((p & 0x2) == 0x2)
			{
				r = (float)Math.Ceiling(a.LowF);
			}
			else if ((p & 0x1) == 0x1)
			{
				r = (float)Math.Floor(a.LowF);
			}
			else
			{
				r = (float)Math.Round(a.LowF);
			}

			StoreFloatValue(cpu, instruction.Operand1, r, size);
		}
	}
}
