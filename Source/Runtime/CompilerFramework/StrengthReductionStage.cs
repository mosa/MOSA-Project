/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <simon_wollwage@yahoo.co.jp>
 */

using System;
using Mosa.Runtime.CompilerFramework.Operands;
using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Performs IR constant folding of arithmetic instructions to optimize
	/// the code down to fewer calculations.
	/// </summary>
	public sealed class StrengthReductionStage : CodeTransformationStage, CIL.ICILVisitor, IPipelineStage
	{

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"Strength Reduction"; } }

		#endregion

		#region ICILVisitor

		/// <summary>
		/// Folds multiplication when one of the constants is zero
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Mul(Context ctx)
		{
			bool multiplyByZero = false;

			if (ctx.Operand1 is ConstantOperand)
				if (IsValueZero(ctx.Result.Type.Type, ctx.Operand1 as ConstantOperand))
					multiplyByZero = true;

			if (ctx.Operand2 is ConstantOperand)
				if (IsValueZero(ctx.Result.Type.Type, ctx.Operand2 as ConstantOperand))
					multiplyByZero = true;

			if (multiplyByZero) {
				ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, 0));
				return;
			}

			if (ctx.Operand1 is ConstantOperand)
				if (IsValueOne(ctx.Result.Type.Type, ctx.Operand1 as ConstantOperand)) {
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, ctx.Operand2);
					return;
				}

			if (ctx.Operand2 is ConstantOperand)
				if (IsValueOne(ctx.Result.Type.Type, ctx.Operand2 as ConstantOperand)) {
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, ctx.Operand1);
					return;
				}
		}

		/// <summary>
		/// Determines whether the value is zero.
		/// </summary>
		/// <param name="cilElementType">Type of the cil element.</param>
		/// <param name="constantOperand">The constant operand.</param>
		/// <returns>
		/// 	<c>true</c> if the value is zero; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsValueZero(Metadata.CilElementType cilElementType, ConstantOperand constantOperand)
		{
			switch (cilElementType) {
				case Metadata.CilElementType.Char: goto case Metadata.CilElementType.U2;
				case Metadata.CilElementType.U1: return (byte)(constantOperand.Value) == 0;
				case Metadata.CilElementType.U2: return (ushort)(constantOperand.Value) == 0;
				case Metadata.CilElementType.U4: return (int)(constantOperand.Value) == 0;
				case Metadata.CilElementType.I1: return (sbyte)(constantOperand.Value) == 0;
				case Metadata.CilElementType.I2: return (short)(constantOperand.Value) == 0;
				case Metadata.CilElementType.I4: return (int)(constantOperand.Value) == 0;
				case Metadata.CilElementType.R4: return (float)(constantOperand.Value) == 0;
				case Metadata.CilElementType.R8: return (double)(constantOperand.Value) == 0;
				case Metadata.CilElementType.I: goto case Metadata.CilElementType.I4;
				case Metadata.CilElementType.U: goto case Metadata.CilElementType.U4;
				default: goto case Metadata.CilElementType.I4;
			}
		}

		/// <summary>
		/// Determines whether the value is one.
		/// </summary>
		/// <param name="cilElementType">Type of the cil element.</param>
		/// <param name="constantOperand">The constant operand.</param>
		/// <returns>
		/// 	<c>true</c> if the value is one; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsValueOne(Metadata.CilElementType cilElementType, ConstantOperand constantOperand)
		{
			switch (cilElementType) {
				case Metadata.CilElementType.Char: goto case Metadata.CilElementType.U2;
				case Metadata.CilElementType.U1: return (byte)(constantOperand.Value) == 1;
				case Metadata.CilElementType.U2: return (ushort)(constantOperand.Value) == 1;
				case Metadata.CilElementType.U4: return (int)(constantOperand.Value) == 1;
				case Metadata.CilElementType.I1: return (sbyte)(constantOperand.Value) == 1;
				case Metadata.CilElementType.I2: return (short)(constantOperand.Value) == 1;
				case Metadata.CilElementType.I4: return (int)(constantOperand.Value) == 1;
				case Metadata.CilElementType.R4: return (float)(constantOperand.Value) == 1;
				case Metadata.CilElementType.R8: return (double)(constantOperand.Value) == 1;
				case Metadata.CilElementType.I: goto case Metadata.CilElementType.I4;
				case Metadata.CilElementType.U: goto case Metadata.CilElementType.U4;
				default: goto case Metadata.CilElementType.I4;
			}
		}

		#endregion // Methods

		#region ICILVisitor - Unused

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Nop"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Nop(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Break"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Break(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldarg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldarg(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldarga"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldarga(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldloca"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldloca(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldstr"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldstr(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldflda(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldsfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldsflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldsflda(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldftn"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldftn(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldvirtftn"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldvirtftn(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldtoken"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldtoken(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Starg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Starg(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stsfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Dup"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Dup(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Pop"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Pop(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Jmp"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Jmp(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Call"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Call(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Calli"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Calli(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ret"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ret(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Branch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Branch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.UnaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.UnaryBranch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.BinaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.BinaryBranch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Switch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Switch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.BinaryLogic"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.BinaryLogic(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Shift"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Shift(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Neg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Neg(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Not"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Not(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Conversion"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Conversion(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Callvirt"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Callvirt(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Cpobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Cpobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Newobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Newobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Castclass"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Castclass(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Isinst"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Isinst(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Unbox"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Unbox(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Throw"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Throw(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Box"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Box(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Newarr"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Newarr(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldlen"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldlen(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldelema"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldelema(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldelem(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stelem(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.UnboxAny"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.UnboxAny(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Refanyval"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Refanyval(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.UnaryArithmetic"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.UnaryArithmetic(Context ctx) { }

		/// <summary>
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Mkrefany(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.ArithmeticOverflow"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.ArithmeticOverflow(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Endfinally"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Endfinally(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Leave"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Leave(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Arglist"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Arglist(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.BinaryComparison"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.BinaryComparison(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Localalloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Localalloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Endfilter"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Endfilter(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.InitObj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.InitObj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Cpblk"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Cpblk(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Initblk"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Initblk(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Prefix"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Prefix(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Rethrow"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Rethrow(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Sizeof"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Sizeof(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Refanytype"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Refanytype(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Add"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Add(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Sub"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Sub(Context ctx) { }


		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Div"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Div(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Rem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Rem(Context ctx) { }

		#endregion // ICILVisitor - Unused
	}
}
