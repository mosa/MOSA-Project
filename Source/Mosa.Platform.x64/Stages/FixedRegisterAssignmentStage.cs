// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

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

			AddVisitation(X64.CpuId, CpuId);
		}

		#region Visitation Methods - 32bit

		private void Cdq32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == CPURegister.RDX
				&& context.Result2.Register == CPURegister.RAX
				&& context.Operand1.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var result = context.Result;
			var result2 = context.Result2;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rax, operand1);
			context.AppendInstruction2(X64.Cdq32, rdx, rax, rax);
			context.AppendInstruction(X64.Mov64, result, rdx);
			context.AppendInstruction(X64.Mov64, result2, rax);
		}

		private void Div32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == CPURegister.RDX
				&& context.Result2.Register == CPURegister.RAX
				&& context.Operand1.Register == CPURegister.RDX
				&& context.Operand2.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;
			var result2 = context.Result2;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rdx, operand1);
			context.AppendInstruction(X64.Mov64, rax, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X64.Div32, rdx, rax, rdx, rax, operand3);
			}
			else
			{
				var v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X64.Mov64, v3, operand3);
				context.AppendInstruction2(X64.Div32, rdx, rax, rdx, rax, v3);
			}

			context.AppendInstruction(X64.Mov64, result2, rax);
			context.AppendInstruction(X64.Mov64, result, rdx);
		}

		private void IDiv32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Operand2.IsCPURegister
				&& context.Result.Register == CPURegister.RDX
				&& context.Result2.Register == CPURegister.RAX
				&& context.Operand1.Register == CPURegister.RDX
				&& context.Operand2.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;
			var result2 = context.Result2;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rdx, operand1);
			context.AppendInstruction(X64.Mov64, rax, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X64.IDiv32, rdx, rax, rdx, rax, operand3);
			}
			else
			{
				var v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X64.Mov64, v3, operand3);
				context.AppendInstruction2(X64.IDiv32, rdx, rax, rdx, rax, v3);
			}

			context.AppendInstruction(X64.Mov64, result2, rax);
			context.AppendInstruction(X64.Mov64, result, rdx);
		}

		private void IMul32(Context context)
		{
			if (!context.Operand2.IsConstant)
				return;

			var v1 = AllocateVirtualRegister(context.Operand2);
			var operand2 = context.Operand2;

			context.Operand2 = v1;
			context.InsertBefore().SetInstruction(X64.Mov64, v1, operand2);
		}

		private void In16(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == CPURegister.RAX
				&& (context.Operand1.Register == CPURegister.RDX || context.Operand1.IsConstant))
				return;

			var result = context.Result;
			var operand1 = context.Operand1;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(operand1.Type, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rdx, operand1);
			context.AppendInstruction(X64.In16, rax, rdx);
			context.AppendInstruction(X64.Mov64, result, rax);
		}

		private void In32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == CPURegister.RAX
				&& (context.Operand1.Register == CPURegister.RDX || context.Operand1.IsConstant))
				return;

			var result = context.Result;
			var operand1 = context.Operand1;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(operand1.Type, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rdx, operand1);
			context.AppendInstruction(X64.In32, rax, rdx);
			context.AppendInstruction(X64.Mov64, result, rax);
		}

		private void In8(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == CPURegister.RAX
				&& (context.Operand1.Register == CPURegister.RDX || context.Operand1.IsConstant))
				return;

			var result = context.Result;
			var operand1 = context.Operand1;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(operand1.Type, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rdx, operand1);
			context.AppendInstruction(X64.In8, rax, rdx);
			context.AppendInstruction(X64.Mov64, result, rax);
		}

		private void MovStoreSeg32(Context context)
		{
			if (!context.Operand1.IsConstant)
				return;

			var result = context.Result;
			var operand1 = context.Operand1;

			var v1 = AllocateVirtualRegister(operand1);

			context.SetInstruction(X64.Mov64, v1, operand1);
			context.AppendInstruction(X64.MovStoreSeg32, result, v1);
		}

		private void Mul32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& !context.Operand2.IsConstant
				&& context.Result.Register == CPURegister.RDX
				&& context.Result2.Register == CPURegister.RAX
				&& context.Operand1.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;
			var result2 = context.Result2;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rax, operand1);

			if (operand2.IsConstant)
			{
				Operand v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X64.Mov64, v3, operand2);
				operand2 = v3;
			}

			Debug.Assert(operand2.IsCPURegister || operand2.IsVirtualRegister);

			context.AppendInstruction2(X64.Mul32, rdx, rax, rax, operand2);
			context.AppendInstruction(X64.Mov64, result, rdx);
			context.AppendInstruction(X64.Mov64, result2, rax);
		}

		private void Out(Context context)
		{
			// TRANSFORM: OUT <= rdx, rax && OUT <= imm8, rax
			if (context.Operand1.IsCPURegister
				&& context.Operand2.IsCPURegister
				&& (context.Operand1.Register == CPURegister.RDX || context.Operand1.IsConstant)
				&& context.Operand2.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var instruction = context.Instruction;

			var rax = Operand.CreateCPURegister(operand2.Type, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(operand1.Type, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rdx, operand1);
			context.AppendInstruction(X64.Mov64, rax, operand2);
			context.AppendInstruction(instruction, null, rdx, rax);
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

			if (context.Operand3.Register == CPURegister.RCX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;

			var rcx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RCX);

			context.SetInstruction(X64.Mov64, rcx, operand3);
			context.AppendInstruction(X64.Shld32, result, operand1, operand2, rcx);
		}

		private void Shr32(Context context)
		{
			HandleShiftOperation32(context, X64.Shr32);
		}

		private void Shrd32(Context context)
		{
			if (context.Operand3.IsConstant)
				return;

			if (context.Operand3.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;

			var rcx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RCX);

			context.SetInstruction(X64.Mov64, rcx, operand3);
			context.AppendInstruction(X64.Shrd32, result, operand1, operand2, rcx);
		}

		#endregion Visitation Methods - 32bit

		#region Visitation Methods - 64bit

		private void Cdq64(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == CPURegister.RDX
				&& context.Result2.Register == CPURegister.RAX
				&& context.Operand1.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var result = context.Result;
			var result2 = context.Result2;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rax, operand1);
			context.AppendInstruction2(X64.Cdq64, rdx, rax, rax);
			context.AppendInstruction(X64.Mov64, result, rdx);
			context.AppendInstruction(X64.Mov64, result2, rax);
		}

		private void Div64(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == CPURegister.RDX
				&& context.Result2.Register == CPURegister.RAX
				&& context.Operand1.Register == CPURegister.RDX
				&& context.Operand2.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;
			var result2 = context.Result2;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rdx, operand1);
			context.AppendInstruction(X64.Mov64, rax, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X64.Div64, rdx, rax, rdx, rax, operand3);
			}
			else
			{
				var v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X64.Mov64, v3, operand3);
				context.AppendInstruction2(X64.Div64, rdx, rax, rdx, rax, v3);
			}

			context.AppendInstruction(X64.Mov64, result2, rax);
			context.AppendInstruction(X64.Mov64, result, rdx);
		}

		private void IDiv64(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Operand2.IsCPURegister
				&& context.Result.Register == CPURegister.RDX
				&& context.Result2.Register == CPURegister.RAX
				&& context.Operand1.Register == CPURegister.RDX
				&& context.Operand2.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;
			var result2 = context.Result2;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rdx, operand1);
			context.AppendInstruction(X64.Mov64, rax, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X64.IDiv64, rdx, rax, rdx, rax, operand3);
			}
			else
			{
				var v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X64.Mov64, v3, operand3);
				context.AppendInstruction2(X64.IDiv64, rdx, rax, rdx, rax, v3);
			}

			context.AppendInstruction(X64.Mov64, result2, rax);
			context.AppendInstruction(X64.Mov64, result, rdx);
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
				&& context.Result.Register == CPURegister.RDX
				&& context.Result2.Register == CPURegister.RAX
				&& context.Operand1.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;
			var result2 = context.Result2;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rax, operand1);

			if (operand2.IsConstant)
			{
				Operand v3 = AllocateVirtualRegisterI32();
				context.AppendInstruction(X64.Mov64, v3, operand2);
				operand2 = v3;
			}

			Debug.Assert(operand2.IsCPURegister || operand2.IsVirtualRegister);

			context.AppendInstruction2(X64.Mul64, rdx, rax, rax, operand2);
			context.AppendInstruction(X64.Mov64, result, rdx);
			context.AppendInstruction(X64.Mov64, result2, rax);
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

			if (context.Operand3.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;

			var rcx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RCX);

			context.SetInstruction(X64.Mov64, rcx, operand3);
			context.AppendInstruction(X64.Shld64, result, operand1, operand2, rcx);
		}

		private void Shr64(Context context)
		{
			HandleShiftOperation64(context, X64.Shr64);
		}

		private void Shrd64(Context context)
		{
			if (context.Operand3.IsConstant)
				return;

			if (context.Operand3.Register == CPURegister.RCX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;

			var rcx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RCX);

			context.SetInstruction(X64.Mov64, rcx, operand3);
			context.AppendInstruction(X64.Shrd64, result, operand1, operand2, rcx);
		}

		private void RdMSR(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == CPURegister.RAX
				&& context.Result2.Register == CPURegister.RDX
				&& context.Operand1.Register == CPURegister.RCX)
				return;

			var operand1 = context.Operand1;
			var result = context.Result;
			var result2 = context.Result2;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, CPURegister.RDX);
			var rcx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, CPURegister.RCX);

			context.SetInstruction(X64.Mov64, rcx, operand1);
			context.AppendInstruction2(X64.RdMSR, rax, rdx, rcx);
			context.AppendInstruction(X64.Mov64, result, rax);
			context.AppendInstruction(X64.Mov64, result2, rdx);
		}

		private void WrMSR(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == CPURegister.RAX
				&& context.Operand1.Register == CPURegister.RAX
				&& context.Operand2.Register == CPURegister.RDX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, CPURegister.RDX);
			var rcx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I8, CPURegister.RCX);

			context.SetInstruction(X64.Mov64, rax, operand1);
			context.AppendInstruction(X64.Mov64, rdx, operand2);
			context.AppendInstruction(X64.WrMSR, rcx, rax, rdx);
			context.AppendInstruction(X64.Mov64, result, rcx);
		}

		private void CpuId(Context context)
		{
			if (context.Operand1.IsCPURegister
				&& context.Operand2.IsCPURegister
				&& context.Operand1.Register == CPURegister.RAX
				&& context.Operand2.Register == CPURegister.RCX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var rax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RAX);
			var rcx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RCX);

			context.SetInstruction(X64.Mov64, rax, operand1);
			context.AppendInstruction(X64.Mov64, rcx, operand2);
			context.AppendInstruction(X64.CpuId, rax, rax, rcx);
			context.AppendInstruction(X64.Mov64, result, rax);
		}

		#endregion Visitation Methods - 64bit

		private void HandleShiftOperation32(Context context, BaseInstruction instruction)
		{
			if (context.Operand2.IsConstant)
				return;

			if (context.Operand2.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RAX);

			context.SetInstruction(X64.Mov64, ECX, operand2);
			context.AppendInstruction(instruction, result, operand1, ECX);
		}

		private void HandleShiftOperation64(Context context, BaseInstruction instruction)
		{
			if (context.Operand2.IsConstant)
				return;

			if (context.Operand2.Register == CPURegister.RAX)
				return;

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, CPURegister.RAX);

			context.SetInstruction(X64.Mov64, ECX, operand2);
			context.AppendInstruction(instruction, result, operand1, ECX);
		}
	}
}
