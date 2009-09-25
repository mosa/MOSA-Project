/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;
using Mosa.Runtime.CompilerFramework.IR2;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// Transforms CIL instructions into their appropriate IR.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms CIL instructions into their equivalent IR sequences.
	/// </remarks>
	public sealed class CilToIrTransformationStage : CILStage
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

		#region Members

		/// <summary>
		/// Ldargs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Ldarg(Context ctx)
		{
			ProcessLoadInstruction(ctx);
		}

		/// <summary>
		/// Ldargas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Ldarga(Context ctx)
		{
			//ctx.SetInstruction(IR2.Instruction.AddressOfInstruction(ctx.InstructionSet.Data[ctx.Index].Result, ctx.InstructionSet.Data[ctx.Index].Operand1));
			ctx.SetInstruction(IR2.Instruction.AddressOfInstruction, ctx.Result, ctx.Operand1);
		}

		/// <summary>
		/// Ldlocs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Ldloc(Context ctx)
		{
			ProcessLoadInstruction(ctx);
		}

		/// <summary>
		/// Ldlocas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Ldloca(Context ctx)
		{
			//ctx.SetInstruction(IR2.Instruction.AddressOfInstruction(ctx.Result, ctx.Operand1));
			ctx.SetInstruction(IR2.Instruction.AddressOfInstruction, ctx.Result, ctx.Operand1);
		}

		/// <summary>
		/// LDCs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Ldc(Context ctx)
		{
			ProcessLoadInstruction(ctx);
		}

		/// <summary>
		/// Ldobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Ldobj(Context ctx)
		{
			// This is actually ldind.* and ldobj - the opcodes have the same meanings
			ctx.SetInstruction(IR2.Instruction.LoadInstruction, ctx.Result, ctx.Operand1);
		}

		/// <summary>
		/// LDSFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Ldsfld(Context ctx)
		{
			ctx.SetInstruction(IR2.Instruction.MoveInstruction, ctx.Result, new MemberOperand(ctx.RuntimeField));
		}

		/// <summary>
		/// Ldsfldas the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Ldsflda(Context ctx)
		{
			ctx.SetInstruction(IR2.Instruction.AddressOfInstruction, ctx.Result, ctx.Operand1);
		}

		/// <summary>
		/// LDFTNs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Ldftn(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.GetFunctionPtr);
		}

		/// <summary>
		/// Ldvirtftns the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Ldvirtftn(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.GetVirtualFunctionPtr);
		}

		/// <summary>
		/// Ldtokens the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Ldtoken(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.GetHandleForToken);
		}

		/// <summary>
		/// Stlocs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Stloc(Context ctx)
		{
			ProcessStoreInstruction(ctx);
		}

		/// <summary>
		/// Stargs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Starg(Context ctx)
		{
			ProcessStoreInstruction(ctx);
		}

		/// <summary>
		/// Stobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Stobj(Context ctx)
		{
			// This is actually stind.* and stobj - the opcodes have the same meanings
			ctx.SetInstruction(IR2.Instruction.StoreInstruction, ctx.Operand1, ctx.Operand2);
		}

		/// <summary>
		/// STSFLDs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Stsfld(Context ctx)
		{
			//			Replace(ctx, new MoveInstruction(new MemberOperand(ctx.RuntimeField), ctx.Operand1));
			ctx.SetInstruction(IR2.Instruction.MoveInstruction, new MemberOperand(ctx.RuntimeField), ctx.Operand1);
		}

		/// <summary>
		/// Dups the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Dup(Context ctx)
		{
			// We don't need the dup anymore.
			Remove(ctx);
		}

		/// <summary>
		/// Calls the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Call(Context ctx)
		{
			ProcessRedirectableInvokeInstruction(ctx);
		}

		/// <summary>
		/// Callis the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Calli(Context ctx)
		{
			ProcessInvokeInstruction(ctx);
		}

		/// <summary>
		/// Rets the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Ret(Context ctx)
		{
			if (ctx.OperandCount == 1)
				ctx.SetInstruction(IR2.Instruction.ReturnInstruction, ctx.Operand1);
			else
				ctx.SetInstruction(IR2.Instruction.ReturnInstruction);
		}

		/// <summary>
		/// Binaries the logic.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void BinaryLogic(Context ctx)
		{
			Type type;
			switch ((ctx.Instruction as BaseInstruction).OpCode) {
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

			LegacyInstruction result = Architecture.CreateInstruction(type, ctx.Result, ctx.Operand1, ctx.Operand2);
			Replace(ctx, result);
		}

		/// <summary>
		/// Shifts the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Shift(Context ctx)
		{
			Type replType;
			switch ((ctx.Instruction as BaseInstruction).OpCode) {
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

			LegacyInstruction result = Architecture.CreateInstruction(replType, ctx.Result, ctx.Operand1, ctx.Operand2);
			Replace(ctx, result);
		}

		/// <summary>
		/// Negs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Neg(Context ctx)
		{
			Replace(ctx, new[] {
                Architecture.CreateInstruction(typeof(SubInstruction), OpCode.Sub, ctx.Result, ctx.Operand1) 
            });
		}

		/// <summary>
		/// Nots the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Not(Context ctx)
		{
			Replace(ctx, Architecture.CreateInstruction(typeof(LogicalNotInstruction), ctx.Result, ctx.Operand1));
		}

		/// <summary>
		/// Conversions the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Conversion(Context ctx)
		{
			ProcessConversionInstruction(ctx);
		}

		/// <summary>
		/// Callvirts the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Callvirt(Context ctx)
		{
			ProcessInvokeInstruction(ctx);
		}

		/// <summary>
		/// Newobjs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Newobj(Context ctx)
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
		/// Castclasses the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Castclass(Context ctx)
		{
			// We don't need to check the result, if the icall fails, it'll happily throw
			// the InvalidCastException.
			ReplaceWithInternalCall(ctx, VmCall.Castclass);
		}

		/// <summary>
		/// Isinsts the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Isinst(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.IsInstanceOfType);
		}

		/// <summary>
		/// Unboxes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Unbox(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Unbox);
		}

		/// <summary>
		/// Throws the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Throw(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Throw);
		}

		/// <summary>
		/// Boxes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Box(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Box);
		}

		/// <summary>
		/// Newarrs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Newarr(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Allocate);
		}

		/// <summary>
		/// Binaries the comparison.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void BinaryComparison(Context ctx)
		{
			ConditionCode code = GetConditionCode((ctx.Instruction as BaseInstruction).OpCode);

			if (ctx.Operand1.StackType == StackTypeCode.F)
				ctx.SetInstruction(IR2.Instruction.FloatingPointCompareInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
			else
				ctx.SetInstruction(IR2.Instruction.IntegerCompareInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);

			ctx.ConditionCode = code;
		}

		/// <summary>
		/// CPBLKs the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Cpblk(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Memcpy);
		}

		/// <summary>
		/// Initblks the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Initblk(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Memset);
		}

		/// <summary>
		/// Rethrows the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Rethrow(Context ctx)
		{
			ReplaceWithInternalCall(ctx, VmCall.Rethrow);
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
		/// <param name="ctx">The transformation context.</param>
		private void ProcessConversionInstruction(Context ctx)
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

				List<LegacyInstruction> instructions = new List<LegacyInstruction>();
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

		private void ProcessMixedTypeConversion(List<LegacyInstruction> instructionList, Type type, uint mask, Operand destinationOperand, Operand sourceOperand)
		{
			instructionList.AddRange(new[] {
                Architecture.CreateInstruction(type, destinationOperand, sourceOperand),
                Architecture.CreateInstruction(typeof(LogicalAndInstruction), destinationOperand, destinationOperand, new ConstantOperand(new SigType(CilElementType.U4), mask))
            });
		}

		private void ProcessSingleTypeTruncation(List<LegacyInstruction> instructionList, Type type, uint mask, Operand destinationOperand, Operand sourceOperand)
		{
			if (sourceOperand.Type.Type == CilElementType.I8 || sourceOperand.Type.Type == CilElementType.U8) {
				instructionList.Add(Architecture.CreateInstruction(typeof(IR.MoveInstruction), destinationOperand, sourceOperand));
				instructionList.Add(Architecture.CreateInstruction(type, destinationOperand, sourceOperand, new ConstantOperand(new SigType(CilElementType.U4), mask)));
			}
			else
				instructionList.Add(Architecture.CreateInstruction(type, destinationOperand, sourceOperand, new ConstantOperand(new SigType(CilElementType.U4), mask)));
		}

		private void ExtendAndTruncateResult(List<LegacyInstruction> instructionList, Type extensionType, Operand destinationOperand)
		{
			if (null != extensionType && destinationOperand is RegisterOperand) {
				RegisterOperand resultOperand = new RegisterOperand(new SigType(CilElementType.I4), ((RegisterOperand)destinationOperand).Register);
				instructionList.Add(Architecture.CreateInstruction(extensionType, resultOperand, destinationOperand));
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
			Debug.Assert(null != rm, @"Call doesn't have a target.");
			// Retrieve the runtime type
			RuntimeType rt = RuntimeBase.Instance.TypeLoader.GetType(@"Mosa.Runtime.CompilerFramework.IntrinsicAttribute, Mosa.Runtime");
			// The replacement instruction
			LegacyInstruction replacement = null;

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

								foreach (Operand result in ctx.Results)
									args[idx++] = result;

								foreach (Operand operand in ctx.Operands)
									args[idx++] = operand;

								replacement = (LegacyInstruction)Activator.CreateInstance(ia.InstructionType, args, null);
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
				ProcessInvokeInstruction(ctx);
			else
				Replace(ctx, replacement);
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
		/// <param name="ctx">Provides the transformation context.</param>
		private void ProcessStoreInstruction(Context ctx)
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
			call.SetInvokeTarget(ref ctx.InstructionSet.Data[ctx.Index], Compiler, callTarget);

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
