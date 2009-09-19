/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// Transforms CIL instructions into their appropriate IR.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms CIL instructions into their equivalent IR sequences.
	/// </remarks>
	public sealed class CilToIrTransformationStage :
		CodeTransformationStage,
		CILVisitor<Context>
	{
		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name
		{
			get { return @"CilToIrTransformationStage"; }
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
		}

		#endregion // IMethodCompilerStage Members

		#region CILVisitor<Context> Members

		void CILVisitor<Context>.Nop(NopInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Break(BreakInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Ldarg(LdargInstruction instruction, Context ctx)
		{
			ProcessLoadInstruction(instruction, ctx);
		}

		void CILVisitor<Context>.Ldarga(LdargaInstruction instruction, Context ctx)
		{
			Replace(ctx, new AddressOfInstruction(ctx.Instructions.instructions[ctx.Index].Result, ctx.Instructions.instructions[ctx.Index].Operand1));
		}

		void CILVisitor<Context>.Ldloc(LdlocInstruction instruction, Context ctx)
		{
			ProcessLoadInstruction(instruction, ctx);
		}

		void CILVisitor<Context>.Ldloca(LdlocaInstruction instruction, Context ctx)
		{
			Replace(ctx, new AddressOfInstruction(ctx.Result, ctx.Operand1));
		}

		void CILVisitor<Context>.Ldc(LdcInstruction instruction, Context ctx)
		{
			ProcessLoadInstruction(instruction, ctx);
		}

		void CILVisitor<Context>.Ldobj(LdobjInstruction instruction, Context ctx)
		{
			// This is actually ldind.* and ldobj - the opcodes have the same meanings
			Replace(ctx, new IR.LoadInstruction(ctx.Result, ctx.Operand1));
		}

		void CILVisitor<Context>.Ldstr(LdstrInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Ldfld(LdfldInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Ldflda(LdfldaInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Ldsfld(LdsfldInstruction instruction, Context ctx)
		{
			Replace(ctx, new MoveInstruction(ctx.Result, new MemberOperand(ctx.RuntimeField)));
		}

		void CILVisitor<Context>.Ldsflda(LdsfldaInstruction instruction, Context ctx)
		{
			Replace(ctx, new AddressOfInstruction(ctx.Result, ctx.Operand1));
		}

		void CILVisitor<Context>.Ldftn(LdftnInstruction instruction, Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.GetFunctionPtr);
		}

		void CILVisitor<Context>.Ldvirtftn(LdvirtftnInstruction instruction, Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.GetVirtualFunctionPtr);
		}

		void CILVisitor<Context>.Ldtoken(LdtokenInstruction instruction, Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.GetHandleForToken);
		}

		void CILVisitor<Context>.Stloc(StlocInstruction instruction, Context ctx)
		{
			ProcessStoreInstruction(instruction, ctx);
		}

		void CILVisitor<Context>.Starg(StargInstruction instruction, Context ctx)
		{
			ProcessStoreInstruction(instruction, ctx);
		}

		void CILVisitor<Context>.Stobj(StobjInstruction instruction, Context ctx)
		{
			// This is actually stind.* and stobj - the opcodes have the same meanings
			Replace(ctx, new Mosa.Runtime.CompilerFramework.IR.StoreInstruction(ctx.Operand1, ctx.Operand2));
		}

		void CILVisitor<Context>.Stfld(StfldInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Stsfld(StsfldInstruction instruction, Context ctx)
		{
			Replace(ctx, new MoveInstruction(new MemberOperand(ctx.RuntimeField), ctx.Operand1));
		}

		void CILVisitor<Context>.Dup(DupInstruction instruction, Context ctx)
		{
			// We don't need the dup anymore.
			Remove(ctx);
		}

		void CILVisitor<Context>.Pop(PopInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Jmp(JumpInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Call(CallInstruction instruction, Context ctx)
		{
			ProcessRedirectableInvokeInstruction(instruction, ctx);
		}

		void CILVisitor<Context>.Calli(CalliInstruction instruction, Context ctx)
		{
			ProcessInvokeInstruction(instruction, ctx);
		}

		void CILVisitor<Context>.Ret(ReturnInstruction instruction, Context ctx)
		{
			if (ctx.OperandCount == 1)
				Replace(ctx, new IR.ReturnInstruction(ctx.Operand1));
			else
				Replace(ctx, new IR.ReturnInstruction());
		}

		void CILVisitor<Context>.Branch(BranchInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.UnaryBranch(UnaryBranchInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.BinaryBranch(BinaryBranchInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Switch(SwitchInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.BinaryLogic(BinaryLogicInstruction instruction, Context ctx)
		{
			Type type;
			switch (instruction.OpCode) {
				case OpCode.And:
					type = typeof(LogicalAndInstruction);
					break;

				case OpCode.Or:
					type = typeof(LogicalOrInstruction);
					break;

				case OpCode.Xor:
					type = typeof(LogicalXorInstruction);
					break;

				case OpCode.Div_un:
					type = typeof(UDivInstruction);
					break;

				case OpCode.Rem_un:
					type = typeof(URemInstruction);
					break;

				default:
					throw new NotSupportedException();
			}

			Instruction result = Architecture.CreateInstruction(type, ctx.Result, ctx.Operand1, ctx.Operand2);
			Replace(ctx, result);
		}

		void CILVisitor<Context>.Shift(ShiftInstruction instruction, Context ctx)
		{
			Type replType;
			switch (instruction.OpCode) {
				case OpCode.Shl:
					replType = typeof(ShiftLeftInstruction);
					break;

				case OpCode.Shr:
					replType = typeof(ArithmeticShiftRightInstruction);
					break;

				case OpCode.Shr_un:
					replType = typeof(ShiftRightInstruction);
					break;

				default:
					throw new NotSupportedException();
			}

			Instruction result = Architecture.CreateInstruction(replType, ctx.Result, ctx.Operand1, ctx.Operand2);
			Replace(ctx, result);
		}

		void CILVisitor<Context>.Neg(NegInstruction instruction, Context ctx)
		{
			Replace(ctx, new[] {
                Architecture.CreateInstruction(typeof(SubInstruction), OpCode.Sub, ctx.Result, ctx.Operand1) 
            });
		}

		void CILVisitor<Context>.Not(NotInstruction instruction, Context ctx)
		{
			Replace(ctx, Architecture.CreateInstruction(typeof(LogicalNotInstruction), ctx.Result, ctx.Operand1));
		}

		void CILVisitor<Context>.Conversion(ConversionInstruction instruction, Context ctx)
		{
			ProcessConversionInstruction(instruction, ctx);
		}

		void CILVisitor<Context>.Callvirt(CallvirtInstruction instruction, Context ctx)
		{
			ProcessInvokeInstruction(instruction, ctx);
		}

		void CILVisitor<Context>.Cpobj(CpobjInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Newobj(NewobjInstruction instruction, Context ctx)
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

		void CILVisitor<Context>.Castclass(CastclassInstruction instruction, Context ctx)
		{
			// We don't need to check the result, if the icall fails, it'll happily throw
			// the InvalidCastException.
			ReplaceWithInternalCall(ctx, VmCall.Castclass);
		}

		void CILVisitor<Context>.Isinst(IsInstInstruction instruction, Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.IsInstanceOfType);
		}

		void CILVisitor<Context>.Unbox(UnboxInstruction instruction, Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Unbox);
		}

		void CILVisitor<Context>.Throw(ThrowInstruction instruction, Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Throw);
		}

		void CILVisitor<Context>.Box(BoxInstruction instruction, Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Box);
		}

		void CILVisitor<Context>.Newarr(NewarrInstruction instruction, Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Allocate);
		}

		void CILVisitor<Context>.Ldlen(LdlenInstruction instruction, Context ctx)
		{
			// FIXME
		}

		void CILVisitor<Context>.Ldelema(LdelemaInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Ldelem(LdelemInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Stelem(StelemInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.UnboxAny(UnboxAnyInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Refanyval(RefanyvalInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.UnaryArithmetic(UnaryArithmeticInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Mkrefany(MkrefanyInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.ArithmeticOverflow(ArithmeticOverflowInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Endfinally(EndfinallyInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Leave(LeaveInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Arglist(ArglistInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.BinaryComparison(BinaryComparisonInstruction instruction, Context ctx)
		{
			ConditionCode code = GetConditionCode(instruction.OpCode);
			Operand op1 = ctx.Operand1;
			if (op1.StackType == StackTypeCode.F) {
				Replace(ctx, new FloatingPointCompareInstruction(ctx.Result, op1, code, ctx.Operand1));
			}
			else {
				Replace(ctx, new IntegerCompareInstruction(ctx.Result, op1, code, ctx.Operand1));
			}
		}

		void CILVisitor<Context>.Localalloc(LocalallocInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Endfilter(EndfilterInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.InitObj(InitObjInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Cpblk(CpblkInstruction instruction, Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Memcpy);
		}

		void CILVisitor<Context>.Initblk(InitblkInstruction instruction, Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Memset);
		}

		void CILVisitor<Context>.Prefix(PrefixInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Rethrow(RethrowInstruction instruction, Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Rethrow);
		}

		void CILVisitor<Context>.Sizeof(SizeofInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Refanytype(RefanytypeInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Add(AddInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Sub(SubInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Mul(MulInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Div(DivInstruction instruction, Context ctx)
		{
		}

		void CILVisitor<Context>.Rem(RemInstruction instruction, Context ctx)
		{
		}

		#endregion // CILVisitor<Context> Members

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
			if (cet == CilElementType.I1 || cet == CilElementType.I2) {
				result = true;
			}
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

		private static readonly Type[][] s_convTable = new Type[13][] {
            /* I1 */ new Type[13] { 
                /* I1 */ typeof(IR.MoveInstruction),
                /* I2 */ typeof(IR.LogicalAndInstruction),
                /* I4 */ typeof(IR.LogicalAndInstruction),
                /* I8 */ typeof(IR.LogicalAndInstruction),
                /* U1 */ typeof(IR.MoveInstruction),
                /* U2 */ typeof(IR.LogicalAndInstruction),
                /* U4 */ typeof(IR.LogicalAndInstruction),
                /* U8 */ typeof(IR.LogicalAndInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* I2 */ new Type[13] { 
                /* I1 */ typeof(IR.SignExtendedMoveInstruction),
                /* I2 */ typeof(IR.MoveInstruction),
                /* I4 */ typeof(IR.LogicalAndInstruction),
                /* I8 */ typeof(IR.LogicalAndInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.MoveInstruction),
                /* U4 */ typeof(IR.LogicalAndInstruction),
                /* U8 */ typeof(IR.LogicalAndInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* I4 */ new Type[13] { 
                /* I1 */ typeof(IR.SignExtendedMoveInstruction),
                /* I2 */ typeof(IR.SignExtendedMoveInstruction),
                /* I4 */ typeof(IR.MoveInstruction),
                /* I8 */ typeof(IR.LogicalAndInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.MoveInstruction),
                /* U8 */ typeof(IR.LogicalAndInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* I8 */ new Type[13] {
                /* I1 */ typeof(IR.SignExtendedMoveInstruction),
                /* I2 */ typeof(IR.SignExtendedMoveInstruction),
                /* I4 */ typeof(IR.SignExtendedMoveInstruction),
                /* I8 */ typeof(IR.MoveInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U8 */ typeof(IR.MoveInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* U1 */ new Type[13] {
                /* I1 */ typeof(IR.MoveInstruction),
                /* I2 */ typeof(IR.LogicalAndInstruction),
                /* I4 */ typeof(IR.LogicalAndInstruction),
                /* I8 */ typeof(IR.LogicalAndInstruction),
                /* U1 */ typeof(IR.MoveInstruction),
                /* U2 */ typeof(IR.LogicalAndInstruction),
                /* U4 */ typeof(IR.LogicalAndInstruction),
                /* U8 */ typeof(IR.LogicalAndInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* U2 */ new Type[13] {
                /* I1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I2 */ typeof(IR.MoveInstruction),
                /* I4 */ typeof(IR.LogicalAndInstruction),
                /* I8 */ typeof(IR.LogicalAndInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.MoveInstruction),
                /* U4 */ typeof(IR.LogicalAndInstruction),
                /* U8 */ typeof(IR.LogicalAndInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* U4 */ new Type[13] {
                /* I1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I4 */ typeof(IR.MoveInstruction),
                /* I8 */ typeof(IR.LogicalAndInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.MoveInstruction),
                /* U8 */ typeof(IR.LogicalAndInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* U8 */ new Type[13] {
                /* I1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I8 */ typeof(IR.MoveInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U8 */ typeof(IR.MoveInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.LogicalAndInstruction),
                /* U  */ typeof(IR.LogicalAndInstruction),
                /* Ptr*/ typeof(IR.LogicalAndInstruction),
            },
            /* R4 */ new Type[13] {
                /* I1 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* I2 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* I4 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* I8 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U1 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U2 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U4 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U8 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* R4 */ typeof(IR.MoveInstruction),
                /* R8 */ typeof(IR.MoveInstruction),
                /* I  */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U  */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* Ptr*/ null,
            },
            /* R8 */ new Type[13] {
                /* I1 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* I2 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* I4 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* I8 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U1 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U2 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U4 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U8 */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* R4 */ typeof(IR.MoveInstruction),
                /* R8 */ typeof(IR.MoveInstruction),
                /* I  */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* U  */ typeof(IR.IntegerToFloatingPointConversionInstruction),
                /* Ptr*/ null,
            },
            /* I  */ new Type[13] {
                /* I1 */ typeof(IR.SignExtendedMoveInstruction),
                /* I2 */ typeof(IR.SignExtendedMoveInstruction),
                /* I4 */ typeof(IR.SignExtendedMoveInstruction),
                /* I8 */ typeof(IR.SignExtendedMoveInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U8 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.MoveInstruction),
                /* U  */ typeof(IR.MoveInstruction),
                /* Ptr*/ typeof(IR.MoveInstruction),
            },
            /* U  */ new Type[13] {
                /* I1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I8 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U8 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* R4 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* R8 */ typeof(IR.FloatingPointToIntegerConversionInstruction),
                /* I  */ typeof(IR.MoveInstruction),
                /* U  */ typeof(IR.MoveInstruction),
                /* Ptr*/ typeof(IR.MoveInstruction),
            },
            /* Ptr*/ new Type[13] {
                /* I1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* I8 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U1 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U2 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U4 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* U8 */ typeof(IR.ZeroExtendedMoveInstruction),
                /* R4 */ null,
                /* R8 */ null,
                /* I  */ typeof(IR.MoveInstruction),
                /* U  */ typeof(IR.MoveInstruction),
                /* Ptr*/ typeof(IR.MoveInstruction),
            },
        };

		/// <summary>
		/// Selects the appropriate IR conversion instruction.
		/// </summary>
		/// <param name="instruction">The IL conversion instruction.</param>
		/// <param name="ctx">The transformation context.</param>
		private void ProcessConversionInstruction(ConversionInstruction instruction, Context ctx)
		{
			Operand dest = ctx.Result;
			Operand src = ctx.Operand1;

			CheckAndConvertInstruction(ctx, dest, src);
		}

		private void CheckAndConvertInstruction(Context context, Operand destinationOperand, Operand sourceOperand)
		{
			uint mask = 0;
			Type extension = null;
			ConvType ctDest = ConvTypeFromCilType(destinationOperand.Type.Type);
			ConvType ctSrc = ConvTypeFromCilType(sourceOperand.Type.Type);

			Type type = s_convTable[(int)ctDest][(int)ctSrc];
			if (null == type)
				throw new NotSupportedException();

			ComputeExtensionTypeAndMask(ctDest, ref extension, ref mask);

			if (type == typeof(LogicalAndInstruction) || 0 != mask) {
				Debug.Assert(0 != mask, @"Conversion is an AND, but no mask given.");

				List<Instruction> instructions = new List<Instruction>();
				if (type != typeof(LogicalAndInstruction)) {
					ProcessMixedTypeConversion(instructions, type, mask, destinationOperand, sourceOperand);
				}
				else {
					ProcessSingleTypeTruncation(instructions, type, mask, destinationOperand, sourceOperand);
				}

				ExtendAndTruncateResult(instructions, extension, destinationOperand);

				Replace(context, instructions);
			}
			else {
				Replace(context, Architecture.CreateInstruction(type, destinationOperand, sourceOperand));
			}
		}

		private void ComputeExtensionTypeAndMask(ConvType destinationType, ref Type extensionType, ref uint mask)
		{
			switch (destinationType) {
				case ConvType.I1:
					mask = 0xFF;
					extensionType = typeof(SignExtendedMoveInstruction);
					break;

				case ConvType.I2:
					mask = 0xFFFF;
					extensionType = typeof(SignExtendedMoveInstruction);
					break;

				case ConvType.I4:
					mask = 0xFFFFFFFF;
					break;

				case ConvType.I8:
					break;

				case ConvType.U1:
					mask = 0xFF;
					extensionType = typeof(ZeroExtendedMoveInstruction);
					break;

				case ConvType.U2:
					mask = 0xFFFF;
					extensionType = typeof(ZeroExtendedMoveInstruction);
					break;

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
		}

		private void ProcessMixedTypeConversion(List<Instruction> instructionList, Type type, uint mask, Operand destinationOperand, Operand sourceOperand)
		{
			instructionList.AddRange(new[] {
                Architecture.CreateInstruction(type, destinationOperand, sourceOperand),
                Architecture.CreateInstruction(typeof(LogicalAndInstruction), destinationOperand, destinationOperand, new ConstantOperand(new SigType(CilElementType.U4), mask))
            });
		}

		private void ProcessSingleTypeTruncation(List<Instruction> instructionList, Type type, uint mask, Operand destinationOperand, Operand sourceOperand)
		{
			if (sourceOperand.Type.Type == CilElementType.I8 || sourceOperand.Type.Type == CilElementType.U8) {
				instructionList.Add(Architecture.CreateInstruction(typeof(IR.MoveInstruction), destinationOperand, sourceOperand));
				instructionList.Add(Architecture.CreateInstruction(type, destinationOperand, sourceOperand, new ConstantOperand(new SigType(CilElementType.U4), mask)));
			}
			else
				instructionList.Add(Architecture.CreateInstruction(type, destinationOperand, sourceOperand, new ConstantOperand(new SigType(CilElementType.U4), mask)));
		}

		private void ExtendAndTruncateResult(List<Instruction> instructionList, Type extensionType, Operand destinationOperand)
		{
			if (null != extensionType && destinationOperand is RegisterOperand) {
				RegisterOperand resultOperand = new RegisterOperand(new SigType(CilElementType.I4), ((RegisterOperand)destinationOperand).Register);
				instructionList.Add(Architecture.CreateInstruction(extensionType, resultOperand, destinationOperand));
			}
		}

		/// <summary>
		/// Processes redirectable method call instructions.
		/// </summary>
		/// <param name="instruction">The call instruction.</param>
		/// <param name="ctx">The transformation context.</param>
		/// <remarks>
		/// This method checks if the call target has an Intrinsic-Attribute applied with
		/// the current architecture. If it has, the method call is replaced by the specified
		/// native instruction.
		/// </remarks>
		private void ProcessRedirectableInvokeInstruction(CallInstruction instruction, Context ctx)
		{
			// Retrieve the invocation target
			RuntimeMethod rm = ctx.InvokeTarget;
			Debug.Assert(null != rm, @"Call doesn't have a target.");
			// Retrieve the runtime type
			RuntimeType rt = RuntimeBase.Instance.TypeLoader.GetType(@"Mosa.Runtime.CompilerFramework.IntrinsicAttribute, Mosa.Runtime");
			// The replacement instruction
			Instruction replacement = null;

			if (rm.IsDefined(rt)) {
				// FIXME: Change this to a GetCustomAttributes call, once we can do that :)
				foreach (RuntimeAttribute ra in rm.CustomAttributes) {
					if (ra.Type == rt) {
						// Get the intrinsic attribute
						IntrinsicAttribute ia = (IntrinsicAttribute)ra.GetAttribute();
						if (ia.Architecture.IsInstanceOfType(Architecture)) {
							// Found a replacement for the call...
							try {
								Operand[] args = new Operand[ctx.ResultCount + ctx.OperandCount];
								int idx = 0;

								if (ctx.ResultCount > 0)
									args[idx++] = ctx.Result;
								else if (ctx.ResultCount > 1)
									args[idx++] = ctx.Result2;

								if (ctx.OperandCount > 0)
									args[idx++] = ctx.Operand1;
								else if (ctx.OperandCount > 1)
									args[idx++] = ctx.Operand2;
								else if (ctx.OperandCount > 2)
									args[idx++] = ctx.Operand3;

								replacement = (Instruction)Activator.CreateInstance(ia.InstructionType, args, null);
								break;
							}
							catch (Exception e) {
								Trace.WriteLine("Failed to replace intrinsic call with its instruction:");
								Trace.WriteLine(e);
							}
						}
					}
				}
			}

			if (replacement == null)
				ProcessInvokeInstruction(instruction, ctx);
			else
				Replace(ctx, replacement);
		}

		/// <summary>
		/// Processes a method call instruction.
		/// </summary>
		/// <param name="instruction">The call instruction.</param>
		/// <param name="ctx">The transformation context.</param>
		private void ProcessInvokeInstruction(InvokeInstruction instruction, Context ctx)
		{
			/* FIXME: Check for IntrinsicAttribute and replace the invoke instruction with the intrinsic,
			 * if necessary.
			 */
		}

		/// <summary>
		/// Replaces the IL load instruction by an appropriate IR move instruction or removes it entirely, if
		/// it is a native size.
		/// </summary>
		/// <param name="load">The IL load instruction to process.</param>
		/// <param name="ctx">Provides the transformation context.</param>
		private void ProcessLoadInstruction(LoadInstruction load, Context ctx)
		{
			// We don't need to rewire the source/destination yet, its already there. :(
			Remove(ctx);

			/* FIXME: This is only valid with reg alloc!
						Type type = null;

						load = load as LoadInstruction;

						// Is this a sign or zero-extending move?
						if (true == IsSignExtending(load.Source))
						{
							type = typeof(IR.SignExtendedMoveInstruction);
						}
						else if (true == IsZeroExtending(load.Source))
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
		/// <param name="store">The IL store instruction to replace.</param>
		/// <param name="ctx">Provides the transformation context.</param>
		private void ProcessStoreInstruction(StoreInstruction store, Context ctx)
		{
			// Do we need to truncate the value?
			if (IsTruncating(ctx.Result, ctx.Operand1)) {
				// Replace it with a truncating move...
				// FIXME: Implement proper truncation as specified in the CIL spec
				//Debug.Assert(false);
				if (IsSignExtending(ctx.Operand1))
					Replace(ctx, Architecture.CreateInstruction(typeof(SignExtendedMoveInstruction), ctx.Result, ctx.Operand1));
				else
					Replace(ctx, Architecture.CreateInstruction(typeof(ZeroExtendedMoveInstruction), ctx.Result, ctx.Operand1));
				return;
			}
			if (1 == ctx.Operand1.Definitions.Count && 1 == ctx.Operand1.Uses.Count) {
				ctx.Operand1.Replace(ctx.Result);
				Remove(ctx);
				return;
			}

			Replace(ctx, Architecture.CreateInstruction(typeof(MoveInstruction), ctx.Result, ctx.Operand1));
		}


		/// <summary>
		/// Gets the condition code for an opcode.
		/// </summary>
		/// <param name="opCode">The opcode.</param>
		/// <returns>The IR condition code.</returns>
		private ConditionCode GetConditionCode(OpCode opCode)
		{
			ConditionCode result;
			switch (opCode) {
				case OpCode.Ceq: result = ConditionCode.Equal; break;
				case OpCode.Cgt: result = ConditionCode.GreaterThan; break;
				case OpCode.Cgt_un: result = ConditionCode.UnsignedGreaterThan; break;
				case OpCode.Clt: result = ConditionCode.LessThan; break;
				case OpCode.Clt_un: result = ConditionCode.UnsignedLessThan; break;

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

			// Transform the opcode with an internal call
			CallInstruction call = new CallInstruction(OpCode.Call);
			call.SetInvokeTarget(ref ctx.Instructions.instructions[ctx.Index], Compiler, callTarget);

			// FIXME PG
			// int i = 0;

			//if (ctx.ResultCount > 0)
			//    call.SetOperand(i++, ctx.Result);
			//else if (ctx.ResultCount > 1)
			//    call.SetOperand(i++, ctx.Result2);

			//i = 0;

			//if (ctx.OperandCount > 0)
			//    call.SetResult(i++, ctx.Operand1);
			//else if (ctx.OperandCount > 1)
			//    call.SetResult(i++, ctx.Operand2);
			//else if (ctx.OperandCount > 2)
			//    call.SetResult(i++, ctx.Operand3);

			// Replace(ctx, call);
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
