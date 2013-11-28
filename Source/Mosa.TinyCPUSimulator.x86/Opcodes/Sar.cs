﻿/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Sar : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			int shift = ((int)b) & 0x1F;

			if (shift == 0)
				return; // no changes

			bool sign = IsSign(a, size);
			uint u = a >> shift;

			if (sign)
			{
				uint m = (uint.MaxValue << (int)(32 - shift));
				u = u | m;
			}

			StoreValue(cpu, instruction.Operand1, (uint)u, size);

			UpdateFlags(cpu, size, (long)u, u, true, true, false, false, false);

			cpu.EFLAGS.Overflow = (shift != 1);
			cpu.EFLAGS.Carry = ((a >> (shift - 1)) & 0x1) == 1;
			cpu.EFLAGS.Sign = sign;
		}
	}
}