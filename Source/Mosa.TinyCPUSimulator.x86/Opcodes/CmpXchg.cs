/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class CmpXchg : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand1);
			uint b = LoadValue(cpu, instruction.Operand2);
			uint eax = cpu.EAX.Value;
			int size = instruction.Operand1.Size;

			long s = (long)(int)a - (long)(int)eax;
			ulong u = (ulong)a - (ulong)eax;

			UpdateFlags(cpu, size, s, u, true, true, true, true, true);

			cpu.EFLAGS.Adjust = IsAdjustAfterSub(a, b);

			if (cpu.EFLAGS.Zero)
			{
				StoreValue(cpu, instruction.Operand1, b, size);
			}
			else
			{
				cpu.EAX.Value = a;
			}
		}
	}
}