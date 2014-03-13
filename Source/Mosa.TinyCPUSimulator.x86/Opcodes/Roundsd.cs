/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Roundsd : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			double a = LoadFloatValue(cpu, instruction.Operand1);
			uint p = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			//TODO - p is ignored

			double r = Math.Truncate(a);

			StoreFloatValue(cpu, instruction.Operand1, r, size);
		}
	}
}