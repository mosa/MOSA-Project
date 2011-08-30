/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework.Operands;

using CIL = Mosa.Runtime.CompilerFramework.CIL;

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
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Mul(Context context)
		{
			if (context.Operand1 is ConstantOperand && context.Operand2 is ConstantOperand)
			{
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (context.Result.Type.Type)
				{
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(context.Operand1 as ConstantOperand).Value) * ((byte)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(context.Operand1 as ConstantOperand).Value) * ((ushort)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(context.Operand1 as ConstantOperand).Value) * ((uint)(context.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(context.Operand1 as ConstantOperand).Value) * ((sbyte)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(context.Operand1 as ConstantOperand).Value) * ((short)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(context.Operand1 as ConstantOperand).Value) * ((int)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(context.Operand1 as ConstantOperand).Value) * ((float)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(context.Operand1 as ConstantOperand).Value) * ((double)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (context.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, fresult));
				else if (context.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, dresult));
				else
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds divisions with 2 constants
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Div(Context context)
		{
			if (context.Operand1 is ConstantOperand && context.Operand2 is ConstantOperand)
			{
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (context.Result.Type.Type)
				{
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(context.Operand1 as ConstantOperand).Value) / ((byte)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(context.Operand1 as ConstantOperand).Value) / ((ushort)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(context.Operand1 as ConstantOperand).Value) / ((uint)(context.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(context.Operand1 as ConstantOperand).Value) / ((sbyte)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(context.Operand1 as ConstantOperand).Value) / ((short)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(context.Operand1 as ConstantOperand).Value) / ((int)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(context.Operand1 as ConstantOperand).Value) / ((float)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(context.Operand1 as ConstantOperand).Value) / ((double)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (context.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, fresult));
				else if (context.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, dresult));
				else
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds the remainder of 2 constants
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Rem(Context context)
		{
			if (context.Operand1 is ConstantOperand && context.Operand2 is ConstantOperand)
			{
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (context.Result.Type.Type)
				{
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(context.Operand1 as ConstantOperand).Value) % ((byte)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(context.Operand1 as ConstantOperand).Value) % ((ushort)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(context.Operand1 as ConstantOperand).Value) % ((uint)(context.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(context.Operand1 as ConstantOperand).Value) % ((sbyte)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(context.Operand1 as ConstantOperand).Value) % ((short)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(context.Operand1 as ConstantOperand).Value) % ((int)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(context.Operand1 as ConstantOperand).Value) % ((float)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(context.Operand1 as ConstantOperand).Value) % ((double)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (context.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, fresult));
				else if (context.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, dresult));
				else
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds additions with 2 constants
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Add(Context context)
		{
			if (context.Operand1 is ConstantOperand && context.Operand2 is ConstantOperand)
			{
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (context.Result.Type.Type)
				{
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(context.Operand1 as ConstantOperand).Value) + ((byte)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(context.Operand1 as ConstantOperand).Value) + ((ushort)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(context.Operand1 as ConstantOperand).Value) + ((uint)(context.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(context.Operand1 as ConstantOperand).Value) + ((sbyte)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(context.Operand1 as ConstantOperand).Value) + ((short)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(context.Operand1 as ConstantOperand).Value) + ((int)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(context.Operand1 as ConstantOperand).Value) + ((float)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(context.Operand1 as ConstantOperand).Value) + ((double)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}

				if (context.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, fresult));
				else if (context.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, dresult));
				else
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds substractions with 2 constants
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Sub(Context context)
		{
			if (context.Operand1 is ConstantOperand && context.Operand2 is ConstantOperand)
			{
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (context.Result.Type.Type)
				{
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(context.Operand1 as ConstantOperand).Value) - ((byte)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(context.Operand1 as ConstantOperand).Value) - ((ushort)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(context.Operand1 as ConstantOperand).Value) - ((uint)(context.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(context.Operand1 as ConstantOperand).Value) - ((sbyte)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(context.Operand1 as ConstantOperand).Value) - ((short)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(context.Operand1 as ConstantOperand).Value) - ((int)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(context.Operand1 as ConstantOperand).Value) - ((float)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(context.Operand1 as ConstantOperand).Value) - ((double)(context.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (context.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, fresult));
				else if (context.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, dresult));
				else
					context.SetInstruction(IR.Instruction.MoveInstruction, context.Result, new ConstantOperand(context.Result.Type, result));
			}
		}

		#endregion // Methods

		#region ICILVisitor - Unused

		/// <summary>
		/// Visitation function for Nop instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Nop(Context context) { }

		/// <summary>
		/// Visitation function for Break instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Break(Context context) { }

		/// <summary>
		/// Visitation function for Ldarg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldarg(Context context) { }

		/// <summary>
		/// Visitation function for Ldarga instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldarga(Context context) { }

		/// <summary>
		/// Visitation function for Ldloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldloc(Context context) { }

		/// <summary>
		/// Visitation function for Ldloca instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldloca(Context context) { }

		/// <summary>
		/// Visitation function for Ldc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldc(Context context) { }

		/// <summary>
		/// Visitation function for Ldobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldobj(Context context) { }

		/// <summary>
		/// Visitation function for Ldstr instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldstr(Context context) { }

		/// <summary>
		/// Visitation function for Ldfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldfld(Context context) { }

		/// <summary>
		/// Visitation function for Ldflda instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldflda(Context context) { }

		/// <summary>
		/// Visitation function for Ldsfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldsfld(Context context) { }

		/// <summary>
		/// Visitation function for Ldsflda instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldsflda(Context context) { }

		/// <summary>
		/// Visitation function for Ldftn instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldftn(Context context) { }

		/// <summary>
		/// Visitation function for Ldvirtftn instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldvirtftn(Context context) { }

		/// <summary>
		/// Visitation function for Ldtoken instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldtoken(Context context) { }

		/// <summary>
		/// Visitation function for Stloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stloc(Context context) { }

		/// <summary>
		/// Visitation function for Starg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Starg(Context context) { }

		/// <summary>
		/// Visitation function for Stobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stobj(Context context) { }

		/// <summary>
		/// Visitation function for Stfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stfld(Context context) { }

		/// <summary>
		/// Visitation function for Stsfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stsfld(Context context) { }

		/// <summary>
		/// Visitation function for Dup instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Dup(Context context) { }

		/// <summary>
		/// Visitation function for Pop instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Pop(Context context) { }

		/// <summary>
		/// Visitation function for Jmp instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Jmp(Context context) { }

		/// <summary>
		/// Visitation function for Call instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Call(Context context) { }

		/// <summary>
		/// Visitation function for Calli instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Calli(Context context) { }

		/// <summary>
		/// Visitation function for Ret instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ret(Context context) { }

		/// <summary>
		/// Visitation function for Branch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Branch(Context context) { }

		/// <summary>
		/// Visitation function for UnaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.UnaryBranch(Context context) { }

		/// <summary>
		/// Visitation function for BinaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.BinaryBranch(Context context) { }

		/// <summary>
		/// Visitation function for Switch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Switch(Context context) { }

		/// <summary>
		/// Visitation function for BinaryLogic instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.BinaryLogic(Context context) { }

		/// <summary>
		/// Visitation function for Shift instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Shift(Context context) { }

		/// <summary>
		/// Visitation function for Neg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Neg(Context context) { }

		/// <summary>
		/// Visitation function for Not instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Not(Context context) { }

		/// <summary>
		/// Visitation function for Conversion instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Conversion(Context context) { }

		/// <summary>
		/// Visitation function for Callvirt instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Callvirt(Context context) { }

		/// <summary>
		/// Visitation function for Cpobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Cpobj(Context context) { }

		/// <summary>
		/// Visitation function for Newobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Newobj(Context context) { }

		/// <summary>
		/// Visitation function for Castclass instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Castclass(Context context) { }

		/// <summary>
		/// Visitation function for Isinst instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.IsInst(Context context) { }

		/// <summary>
		/// Visitation function for Unbox instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Unbox(Context context) { }

		/// <summary>
		/// Visitation function for Throw instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Throw(Context context) { }

		/// <summary>
		/// Visitation function for Box instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Box(Context context) { }

		/// <summary>
		/// Visitation function for Newarr instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Newarr(Context context) { }

		/// <summary>
		/// Visitation function for Ldlen instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldlen(Context context) { }

		/// <summary>
		/// Visitation function for Ldelema instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldelema(Context context) { }

		/// <summary>
		/// Visitation function for Ldelem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldelem(Context context) { }

		/// <summary>
		/// Visitation function for Stelem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stelem(Context context) { }

		/// <summary>
		/// Visitation function for UnboxAny instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.UnboxAny(Context context) { }

		/// <summary>
		/// Visitation function for Refanyval instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Refanyval(Context context) { }

		/// <summary>
		/// Visitation function for UnaryArithmetic instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.UnaryArithmetic(Context context) { }

		/// <summary>
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Mkrefany(Context context) { }

		/// <summary>
		/// Visitation function for ArithmeticOverflow instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.ArithmeticOverflow(Context context) { }

		/// <summary>
		/// Visitation function for Endfinally instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Endfinally(Context context) { }

		/// <summary>
		/// Visitation function for Leave instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Leave(Context context) { }

		/// <summary>
		/// Visitation function for Arglist instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Arglist(Context context) { }

		/// <summary>
		/// Visitation function for BinaryComparison instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.BinaryComparison(Context context) { }

		/// <summary>
		/// Visitation function for Localalloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Localalloc(Context context) { }

		/// <summary>
		/// Visitation function for Endfilter instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Endfilter(Context context) { }

		/// <summary>
		/// Visitation function for InitObj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.InitObj(Context context) { }

		/// <summary>
		/// Visitation function for Cpblk instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Cpblk(Context context) { }

		/// <summary>
		/// Visitation function for Initblk instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Initblk(Context context) { }

		/// <summary>
		/// Visitation function for Prefix instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Prefix(Context context) { }

		/// <summary>
		/// Visitation function for Rethrow instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Rethrow(Context context) { }

		/// <summary>
		/// Visitation function for Sizeof instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Sizeof(Context context) { }

		/// <summary>
		/// Visitation function for Refanytype instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Refanytype(Context context) { }

		#endregion // ICILVisitor - Unused
	}
}
