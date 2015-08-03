// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Movsx : BaseX86Opcode
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			uint a = LoadValue(cpu, instruction.Operand2);
			int size = instruction.Operand1.Size;

			int s = SignExtend(a, instruction.Operand2.Size, size);

			StoreValue(cpu, instruction.Operand1, (uint)s, size);
		}

		protected int SignExtend(uint a, int fromsize, int tosize)
		{
			if (fromsize == tosize)
				return (int)a;

			if (tosize == 32)
			{
				if (fromsize == 16)
				{
					return (int)(short)a;
				}

				if (fromsize == 8)
				{
					return (int)(sbyte)a;
				}
			}

			throw new NotImplementedException();
		}
	}
}