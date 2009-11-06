/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.CIL;

using IR = Mosa.Runtime.CompilerFramework.IR;
using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework.IR
{
	/// <summary>
	/// Transforms CIL instructions into their appropriate IR.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms CIL instructions into their equivalent IR sequences.
	/// </remarks>
	public sealed class CilTransformationStage
		: CodeTransformationStage, CIL.ICILVisitor
	{
		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name
		{
			get { return @"IR.CilTransformationStage"; }
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void SetPipelinePosition(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
		}

		#endregion // IMethodCompilerStage Members

		#region ICILVisitor

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldarg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldarg(Context ctx)
		{
			ProcessLoadInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldarga"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldarga(Context ctx)
		{
			//ctx.SetInstruction(IR.Instruction.AddressOfInstruction(ctx.InstructionSet.Data[ctx.Index].Result, ctx.InstructionSet.Data[ctx.Index].Operand1));
			ctx.SetInstruction(IR.Instruction.AddressOfInstruction, ctx.Result, ctx.Operand1);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldloc(Context ctx)
		{
			ProcessLoadInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldloca"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldloca(Context ctx)
		{
			//ctx.SetInstruction(IR.Instruction.AddressOfInstruction(ctx.Result, ctx.Operand1));
			ctx.SetInstruction(IR.Instruction.AddressOfInstruction, ctx.Result, ctx.Operand1);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldc(Context ctx)
		{
			ProcessLoadInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldobj(Context ctx)
		{
			// This is actually ldind.* and ldobj - the opcodes have the same meanings
			ctx.SetInstruction(IR.Instruction.LoadInstruction, ctx.Result, ctx.Operand1);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldsfld(Context ctx)
		{
			ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, new MemberOperand(ctx.RuntimeField));
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldsflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldsflda(Context ctx)
		{
			ctx.SetInstruction(IR.Instruction.AddressOfInstruction, ctx.Result, ctx.Operand1);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldftn"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldftn(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.GetFunctionPtr);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldvirtftn"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldvirtftn(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.GetVirtualFunctionPtr);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldtoken"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldtoken(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.GetHandleForToken);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Stloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Stloc(Context ctx)
		{
			ProcessStoreInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Starg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Starg(Context ctx)
		{
			ProcessStoreInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Stobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Stobj(Context ctx)
		{
			// This is actually stind.* and stobj - the opcodes have the same meanings
			ctx.SetInstruction(IR.Instruction.StoreInstruction, ctx.Operand1, ctx.Operand2);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Stsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Stsfld(Context ctx)
		{
			ctx.SetInstruction(IR.Instruction.MoveInstruction, new MemberOperand(ctx.RuntimeField), ctx.Operand1);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Dup"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Dup(Context ctx)
		{
			// We don't need the dup anymore.
			//Remove(ctx);
			ctx.Remove();
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Call"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Call(Context ctx)
		{
			ProcessRedirectableInvokeInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Calli"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Calli(Context ctx)
		{
			ProcessInvokeInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ret"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ret(Context ctx)
		{
			ctx.ReplaceInstructionOnly(IR.Instruction.ReturnInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.BinaryLogic"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.BinaryLogic(Context ctx)
		{
			switch ((ctx.Instruction as CIL.BaseInstruction).OpCode) {
				case OpCode.And:
					ctx.SetInstruction(IR.Instruction.LogicalAndInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
					break;
				case OpCode.Or:
					ctx.SetInstruction(IR.Instruction.LogicalOrInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
					break;
				case OpCode.Xor:
					ctx.SetInstruction(IR.Instruction.LogicalXorInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
					break;
				case OpCode.Div_un:
					ctx.SetInstruction(IR.Instruction.UDivInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
					break;
				case OpCode.Rem_un:
					ctx.SetInstruction(IR.Instruction.URemInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
					break;
				default:
					throw new NotSupportedException();
			}

		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Shift"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Shift(Context ctx)
		{
			switch ((ctx.Instruction as CIL.BaseInstruction).OpCode) {
				case OpCode.Shl:
					ctx.SetInstruction(IR.Instruction.ShiftLeftInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
					break;
				case OpCode.Shr:
					ctx.SetInstruction(IR.Instruction.ArithmeticShiftRightInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
					break;
				case OpCode.Shr_un:
					ctx.SetInstruction(IR.Instruction.ShiftRightInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
					break;
				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Neg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Neg(Context ctx)
		{
			ctx.SetInstruction(CIL.Instruction.Get(OpCode.Sub), ctx.Result, ctx.Operand1);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Not"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Not(Context ctx)
		{
			ctx.SetInstruction(IR.Instruction.LogicalNotInstruction, ctx.Result, ctx.Operand1);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Conversion"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Conversion(Context ctx)
		{
			ProcessConversionInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Callvirt"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Callvirt(Context ctx)
		{
			ProcessInvokeInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Newobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Newobj(Context ctx)
		{
			/* FIXME:
			 * 
			 * - Newobj is actually three things at once:
			 *   - Find the type From the token
			 *   - Allocate memory for the found type
			 *   - Invoke the ctor with the arguments
			 * - We're rewriting it exactly as specified above, e.g. first we find
			 *   the type handle (similar to ldtoken), we pass that to an internal
			 *   call to allocate the memory and finally we call the ctor on the
			 *   allocated object.
			 * - We don't have to check the allocation result, if it fails, the 
			 *   allocator will happily throw the OutOfMemoryException.
			 * - We do need to find the right ctor to invoke though.
			 * 
			 */
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Castclass"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Castclass(Context ctx)
		{
			// We don't need to check the result, if the icall fails, it'll happily throw
			// the InvalidCastException.
			ReplaceWithInternalCall(ctx, VmCall.Castclass);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Isinst"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Isinst(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.IsInstanceOfType);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Unbox"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Unbox(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Unbox);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Throw"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Throw(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Throw);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Box"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Box(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Box);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Newarr"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Newarr(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Allocate);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.BinaryComparison"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.BinaryComparison(Context ctx)
		{
			IR.ConditionCode code = GetConditionCode((ctx.Instruction as CIL.BaseInstruction).OpCode);

			if (ctx.Operand1.StackType == StackTypeCode.F)
				ctx.SetInstruction(IR.Instruction.FloatingPointCompareInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
			else
				ctx.SetInstruction(IR.Instruction.IntegerCompareInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);

			ctx.ConditionCode = code;
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Cpblk"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Cpblk(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Memcpy);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Initblk"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Initblk(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Memset);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Rethrow"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Rethrow(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Rethrow);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Nop"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Nop(Context ctx)
		{
			ctx.ReplaceInstructionOnly(IR.Instruction.NopInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Pop"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Pop(Context ctx)
		{
			ctx.Remove();
		}

		#endregion // ICILVisitor

		#region ICILVisitor - Unused

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Break"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Break(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldstr"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldstr(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldflda(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Stfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Stfld(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Jmp"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Jmp(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Branch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Branch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.UnaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.UnaryBranch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.BinaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.BinaryBranch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Switch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Switch(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Cpobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Cpobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldlen"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldlen(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldelema"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldelema(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Ldelem(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Stelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Stelem(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.UnboxAny"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.UnboxAny(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Refanyval"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Refanyval(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.UnaryArithmetic"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.UnaryArithmetic(Context ctx) { }

		/// <summary>
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Mkrefany(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.ArithmeticOverflow"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.ArithmeticOverflow(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Endfinally"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Endfinally(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Leave"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Leave(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Arglist"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Arglist(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Localalloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Localalloc(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Endfilter"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Endfilter(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.InitObj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.InitObj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Prefix"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Prefix(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Sizeof"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Sizeof(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Refanytype"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Refanytype(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Add"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Add(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Sub"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Sub(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Mul"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Mul(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Div"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Div(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Rem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Rem(Context ctx) { }

		#endregion // ICILVisitor

		#region Internals

		/// <summary>
		/// Determines if a store is silently truncating the value.
		/// </summary>
		/// <param name="dest">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <returns>True if the store is truncating, otherwise false.</returns>
		private static bool IsTruncating(Operand dest, Operand source)
		{
			CilElementType cetDest = dest.Type.Type;
			CilElementType cetSource = source.Type.Type;

			if (cetDest == CilElementType.I4 || cetDest == CilElementType.U4) {
				return (cetSource == CilElementType.I8 || cetSource == CilElementType.U8);
			}
			if (cetDest == CilElementType.I2 || cetDest == CilElementType.U2 || cetDest == CilElementType.Char) {
				return (cetSource == CilElementType.I8 || cetSource == CilElementType.U8 || cetSource == CilElementType.I4 || cetSource == CilElementType.U4);
			}
			if (cetDest == CilElementType.I1 || cetDest == CilElementType.U1) {
				return (cetSource == CilElementType.I8 || cetSource == CilElementType.U8 || cetSource == CilElementType.I4 || cetSource == CilElementType.U4 || cetSource == CilElementType.I2 || cetSource == CilElementType.U2 || cetSource == CilElementType.U2);
			}

			return false;
		}

		/// <summary>
		/// Determines if the load should sign extend the given source operand.
		/// </summary>
		/// <param name="source">The source operand to determine sign extension for.</param>
		/// <returns>True if the given operand should be loaded with its sign extended.</returns>
		private static bool IsSignExtending(Operand source)
		{
			bool result = false;
			CilElementType cet = source.Type.Type;

			if (cet == CilElementType.I1 || cet == CilElementType.I2)
				result = true;

			return result;
		}

		/// <summary>
		/// Determines the implicit result type of the load instruction.
		/// </summary>
		/// <param name="sigType">The signature type of the source operand.</param>
		/// <returns>The signature type of the result.</returns>
		/// <remarks>
		/// This method performs the implicit type conversion mandated by the CIL spec
		/// for load instructions.
		/// </remarks>
		private SigType GetImplicitResultSigType(SigType sigType)
		{
			SigType result;

			switch (sigType.Type) {
				case CilElementType.I1: goto case CilElementType.I2;
				case CilElementType.I2:
					result = new SigType(CilElementType.I4);
					break;

				case CilElementType.U1: goto case CilElementType.U2;
				case CilElementType.U2:
					result = new SigType(CilElementType.U4);
					break;

				case CilElementType.Char: goto case CilElementType.U2;
				case CilElementType.Boolean: goto case CilElementType.U2;


				default:
					result = sigType;
					break;
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		private enum ConvType
		{
			I1 = 0,
			I2 = 1,
			I4 = 2,
			I8 = 3,
			U1 = 4,
			U2 = 5,
			U4 = 6,
			U8 = 7,
			R4 = 8,
			R8 = 9,
			I = 10,
			U = 11,
			Ptr = 12
		}

		/// <summary>
		/// Converts a <see cref="CilElementType"/> to <see cref="ConvType"/>.
		/// </summary>
		/// <param name="cet">The CilElementType to convert.</param>
		/// <returns>The equivalent ConvType.</returns>
		/// <exception cref="T:System.NotSupportedException"><paramref name="cet"/> can't be converted.</exception>
		private ConvType ConvTypeFromCilType(CilElementType cet)
		{
			switch (cet) {
				case CilElementType.Char: return ConvType.U2;
				case CilElementType.I1: return ConvType.I1;
				case CilElementType.I2: return ConvType.I2;
				case CilElementType.I4: return ConvType.I4;
				case CilElementType.I8: return ConvType.I8;
				case CilElementType.U1: return ConvType.U1;
				case CilElementType.U2: return ConvType.U2;
				case CilElementType.U4: return ConvType.U4;
				case CilElementType.U8: return ConvType.U8;
				case CilElementType.R4: return ConvType.R4;
				case CilElementType.R8: return ConvType.R8;
				case CilElementType.I: return ConvType.I;
				case CilElementType.U: return ConvType.U;
				case CilElementType.Ptr: return ConvType.Ptr;
			}

			// Requested CilElementType is not supported
			throw new NotSupportedException();
		}

        private static readonly BaseInstruction[][] s_convTable = new BaseInstruction[13][] {
            /* I1 */ new BaseInstruction[13] { 
                /* I1 */ IR.Instruction.MoveInstruction,
                /* I2 */ IR.Instruction.LogicalAndInstruction,
                /* I4 */ IR.Instruction.LogicalAndInstruction,
                /* I8 */ IR.Instruction.LogicalAndInstruction,
                /* U1 */ IR.Instruction.MoveInstruction,
                /* U2 */ IR.Instruction.LogicalAndInstruction,
                /* U4 */ IR.Instruction.LogicalAndInstruction,
                /* U8 */ IR.Instruction.LogicalAndInstruction,
                /* R4 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* R8 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* I  */ IR.Instruction.LogicalAndInstruction,
                /* U  */ IR.Instruction.LogicalAndInstruction,
                /* Ptr*/ IR.Instruction.LogicalAndInstruction,
            },
            /* I2 */ new BaseInstruction[13] { 
                /* I1 */ IR.Instruction.SignExtendedMoveInstruction,
                /* I2 */ IR.Instruction.MoveInstruction,
                /* I4 */ IR.Instruction.LogicalAndInstruction,
                /* I8 */ IR.Instruction.LogicalAndInstruction,
                /* U1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U2 */ IR.Instruction.MoveInstruction,
                /* U4 */ IR.Instruction.LogicalAndInstruction,
                /* U8 */ IR.Instruction.LogicalAndInstruction,
                /* R4 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* R8 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* I  */ IR.Instruction.LogicalAndInstruction,
                /* U  */ IR.Instruction.LogicalAndInstruction,
                /* Ptr*/ IR.Instruction.LogicalAndInstruction,
            },
            /* I4 */ new BaseInstruction[13] { 
                /* I1 */ IR.Instruction.SignExtendedMoveInstruction,
                /* I2 */ IR.Instruction.SignExtendedMoveInstruction,
                /* I4 */ IR.Instruction.MoveInstruction,
                /* I8 */ IR.Instruction.LogicalAndInstruction,
                /* U1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U2 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U4 */ IR.Instruction.MoveInstruction,
                /* U8 */ IR.Instruction.LogicalAndInstruction,
                /* R4 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* R8 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* I  */ IR.Instruction.LogicalAndInstruction,
                /* U  */ IR.Instruction.LogicalAndInstruction,
                /* Ptr*/ IR.Instruction.LogicalAndInstruction,
            },
            /* I8 */ new BaseInstruction[13] {
                /* I1 */ IR.Instruction.SignExtendedMoveInstruction,
                /* I2 */ IR.Instruction.SignExtendedMoveInstruction,
                /* I4 */ IR.Instruction.SignExtendedMoveInstruction,
                /* I8 */ IR.Instruction.MoveInstruction,
                /* U1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U2 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U4 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U8 */ IR.Instruction.MoveInstruction,
                /* R4 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* R8 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* I  */ IR.Instruction.LogicalAndInstruction,
                /* U  */ IR.Instruction.LogicalAndInstruction,
                /* Ptr*/ IR.Instruction.LogicalAndInstruction,
            },
            /* U1 */ new BaseInstruction[13] {
                /* I1 */ IR.Instruction.MoveInstruction,
                /* I2 */ IR.Instruction.LogicalAndInstruction,
                /* I4 */ IR.Instruction.LogicalAndInstruction,
                /* I8 */ IR.Instruction.LogicalAndInstruction,
                /* U1 */ IR.Instruction.MoveInstruction,
                /* U2 */ IR.Instruction.LogicalAndInstruction,
                /* U4 */ IR.Instruction.LogicalAndInstruction,
                /* U8 */ IR.Instruction.LogicalAndInstruction,
                /* R4 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* R8 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* I  */ IR.Instruction.LogicalAndInstruction,
                /* U  */ IR.Instruction.LogicalAndInstruction,
                /* Ptr*/ IR.Instruction.LogicalAndInstruction,
            },
            /* U2 */ new BaseInstruction[13] {
                /* I1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* I2 */ IR.Instruction.MoveInstruction,
                /* I4 */ IR.Instruction.LogicalAndInstruction,
                /* I8 */ IR.Instruction.LogicalAndInstruction,
                /* U1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U2 */ IR.Instruction.MoveInstruction,
                /* U4 */ IR.Instruction.LogicalAndInstruction,
                /* U8 */ IR.Instruction.LogicalAndInstruction,
                /* R4 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* R8 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* I  */ IR.Instruction.LogicalAndInstruction,
                /* U  */ IR.Instruction.LogicalAndInstruction,
                /* Ptr*/ IR.Instruction.LogicalAndInstruction,
            },
            /* U4 */ new BaseInstruction[13] {
                /* I1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* I2 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* I4 */ IR.Instruction.MoveInstruction,
                /* I8 */ IR.Instruction.LogicalAndInstruction,
                /* U1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U2 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U4 */ IR.Instruction.MoveInstruction,
                /* U8 */ IR.Instruction.LogicalAndInstruction,
                /* R4 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* R8 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* I  */ IR.Instruction.LogicalAndInstruction,
                /* U  */ IR.Instruction.LogicalAndInstruction,
                /* Ptr*/ IR.Instruction.LogicalAndInstruction,
            },
            /* U8 */ new BaseInstruction[13] {
                /* I1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* I2 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* I4 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* I8 */ IR.Instruction.MoveInstruction,
                /* U1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U2 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U4 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U8 */ IR.Instruction.MoveInstruction,
                /* R4 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* R8 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* I  */ IR.Instruction.LogicalAndInstruction,
                /* U  */ IR.Instruction.LogicalAndInstruction,
                /* Ptr*/ IR.Instruction.LogicalAndInstruction,
            },
            /* R4 */ new BaseInstruction[13] {
                /* I1 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* I2 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* I4 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* I8 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* U1 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* U2 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* U4 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* U8 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* R4 */ IR.Instruction.MoveInstruction,
                /* R8 */ IR.Instruction.MoveInstruction,
                /* I  */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* U  */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* Ptr*/ null,
            },
            /* R8 */ new BaseInstruction[13] {
                /* I1 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* I2 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* I4 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* I8 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* U1 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* U2 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* U4 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* U8 */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* R4 */ IR.Instruction.MoveInstruction,
                /* R8 */ IR.Instruction.MoveInstruction,
                /* I  */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* U  */ IR.Instruction.IntegerToFloatingPointConversionInstruction,
                /* Ptr*/ null,
            },
            /* I  */ new BaseInstruction[13] {
                /* I1 */ IR.Instruction.SignExtendedMoveInstruction,
                /* I2 */ IR.Instruction.SignExtendedMoveInstruction,
                /* I4 */ IR.Instruction.SignExtendedMoveInstruction,
                /* I8 */ IR.Instruction.SignExtendedMoveInstruction,
                /* U1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U2 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U4 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U8 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* R4 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* R8 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* I  */ IR.Instruction.MoveInstruction,
                /* U  */ IR.Instruction.MoveInstruction,
                /* Ptr*/ IR.Instruction.MoveInstruction,
            },
            /* U  */ new BaseInstruction[13] {
                /* I1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* I2 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* I4 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* I8 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U2 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U4 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U8 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* R4 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* R8 */ IR.Instruction.FloatingPointToIntegerConversionInstruction,
                /* I  */ IR.Instruction.MoveInstruction,
                /* U  */ IR.Instruction.MoveInstruction,
                /* Ptr*/ IR.Instruction.MoveInstruction,
            },
            /* Ptr*/ new BaseInstruction[13] {
                /* I1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* I2 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* I4 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* I8 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U1 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U2 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U4 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* U8 */ IR.Instruction.ZeroExtendedMoveInstruction,
                /* R4 */ null,
                /* R8 */ null,
                /* I  */ IR.Instruction.MoveInstruction,
                /* U  */ IR.Instruction.MoveInstruction,
                /* Ptr*/ IR.Instruction.MoveInstruction,
            },
        };

		/// <summary>
		/// Selects the appropriate IR conversion instruction.
		/// </summary>
		/// <param name="ctx">The transformation context.</param>
		private void ProcessConversionInstruction(Context ctx)
		{
			Operand dest = ctx.Result;
			Operand src = ctx.Operand1;

			CheckAndConvertInstruction(ctx, dest, src);
		}

		private void CheckAndConvertInstruction(Context ctx, Operand destinationOperand, Operand sourceOperand)
		{
			ConvType ctDest = ConvTypeFromCilType(destinationOperand.Type.Type);
			ConvType ctSrc = ConvTypeFromCilType(sourceOperand.Type.Type);

			BaseInstruction type = s_convTable[(int)ctDest][(int)ctSrc];
			if (type == null)
				throw new NotSupportedException();

			uint mask = 0;
			IInstruction instruction = ComputeExtensionTypeAndMask(ctDest, ref mask);

            if (type == IR.Instruction.LogicalAndInstruction || mask != 0)
            {
				Debug.Assert(mask != 0, @"Conversion is an AND, but no mask given.");

				ctx.Remove();

				if (instruction != IR.Instruction.LogicalAndInstruction)
					ProcessMixedTypeConversion(ctx, instruction, mask, destinationOperand, sourceOperand);
				else
					ProcessSingleTypeTruncation(ctx, instruction, mask, destinationOperand, sourceOperand);

				ExtendAndTruncateResult(ctx, instruction, destinationOperand);
			}
			else
                ctx.SetInstruction(type, destinationOperand, sourceOperand);
		}

		private IInstruction ComputeExtensionTypeAndMask(ConvType destinationType, ref uint mask)
		{
			mask = 0;

			switch (destinationType) {
				case ConvType.I1:
					mask = 0xFF;
					return IR.Instruction.SignExtendedMoveInstruction;
				case ConvType.I2:
					mask = 0xFFFF;
					return IR.Instruction.SignExtendedMoveInstruction;
				case ConvType.I4:
					mask = 0xFFFFFFFF;
					break;
				case ConvType.I8:
					break;
				case ConvType.U1:
					mask = 0xFF;
					return IR.Instruction.ZeroExtendedMoveInstruction;
				case ConvType.U2:
					mask = 0xFFFF;
					return IR.Instruction.ZeroExtendedMoveInstruction;
				case ConvType.U4:
					mask = 0xFFFFFFFF;
					break;
				case ConvType.U8:
					break;
				case ConvType.R4:
					break;
				case ConvType.R8:
					break;
				case ConvType.I:
					break;
				case ConvType.U:
					break;
				case ConvType.Ptr:
					break;
				default:
					Debug.Assert(false);
					throw new NotSupportedException();
			}

			return null;
		}

		private void ProcessMixedTypeConversion(Context ctx, IInstruction instruction, uint mask, Operand destinationOperand, Operand sourceOperand)
		{
			ctx.AppendInstruction(instruction, destinationOperand, sourceOperand);
			ctx.AppendInstruction(IR.Instruction.LogicalAndInstruction, destinationOperand, sourceOperand, new ConstantOperand(new SigType(CilElementType.U4), mask));
		}

		private void ProcessSingleTypeTruncation(Context ctx, IInstruction instruction, uint mask, Operand destinationOperand, Operand sourceOperand)
		{
			if (sourceOperand.Type.Type == CilElementType.I8 || sourceOperand.Type.Type == CilElementType.U8) {
				ctx.AppendInstruction(IR.Instruction.MoveInstruction, destinationOperand, sourceOperand);
				ctx.AppendInstruction(instruction, destinationOperand, sourceOperand, new ConstantOperand(new SigType(CilElementType.U4), mask));
			}
			else
				ctx.AppendInstruction(instruction, destinationOperand, sourceOperand, new ConstantOperand(new SigType(CilElementType.U4), mask));
		}

		private void ExtendAndTruncateResult(Context ctx, IInstruction instruction, Operand destinationOperand)
		{
			if (instruction != null && destinationOperand is RegisterOperand) {
				RegisterOperand resultOperand = new RegisterOperand(new SigType(CilElementType.I4), ((RegisterOperand)destinationOperand).Register);
				ctx.AppendInstruction(instruction, resultOperand, destinationOperand);
			}
		}

		/// <summary>
		/// Processes redirectable method call instructions.
		/// </summary>
		/// <param name="ctx">The transformation context.</param>
		/// <remarks>
		/// This method checks if the call target has an Intrinsic-Attribute applied with
		/// the current architecture. If it has, the method call is replaced by the specified
		/// native instruction.
		/// </remarks>
		private void ProcessRedirectableInvokeInstruction(Context ctx)
		{
			// Retrieve the invocation target
			RuntimeMethod rm = ctx.InvokeTarget;
			Debug.Assert(rm != null, @"Call doesn't have a target.");
			// Retrieve the runtime type
			RuntimeType rt = RuntimeBase.Instance.TypeLoader.GetType(@"Mosa.Runtime.CompilerFramework.IntrinsicAttribute, Mosa.Runtime");

			if (rm.IsDefined(rt)) {
				// FIXME: Change this to a GetCustomAttributes call, once we can do that :)
				foreach (RuntimeAttribute ra in rm.CustomAttributes) {
					if (ra.Type == rt) {
						// Get the intrinsic attribute
						IntrinsicAttribute ia = (IntrinsicAttribute)ra.GetAttribute();
						if (ia.Architecture.IsInstanceOfType(Architecture)) {
							// Found a replacement for the call...
							try {
								ctx.ReplaceInstructionOnly(MethodCompiler.Architecture.GetIntrinsicIntruction(ia.InstructionType));

								return;
							}
							catch (Exception e) {
								Trace.WriteLine("Failed to replace intrinsic call with its instruction:");
								Trace.WriteLine(e);
							}
						}
					}
				}
			}

			ProcessInvokeInstruction(ctx);
		}

		/// <summary>
		/// Processes a method call instruction.
		/// </summary>
		/// <param name="ctx">The transformation context.</param>
		private void ProcessInvokeInstruction(Context ctx)
		{
			/* FIXME: Check for IntrinsicAttribute and replace the invoke instruction with the intrinsic,
			 * if necessary.
			 */
		}

		/// <summary>
		/// Replaces the IL load instruction by an appropriate IR move instruction or removes it entirely, if
		/// it is a native size.
		/// </summary>
		/// <param name="ctx">Provides the transformation context.</param>
		private void ProcessLoadInstruction(Context ctx)
		{
			// We don't need to rewire the source/destination yet, its already there. :(
			//Remove(ctx);
			ctx.Remove();

			/* FIXME: This is only valid with reg alloc!
			Type type = null;

			load = load as LoadInstruction;

			// Is this a sign or zero-extending move?
			if (IsSignExtending(load.Source))
			{
				type = typeof(IR.SignExtendedMoveInstruction);
			}
			else if (IsZeroExtending(load.Source))
			{
				type = typeof(IR.ZeroExtendedMoveInstruction);
			}

			// Do we have a move replacement?
			if (null == type)
			{
				// No, we can safely drop the load instruction and can rewire the operands.
				/*if (1 == load.Destination.Definitions.Count && 1 == load.Destination.Uses.Count)
				{
					load.Destination.Replace(load.Source);
					Remove(ctx);
				}
				return;
			}
			else
			{
				Replace(ctx, Architecture.CreateInstruction(type, load.Destination, load.Source));
			}*/
		}

		/// <summary>
		/// Replaces the IL store instruction by an appropriate IR move instruction.
		/// </summary>
		/// <param name="ctx">Provides the transformation context.</param>
		private void ProcessStoreInstruction(Context ctx)
		{
			// Do we need to truncate the value?
			if (IsTruncating(ctx.Result, ctx.Operand1)) {
				// Replace it with a truncating move...
				// FIXME: Implement proper truncation as specified in the CIL spec
				//Debug.Assert(false);
				if (IsSignExtending(ctx.Operand1))
					ctx.SetInstruction(IR.Instruction.SignExtendedMoveInstruction, ctx.Result, ctx.Operand1);
				else
					ctx.SetInstruction(IR.Instruction.ZeroExtendedMoveInstruction, ctx.Result, ctx.Operand1);
				return;
			}
			// FIXME PG - below seems like an optimization; ignoring it for now
			//if (ctx.Operand1.Definitions.Count == 1 && ctx.Operand1.Uses.Count == 1) {
			//    //ctx.Operand1.Replace(ctx.Result); // FIXME PG
			//    ctx.Remove();
			//    return;
			//}

			ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, ctx.Operand1);
		}


		/// <summary>
		/// Gets the condition code for an opcode.
		/// </summary>
		/// <param name="opCode">The opcode.</param>
		/// <returns>The IR condition code.</returns>
		private IR.ConditionCode GetConditionCode(CIL.OpCode opCode)
		{
			IR.ConditionCode result;
			switch (opCode) {
				case OpCode.Ceq: result = IR.ConditionCode.Equal; break;
				case OpCode.Cgt: result = IR.ConditionCode.GreaterThan; break;
				case OpCode.Cgt_un: result = IR.ConditionCode.UnsignedGreaterThan; break;
				case OpCode.Clt: result = IR.ConditionCode.LessThan; break;
				case OpCode.Clt_un: result = IR.ConditionCode.UnsignedLessThan; break;

				default:
					throw new NotSupportedException();
			}
			return result;
		}

		/// <summary>
		/// Replaces the instruction with an internal call.
		/// </summary>
		/// <param name="ctx">The transformation context.</param>
		/// <param name="internalCallTarget">The internal call target.</param>
		private void ReplaceWithInternalCall(Context ctx, object internalCallTarget)
		{
			RuntimeType rt = RuntimeBase.Instance.TypeLoader.GetType(@"Mosa.Runtime.RuntimeBase");
			RuntimeMethod callTarget = FindMethod(rt, internalCallTarget.ToString());

			//ctx.ReplaceInstructionOnly(CIL.Instruction.Get(OpCode.Call));
			ctx.ReplaceInstructionOnly(IR.Instruction.CallInstruction);
			ctx.InvokeTarget = callTarget;
		}

		/// <summary>
		/// Finds the method.
		/// </summary>
		/// <param name="rt">The rt.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private RuntimeMethod FindMethod(RuntimeType rt, string name)
		{
			//name[0] = Char.ToUpper(name[0]);
			foreach (RuntimeMethod method in rt.Methods) {
				if (name == method.Name) {
					return method;
				}
			}

			throw new MissingMethodException(rt.Name, name);
		}

		#endregion // Internals
	}
}
