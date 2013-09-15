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
	public class Pop : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			int size = instruction.Operand1.Size;

			uint value = Read(cpu, cpu.ESP.Value, size);

			(instruction.Operand1.Register as GeneralPurposeRegister).Value = value;

			cpu.ESP.Value = (uint)(cpu.ESP.Value + (size / 8));
		}
	}
}