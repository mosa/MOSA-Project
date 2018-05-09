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
			AddVisitation(X86.BtConst32, ConvertOperand2ToByte);
			AddVisitation(X86.CallReg, CallReg);
			AddVisitation(X86.Mov32, Mov32);
			AddVisitation(X86.MovLoad16, MovLoad16);
			AddVisitation(X86.MovLoad8, MovLoad8);
			AddVisitation(X86.Movsd, Movsd);
			AddVisitation(X86.Movss, Movss);
			AddVisitation(X86.MovStore16, MovStore16);
			AddVisitation(X86.MovStore8, MovStore8);
			AddVisitation(X86.Movzx16To32, Movzx16To32);
			AddVisitation(X86.Movzx8To32, Movzx8To32);
			AddVisitation(X86.Nop, Nop);
			AddVisitation(X86.SarConst32, ConvertOperand2ToByte);
			AddVisitation(X86.SetByteIfCarry, Setcc);
			AddVisitation(X86.SetByteIfEqual, Setcc);
			AddVisitation(X86.SetByteIfGreaterOrEqual, Setcc);
			AddVisitation(X86.SetByteIfGreaterThan, Setcc);
			AddVisitation(X86.SetByteIfLessOrEqual, Setcc);
			AddVisitation(X86.SetByteIfLessThan, Setcc);
			AddVisitation(X86.SetByteIfNoCarry, Setcc);
			AddVisitation(X86.SetByteIfNoOverflow, Setcc);
			AddVisitation(X86.SetByteIfNoParity, Setcc);
			AddVisitation(X86.SetByteIfNotEqual, Setcc);
			AddVisitation(X86.SetByteIfNotSigned, Setcc);
			AddVisitation(X86.SetByteIfNotZero, Setcc);
			AddVisitation(X86.SetByteIfOverflow, Setcc);
			AddVisitation(X86.SetByteIfParity, Setcc);
			AddVisitation(X86.SetByteIfSigned, Setcc);
			AddVisitation(X86.SetByteIfUnsignedGreaterOrEqual, Setcc);
			AddVisitation(X86.SetByteIfUnsignedGreaterThan, Setcc);
			AddVisitation(X86.SetByteIfUnsignedLessOrEqual, Setcc);
			AddVisitation(X86.SetByteIfUnsignedLessThan, Setcc);
			AddVisitation(X86.SetByteIfZero, Setcc);
			AddVisitation(X86.ShlConst32, ConvertOperand2ToByte);
			AddVisitation(X86.ShldConst32, ConvertOperand3ToByte);
			AddVisitation(X86.ShrConst32, ConvertOperand2ToByte);
			AddVisitation(X86.ShrdConst32, ConvertOperand3ToByte);
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

			if (before.Instruction != X86.Mov32)
				return;

			if (!before.Result.IsCPURegister)
				return;

			if (context.Operand1.Register != before.Result.Register)
				return;

			before.SetInstruction(X86.CallReg, null, before.Operand1);
			context.Empty();
		}

		//public void In8(Context context)
		//{
		//	Debug.Assert(context.Result.Register == GeneralPurposeRegister.EAX);

		//	// NOTE: Other option is to use Movzx after IN instruction
		//	context.InsertBefore().SetInstruction(X86.MovConst32, context.Result, ConstantZero);
		//}

		//public void In16(Context context)
		//{
		//	Debug.Assert(context.Result.Register == GeneralPurposeRegister.EAX);

		//	// NOTE: Other option is to use Movzx after IN instruction
		//	context.InsertBefore().SetInstruction(X86.MovConst32, context.Result, ConstantZero);
		//}

		public void Mov32(Context context)
		{
			Operand source = context.Operand1;
			Operand result = context.Result;

			Debug.Assert(result.IsCPURegister);

			if (source.IsCPURegister && result.Register == source.Register)
			{
				context.Empty();
				return;
			}
		}

		public void MovLoad16(Context context)
		{
			Operand result = context.Result;

			Debug.Assert(result.IsCPURegister);

			if (result.Register == GeneralPurposeRegister.ESI || result.Register == GeneralPurposeRegister.EDI)
			{
				Operand source = context.Operand1;
				Operand offset = context.Operand2;

				context.SetInstruction(X86.MovLoad32, result, source, offset);
				context.AppendInstruction(X86.AndConst32, result, result, CreateConstant(0x0000ffff));
			}
		}

		public void MovLoad8(Context context)
		{
			Operand result = context.Result;

			Debug.Assert(result.IsCPURegister);

			if (result.Register == GeneralPurposeRegister.ESI || result.Register == GeneralPurposeRegister.EDI)
			{
				Operand source = context.Operand1;
				Operand offset = context.Operand2;

				context.SetInstruction(X86.MovLoad32, result, source, offset);
				context.AppendInstruction(X86.AndConst32, result, result, CreateConstant(0x000000ff));
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

		public void MovStore16(Context context)
		{
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

				context.SetInstruction2(X86.XChg32, temporaryRegister, value, value, temporaryRegister);
				context.AppendInstruction(X86.MovStore16, null, dest, offset, temporaryRegister);
				context.AppendInstruction2(X86.XChg32, value, temporaryRegister, temporaryRegister, value);
			}
		}

		public void MovStore8(Context context)
		{
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

				context.SetInstruction2(X86.XChg32, temporaryRegister, value, value, temporaryRegister);
				context.AppendInstruction(X86.MovStore8, null, dest, offset, temporaryRegister);
				context.AppendInstruction2(X86.XChg32, value, temporaryRegister, temporaryRegister, value);
			}
		}
		public void Movzx16To32(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);

			// Movzx8To32 can not use with ESI or EDI registers
			if (context.Operand1.Register != GeneralPurposeRegister.ESI && context.Operand1.Register != GeneralPurposeRegister.EDI)
				return;

			Operand result = context.Result;
			Operand source = context.Operand1;

			if (source.Register != result.Register)
			{
				context.SetInstruction(X86.Mov32, result, source);
				context.AppendInstruction(X86.AndConst32, result, result, CreateConstant(0xffff));
			}
			else
			{
				context.SetInstruction(X86.AndConst32, result, result, CreateConstant(0xffff));
			}
		}

		public void Movzx8To32(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);

			// Movzx8To32 can not use with ESI or EDI registers
			if (context.Operand1.Register != GeneralPurposeRegister.ESI && context.Operand1.Register != GeneralPurposeRegister.EDI)
				return;

			Operand result = context.Result;
			Operand source = context.Operand1;

			if (source.Register != result.Register)
			{
				context.SetInstruction(X86.Mov32, result, source);
				context.AppendInstruction(X86.AndConst32, result, result, CreateConstant(0xff));
			}
			else
			{
				context.SetInstruction(X86.AndConst32, result, result, CreateConstant(0xff));
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

				context.SetInstruction2(X86.XChg32, eax, result, result, eax);
				context.AppendInstruction(instruction, condition, eax);
				context.AppendInstruction2(X86.XChg32, result, eax, eax, result);
			}
		}
		/// <summary>
		/// Converts the operand1 to byte.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConvertOperand1ToByte(Context context)
		{
			if (!context.Operand1.IsConstant || context.Operand1.IsByte)
				return;

			context.Operand1 = CreateConstant((byte)context.Operand1.ConstantUnsignedLongInteger);
		}

		/// <summary>
		/// Converts the operand2 to byte.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConvertOperand2ToByte(Context context)
		{
			if (!context.Operand2.IsConstant || context.Operand2.IsByte)
				return;

			context.Operand2 = CreateConstant((byte)context.Operand2.ConstantUnsignedLongInteger);
		}

		/// <summary>
		/// Converts the operand3 to byte.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConvertOperand3ToByte(Context context)
		{
			if (!context.Operand3.IsConstant || context.Operand3.IsByte)
				return;

			context.Operand3 = CreateConstant((byte)context.Operand3.ConstantUnsignedLongInteger);
		}
		#endregion Visitation Methods
	}
}
