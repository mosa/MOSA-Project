/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <simon_wollwage@yahoo.co.jp>
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Runtime.CompilerFramework.IR
{
	/// <summary>
	/// Performs IR constant folding of arithmetic instructions to optimize
	/// the code down to fewer calculations.
	/// </summary>
	public sealed class IRConstantFoldingStage : CodeTransformationStage, IR.IIRVisitor, IPipelineStage
    {
        #region IPipelineStage

        /// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
        string IPipelineStage.Name { get { return @"IR.ConstantFoldingStage"; } }

		#endregion

		#region IIRVisitor

        public void AddSInstruction(Context ctx)
        {
        }

	    public void AddUInstruction(Context context)
	    {
	    }

	    public void AddFInstruction(Context context)
	    {
	    }

	    public void DivFInstruction(Context context)
	    {
	    }

	    public void DivSInstruction(Context context)
	    {
	    }

		/// <summary>
		/// Folds logical ANDs with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void LogicalAndInstruction(Context ctx)
		{
			if (ctx.Operand2 is ConstantOperand && ctx.Operand3 is ConstantOperand) {
				int result = 0;
				switch (ctx.Result.Type.Type) {
					case Metadata.CilElementType.Char:
						goto case Metadata.CilElementType.U2;
					case Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand2 as ConstantOperand).Value) & ((byte)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand2 as ConstantOperand).Value) & ((ushort)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand2 as ConstantOperand).Value) & ((uint)(ctx.Operand3 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand2 as ConstantOperand).Value) & ((sbyte)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand2 as ConstantOperand).Value) & ((short)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand2 as ConstantOperand).Value) & ((int)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds logical ORs with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void LogicalOrInstruction(Context ctx)
		{
			if (ctx.Operand2 is ConstantOperand && ctx.Operand3 is ConstantOperand) {
				int result = 0;
				switch (ctx.Result.Type.Type) {
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand2 as ConstantOperand).Value) | ((byte)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand2 as ConstantOperand).Value) | ((ushort)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand2 as ConstantOperand).Value) | ((uint)(ctx.Operand3 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = (sbyte)(((uint)(sbyte)(ctx.Operand2 as ConstantOperand).Value) | ((uint)(sbyte)(ctx.Operand3 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand2 as ConstantOperand).Value) | ((short)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand2 as ConstantOperand).Value) | ((int)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds logical XORs with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void LogicalXorInstruction(Context ctx)
		{
			if (ctx.Operand2 is ConstantOperand && ctx.Operand3 is ConstantOperand) {
				int result = 0;
				switch (ctx.Result.Type.Type) {
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand2 as ConstantOperand).Value) ^ ((byte)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand2 as ConstantOperand).Value) ^ ((ushort)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand2 as ConstantOperand).Value) ^ ((uint)(ctx.Operand3 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand2 as ConstantOperand).Value) ^ ((sbyte)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand2 as ConstantOperand).Value) ^ ((short)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand2 as ConstantOperand).Value) ^ ((int)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, result));
			}
		}

		#endregion // IIRVisitor

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.AddressOfInstruction"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddressOfInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ArithmeticShiftRightInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ArithmeticShiftRightInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.BranchInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.BranchInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.CallInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.CallInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.EpilogueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.EpilogueInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.FloatingPointCompareInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointCompareInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.FloatingPointToIntegerConversionInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointToIntegerConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.IntegerCompareInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerCompareInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.IntegerToFloatingPointConversionInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerToFloatingPointConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.JmpInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.JmpInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LiteralInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LiteralInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LoadInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LoadInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.LogicalNotInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalNotInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.MoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MoveInstruction(Context context) { }

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
		/// Visitation function for <see cref="IR.IIRVisitor.PrologueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PrologueInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.PushInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PushInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ReturnInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ReturnInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ShiftLeftInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ShiftLeftInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ShiftRightInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ShiftRightInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.SignExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SignExtendedMoveInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.StoreInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.StoreInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.DivUInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void DivUInstruction(Context context)
		{
		}

	    public void MulSInstruction(Context context)
	    {
	    }

	    public void MulFInstruction(Context context)
	    {
	    }

	    public void MulUInstruction(Context context)
	    {
	    }

	    public void SubFInstruction(Context context)
	    {
	    }

	    public void SubSInstruction(Context context)
	    {
	    }

	    public void SubUInstruction(Context context)
	    {
	    }

	    public void RemFInstruction(Context context)
	    {
	    }

	    public void RemSInstruction(Context context)
	    {
	    }

		public void RemUInstruction(Context context)
		{
		}

	    public void SwitchInstruction(Context context)
	    {
	    }

	    public void BreakInstruction(Context context)
	    {
	    }

	    /// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.ZeroExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void ZeroExtendedMoveInstruction(Context context)
	    {
	    }

		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.NopInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void NopInstruction(Context context)
		{
		}

		#endregion // IIRVisitor - Unused

	}
}
