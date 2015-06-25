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
	public class Comiss : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			var a = LoadFloatValue(cpu, instruction.Operand1).LowF;
			var b = LoadFloatValue(cpu, instruction.Operand2).LowF;
			int size = instruction.Operand1.Size;

			if (float.IsNaN(a) || float.IsNaN(b))
			{
				cpu.EFLAGS.Zero = true;
				cpu.EFLAGS.Parity = true;
				cpu.EFLAGS.Carry = true;
			}
			else if (a == b)
			{
				cpu.EFLAGS.Zero = true;
				cpu.EFLAGS.Parity = false;
				cpu.EFLAGS.Carry = false;
			}
			else if (a > b)
			{
				cpu.EFLAGS.Zero = false;
				cpu.EFLAGS.Parity = false;
				cpu.EFLAGS.Carry = false;
			}
			else
			{
				cpu.EFLAGS.Zero = false;
				cpu.EFLAGS.Parity = false;
				cpu.EFLAGS.Carry = true;
			}
		}
	}
}