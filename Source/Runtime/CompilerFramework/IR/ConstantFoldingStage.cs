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
	public sealed class IRConstantFoldingStage : BaseCodeTransformationStage, IR.IIRVisitor, IPipelineStage
	{
		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"IR.ConstantFoldingStage"; } }

		#endregion

		#region IIRVisitor

		/// <summary>
		/// Folds logical ANDs with 2 constants
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalAndInstruction(Context context)
		{
			if (context.Operand2 is ConstantOperand && context.Operand3 is ConstantOperand)
			{
				int result = 0;
				switch (context.Result.Type.Type)
				{
					case Metadata.CilElementType.Char:
						goto case Metadata.CilElementType.U2;
					case Metadata.CilElementType.U1:
						result = ((byte)(context.Operand2 as ConstantOperand).Value) & ((byte)(context.Operand3 as ConstantOperand).Value);
						break;
					case Metadata.CilElementType.U2:
						result = ((ushort)(context.Operand2 as ConstantOperand).Value) & ((ushort)(context.Operand3 as ConstantOperand).Value);
						break;
					case Metadata.CilElementType.U4:
						result = (int)(((uint)(context.Operand2 as ConstantOperand).Value) & ((uint)(context.Operand3 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(context.Operand2 as ConstantOperand).Value) & ((sbyte)(context.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(context.Operand2 as ConstantOperand).Value) & ((short)(context.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(context.Operand2 as ConstantOperand).Value) & ((int)(context.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds logical ORs with 2 constants
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalOrInstruction(Context context)
		{
			if (context.Operand2 is ConstantOperand && context.Operand3 is ConstantOperand)
			{
				int result = 0;
				switch (context.Result.Type.Type)
				{
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(context.Operand2 as ConstantOperand).Value) | ((byte)(context.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(context.Operand2 as ConstantOperand).Value) | ((ushort)(context.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(context.Operand2 as ConstantOperand).Value) | ((uint)(context.Operand3 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = (sbyte)(((uint)(sbyte)(context.Operand2 as ConstantOperand).Value) | ((uint)(sbyte)(context.Operand3 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(context.Operand2 as ConstantOperand).Value) | ((short)(context.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(context.Operand2 as ConstantOperand).Value) | ((int)(context.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds logical XORs with 2 constants
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalXorInstruction(Context context)
		{
			if (context.Operand2 is ConstantOperand && context.Operand3 is ConstantOperand)
			{
				int result = 0;
				switch (context.Result.Type.Type)
				{
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(context.Operand2 as ConstantOperand).Value) ^ ((byte)(context.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(context.Operand2 as ConstantOperand).Value) ^ ((ushort)(context.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(context.Operand2 as ConstantOperand).Value) ^ ((uint)(context.Operand3 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(context.Operand2 as ConstantOperand).Value) ^ ((sbyte)(context.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(context.Operand2 as ConstantOperand).Value) ^ ((short)(context.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(context.Operand2 as ConstantOperand).Value) ^ ((int)(context.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, result));
			}
		}

		#endregion // IIRVisitor

		#region IIRVisitor - Unused

		/// <summary>
		/// Visitation function for <see cref="AddressOfInstruction"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddressOfInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="ArithmeticShiftRightInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ArithmeticShiftRightInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="BranchInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.BranchInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CallInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.CallInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="EpilogueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.EpilogueInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="FloatingPointCompareInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointCompareInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="FloatingPointToIntegerConversionInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.FloatingPointToIntegerConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IntegerCompareInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerCompareInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="IntegerToFloatingPointConversionInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.IntegerToFloatingPointConversionInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="JmpInstruction"/> instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.JmpInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="LiteralInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LiteralInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="LoadInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LoadInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="LogicalNotInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.LogicalNotInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="MoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MoveInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="PhiInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PhiInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="PopInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PopInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="PrologueInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PrologueInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="PushInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.PushInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="ReturnInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ReturnInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="ShiftLeftInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ShiftLeftInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="ShiftRightInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ShiftRightInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="SignExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SignExtendedMoveInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="StoreInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.StoreInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="DivUInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivUInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="MulSInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulSInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="MulFInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulFInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="MulUInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.MulUInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="SubFInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubFInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="SubSInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubSInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="SubUInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SubUInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="RemFInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemFInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="RemSInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemSInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="RemUInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.RemUInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="SwitchInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.SwitchInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="BreakInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.BreakInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="ZeroExtendedMoveInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ZeroExtendedMoveInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="NopInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.NopInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="ThrowInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.ThrowInstruction(Context context) { }

		/// <summary>
		/// Adds the S instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddSInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="AddUInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddUInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="AddFInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.AddFInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="DivFInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivFInstruction(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="DivSInstruction"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IR.IIRVisitor.DivSInstruction(Context context) { }

		#endregion // IIRVisitor - Unused

	}
}
