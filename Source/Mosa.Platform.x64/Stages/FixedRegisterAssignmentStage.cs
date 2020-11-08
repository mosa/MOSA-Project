// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;
using System.Diagnostics;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// Fixed Register Assignment Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.BaseTransformationStage" />
	public sealed class FixedRegisterAssignmentStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X64.Cdq32, Cdq32);
			AddVisitation(X64.Div32, Div32);
			AddVisitation(X64.IDiv32, IDiv32);
			AddVisitation(X64.IMul32, IMul32);
			AddVisitation(X64.In8, In8);
			AddVisitation(X64.In16, In16);
			AddVisitation(X64.In32, In32);
			AddVisitation(X64.MovStoreSeg32, MovStoreSeg32);
			AddVisitation(X64.Mul32, Mul32);
			AddVisitation(X64.Out8, Out);
			AddVisitation(X64.Out16, Out);
			AddVisitation(X64.Out32, Out);
			AddVisitation(X64.Rcr32, Rcr32);
			AddVisitation(X64.Sar32, Sar32);
			AddVisitation(X64.Shl32, Shl32);
			AddVisitation(X64.Shld32, Shld32);
			AddVisitation(X64.Shr32, Shr32);
			AddVisitation(X64.Shrd32, Shrd32);
			AddVisitation(X64.RdMSR, RdMSR);
			AddVisitation(X64.WrMSR, WrMSR);

			AddVisitation(X64.Cdq64, Cdq64);
			AddVisitation(X64.Div64, Div64);
			AddVisitation(X64.IDiv64, IDiv64);
			AddVisitation(X64.IMul64, IMul64);
			AddVisitation(X64.MovStoreSeg64, MovStoreSeg64);
			AddVisitation(X64.Mul64, Mul64);
			AddVisitation(X64.Rcr64, Rcr64);
			AddVisitation(X64.Sar64, Sar64);
			AddVisitation(X64.Shl64, Shl64);
			AddVisitation(X64.Shld64, Shld64);
			AddVisitation(X64.Shr64, Shr64);
			AddVisitation(X64.Shrd64, Shrd64);
		}

		#region Visitation Methods - 32bit

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

			context.SetInstruction(X64.Mov32, EAX, operand1);
			context.AppendInstruction2(X64.Cdq32, EDX, EAX, EAX);
			context.AppendInstruction(X64.Mov32, result, EDX);
			context.AppendInstruction(X64.Mov32, result2, EAX);
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

			context.SetInstruction(X64.Mov32, EDX, operand1);
			context.AppendInstruction(X64.Mov32, EAX, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X64.Div32, EDX, EAX, EDX, EAX, operand3);
			}
			else
			{
				var v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X64.Mov32, v3, operand3);
				context.AppendInstruction2(X64.Div32, EDX, EAX, EDX, EAX, v3);
			}

			context.AppendInstruction(X64.Mov32, result2, EAX);
			context.AppendInstruction(X64.Mov32, result, EDX);
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

			context.SetInstruction(X64.Mov32, EDX, operand1);
			context.AppendInstruction(X64.Mov32, EAX, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X64.IDiv32, EDX, EAX, EDX, EAX, operand3);
			}
			else
			{
				var v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X64.Mov32, v3, operand3);
				context.AppendInstruction2(X64.IDiv32, EDX, EAX, EDX, EAX, v3);
			}

			context.AppendInstruction(X64.Mov32, result2, EAX);
			context.AppendInstruction(X64.Mov32, result, EDX);
		}

		private void IMul32(Context context)
		{
			if (!context.Operand2.IsConstant)
				return;

			var v1 = AllocateVirtualRegister(context.Operand2);
			var operand2 = context.Operand2;

			context.Operand2 = v1;
			context.InsertBefore().SetInstruction(X64.Mov32, v1, operand2);
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

			context.SetInstruction(X64.Mov32, EDX, operand1);
			context.AppendInstruction(X64.In16, EAX, EDX);
			context.AppendInstruction(X64.Mov32, result, EAX);
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

			context.SetInstruction(X64.Mov32, EDX, operand1);
			context.AppendInstruction(X64.In32, EAX, EDX);
			context.AppendInstruction(X64.Mov32, result, EAX);
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

			context.SetInstruction(X64.Mov32, EDX, operand1);
			context.AppendInstruction(X64.In8, EAX, EDX);
			context.AppendInstruction(X64.Mov32, result, EAX);
		}

		private void MovStoreSeg32(Context context)
		{
			if (!context.Operand1.IsConstant)
				return;

			var result = context.Result;
			var operand1 = context.Operand1;

			var v1 = AllocateVirtualRegister(operand1);

			context.SetInstruction(X64.Mov32, v1, operand1);
			context.AppendInstruction(X64.MovStoreSeg32, result, v1);
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

			context.SetInstruction(X64.Mov32, EAX, operand1);

			if (operand2.IsConstant)
			{
				Operand v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X64.Mov32, v3, operand2);
				operand2 = v3;
			}

			Debug.Assert(operand2.IsCPURegister || operand2.IsVirtualRegister);

			context.AppendInstruction2(X64.Mul32, EDX, EAX, EAX, operand2);
			context.AppendInstruction(X64.Mov32, result, EDX);
			context.AppendInstruction(X64.Mov32, result2, EAX);
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

			context.SetInstruction(X64.Mov32, EDX, operand1);
			context.AppendInstruction(X64.Mov32, EAX, operand2);
			context.AppendInstruction(instruction, null, EDX, EAX);
		}

		private void Rcr32(Context context)
		{
			HandleShiftOperation32(context, X64.Rcr32);
		}

		private void Sar32(Context context)
		{
			HandleShiftOperation32(context, X64.Sar32);
		}

		private void Shl32(Context context)
		{
			HandleShiftOperation32(context, X64.Shl32);
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

			context.SetInstruction(X64.Mov32, ECX, operand3);
			context.AppendInstruction(X64.Shld32, result, operand1, operand2, ECX);
		}

		private void Shr32(Context context)
		{
			HandleShiftOperation32(context, X64.Shr32);
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

			context.SetInstruction(X64.Mov32, ECX, operand3);
			context.AppendInstruction(X64.Shrd32, result, operand1, operand2, ECX);
		}

		#endregion Visitation Methods - 32bit

		#region Visitation Methods - 64bit

		private void Cdq64(Context context)
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

			context.SetInstruction(X64.Mov64, EAX, operand1);
			context.AppendInstruction2(X64.Cdq64, EDX, EAX, EAX);
			context.AppendInstruction(X64.Mov64, result, EDX);
			context.AppendInstruction(X64.Mov64, result2, EAX);
		}

		private void Div64(Context context)
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

			context.SetInstruction(X64.Mov64, EDX, operand1);
			context.AppendInstruction(X64.Mov64, EAX, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X64.Div64, EDX, EAX, EDX, EAX, operand3);
			}
			else
			{
				var v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X64.Mov64, v3, operand3);
				context.AppendInstruction2(X64.Div64, EDX, EAX, EDX, EAX, v3);
			}

			context.AppendInstruction(X64.Mov64, result2, EAX);
			context.AppendInstruction(X64.Mov64, result, EDX);
		}

		private void IDiv64(Context context)
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

			context.SetInstruction(X64.Mov64, EDX, operand1);
			context.AppendInstruction(X64.Mov64, EAX, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X64.IDiv64, EDX, EAX, EDX, EAX, operand3);
			}
			else
			{
				var v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X64.Mov64, v3, operand3);
				context.AppendInstruction2(X64.IDiv64, EDX, EAX, EDX, EAX, v3);
			}

			context.AppendInstruction(X64.Mov64, result2, EAX);
			context.AppendInstruction(X64.Mov64, result, EDX);
		}

		private void IMul64(Context context)
		{
			if (!context.Operand2.IsConstant)
				return;

			var v1 = AllocateVirtualRegister(context.Operand2);
			var operand2 = context.Operand2;

			context.Operand2 = v1;
			context.InsertBefore().SetInstruction(X64.Mov64, v1, operand2);
		}

		private void MovStoreSeg64(Context context)
		{
			if (!context.Operand1.IsConstant)
				return;

			var result = context.Result;
			var operand1 = context.Operand1;

			var v1 = AllocateVirtualRegister(operand1);

			context.SetInstruction(X64.Mov64, v1, operand1);
			context.AppendInstruction(X64.MovStoreSeg64, result, v1);
		}

		private void Mul64(Context context)
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

			context.SetInstruction(X64.Mov64, EAX, operand1);

			if (operand2.IsConstant)
			{
				Operand v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X64.Mov64, v3, operand2);
				operand2 = v3;
			}

			Debug.Assert(operand2.IsCPURegister || operand2.IsVirtualRegister);

			context.AppendInstruction2(X64.Mul64, EDX, EAX, EAX, operand2);
			context.AppendInstruction(X64.Mov64, result, EDX);
			context.AppendInstruction(X64.Mov64, result2, EAX);
		}

		private void Rcr64(Context context)
		{
			HandleShiftOperation64(context, X64.Rcr64);
		}

		private void Sar64(Context context)
		{
			HandleShiftOperation64(context, X64.Sar64);
		}

		private void Shl64(Context context)
		{
			HandleShiftOperation64(context, X64.Shl64);
		}

		private void Shld64(Context context)
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

			context.SetInstruction(X64.Mov64, ECX, operand3);
			context.AppendInstruction(X64.Shld64, result, operand1, operand2, ECX);
		}

		private void Shr64(Context context)
		{
			HandleShiftOperation64(context, X64.Shr64);
		}

		private void Shrd64(Context context)
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

			context.SetInstruction(X64.Mov64, ECX, operand3);
			context.AppendInstruction(X64.Shrd64, result, operand1, operand2, ECX);
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

			context.SetInstruction(X64.Mov32, ECX, operand1);
			context.AppendInstruction2(X64.RdMSR, EAX, EDX, ECX);
			context.AppendInstruction(X64.Mov32, result, EAX);
			context.AppendInstruction(X64.Mov32, result2, EDX);
		}

		private void WrMSR(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.ECX
				&& context.Operand1.Register == GeneralPurposeRegister.EAX
				&& context.Operand2.Register == GeneralPurposeRegister.EDX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			var EDX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);
			var ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			context.SetInstruction(X64.Mov32, EAX, operand1);
			context.AppendInstruction(X64.Mov32, EDX, operand2);
			context.AppendInstruction(X64.WrMSR, ECX, EAX, EDX);
			context.AppendInstruction(X64.Mov32, result, ECX);
		}

		#endregion Visitation Methods - 64bit

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

			context.SetInstruction(X64.Mov32, ECX, operand2);
			context.AppendInstruction(instruction, result, operand1, ECX);
		}

		private void HandleShiftOperation64(Context context, BaseInstruction instruction)
		{
			if (context.Operand2.IsConstant)
				return;

			if (context.Operand2.Register == GeneralPurposeRegister.ECX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			context.SetInstruction(X64.Mov64, ECX, operand2);
			context.AppendInstruction(instruction, result, operand1, ECX);
		}
	}
}
