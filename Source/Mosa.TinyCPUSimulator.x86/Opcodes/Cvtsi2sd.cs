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
	public class Cvtsi2sd : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			int a = (int)LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			StoreFloatValue(cpu, instruction.Operand1, (double)a, size);
		}
	}
}