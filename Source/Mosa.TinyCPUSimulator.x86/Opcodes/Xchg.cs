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
	public class Xchg : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand2.Size;

			// memory must update happen first due to protential page fault
			if (instruction.Operand1.IsRegister)
			{
				StoreValue(cpu, instruction.Operand2, a, size);
				StoreValue(cpu, instruction.Operand1, b, size);
			}
			else
			{
				StoreValue(cpu, instruction.Operand1, b, size);
				StoreValue(cpu, instruction.Operand2, a, size);
			}
		}
	}
}