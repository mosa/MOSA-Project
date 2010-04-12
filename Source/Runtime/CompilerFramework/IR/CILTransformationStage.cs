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
using Mosa.Runtime.CompilerFramework.Operands;
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
	public sealed class CILTransformationStage : CodeTransformationStage, CIL.ICILVisitor, IPipelineStage
	{
		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"IR.CILTransformationStage"; } }

		#endregion // IMethodCompilerStage Members

		#region ICILVisitor

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldarg"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldarg(Context ctx)
		{
			ProcessLoadInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldarga"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldarga(Context ctx)
		{
			ctx.ReplaceInstructionOnly(IR.Instruction.AddressOfInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldloc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldloc(Context ctx)
		{
            if (ctx.Ignore == true)
            {
                ctx.Remove();
            }
            else
            {
                this.ProcessLoadInstruction(ctx);
            }
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldloca"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldloca(Context ctx)
		{
			ctx.ReplaceInstructionOnly(Instruction.AddressOfInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldc"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldc(Context ctx)
		{
			ProcessLoadInstruction(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldobj(Context ctx)
		{
            IInstruction loadInstruction = Instruction.LoadInstruction;
            Operand destination = ctx.Result;
		    Operand source = ctx.Operand1;
            RefSigType refSigType = (RefSigType)source.Type;

			// This is actually ldind.* and ldobj - the opcodes have the same meanings
            if (MustSignExtendOnLoad(refSigType.ElementType.Type))
            {
                loadInstruction = Instruction.SignExtendedMoveInstruction;
            }
            else if (MustZeroExtendOnLoad(refSigType.ElementType.Type))
            {
                loadInstruction = Instruction.ZeroExtendedMoveInstruction;                
            }

		    ctx.SetInstruction(loadInstruction, destination, source, ConstantOperand.FromValue(0));
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldsfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldsfld(Context ctx)
		{
		    SigType sigType = ctx.RuntimeField.SignatureType;
		    Operand source = new MemberOperand(ctx.RuntimeField);
		    Operand destination = ctx.Result;

            IInstruction loadInstruction = Instruction.MoveInstruction;
            if (MustSignExtendOnLoad(sigType.Type))
            {
                loadInstruction = Instruction.SignExtendedMoveInstruction;
            }
            else if (MustZeroExtendOnLoad(sigType.Type))
            {
                loadInstruction = Instruction.ZeroExtendedMoveInstruction;
            }

            ctx.SetInstruction(loadInstruction, destination, source);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldsflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldsflda(Context ctx)
		{
			//ctx.SetInstruction(IR.Instruction.AddressOfInstruction, ctx.Result, new MemberOperand (ctx.RuntimeField));
            //ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, ctx.Operand1);
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
		public void Stloc(Context ctx)
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
			ctx.SetInstruction(IR.Instruction.StoreInstruction, ctx.Operand1, ConstantOperand.FromValue(0), ctx.Operand2);
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
					ctx.SetInstruction(IR.Instruction.DivUInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
					break;
				case OpCode.Rem_un:
					ctx.SetInstruction(IR.Instruction.RemUInstruction, ctx.Result, ctx.Operand1, ctx.Operand2);
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
		public void Neg(Context ctx)
		{
            if (IsUnsigned(ctx.Operand1))
            {
                ConstantOperand zero = new ConstantOperand(ctx.Operand1.Type, 0UL);
                ctx.SetInstruction(Instruction.SubUInstruction, ctx.Result, zero, ctx.Operand1);
            }
            else
            {
                ConstantOperand minusOne = new ConstantOperand(ctx.Operand1.Type, -1L);
                ctx.SetInstruction(Instruction.MulSInstruction, ctx.Result, minusOne, ctx.Operand1);
            }
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
		public void Newarr(Context ctx)
		{
            SZArraySigType arrayType = (SZArraySigType)ctx.Result.Type;
            ClassSigType elementType = arrayType.ElementType as ClassSigType;
            Debug.Assert(elementType != null, @"Newarr didn't specify class signature?");

			ReplaceWithInternalCall(ctx, VmCall.AllocateArray);

            ctx.Operand3 = ctx.Operand1;
            ctx.Operand1 = new ConstantOperand(BuiltInSigType.Int32, this.MethodCompiler.Assembly.LoadOrder);
            ctx.Operand2 = new ConstantOperand(BuiltInSigType.IntPtr, elementType.Token);
            ctx.OperandCount = 3;
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.BinaryComparison"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void BinaryComparison(Context ctx)
		{
			IR.ConditionCode code = ConvertCondition((ctx.Instruction as CIL.BaseInstruction).OpCode);

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
		void ICILVisitor.Ldstr(Context ctx) 
        {
            return;
        }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldfld(Context ctx)
		{
            Operand resultOperand = ctx.Result;
            Operand objectOperand = ctx.Operand1;

		    RuntimeField field = ctx.RuntimeField;
		    IntPtr address = field.Address;
            ConstantOperand offsetOperand = new ConstantOperand(BuiltInSigType.IntPtr, address.ToInt64());

		    IInstruction loadInstruction = Instruction.LoadInstruction;
            if (MustSignExtendOnLoad(field.SignatureType.Type))
            {
                loadInstruction = Instruction.SignExtendedMoveInstruction;
            }
            else if (MustZeroExtendOnLoad(field.SignatureType.Type))
            {
                loadInstruction = Instruction.ZeroExtendedMoveInstruction;
            }

            ctx.SetInstruction(loadInstruction, resultOperand, objectOperand, offsetOperand);
        }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldflda"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldflda(Context ctx)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Stfld"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Stfld(Context ctx) 
        {
            Operand objectOperand = ctx.Operand1;
            Operand valueOperand = ctx.Operand2;

            IntPtr address = ctx.RuntimeField.Address;
            ConstantOperand offsetOperand = new ConstantOperand(BuiltInSigType.IntPtr, address.ToInt64());

            ctx.SetInstruction(Instruction.StoreInstruction, objectOperand, offsetOperand, valueOperand);
        }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Jmp"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Jmp(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Branch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Branch(Context ctx)
		{
		    ctx.ReplaceInstructionOnly(Instruction.JmpInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.UnaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void UnaryBranch(Context ctx)
		{
            IBranch branch = ctx.Branch;

            ConditionCode cc;
            Operand first = ctx.Operand1;
            Operand second = new ConstantOperand(BuiltInSigType.Int32, 0UL);

		    OpCode opcode = ((ICILInstruction)ctx.Instruction).OpCode;
            if (opcode == OpCode.Brtrue || opcode == OpCode.Brtrue_s)
            {
                cc = ConditionCode.NotEqual;
            }
            else if (opcode == OpCode.Brfalse || opcode == OpCode.Brfalse_s)
            {
                cc = ConditionCode.Equal;
            }
            else
            {
                throw new NotSupportedException(@"CILTransformationStage.UnaryBranch doesn't support CIL opcode " + opcode);
            }

            Operand comparisonResult = this.MethodCompiler.CreateTemporary(BuiltInSigType.Int32);
            ctx.SetInstruction(Instruction.IntegerCompareInstruction, comparisonResult, first, second);
            ctx.ConditionCode = cc;

            ctx.AppendInstruction(Instruction.BranchInstruction, comparisonResult);
		    ctx.ConditionCode = cc;
            ctx.SetBranch(branch.Targets[0]);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.BinaryBranch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void BinaryBranch(Context ctx)
		{
		    IBranch branch = ctx.Branch;

		    ConditionCode cc = ConvertCondition(((ICILInstruction)ctx.Instruction).OpCode);
		    Operand first = ctx.Operand1;
		    Operand second = ctx.Operand2;

		    IInstruction comparisonInstruction = Instruction.IntegerCompareInstruction;
            if (first.StackType == StackTypeCode.F)
            {
                comparisonInstruction = Instruction.FloatingPointCompareInstruction;
            }

            Operand comparisonResult = this.MethodCompiler.CreateTemporary(BuiltInSigType.Int32);
            ctx.SetInstruction(comparisonInstruction, comparisonResult, first, second);
            ctx.ConditionCode = cc;
		    
            ctx.AppendInstruction(Instruction.BranchInstruction, comparisonResult);
		    ctx.ConditionCode = cc;
            ctx.SetBranch(branch.Targets[0]);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Switch"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Switch(Context ctx)
		{
		    ctx.ReplaceInstructionOnly(Instruction.SwitchInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Cpobj"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void ICILVisitor.Cpobj(Context ctx) { }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldlen"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldlen(Context ctx) 
        {
            Operand arrayOperand = ctx.Operand1;
            Operand arrayLength = ctx.Result;
            ConstantOperand constantOffset = ConstantOperand.FromValue(8);

            Operand arrayAddress = this.MethodCompiler.CreateTemporary(new PtrSigType(BuiltInSigType.Int32));
		    ctx.SetInstruction(Instruction.MoveInstruction, arrayAddress, arrayOperand);
            ctx.AppendInstruction(IR.Instruction.LoadInstruction, arrayLength, arrayAddress, constantOffset);
        }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldelema"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldelema(Context ctx)
		{
            Operand result = ctx.Result;
            Operand arrayOperand = ctx.Operand1;
            Operand arrayIndexOperand = ctx.Operand2;

            SZArraySigType arraySigType = arrayOperand.Type as SZArraySigType;
            if (arraySigType == null)
            {
                throw new CompilationException(@"Array operation performed on non-array operand.");
            }

            Operand arrayAddress = this.LoadArrayBaseAddress(ctx, arraySigType, arrayOperand);
            Operand elementOffset = this.CalculateArrayElementOffset(ctx, arraySigType, arrayIndexOperand);
            ctx.AppendInstruction(IR.Instruction.AddSInstruction, result, arrayAddress, elementOffset);
        }

	    private Operand CalculateArrayElementOffset(Context ctx, SZArraySigType arraySignatureType, Operand arrayIndexOperand)
	    {
	        int elementSizeInBytes = 0, alignment = 0;
            this.Architecture.GetTypeRequirements(arraySignatureType.ElementType, out elementSizeInBytes, out alignment);

	        //
	        // The sequence we're emitting is:
	        //
	        //      offset = arrayIndexOperand * elementSize
	        //      temp = offset + 12
	        //      result = *(arrayOperand * temp)
	        //
	        // The array data starts at offset 12 from the array object itself. The 12 is a hardcoded assumption
	        // of x86, which might change for other platforms. We need to refactor this into some helper classes.
	        //

	        Operand elementOffset = this.MethodCompiler.CreateTemporary(BuiltInSigType.Int32);
	        Operand elementSizeOperand = new ConstantOperand(BuiltInSigType.Int32, elementSizeInBytes);
	        ctx.AppendInstruction(IR.Instruction.MulSInstruction, elementOffset, arrayIndexOperand, elementSizeOperand);

	        return elementOffset;
        }

	    private Operand LoadArrayBaseAddress(Context ctx, SZArraySigType arraySignatureType, Operand arrayOperand)
	    {
	        Operand arrayAddress = this.MethodCompiler.CreateTemporary(new PtrSigType(arraySignatureType.ElementType));
	        Operand fixedOffset = new ConstantOperand(BuiltInSigType.Int32, 12);
	        ctx.SetInstruction(Instruction.AddSInstruction, arrayAddress, arrayOperand, fixedOffset);
	        return arrayAddress;
	    }

	    /// <summary>
		/// Visitation function for <see cref="ICILVisitor.Ldelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Ldelem(Context ctx) 
        {
	        IInstruction loadInstruction = Instruction.LoadInstruction;
            Operand result = ctx.Result;
            MemoryOperand arrayOperand = (MemoryOperand)ctx.Operand1;
	        Operand arrayIndexOperand = ctx.Operand2;

            SZArraySigType arraySigType = arrayOperand.Type as SZArraySigType;
            if (arraySigType == null)
            {
                throw new CompilationException(@"Array operation performed on non-array operand.");
            }

            if (MustSignExtendOnLoad(arraySigType.ElementType.Type) == true)
            {
                loadInstruction = Instruction.SignExtendedMoveInstruction;
            }
            else if (MustZeroExtendOnLoad(arraySigType.ElementType.Type) == true)
            {
                loadInstruction = Instruction.ZeroExtendedMoveInstruction;
            }

            Operand arrayAddress = this.LoadArrayBaseAddress(ctx, arraySigType, arrayOperand);
            Operand elementOffset = this.CalculateArrayElementOffset(ctx, arraySigType, arrayIndexOperand);
            ctx.AppendInstruction(loadInstruction, result, arrayAddress, elementOffset);
        }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Stelem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Stelem(Context ctx) 
        {
            Operand arrayOperand = ctx.Operand1;
		    Operand arrayIndexOperand = ctx.Operand2;
            Operand value = ctx.Operand3;
            SZArraySigType arraySigType = arrayOperand.Type as SZArraySigType;
            if (arraySigType == null)
            {
                throw new CompilationException(@"Array operation performed on non-array operand.");
            }

		    Operand arrayAddress = this.LoadArrayBaseAddress(ctx, arraySigType, arrayOperand);
            Operand elementOffset = this.CalculateArrayElementOffset(ctx, arraySigType, arrayIndexOperand);
            ctx.AppendInstruction(Instruction.StoreInstruction, arrayAddress, elementOffset, value);
        }

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
		public void Add(Context ctx) 
        {
            Replace(ctx, Instruction.AddFInstruction, Instruction.AddSInstruction, Instruction.AddUInstruction);
        }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Sub"/>.
		/// </summary>
        /// <param name="context">The context.</param>
		public void Sub(Context context)
		{
		    Replace(context, Instruction.SubFInstruction, Instruction.SubSInstruction, Instruction.SubUInstruction);
		}
	    /// <summary>
		/// Visitation function for <see cref="ICILVisitor.Mul"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Mul(Context ctx) 
        {
            Replace(ctx, Instruction.MulFInstruction, Instruction.MulSInstruction, Instruction.MulUInstruction);
        }

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Div"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Div(Context ctx)
		{
		    Replace(ctx, Instruction.DivFInstruction, Instruction.DivSInstruction, Instruction.DivUInstruction);
		}

		/// <summary>
		/// Visitation function for <see cref="ICILVisitor.Rem"/>.
		/// </summary>
		/// <param name="ctx">The context.</param>
		public void Rem(Context ctx)
		{
		    Replace(ctx, Instruction.RemFInstruction, Instruction.RemSInstruction, Instruction.RemUInstruction);
		}

		#endregion // ICILVisitor

		#region Internals

        private static void Replace(Context context, IIRInstruction floatingPointInstruction, IIRInstruction signedInstruction, IIRInstruction unsignedInstruction)
        {
            if (IsFloatingPoint(context))
            {
                context.ReplaceInstructionOnly(floatingPointInstruction);
            }
            else if (IsUnsigned(context))
            {
                context.ReplaceInstructionOnly(unsignedInstruction);
            }
            else
            {
                context.ReplaceInstructionOnly(signedInstruction);
            }
        }

        private static bool IsUnsigned(Context context)
        {
            Operand operand = context.Result;
            return IsUnsigned(operand);
        }

	    private static bool IsUnsigned(Operand operand)
	    {
	        CilElementType type = operand.Type.Type;
	        return type == CilElementType.U
	               || type == CilElementType.U1
	               || type == CilElementType.U2
	               || type == CilElementType.U4
	               || type == CilElementType.U8;
	    }

	    private static bool IsFloatingPoint(Context context)
        {
            return context.Result.StackType == StackTypeCode.F;
        }

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
		    return MustSignExtendOnLoad(source.Type.Type);
		}

	    private static bool MustSignExtendOnLoad(CilElementType elementType)
	    {
	        return (elementType == CilElementType.I1 || elementType == CilElementType.I2);
	    }

	    /// <summary>
        /// Determines if the load should sign extend the given source operand.
        /// </summary>
        /// <param name="source">The source operand to determine sign extension for.</param>
        /// <returns>True if the given operand should be loaded with its sign extended.</returns>
        private static bool IsZeroExtending(Operand source)
        {
	        return MustZeroExtendOnLoad(source.Type.Type);
        }

	    private static bool MustZeroExtendOnLoad(CilElementType elementType)
	    {
	        return (elementType == CilElementType.U1 || elementType == CilElementType.U2 || elementType == CilElementType.Char);
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

			uint mask = 0xFFFFFFFF;
			IInstruction instruction = ComputeExtensionTypeAndMask(ctDest, ref mask);

			if (type == IR.Instruction.LogicalAndInstruction || mask != 0) {
				Debug.Assert(mask != 0, @"Conversion is an AND, but no mask given.");

				if (type != IR.Instruction.LogicalAndInstruction)
					ProcessMixedTypeConversion(ctx, type, mask, destinationOperand, sourceOperand);
				else
					ProcessSingleTypeTruncation(ctx, type, mask, destinationOperand, sourceOperand);

				ExtendAndTruncateResult(ctx, instruction, destinationOperand);
			}
			else
				ctx.SetInstruction(type, destinationOperand, sourceOperand);
		}

		private IInstruction ComputeExtensionTypeAndMask(ConvType destinationType, ref uint mask)
		{
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
			ctx.SetInstruction(instruction, destinationOperand, sourceOperand);
			ctx.AppendInstruction(IR.Instruction.LogicalAndInstruction, destinationOperand, /*sourceOperand,*/ new ConstantOperand(new SigType(CilElementType.U4), mask));
		}

		private void ProcessSingleTypeTruncation(Context ctx, IInstruction instruction, uint mask, Operand destinationOperand, Operand sourceOperand)
		{
			if (sourceOperand.Type.Type == CilElementType.I8 || sourceOperand.Type.Type == CilElementType.U8) {
				ctx.SetInstruction(IR.Instruction.MoveInstruction, destinationOperand, sourceOperand);
				ctx.AppendInstruction(instruction, destinationOperand, sourceOperand, new ConstantOperand(new SigType(CilElementType.U4), mask));
			}
			else
				ctx.SetInstruction(instruction, destinationOperand, sourceOperand, new ConstantOperand(new SigType(CilElementType.U4), mask));
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
								IIntrinsicMethod instrinsic = Architecture.GetIntrinsicMethod(ia.InstructionType);

								instrinsic.ReplaceIntrinsicCall(ctx);

								return;
							}
							catch (Exception e) {
								Trace.WriteLine("Failed to replace intrinsic call with its instruction: " + ia.InstructionType.ToString());
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
            ctx.ReplaceInstructionOnly(Instruction.CallInstruction);
		}

		/// <summary>
		/// Replaces the IL load instruction by an appropriate IR move instruction or removes it entirely, if
		/// it is a native size.
		/// </summary>
        /// <param name="context">Provides the transformation context.</param>
        private void ProcessLoadInstruction(Context context)
		{
		    IInstruction extension = null;
		    SigType extendedType = null;
            Operand source = context.Operand1;
		    Operand destination = context.Result;

            // Is this a sign or zero-extending move?
            if (source != null)
            {
                if (IsSignExtending(source))
                {
                    extension = Instruction.SignExtendedMoveInstruction;
                    extendedType = BuiltInSigType.Int32;
                }
                else if (IsZeroExtending(source))
                {
                    extension = Instruction.ZeroExtendedMoveInstruction;
                    extendedType = BuiltInSigType.UInt32;
                }
            }

		    if (extension != null)
            {
                Operand temp = this.MethodCompiler.CreateTemporary(extendedType);
                destination.Replace(temp, context.InstructionSet);
                context.SetInstruction(extension, temp, source);
            }
            else
            {
                context.ReplaceInstructionOnly(Instruction.MoveInstruction);
            }
		}

		/// <summary>
		/// Replaces the IL store instruction by an appropriate IR move instruction.
		/// </summary>
		/// <param name="ctx">Provides the transformation context.</param>
		private void ProcessStoreInstruction(Context ctx)
		{
            ctx.SetInstruction(IR.Instruction.MoveInstruction, ctx.Result, ctx.Operand1);
		}

		/// <summary>
		/// Replaces the instruction with an internal call.
		/// </summary>
		/// <param name="ctx">The transformation context.</param>
		/// <param name="internalCallTarget">The internal call target.</param>
		private void ReplaceWithInternalCall(Context ctx, VmCall internalCallTarget)
		{
			RuntimeType rt = RuntimeBase.Instance.TypeLoader.GetType(@"Mosa.Runtime.RuntimeBase");
			RuntimeMethod callTarget = FindMethod(rt, internalCallTarget.ToString());

			ctx.ReplaceInstructionOnly(IR.Instruction.CallInstruction);
			ctx.InvokeTarget = callTarget;
		}

		/// <summary>
		/// Finds the method.
		/// </summary>
		/// <param name="rt">The runtime type.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private RuntimeMethod FindMethod(RuntimeType rt, string name)
		{
			foreach (RuntimeMethod method in rt.Methods) 
				if (name == method.Name) 
					return method;

			throw new MissingMethodException(rt.Name, name);
		}

		#endregion // Internals
	}
}
