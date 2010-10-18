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

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// Performs IR constant folding of arithmetic instructions to optimize
	/// the code down to fewer calculations.
	/// </summary>
	public sealed class ConstantFoldingStage : BaseCodeTransformationStage, CIL.ICILVisitor, IPipelineStage
	{

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name
		{
			get { return @"CIL.ConstantFoldingStage"; }
		}

		#endregion // IPipelineStage

		#region ICILVisitor

		/// <summary>
		/// Folds multiplication with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Mul(Context ctx)
		{
			if (ctx.Operand1 is ConstantOperand && ctx.Operand2 is ConstantOperand)
			{
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (ctx.Result.Type.Type)
				{
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand1 as ConstantOperand).Value) * ((byte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand1 as ConstantOperand).Value) * ((ushort)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand1 as ConstantOperand).Value) * ((uint)(ctx.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand1 as ConstantOperand).Value) * ((sbyte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand1 as ConstantOperand).Value) * ((short)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand1 as ConstantOperand).Value) * ((int)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(ctx.Operand1 as ConstantOperand).Value) * ((float)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(ctx.Operand1 as ConstantOperand).Value) * ((double)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, fresult));
				else if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, dresult));
				else
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds divisions with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Div(Context ctx)
		{
			if (ctx.Operand1 is ConstantOperand && ctx.Operand2 is ConstantOperand)
			{
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (ctx.Result.Type.Type)
				{
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand1 as ConstantOperand).Value) / ((byte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand1 as ConstantOperand).Value) / ((ushort)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand1 as ConstantOperand).Value) / ((uint)(ctx.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand1 as ConstantOperand).Value) / ((sbyte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand1 as ConstantOperand).Value) / ((short)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand1 as ConstantOperand).Value) / ((int)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(ctx.Operand1 as ConstantOperand).Value) / ((float)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(ctx.Operand1 as ConstantOperand).Value) / ((double)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, fresult));
				else if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, dresult));
				else
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds the remainder of 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Rem(Context ctx)
		{
			if (ctx.Operand1 is ConstantOperand && ctx.Operand2 is ConstantOperand)
			{
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (ctx.Result.Type.Type)
				{
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand1 as ConstantOperand).Value) % ((byte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand1 as ConstantOperand).Value) % ((ushort)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand1 as ConstantOperand).Value) % ((uint)(ctx.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand1 as ConstantOperand).Value) % ((sbyte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand1 as ConstantOperand).Value) % ((short)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand1 as ConstantOperand).Value) % ((int)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(ctx.Operand1 as ConstantOperand).Value) % ((float)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(ctx.Operand1 as ConstantOperand).Value) % ((double)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, fresult));
				else if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, dresult));
				else
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds additions with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Add(Context ctx)
		{
			if (ctx.Operand1 is ConstantOperand && ctx.Operand2 is ConstantOperand)
			{
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (ctx.Result.Type.Type)
				{
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand1 as ConstantOperand).Value) + ((byte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand1 as ConstantOperand).Value) + ((ushort)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand1 as ConstantOperand).Value) + ((uint)(ctx.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand1 as ConstantOperand).Value) + ((sbyte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand1 as ConstantOperand).Value) + ((short)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand1 as ConstantOperand).Value) + ((int)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(ctx.Operand1 as ConstantOperand).Value) + ((float)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(ctx.Operand1 as ConstantOperand).Value) + ((double)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}

				if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, fresult));
				else if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, dresult));
				else
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds substractions with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Sub(Context ctx)
		{
			if (ctx.Operand1 is ConstantOperand && ctx.Operand2 is ConstantOperand)
			{
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (ctx.Result.Type.Type)
				{
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand1 as ConstantOperand).Value) - ((byte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand1 as ConstantOperand).Value) - ((ushort)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand1 as ConstantOperand).Value) - ((uint)(ctx.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand1 as ConstantOperand).Value) - ((sbyte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand1 as ConstantOperand).Value) - ((short)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand1 as ConstantOperand).Value) - ((int)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(ctx.Operand1 as ConstantOperand).Value) - ((float)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(ctx.Operand1 as ConstantOperand).Value) - ((double)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, fresult));
				else if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, dresult));
				else
					ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, result));
			}
		}

		#endregion // Methods

		#region ICILVisitor - Unused

		/// <summary>
		/// Visitation function for <see cref="Nop"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Nop(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Break"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Break(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldarg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldarg(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldarga"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldarga(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldloca"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldloca(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldstr"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldstr(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldflda(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldsfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldsflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldsflda(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldftn"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldftn(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldvirtftn"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldvirtftn(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldtoken"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldtoken(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Stloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Starg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Starg(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Stobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Stfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Stsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stsfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Dup"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Dup(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Pop"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Pop(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Jmp"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Jmp(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Call"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Call(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Calli"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Calli(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ret"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ret(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Branch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Branch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="UnaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.UnaryBranch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="BinaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.BinaryBranch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Switch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Switch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="BinaryLogic"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.BinaryLogic(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Shift"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Shift(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Neg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Neg(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Not"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Not(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Conversion"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Conversion(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Callvirt"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Callvirt(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Cpobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Cpobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Newobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Newobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Castclass"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Castclass(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Isinst"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Isinst(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Unbox"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Unbox(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Throw"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Throw(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Box"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Box(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Newarr"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Newarr(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldlen"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldlen(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldelema"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldelema(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Ldelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Ldelem(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Stelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Stelem(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="UnboxAny"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.UnboxAny(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Refanyval"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Refanyval(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="UnaryArithmetic"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.UnaryArithmetic(Context ctx) { }

		/// <summary>
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Mkrefany(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ArithmeticOverflow"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.ArithmeticOverflow(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Endfinally"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Endfinally(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Leave"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Leave(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Arglist"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Arglist(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="BinaryComparison"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.BinaryComparison(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Localalloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Localalloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Endfilter"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Endfilter(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="InitObj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.InitObj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Cpblk"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Cpblk(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Initblk"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Initblk(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Prefix"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Prefix(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Rethrow"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Rethrow(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Sizeof"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Sizeof(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="Refanytype"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CIL.ICILVisitor.Refanytype(Context ctx) { }

		#endregion // ICILVisitor - Unused
	}
}
