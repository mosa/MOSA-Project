// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{
	public class Setcc : BaseX86Opcode
	{
		/// <summary>
		/// Sets the Operand value to 1 if the condition is met, otherwise set to 0.
		/// </summary>
		/// <param name="cpu"></param>
		/// <param name="instruction"></param>
		/// <param name="conditionMet">Boolean indicating if the condition was met</param>
		public void SetValue(CPUx86 cpu, SimInstruction instruction, bool conditionMet)
		{
			if (conditionMet)
			{
				StoreValue(cpu, instruction.Operand1, 1, 8);
			}
			else
			{
				StoreValue(cpu, instruction.Operand1, 0, 8);
			}
		}
	}

	public class Seta : Setcc
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			SetValue(cpu, instruction, !cpu.EFLAGS.Carry && !cpu.EFLAGS.Zero);
		}
	}

	public class Setbe : Setcc
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			SetValue(cpu, instruction, cpu.EFLAGS.Carry || cpu.EFLAGS.Zero);
		}
	}

	public class Setc : Setcc
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			SetValue(cpu, instruction, cpu.EFLAGS.Carry);
		}
	}

	public class Setnc : Setcc
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			SetValue(cpu, instruction, !cpu.EFLAGS.Carry);
		}
	}

	public class Sete : Setz
	{
	}

	public class Setne : Setnz
	{
	}

	public class Setg : Setcc
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			SetValue(cpu, instruction, !cpu.EFLAGS.Zero && cpu.EFLAGS.Sign == cpu.EFLAGS.Overflow);
		}
	}

	public class Setge : Setcc
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			SetValue(cpu, instruction, cpu.EFLAGS.Sign == cpu.EFLAGS.Overflow);
		}
	}

	public class Setl : Setcc
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			SetValue(cpu, instruction, cpu.EFLAGS.Sign != cpu.EFLAGS.Overflow);
		}
	}

	public class Setle : Setcc
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			SetValue(cpu, instruction, cpu.EFLAGS.Sign != cpu.EFLAGS.Overflow || cpu.EFLAGS.Zero);
		}
	}

	public class Setnp : Setcc
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			SetValue(cpu, instruction, !cpu.EFLAGS.Parity);
		}
	}

	public class Setp : Setcc
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			SetValue(cpu, instruction, cpu.EFLAGS.Parity);
		}
	}

	public class Setpe : Setp
	{
	}

	public class Setpo : Setnp
	{
	}

	public class Setnz : Setcc
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			SetValue(cpu, instruction, !cpu.EFLAGS.Zero);
		}
	}

	public class Setz : Setcc
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			SetValue(cpu, instruction, cpu.EFLAGS.Zero);
		}
	}
}
