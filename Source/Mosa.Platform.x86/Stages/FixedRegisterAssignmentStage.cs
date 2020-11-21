// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Fixed Register Assignment Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class FixedRegisterAssignmentStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X86.Cdq32, Cdq32);
			AddVisitation(X86.Div32, Div32);
			AddVisitation(X86.IDiv32, IDiv32);
			AddVisitation(X86.IMul32, IMul32);
			AddVisitation(X86.In8, In8);
			AddVisitation(X86.In16, In16);
			AddVisitation(X86.In32, In32);
			AddVisitation(X86.MovStoreSeg32, MovStoreSeg32);
			AddVisitation(X86.Mul32, Mul32);
			AddVisitation(X86.Out8, Out);
			AddVisitation(X86.Out16, Out);
			AddVisitation(X86.Out32, Out);
			AddVisitation(X86.Rcr32, Rcr32);
			AddVisitation(X86.Sar32, Sar32);
			AddVisitation(X86.Shl32, Shl32);
			AddVisitation(X86.Shld32, Shld32);
			AddVisitation(X86.Shr32, Shr32);
			AddVisitation(X86.Shrd32, Shrd32);
			AddVisitation(X86.RdMSR, RdMSR);
			AddVisitation(X86.WrMSR, WrMSR);
		}

		#region Visitation Methods

		private void Cdq32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.EDX
				&& context.Result2.Register == GeneralPurposeRegister.EAX
				&& context.Operand1.Register == GeneralPurposeRegister.EAX)
				return;

			var operand1 = context.Operand1;
			var result = context.Result;
			var result2 = context.Result2;

			var EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			var EDX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EAX, operand1);
			context.AppendInstruction2(X86.Cdq32, EDX, EAX, EAX);
			context.AppendInstruction(X86.Mov32, result, EDX);
			context.AppendInstruction(X86.Mov32, result2, EAX);
		}

		private void Div32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.EDX
				&& context.Result2.Register == GeneralPurposeRegister.EAX
				&& context.Operand1.Register == GeneralPurposeRegister.EDX
				&& context.Operand2.Register == GeneralPurposeRegister.EAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;
			var result2 = context.Result2;

			var EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			var EDX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EDX, operand1);
			context.AppendInstruction(X86.Mov32, EAX, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X86.Div32, EDX, EAX, EDX, EAX, operand3);
			}
			else
			{
				var v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X86.Mov32, v3, operand3);
				context.AppendInstruction2(X86.Div32, EDX, EAX, EDX, EAX, v3);
			}

			context.AppendInstruction(X86.Mov32, result2, EAX);
			context.AppendInstruction(X86.Mov32, result, EDX);
		}

		private void IDiv32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Operand2.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.EDX
				&& context.Result2.Register == GeneralPurposeRegister.EAX
				&& context.Operand1.Register == GeneralPurposeRegister.EDX
				&& context.Operand2.Register == GeneralPurposeRegister.EAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;
			var result2 = context.Result2;

			var EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			var EDX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EDX, operand1);
			context.AppendInstruction(X86.Mov32, EAX, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X86.IDiv32, EDX, EAX, EDX, EAX, operand3);
			}
			else
			{
				var v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X86.Mov32, v3, operand3);
				context.AppendInstruction2(X86.IDiv32, EDX, EAX, EDX, EAX, v3);
			}

			context.AppendInstruction(X86.Mov32, result2, EAX);
			context.AppendInstruction(X86.Mov32, result, EDX);
		}

		private void IMul32(Context context)
		{
			if (!context.Operand2.IsConstant)
				return;

			var v1 = AllocateVirtualRegister(context.Operand2);
			var operand2 = context.Operand2;

			context.Operand2 = v1;
			context.InsertBefore().SetInstruction(X86.Mov32, v1, operand2);
		}

		private void In16(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.EAX
				&& (context.Operand1.Register == GeneralPurposeRegister.EDX || context.Operand1.IsConstant))
				return;

			var result = context.Result;
			var operand1 = context.Operand1;

			var EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			var EDX = Operand.CreateCPURegister(operand1.Type, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EDX, operand1);
			context.AppendInstruction(X86.In16, EAX, EDX);
			context.AppendInstruction(X86.Mov32, result, EAX);
		}

		private void In32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.EAX
				&& (context.Operand1.Register == GeneralPurposeRegister.EDX || context.Operand1.IsConstant))
				return;

			var result = context.Result;
			var operand1 = context.Operand1;

			var EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			var EDX = Operand.CreateCPURegister(operand1.Type, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EDX, operand1);
			context.AppendInstruction(X86.In32, EAX, EDX);
			context.AppendInstruction(X86.Mov32, result, EAX);
		}

		private void In8(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.EAX
				&& (context.Operand1.Register == GeneralPurposeRegister.EDX || context.Operand1.IsConstant))
				return;

			var result = context.Result;
			var operand1 = context.Operand1;

			var EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			var EDX = Operand.CreateCPURegister(operand1.Type, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EDX, operand1);
			context.AppendInstruction(X86.In8, EAX, EDX);
			context.AppendInstruction(X86.Mov32, result, EAX);
		}

		private void MovStoreSeg32(Context context)
		{
			if (!context.Operand1.IsConstant)
				return;

			var result = context.Result;
			var operand1 = context.Operand1;

			var v1 = AllocateVirtualRegister(operand1);

			context.SetInstruction(X86.Mov32, v1, operand1);
			context.AppendInstruction(X86.MovStoreSeg32, result, v1);
		}

		private void Mul32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& !context.Operand2.IsConstant
				&& context.Result.Register == GeneralPurposeRegister.EDX
				&& context.Result2.Register == GeneralPurposeRegister.EAX
				&& context.Operand1.Register == GeneralPurposeRegister.EAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;
			var result2 = context.Result2;

			var EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			var EDX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EAX, operand1);

			if (operand2.IsConstant)
			{
				Operand v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X86.Mov32, v3, operand2);
				operand2 = v3;
			}

			Debug.Assert(operand2.IsCPURegister || operand2.IsVirtualRegister);

			context.AppendInstruction2(X86.Mul32, EDX, EAX, EAX, operand2);
			context.AppendInstruction(X86.Mov32, result, EDX);
			context.AppendInstruction(X86.Mov32, result2, EAX);
		}

		private void Out(Context context)
		{
			// TRANSFORM: OUT <= EDX, EAX && OUT <= imm8, EAX
			if (context.Operand1.IsCPURegister
				&& context.Operand2.IsCPURegister
				&& (context.Operand1.Register == GeneralPurposeRegister.EDX || context.Operand1.IsConstant)
				&& context.Operand2.Register == GeneralPurposeRegister.EAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var instruction = context.Instruction;

			var EAX = Operand.CreateCPURegister(operand2.Type, GeneralPurposeRegister.EAX);
			var EDX = Operand.CreateCPURegister(operand1.Type, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EDX, operand1);
			context.AppendInstruction(X86.Mov32, EAX, operand2);
			context.AppendInstruction(instruction, null, EDX, EAX);
		}

		private void Rcr32(Context context)
		{
			HandleShiftOperation32(context, X86.Rcr32);
		}

		private void Sar32(Context context)
		{
			HandleShiftOperation32(context, X86.Sar32);
		}

		private void Shl32(Context context)
		{
			HandleShiftOperation32(context, X86.Shl32);
		}

		private void Shld32(Context context)
		{
			if (context.Operand3.IsConstant)
				return;

			if (context.Operand3.Register == GeneralPurposeRegister.ECX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;

			var ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov32, ECX, operand3);
			context.AppendInstruction(X86.Shld32, result, operand1, operand2, ECX);
		}

		private void Shr32(Context context)
		{
			HandleShiftOperation32(context, X86.Shr32);
		}

		private void Shrd32(Context context)
		{
			if (context.Operand3.IsConstant)
				return;

			if (context.Operand3.Register == GeneralPurposeRegister.ECX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;

			var ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov32, ECX, operand3);
			context.AppendInstruction(X86.Shrd32, result, operand1, operand2, ECX);
		}

		private void RdMSR(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.EAX
				&& context.Result2.Register == GeneralPurposeRegister.EDX
				&& context.Operand1.Register == GeneralPurposeRegister.ECX)
				return;

			var operand1 = context.Operand1;
			var result = context.Result;
			var result2 = context.Result2;

			var EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			var EDX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);
			var ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov32, ECX, operand1);
			context.AppendInstruction2(X86.RdMSR, EAX, EDX, ECX);
			context.AppendInstruction(X86.Mov32, result, EAX);
			context.AppendInstruction(X86.Mov32, result2, EDX);
		}

		private void WrMSR(Context context)
		{
			if (context.Operand1.IsCPURegister
				&& context.Operand2.IsCPURegister
				&& context.Operand3.IsCPURegister
				&& context.Operand1.Register == GeneralPurposeRegister.ECX
				&& context.Operand2.Register == GeneralPurposeRegister.EAX
				&& context.Operand3.Register == GeneralPurposeRegister.EDX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			var EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			var EDX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);
			var ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov32, ECX, operand1);
			context.AppendInstruction(X86.Mov32, EAX, operand2);
			context.AppendInstruction(X86.Mov32, EDX, operand3);
			context.AppendInstruction(X86.WrMSR, null, ECX, EAX, EDX);
		}

		#endregion Visitation Methods

		private void HandleShiftOperation32(Context context, BaseInstruction instruction)
		{
			if (context.Operand2.IsConstant)
				return;

			if (context.Operand2.Register == GeneralPurposeRegister.ECX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov32, ECX, operand2);
			context.AppendInstruction(instruction, result, operand1, ECX);
		}
	}
}
