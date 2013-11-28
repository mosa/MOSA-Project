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
	public class Movzx : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			StoreValue(cpu, instruction.Operand1, a, size);
		}
	}
}