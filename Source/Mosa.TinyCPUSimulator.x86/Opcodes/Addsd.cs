/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Addsd : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			var a = LoadFloatValue(cpu, instruction.Operand1);
			var b = LoadFloatValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			double r = a.Low + b.Low;

			StoreFloatValue(cpu, instruction.Operand1, r, size);
		}
	}
}