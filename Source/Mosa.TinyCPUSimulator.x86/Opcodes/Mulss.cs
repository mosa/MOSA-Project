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
	public class Mulss : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			var a = LoadFloatValue(cpu, instruction.Operand1, instruction.Size);
			var b = LoadFloatValue(cpu, instruction.Operand2, instruction.Size);
			int size = instruction.Size;

			float r = a.LowF * b.LowF;

			StoreFloatValue(cpu, instruction.Operand1, r, size);
		}
	}
}