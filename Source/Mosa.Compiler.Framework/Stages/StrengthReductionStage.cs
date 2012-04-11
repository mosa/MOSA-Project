/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <simon_wollwage@yahoo.co.jp>
 */

using Mosa.Compiler.Framework.Operands;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Performs IR constant folding of arithmetic instructions to optimize
	/// the code down to fewer calculations.
	/// </summary>
	public sealed class StrengthReductionStage : BaseCodeTransformationStage, CIL.ICILVisitor, IPipelineStage
	{

		#region ICILVisitor

		/// <summary>
		/// Folds multiplication when one of the constants is zero
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Mul(Context context)
		{
			bool multiplyByZero = false;

			if (context.Operand1 is ConstantOperand)
				if (IsValueZero(context.Result.Type.Type, context.Operand1 as ConstantOperand))
					multiplyByZero = true;

			if (context.Operand2 is ConstantOperand)
				if (IsValueZero(context.Result.Type.Type, context.Operand2 as ConstantOperand))
					multiplyByZero = true;

			if (multiplyByZero)
			{
				context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, 0));
				return;
			}

			if (context.Operand1 is ConstantOperand)
				if (IsValueOne(context.Result.Type.Type, context.Operand1 as ConstantOperand))
				{
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, context.Operand2);
					return;
				}

			if (context.Operand2 is ConstantOperand)
				if (IsValueOne(context.Result.Type.Type, context.Operand2 as ConstantOperand))
				{
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, context.Operand1);
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
			switch (cilElementType)
			{
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
			switch (cilElementType)
			{
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
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Nop(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Break"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Break(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldarg"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldarg(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldarga"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldarga(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldloc"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldloc(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldloca"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldloca(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldc"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldc(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldobj"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldobj(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldstr"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldstr(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldfld"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldfld(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldflda"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldflda(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldsfld"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldsfld(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldsflda"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldsflda(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldftn"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldftn(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldvirtftn"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldvirtftn(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldtoken"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldtoken(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stloc"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stloc(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Starg"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Starg(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stobj"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stobj(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stfld"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stfld(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stsfld"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stsfld(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Dup"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Dup(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Pop"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Pop(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Jmp"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Jmp(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Call"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Call(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Calli"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Calli(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ret"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ret(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Branch"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Branch(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.UnaryBranch"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.UnaryBranch(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.BinaryBranch"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.BinaryBranch(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Switch"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Switch(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.BinaryLogic"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.BinaryLogic(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Shift"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Shift(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Neg"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Neg(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Not"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Not(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Conversion"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Conversion(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Callvirt"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Callvirt(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Cpobj"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Cpobj(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Newobj"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Newobj(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Castclass"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Castclass(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.IsInst"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.IsInst(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Unbox"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Unbox(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Throw"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Throw(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Box"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Box(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Newarr"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Newarr(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldlen"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldlen(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldelema"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldelema(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Ldelem"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldelem(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Stelem"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stelem(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.UnboxAny"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.UnboxAny(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Refanyval"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Refanyval(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.UnaryArithmetic"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.UnaryArithmetic(Context context) { }

		/// <summary>
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Mkrefany(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.ArithmeticOverflow"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.ArithmeticOverflow(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Endfinally"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Endfinally(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Leave"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Leave(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Arglist"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Arglist(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.BinaryComparison"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.BinaryComparison(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Localalloc"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Localalloc(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Endfilter"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Endfilter(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.InitObj"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.InitObj(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Cpblk"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Cpblk(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Initblk"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Initblk(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Prefix"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Prefix(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Rethrow"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Rethrow(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Sizeof"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Sizeof(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Refanytype"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Refanytype(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Add"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Add(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Sub"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Sub(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Div"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Div(Context context) { }

		/// <summary>
		/// Visitation function for <see cref="CIL.ICILVisitor.Rem"/>.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Rem(Context context) { }

		#endregion // ICILVisitor - Unused
	}
}
