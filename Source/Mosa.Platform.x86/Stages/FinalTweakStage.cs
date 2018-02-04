// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Final Tweak Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class FinalTweakStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X86.CallReg, CallReg);
			AddVisitation(X86.In8, In8);
			AddVisitation(X86.In16, In16);
			AddVisitation(X86.Mov, Mov);
			AddVisitation(X86.MovLoad, MovLoad);
			AddVisitation(X86.MovStore, MovStore);
			AddVisitation(X86.Movsd, Movsd);
			AddVisitation(X86.Movss, Movss);
			AddVisitation(X86.Nop, Nop);
			AddVisitation(X86.SetOverflow, Setcc);
			AddVisitation(X86.SetNoOverflow, Setcc);
			AddVisitation(X86.SetCarry, Setcc);
			AddVisitation(X86.SetUnsignedLessThan, Setcc);
			AddVisitation(X86.SetUnsignedGreaterOrEqual, Setcc);
			AddVisitation(X86.SetNoCarry, Setcc);
			AddVisitation(X86.SetEqual, Setcc);
			AddVisitation(X86.SetZero, Setcc);
			AddVisitation(X86.SetNotEqual, Setcc);
			AddVisitation(X86.SetNotZero, Setcc);
			AddVisitation(X86.SetUnsignedLessOrEqual, Setcc);
			AddVisitation(X86.SetUnsignedGreaterThan, Setcc);
			AddVisitation(X86.SetSigned, Setcc);
			AddVisitation(X86.SetNotSigned, Setcc);
			AddVisitation(X86.SetParity, Setcc);
			AddVisitation(X86.SetNoParity, Setcc);
			AddVisitation(X86.SetLessThan, Setcc);
			AddVisitation(X86.SetGreaterOrEqual, Setcc);
			AddVisitation(X86.SetLessOrEqual, Setcc);
			AddVisitation(X86.SetGreaterThan, Setcc);
		}

		#region Visitation Methods

		public void CallReg(Context context)
		{
			Debug.Assert(context.Operand1 != null);
			Debug.Assert(context.Operand1.IsCPURegister);

			var before = context.Previous;

			while (before.IsEmpty && !before.IsBlockStartInstruction)
			{
				before = before.Previous;
			}

			if (before == null || before.IsBlockStartInstruction)
				return;

			if (before.Instruction != X86.Mov)
				return;

			if (!before.Result.IsCPURegister)
				return;

			if (context.Operand1.Register != before.Result.Register)
				return;

			before.SetInstruction(X86.CallReg, null, before.Operand1);
			context.Empty();
		}

		public void In8(Context context)
		{
			Debug.Assert(context.Result.Register == GeneralPurposeRegister.EAX);

			// NOTE: Other option is to use Movzx after IN instruction
			//context.InsertBefore().SetInstruction(X86.Mov, context.Result, ConstantZero);
		}

		public void In16(Context context)
		{
			Debug.Assert(context.Result.Register == GeneralPurposeRegister.EAX);

			// NOTE: Other option is to use Movzx after IN instruction
			//context.InsertBefore().SetInstruction(X86.Mov, context.Result, ConstantZero);
		}

		public void Mov(Context context)
		{
			Operand source = context.Operand1;
			Operand result = context.Result;

			Debug.Assert(result.IsCPURegister);

			if (source.IsCPURegister && result.Register == source.Register)
			{
				context.Empty();
				return;
			}

			var size = context.Size;

			// Mov can not use ESI or EDI registers for 8/16bit values
			if (!(size == InstructionSize.Size16 || size == InstructionSize.Size8))
				return;

			if (source.IsCPURegister && (source.Register == GeneralPurposeRegister.ESI || source.Register == GeneralPurposeRegister.EDI))
			{
				Operand eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);

				context.SetInstruction2(X86.Xchg32, eax, source, source, eax);
				context.AppendInstruction(X86.Mov, result, eax);
				context.AppendInstruction2(X86.Xchg32, source, eax, eax, source);
			}
		}

		public void MovLoad(Context context)
		{
			var size = context.Size;

			// Mov can not use ESI or EDI registers for 8/16bit values
			if (!(size == InstructionSize.Size16 || size == InstructionSize.Size8))
				return;

			Operand result = context.Result;

			Debug.Assert(result.IsCPURegister);

			if (result.Register == GeneralPurposeRegister.ESI || result.Register == GeneralPurposeRegister.EDI)
			{
				Operand source = context.Operand1;
				Operand offset = context.Operand2;

				context.SetInstruction(X86.MovLoad, InstructionSize.Size32, result, source, offset);

				if (size == InstructionSize.Size16)
				{
					context.AppendInstruction(X86.AndConst32, result, result, CreateConstant(0x0000ffff));
				}
				else if (size == InstructionSize.Size8)
				{
					context.AppendInstruction(X86.AndConst32, result, result, CreateConstant(0x000000ff));
				}
			}
		}

		public void MovStore(Context context)
		{
			var size = context.Size;

			// Mov can not use ESI or EDI registers for 8/16bit values
			if (!(size == InstructionSize.Size16 || size == InstructionSize.Size8))
				return;

			Operand value = context.Operand3;

			if (value.IsCPURegister && (value.Register == GeneralPurposeRegister.ESI || value.Register == GeneralPurposeRegister.EDI))
			{
				Operand dest = context.Operand1;
				Operand offset = context.Operand2;

				Operand temporaryRegister = null;

				if (dest.Register != GeneralPurposeRegister.EAX && offset.Register != GeneralPurposeRegister.EAX)
				{
					temporaryRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
				}
				else if (dest.Register != GeneralPurposeRegister.EBX && offset.Register != GeneralPurposeRegister.EBX)
				{
					temporaryRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBX);
				}
				else if (dest.Register != GeneralPurposeRegister.ECX && offset.Register != GeneralPurposeRegister.ECX)
				{
					temporaryRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);
				}
				else
				{
					temporaryRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);
				}

				context.SetInstruction2(X86.Xchg32, temporaryRegister, value, value, temporaryRegister);
				context.AppendInstruction(X86.MovStore, size, null, dest, offset, temporaryRegister);
				context.AppendInstruction2(X86.Xchg32, value, temporaryRegister, temporaryRegister, value);
			}
		}

		public void Movsd(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);
			Debug.Assert(context.Operand1.IsCPURegister);

			if (context.Result.Register == context.Operand1.Register)
			{
				context.Empty();
				return;
			}
		}

		public void Movss(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);
			Debug.Assert(context.Operand1.IsCPURegister);

			if (context.Result.Register == context.Operand1.Register)
			{
				context.Empty();
				return;
			}
		}

		public void Nop(Context context)
		{
			context.Empty();
		}

		public void Setcc(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);

			Operand result = context.Result;
			var instruction = context.Instruction;

			Debug.Assert(result.IsCPURegister);

			// SETcc can not use with ESI or EDI registers
			if (result.Register == GeneralPurposeRegister.ESI || result.Register == GeneralPurposeRegister.EDI)
			{
				var condition = context.ConditionCode;

				Operand eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);

				context.SetInstruction2(X86.Xchg32, eax, result, result, eax);
				context.AppendInstruction(instruction, condition, eax);
				context.AppendInstruction2(X86.Xchg32, result, eax, eax, result);
			}
		}

		#endregion Visitation Methods
	}
}
