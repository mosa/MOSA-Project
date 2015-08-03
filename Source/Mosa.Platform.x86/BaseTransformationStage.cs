// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.x86
{
	/// <summary>
	///
	/// </summary>
	public abstract class BaseTransformationStage : BasePlatformTransformationStage, IX86Visitor
	{
		protected override string Platform { get { return "x86"; } }

		public static X86Instruction GetMove(Operand Destination, Operand Source)
		{
			if (Source.IsR8 && Destination.IsR8)
			{
				return X86.Movsd;
			}
			else if (Source.IsR4 && Destination.IsR4)
			{
				return X86.Movss;
			}
			else if (Source.IsR4 && Destination.IsR8)
			{
				return X86.Cvtss2sd;
			}
			else if (Source.IsR8 && Destination.IsR4)
			{
				return X86.Cvtsd2ss;
			}
			else if (Source.IsR8 && Destination.IsMemoryAddress)
			{
				return X86.Movsd;
			}
			else if (Source.IsR4 && Destination.IsMemoryAddress)
			{
				return X86.Movss;
			}
			else
			{
				return X86.Mov;
			}
		}

		#region IX86Visitor

		public virtual void Add(Context context)
		{
		}

		public virtual void Adc(Context context)
		{
		}

		public virtual void And(Context context)
		{
		}

		public virtual void Call(Context context)
		{
		}

		public virtual void DirectCompare(Context context)
		{
		}

		public virtual void Cmp(Context context)
		{
		}

		public virtual void Cmov(Context context)
		{
		}

		public virtual void Or(Context context)
		{
		}

		public virtual void Xor(Context context)
		{
		}

		public virtual void PXor(Context context)
		{
		}

		public virtual void Sub(Context context)
		{
		}

		public virtual void Sbb(Context context)
		{
		}

		public virtual void Mul(Context context)
		{
		}

		public virtual void Div(Context context)
		{
		}

		public virtual void IDiv(Context context)
		{
		}

		public virtual void IMul(Context context)
		{
		}

		public virtual void AddSs(Context context)
		{
		}

		public virtual void SubSS(Context context)
		{
		}

		public virtual void SubSD(Context context)
		{
		}

		public virtual void MulSS(Context context)
		{
		}

		public virtual void MulSD(Context context)
		{
		}

		public virtual void DivSS(Context context)
		{
		}

		public virtual void DivSD(Context context)
		{
		}

		public virtual void Sar(Context context)
		{
		}

		public virtual void Sal(Context context)
		{
		}

		public virtual void Shl(Context context)
		{
		}

		public virtual void Shr(Context context)
		{
		}

		public virtual void Rcr(Context context)
		{
		}

		public virtual void Cvtsi2ss(Context context)
		{
		}

		public virtual void Cvtsi2sd(Context context)
		{
		}

		public virtual void Cvtsd2ss(Context context)
		{
		}

		public virtual void Cvtss2sd(Context context)
		{
		}

		public virtual void Cvttsd2si(Context context)
		{
		}

		public virtual void Cvttss2si(Context context)
		{
		}

		public virtual void Setcc(Context context)
		{
		}

		public virtual void Cdq(Context context)
		{
		}

		public virtual void Shld(Context context)
		{
		}

		public virtual void Shrd(Context context)
		{
		}

		public virtual void Comisd(Context context)
		{
		}

		public virtual void Comiss(Context context)
		{
		}

		public virtual void Ucomisd(Context context)
		{
		}

		public virtual void Ucomiss(Context context)
		{
		}

		public virtual void Jns(Context context)
		{
		}

		public virtual void Branch(Context context)
		{
		}

		public virtual void Jump(Context context)
		{
		}

		public virtual void BochsDebug(Context context)
		{
		}

		public virtual void Cli(Context context)
		{
		}

		public virtual void Cld(Context context)
		{
		}

		public virtual void CmpXchg(Context context)
		{
		}

		public virtual void CpuId(Context context)
		{
		}

		public virtual void Hlt(Context context)
		{
		}

		public virtual void Invlpg(Context context)
		{
		}

		public virtual void In(Context context)
		{
		}

		public virtual void Inc(Context context)
		{
		}

		public virtual void Dec(Context context)
		{
		}

		public virtual void Int(Context context)
		{
		}

		public virtual void Iretd(Context context)
		{
		}

		public virtual void Lea(Context context)
		{
		}

		public virtual void Lgdt(Context context)
		{
		}

		public virtual void Lidt(Context context)
		{
		}

		public virtual void Lock(Context context)
		{
		}

		public virtual void Neg(Context context)
		{
		}

		public virtual void Mov(Context context)
		{
		}

		public virtual void Movsx(Context context)
		{
		}

		public virtual void Movss(Context context)
		{
		}

		public virtual void Movsd(Context context)
		{
		}

		public virtual void MovAPS(Context context)
		{
		}

		public virtual void Movzx(Context context)
		{
		}

		public virtual void Nop(Context context)
		{
		}

		public virtual void Out(Context context)
		{
		}

		public virtual void Pause(Context context)
		{
		}

		public virtual void Pop(Context context)
		{
		}

		public virtual void Popad(Context context)
		{
		}

		public virtual void Popfd(Context context)
		{
		}

		public virtual void Push(Context context)
		{
		}

		public virtual void Pushad(Context context)
		{
		}

		public virtual void Pushfd(Context context)
		{
		}

		public virtual void Rdmsr(Context context)
		{
		}

		public virtual void Rdpmc(Context context)
		{
		}

		public virtual void Rdtsc(Context context)
		{
		}

		public virtual void Rep(Context context)
		{
		}

		public virtual void Sti(Context context)
		{
		}

		public virtual void Stosb(Context context)
		{
		}

		public virtual void Stosd(Context context)
		{
		}

		public virtual void Xchg(Context context)
		{
		}

		public virtual void Not(Context context)
		{
		}

		public virtual void RoundSS(Context context)
		{
		}

		public virtual void RoundSD(Context context)
		{
		}

		public virtual void Test(Context context)
		{
		}

		#endregion IX86Visitor

	}
}