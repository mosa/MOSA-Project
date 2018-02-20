// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
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
			AddVisitation(X86.Cdq, Cdq);
			AddVisitation(X86.Div32, Div32);
			AddVisitation(X86.IDiv32, IDiv32);
			AddVisitation(X86.IMul, IMul);
			AddVisitation(X86.In8, In8);
			AddVisitation(X86.In16, In16);
			AddVisitation(X86.In32, In32);
			AddVisitation(X86.Mul32, Mul32);
			AddVisitation(X86.Out8, Out);
			AddVisitation(X86.Out16, Out);
			AddVisitation(X86.Out32, Out);
			AddVisitation(X86.Rcr32, Rcr32);
			AddVisitation(X86.Sar32, Sar32);
			AddVisitation(X86.Shl32, Shl32);
			AddVisitation(X86.Shld32, Shld32);
			AddVisitation(X86.Shr32, Shr32);
			AddVisitation(X86.ShrConst32, Shr32);
			AddVisitation(X86.Shrd32, Shrd32);
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cdq"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Cdq(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.EDX
				&& context.Result2.Register == GeneralPurposeRegister.EAX
				&& context.Operand1.Register == GeneralPurposeRegister.EAX)
				return;

			Operand operand1 = context.Operand1;
			Operand result = context.Result;
			Operand result2 = context.Result2;

			Operand EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			Operand EDX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EAX, operand1);
			context.AppendInstruction2(X86.Cdq, EDX, EAX, EAX);
			context.AppendInstruction(X86.Mov32, result, EDX);
			context.AppendInstruction(X86.Mov32, result2, EAX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Div"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Div32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.EDX
				&& context.Result2.Register == GeneralPurposeRegister.EAX
				&& context.Operand1.Register == GeneralPurposeRegister.EDX
				&& context.Operand2.Register == GeneralPurposeRegister.EAX)
				return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand operand3 = context.Operand3;
			Operand result = context.Result;
			Operand result2 = context.Result2;

			Operand EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			Operand EDX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EDX, operand1);
			context.AppendInstruction(X86.Mov32, EAX, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X86.Div32, EDX, EAX, EDX, EAX, operand3);
			}
			else
			{
				Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(X86.Mov32, v3, operand3);
				context.AppendInstruction2(X86.Div32, EDX, EAX, EDX, EAX, v3);
			}

			context.AppendInstruction(X86.Mov32, result2, EAX);
			context.AppendInstruction(X86.Mov32, result, EDX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.IDiv"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void IDiv32(Context context)
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

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand operand3 = context.Operand3;
			Operand result = context.Result;
			Operand result2 = context.Result2;

			Operand EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			Operand EDX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EDX, operand1);
			context.AppendInstruction(X86.Mov32, EAX, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X86.IDiv32, EDX, EAX, EDX, EAX, operand3);
			}
			else
			{
				Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(X86.Mov32, v3, operand3);
				context.AppendInstruction2(X86.IDiv32, EDX, EAX, EDX, EAX, v3);
			}

			context.AppendInstruction(X86.Mov32, result2, EAX);
			context.AppendInstruction(X86.Mov32, result, EDX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.IMul" /> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void IMul(Context context)
		{
			if (!context.Operand2.IsConstant)
				return;

			Operand v1 = AllocateVirtualRegister(context.Operand2.Type);
			Operand operand2 = context.Operand2;

			context.Operand2 = v1;
			context.InsertBefore().SetInstruction(X86.Mov32, v1, operand2);
		}

		/// <summary>
		/// In8s the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public void In8(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.EAX
				&& (context.Operand1.Register == GeneralPurposeRegister.EDX || context.Operand1.IsConstant))
				return;

			Operand result = context.Result;
			Operand operand1 = context.Operand1;

			Operand EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			Operand EDX = Operand.CreateCPURegister(operand1.Type, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EDX, operand1);
			context.AppendInstruction(X86.In8, EAX, EDX);
			context.AppendInstruction(X86.Mov32, result, EAX);
		}

		/// <summary>
		/// In16s the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		public void In16(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.EAX
				&& (context.Operand1.Register == GeneralPurposeRegister.EDX || context.Operand1.IsConstant))
				return;

			Operand result = context.Result;
			Operand operand1 = context.Operand1;

			Operand EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			Operand EDX = Operand.CreateCPURegister(operand1.Type, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EDX, operand1);
			context.AppendInstruction(X86.In16, EAX, EDX);
			context.AppendInstruction(X86.Mov32, result, EAX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.In"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void In32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == GeneralPurposeRegister.EAX
				&& (context.Operand1.Register == GeneralPurposeRegister.EDX || context.Operand1.IsConstant))
				return;

			Operand result = context.Result;
			Operand operand1 = context.Operand1;

			Operand EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, GeneralPurposeRegister.EAX);
			Operand EDX = Operand.CreateCPURegister(operand1.Type, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EDX, operand1);
			context.AppendInstruction(X86.In32, EAX, EDX);
			context.AppendInstruction(X86.Mov32, result, EAX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Mul"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Mul32(Context context)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& !context.Operand2.IsConstant
				&& context.Result.Register == GeneralPurposeRegister.EDX
				&& context.Result2.Register == GeneralPurposeRegister.EAX
				&& context.Operand1.Register == GeneralPurposeRegister.EAX)
				return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand result = context.Result;
			Operand result2 = context.Result2;

			Operand EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			Operand EDX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EAX, operand1);

			if (operand2.IsConstant)
			{
				Operand v3 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.AppendInstruction(X86.Mov32, v3, operand2);
				operand2 = v3;
			}

			Debug.Assert(operand2.IsCPURegister || operand2.IsVirtualRegister);

			context.AppendInstruction2(X86.Mul32, EDX, EAX, EAX, operand2);
			context.AppendInstruction(X86.Mov32, result, EDX);
			context.AppendInstruction(X86.Mov32, result2, EAX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Out"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Out(Context context)
		{
			// TRANSFORM: OUT <= EDX, EAX && OUT <= imm8, EAX
			if (context.Operand1.IsCPURegister
				&& context.Operand2.IsCPURegister
				&& (context.Operand1.Register == GeneralPurposeRegister.EDX || context.Operand1.IsConstant)
				&& context.Operand2.Register == GeneralPurposeRegister.EAX)
				return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			BaseInstruction instruction = context.Instruction;

			Operand EAX = Operand.CreateCPURegister(operand2.Type, GeneralPurposeRegister.EAX);
			Operand EDX = Operand.CreateCPURegister(operand1.Type, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov32, EDX, operand1);
			context.AppendInstruction(X86.Mov32, EAX, operand2);
			context.AppendInstruction(instruction, null, EDX, EAX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Rcr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Rcr32(Context context)
		{
			HandleShiftOperation(context, X86.Rcr32);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Sar"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Sar32(Context context)
		{
			HandleShiftOperation(context, X86.Sar32);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shl"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Shl32(Context context)
		{
			HandleShiftOperation(context, X86.Shl32);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shld"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Shld32(Context context)
		{
			if (context.Operand3.IsConstant)
				return;

			if (context.Operand3.Register == GeneralPurposeRegister.ECX)
				return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand operand3 = context.Operand3;
			Operand result = context.Result;

			Debug.Assert(result == operand1);

			Operand ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov32, ECX, operand3);
			context.AppendInstruction(X86.Shld32, result, operand1, operand2, ECX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Shr32(Context context)
		{
			HandleShiftOperation(context, X86.Shr32);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shrd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Shrd32(Context context)
		{
			if (context.Operand3.IsConstant)
				return;

			if (context.Operand3.Register == GeneralPurposeRegister.ECX)
				return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand operand3 = context.Operand3;
			Operand result = context.Result;

			Debug.Assert(result == operand1);

			Operand ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov32, ECX, operand3);
			context.AppendInstruction(X86.Shrd32, result, operand1, operand2, ECX);
		}

		#endregion Visitation Methods

		private void HandleShiftOperation(Context context, BaseInstruction instruction)
		{
			if (context.Operand2.IsConstant || context.Operand2.IsCPURegister)
				return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand result = context.Result;

			Operand ECX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov32, ECX, operand2);
			context.AppendInstruction(X86.Mov32, result, operand1);
			context.AppendInstruction(instruction, result, result, ECX);
		}
	}
}
