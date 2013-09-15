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
	public class Comisd : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			double a = LoadFloatValue(cpu, instruction.Operand1);
			double b = LoadFloatValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			if (double.IsNaN(a) || double.IsNaN(b))
			{
				cpu.FLAGS.Zero = true;
				cpu.FLAGS.Parity = true;
				cpu.FLAGS.Carry = true;
			}
			else if (a == b)
			{
				cpu.FLAGS.Zero = true;
				cpu.FLAGS.Parity = false;
				cpu.FLAGS.Carry = false;
			}
			else if (a > b)
			{
				cpu.FLAGS.Zero = false;
				cpu.FLAGS.Parity = false;
				cpu.FLAGS.Carry = false;
			}
			else
			{
				cpu.FLAGS.Zero = false;
				cpu.FLAGS.Parity = false;
				cpu.FLAGS.Carry = true;
			}
		}
	}
}