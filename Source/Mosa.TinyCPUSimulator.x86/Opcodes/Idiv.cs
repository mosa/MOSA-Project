// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Idiv : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint v1 = cpu.EDX.Value;
			uint v2 = cpu.EAX.Value;
			uint v3 = LoadValue(cpu, instruction.Operand1);
			int size = instruction.Operand1.Size;

			// TODO: for sizes other than 32
			Debug.Assert(size == 32);

			// TODO: exception if v3 is 0

			long v = (((long)v1 << 32) | (long)v2);
			long a = (long)(int)v / (long)(int)v3;
			long r = (long)(int)v % (long)(int)v3;

			cpu.EAX.Value = (uint)a;
			cpu.EDX.Value = (uint)r;
		}
	}
}