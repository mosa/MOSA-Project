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
	public class Imul : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (instruction.OperandCount == 1) Execute1(cpu, instruction);
			else if (instruction.OperandCount == 2) Execute2(cpu, instruction);
			else if (instruction.OperandCount == 3) Execute3(cpu, instruction);
		}

		protected void Execute1(CPUx86 cpu, SimInstruction instruction)
		{
			long v1 = LoadValue(cpu, instruction.Operand1);
			long v2 = cpu.EAX.Value;
			int size = instruction.Operand1.Size;

			long r = v1 * v2;

			cpu.EAX.Value = (uint)r;
			cpu.EDX.Value = (uint)(((ulong)r) >> 32);

			cpu.FLAGS.Overflow = false;
			cpu.FLAGS.Carry = false;
		}

		protected void Execute2(CPUx86 cpu, SimInstruction instruction)
		{
			long v1 = LoadValue(cpu, instruction.Operand1);
			long v2 = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			long r = v1 * v2;

			StoreValue(cpu, instruction.Operand1, (uint)r, size);

			cpu.FLAGS.Overflow = (((ulong)r) >> 32) != 0;
			cpu.FLAGS.Carry = cpu.FLAGS.Overflow;
		}

		protected void Execute3(CPUx86 cpu, SimInstruction instruction)
		{
			long v1 = LoadValue(cpu, instruction.Operand2);
			long v2 = LoadValue(cpu, instruction.Operand3);
			int size = instruction.Operand1.Size;

			long r = v1 * v2;

			StoreValue(cpu, instruction.Operand1, (uint)r, size);

			cpu.FLAGS.Overflow = (((ulong)r) >> 32) != 0;
			cpu.FLAGS.Carry = cpu.FLAGS.Overflow;
		}
	}
}