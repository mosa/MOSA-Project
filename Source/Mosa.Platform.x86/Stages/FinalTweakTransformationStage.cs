// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Final Tweak Transformation Stage
	/// </summary>
	public sealed class FinalTweakTransformationStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X86.Call, Call);
			AddVisitation(X86.In, In);
			AddVisitation(X86.Mov, Mov);
			AddVisitation(X86.MovLoad, MovLoad);
			AddVisitation(X86.MovStore, MovStore);
			AddVisitation(X86.Movsd, Movsd);
			AddVisitation(X86.Movss, Movss);
			AddVisitation(X86.Movsx, Movsx);
			AddVisitation(X86.Movzx, Movzx);
			AddVisitation(X86.Nop, Nop);
			AddVisitation(X86.Setcc, Setcc);
		}

		#region Visitation Methods

		public void Call(Context context)
		{
			if (context.Operand1 == null)
				return;

			if (!context.Operand1.IsCPURegister)
				return;

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

			before.SetInstruction(X86.Call, null, before.Operand1);
			context.Empty();
		}

		public void In(Context context)
		{
			var size = context.Size;

			// Mov can not use ESI or EDI registers for 8/16bit values
			if (!(size == InstructionSize.Size16 || size == InstructionSize.Size8))
				return;

			Debug.Assert(context.Result.Register == GeneralPurposeRegister.EAX);

			// NOTE: Other option is to use Movzx after IN instruction
			context.InsertBefore().SetInstruction(X86.Mov, context.Result, ConstantZero);
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

				context.SetInstruction2(X86.Xchg, eax, source, source, eax);
				context.AppendInstruction(X86.Mov, result, eax);
				context.AppendInstruction2(X86.Xchg, source, eax, eax, source);
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
					context.AppendInstruction(X86.And, result, result, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x0000ffff));
				}
				else if (size == InstructionSize.Size8)
				{
					context.AppendInstruction(X86.And, result, result, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x000000ff));
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

				context.SetInstruction2(X86.Xchg, temporaryRegister, value, value, temporaryRegister);
				context.AppendInstruction(X86.MovStore, size, null, dest, offset, temporaryRegister);
				context.AppendInstruction2(X86.Xchg, value, temporaryRegister, temporaryRegister, value);
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

		public void Movsx(Context context)
		{
			var size = context.Size;

			// Mov can not use ESI or EDI registers for 8/16bit values
			if (!(size == InstructionSize.Size16 || size == InstructionSize.Size8))
				return;

			Operand result = context.Operand1;

			Debug.Assert(result.IsCPURegister);

			if (result.Register == GeneralPurposeRegister.ESI || result.Register == GeneralPurposeRegister.EDI)
			{
				Operand dest = context.Result;

				if (result.Register != dest.Register)
				{
					context.SetInstruction(X86.Mov, dest, result);
				}
				else
				{
					context.Empty();
				}

				if (size == InstructionSize.Size16)
				{
					context.AppendInstruction(X86.And, dest, dest, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x0000ffff));
					context.AppendInstruction(X86.Xor, dest, dest, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x00010000));
					context.AppendInstruction(X86.Sub, dest, dest, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x00010000));
				}
				else if (size == InstructionSize.Size8)
				{
					context.AppendInstruction(X86.And, dest, dest, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x000000ff));
					context.AppendInstruction(X86.Xor, dest, dest, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x00000100));
					context.AppendInstruction(X86.Sub, dest, dest, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x00000100));
				}
			}
		}

		public void Movzx(Context context)
		{
			var size = context.Size;

			// Mov can not use ESI or EDI registers for 8/16bit values
			if (!(size == InstructionSize.Size16 || size == InstructionSize.Size8))
				return;

			Operand result = context.Result;

			Debug.Assert(result.IsCPURegister);

			if (result.Register == GeneralPurposeRegister.ESI || result.Register == GeneralPurposeRegister.EDI)
			{
				Debug.Assert(context.Result.IsCPURegister);

				Operand source = context.Operand1;

				if (source.Register != result.Register)
				{
					context.SetInstruction(X86.Mov, result, source);
				}
				else
				{
					context.Empty();
				}

				if (size == InstructionSize.Size16)
				{
					context.AppendInstruction(X86.And, result, result, Operand.CreateConstant(TypeSystem, 0xffff));
				}
				else if (size == InstructionSize.Size8)
				{
					context.AppendInstruction(X86.And, result, result, Operand.CreateConstant(TypeSystem, 0xff));
				}
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

			Debug.Assert(result.IsCPURegister);

			// SETcc can not use with ESI or EDI registers
			if (result.Register == GeneralPurposeRegister.ESI || result.Register == GeneralPurposeRegister.EDI)
			{
				var condition = context.ConditionCode;

				Operand eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);

				context.SetInstruction2(X86.Xchg, eax, result, result, eax);
				context.AppendInstruction(X86.Setcc, condition, eax);
				context.AppendInstruction2(X86.Xchg, result, eax, eax, result);
			}
		}

		#endregion Visitation Methods
	}
}
