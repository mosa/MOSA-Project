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
	public class Dec : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			int size = instruction.Operand1.Size;

			long s = (long)(int)a;
			ulong u = (ulong)a;

			s--;
			u--;

			StoreValue(cpu, instruction.Operand1, (uint)u, size);

			UpdateFlags(cpu, size, s, u, true, true, true, true, true);

			cpu.EFLAGS.Adjust = IsAdjustAfterAdd(a, 1);
			//cpu.FLAGS.Overflow = !IsSign(a, size) && IsSign(r, size);
		}
	}
}