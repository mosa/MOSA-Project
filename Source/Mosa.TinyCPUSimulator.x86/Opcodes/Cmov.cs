/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace Mosa.TinyCPUSimulator.x86.Opcodes
{

	public class Cmova : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (!cpu.EFLAGS.Carry && !cpu.EFLAGS.Zero) base.Execute(cpu, instruction);
		}
	}

	public class Cmovae : Cmovnc
	{
	}

	public class Cmovb : Cmovc
	{
	}

	public class Cmovbe : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (cpu.EFLAGS.Carry || cpu.EFLAGS.Zero) base.Execute(cpu, instruction);
		}
	}

	public class Cmovc : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (cpu.EFLAGS.Carry) base.Execute(cpu, instruction);
		}
	}

	public class Cmovnc : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (!cpu.EFLAGS.Carry) base.Execute(cpu, instruction);
		}
	}

	public class Cmove : Cmovz
	{
	}

	public class Cmovne : Cmovnz
	{
	}

	public class Cmovg : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (!cpu.EFLAGS.Zero && cpu.EFLAGS.Sign == cpu.EFLAGS.Overflow) base.Execute(cpu, instruction);
		}
	}

	public class Cmovge : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (cpu.EFLAGS.Sign == cpu.EFLAGS.Overflow) base.Execute(cpu, instruction);
		}
	}

	public class Cmovl : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (cpu.EFLAGS.Sign != cpu.EFLAGS.Overflow) base.Execute(cpu, instruction);
		}
	}

	public class Cmovle : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (!cpu.EFLAGS.Zero && cpu.EFLAGS.Sign != cpu.EFLAGS.Overflow) base.Execute(cpu, instruction);
		}
	}

	public class Cmovo : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (cpu.EFLAGS.Overflow) base.Execute(cpu, instruction);
		}
	}

	public class Cmovno : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (!cpu.EFLAGS.Overflow) base.Execute(cpu, instruction);
		}
	}

	public class Cmovp : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (cpu.EFLAGS.Parity) base.Execute(cpu, instruction);
		}
	}

	public class Cmovnp : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (!cpu.EFLAGS.Parity) base.Execute(cpu, instruction);
		}
	}

	public class Cmovs : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (cpu.EFLAGS.Sign) base.Execute(cpu, instruction);
		}
	}

	public class Cmovns : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (!cpu.EFLAGS.Sign) base.Execute(cpu, instruction);
		}
	}

	public class Cmovz : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (cpu.EFLAGS.Zero) base.Execute(cpu, instruction);
		}
	}

	public class Cmovnz : Mov
	{
		public override void Execute(CPUx86 cpu, SimInstruction instruction)
		{
			if (!cpu.EFLAGS.Zero) base.Execute(cpu, instruction);
		}
	}
}