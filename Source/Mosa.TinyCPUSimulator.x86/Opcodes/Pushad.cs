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
	public class Pushad : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			// Push Order: EAX, ECX, EDX, EBX, ESP (original value), EBP, ESI, and EDI
			int size = 32;

			Write(cpu, cpu.ESP.Value - (8 * 0), cpu.EAX.Value, size);
			Write(cpu, cpu.ESP.Value - (8 * 1), cpu.ECX.Value, size);
			Write(cpu, cpu.ESP.Value - (8 * 2), cpu.EDX.Value, size);
			Write(cpu, cpu.ESP.Value - (8 * 3), cpu.EBX.Value, size);
			Write(cpu, cpu.ESP.Value - (8 * 4), cpu.ESP.Value, size);
			Write(cpu, cpu.ESP.Value - (8 * 5), cpu.EBP.Value, size);
			Write(cpu, cpu.ESP.Value - (8 * 6), cpu.ESI.Value, size);
			Write(cpu, cpu.ESP.Value - (8 * 7), cpu.EDI.Value, size);

			cpu.ESP.Value = (uint)(cpu.ESP.Value - (size * 8 / 8));
		}
	}
}