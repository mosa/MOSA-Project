/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class AddressModeConversionStage :
		BaseTransformationStage,
		IR.IIRVisitor,
		IMethodCompilerStage,
		IPlatformTransformationStage
	{

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name
		{
			get { return @"X86.AddressModeConversionStage"; }
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertAfter<TweakTransformationStage>(this);
		}

		#endregion // IMethodCompilerStage Members

		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="ctx">The conversion context.</param>
		private static void ThreeTwoAddressConversion(Context ctx)
		{
			Operand opRes = ctx.Result;
			Operand op1 = ctx.Operand1;
			Operand op2 = ctx.Operand2;

			// Create registers for different data types
			RegisterOperand eax = new RegisterOperand(ctx.Result.Type, ctx.Result.StackType == StackTypeCode.F ? (Register)SSE2Register.XMM0 : GeneralPurposeRegister.EAX);
			
			ctx.Result = eax;
			ctx.Operand1 = op2;

			ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovInstruction, eax, ctx.Operand1);
			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, opRes, eax);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalAndInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalAndInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalOrInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalOrInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalXorInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalXorInstruction(Context ctx)
		{
			ThreeTwoAddressConversion(ctx);
		}



		#region IIRVisitor

		#endregion //  IIRVisitor

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.AddressOfInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.AddressOfInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ArithmeticShiftRightInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ArithmeticShiftRightInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.EpilogueInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.EpilogueInstruction(Context ctx) { }
		/// <summary>
		/// Floatings the point compare instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.FloatingPointCompareInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.IntegerCompareInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.IntegerCompareInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.JmpInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.JmpInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LoadInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LoadInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalNotInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.LogicalNotInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.MoveInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.MoveInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.PrologueInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.PrologueInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ReturnInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ReturnInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ShiftLeftInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ShiftLeftInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ShiftRightInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ShiftRightInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.StoreInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.StoreInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.UDivInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.UDivInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.URemInstruction"/> instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.URemInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.NopInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.NopInstruction(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.SignExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.SignExtendedMoveInstruction(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.CallInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.CallInstruction(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ZeroExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.ZeroExtendedMoveInstruction(Context ctx) { }

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.BranchInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.BranchInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.FloatingPointToIntegerConversionInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointToIntegerConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.IntegerToFloatingPointConversionInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerToFloatingPointConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LiteralInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LiteralInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.PhiInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PhiInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.PopInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PopInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.PushInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PushInstruction(Context context) { }

		#endregion // IIRVisitor

	}
}
