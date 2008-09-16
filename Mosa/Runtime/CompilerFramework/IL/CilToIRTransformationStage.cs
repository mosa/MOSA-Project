using System;
using System.Collections.Generic;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Transforms CIL instructions into their appropriate IR.
    /// </summary>
    /// <remarks>
    /// This transformation stage transforms CIL instructions into their equivalent IR sequences.
    /// </remarks>
    public sealed class CilToIrTransformationStage : 
        IMethodCompilerStage,
        IInstructionVisitor<CilToIrTransformationStage.Context>,
        IILVisitor<CilToIrTransformationStage.Context>
    {
        #region Types

        /// <summary>
        /// Provides context to the visitor functions.
        /// </summary>
        class Context
        {
            /// <summary>
            /// Holds the block being operated on.
            /// </summary>
            private BasicBlock _block;

            /// <summary>
            /// Holds the instruction index operated on.
            /// </summary>
            private int _index;

            /// <summary>
            /// Gets or sets the basic block currently processed.
            /// </summary>
            public BasicBlock Block
            {
                get { return _block; }
                set { _block = value; }
            }

            /// <summary>
            /// Gets or sets the instruction index currently processed.
            /// </summary>
            public int Index
            {
                get { return _index; }
                set { _index = value; }
            }
        };

        #endregion // Types

        #region Data members

        /// <summary>
        /// The architecture of the compilation process.
        /// </summary>
        private IArchitecture _architecture;

        /// <summary>
        /// Holds the executing method compiler.
        /// </summary>
        private MethodCompilerBase _compiler;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="CilToIrTransformationStage"/>.
        /// </summary>
        public CilToIrTransformationStage()
        {
        }

        #endregion // Construction

        #region IMethodCompilerStage Members

        string IMethodCompilerStage.Name
        {
            get { return @"CilToIrTransformationStage"; }
        }

        void IMethodCompilerStage.Run(MethodCompilerBase compiler)
        {
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null == blockProvider)
                throw new InvalidOperationException(@"Instruction stream must have been split to basic blocks.");

            // Save the architecture & compiler
            _architecture = compiler.Architecture;
            _compiler = compiler;

            Context ctx = new Context();
            foreach (BasicBlock block in blockProvider)
            {
                ctx.Block = block;
                for (ctx.Index = 0; ctx.Index < block.Instructions.Count; ctx.Index++)
                {
                    Instruction inst = block.Instructions[ctx.Index];
                    inst.Visit(this, ctx);
                }
            }
        }

        #endregion // IMethodCompilerStage Members

        #region IInstructionVisitor<Context> Members

        void IInstructionVisitor<Context>.Visit(Instruction instruction, Context arg)
        {
            Trace.WriteLine(String.Format(@"Unknown instruction {0} in CilToIRTransformationStage.", instruction.GetType().FullName));
        }

        #endregion // IInstructionVisitor<Context> Members

        #region IILVisitor<Context> Members

        void IILVisitor<Context>.Nop(NopInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Break(BreakInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldarg(LdargInstruction instruction, Context ctx)
        {
            ProcessLoadInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Ldarga(LdargaInstruction instruction, Context ctx)
        {
            Replace(ctx, new IR.AddressOfInstruction(instruction.Results[0], instruction.Operands[0]));
        }

        void IILVisitor<Context>.Ldloc(LdlocInstruction instruction, Context ctx)
        {
            ProcessLoadInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Ldloca(LdlocaInstruction instruction, Context ctx)
        {
            Replace(ctx, new IR.AddressOfInstruction(instruction.Results[0], instruction.Operands[0]));
        }

        void IILVisitor<Context>.Ldc(LdcInstruction instruction, Context ctx)
        {
            ProcessLoadInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Ldobj(LdobjInstruction instruction, Context ctx)
        {
            // This is actually ldind.* and ldobj - the opcodes have the same meanings
            Replace(ctx, new IR.LoadInstruction(instruction.Results[0], instruction.Operands[0]));
        }

        void IILVisitor<Context>.Ldstr(LdstrInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldfld(LdfldInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldflda(LdfldaInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldsfld(LdsfldInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldsflda(LdsfldaInstruction instruction, Context ctx)
        {
            Replace(ctx, new IR.AddressOfInstruction(instruction.Results[0], instruction.Operands[0]));
        }

        void IILVisitor<Context>.Ldftn(LdftnInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldvirtftn(LdvirtftnInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldtoken(LdtokenInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Stloc(StlocInstruction instruction, Context ctx)
        {
            ProcessStoreInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Starg(StargInstruction instruction, Context ctx)
        {
            ProcessStoreInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Stobj(StobjInstruction instruction, Context ctx)
        {
            // This is actually stind.* and stobj - the opcodes have the same meanings
            Replace(ctx, new IR.StoreInstruction(instruction.Operands[0], instruction.Operands[1]));
        }

        void IILVisitor<Context>.Stfld(StfldInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Stsfld(StsfldInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Dup(DupInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Pop(PopInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Jmp(JumpInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Call(CallInstruction instruction, Context ctx)
        {
            ProcessRedirectableInvokeInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Calli(CalliInstruction instruction, Context ctx)
        {
            ProcessInvokeInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Ret(ReturnInstruction instruction, Context ctx)
        {
            // Do we have an operand to return?
            if (0 != instruction.Operands.Length)
            {
                Debug.Assert(1 == instruction.Operands.Length, @"Can't return more than one operand.");
                ICallingConvention cc = _architecture.GetCallingConvention(_compiler.Method.Signature.CallingConvention);
                List<Instruction> instructions = new List<Instruction>();
                Instruction[] resmove = cc.MoveReturnValue(_architecture, instruction.Operands[0]);
                if (null != resmove)
                    instructions.AddRange(resmove);
                instructions.Add(_architecture.CreateInstruction(typeof(BranchInstruction), OpCode.Br, new int[] { Int32.MaxValue }));
                Replace(ctx, instructions);
            }
            else
            {
                // HACK: Should really use the calling convention here
                // A ret jumps to the epilogue to leave
                Instruction result = _architecture.CreateInstruction(typeof(BranchInstruction), OpCode.Br, new int[] { Int32.MaxValue });
                Replace(ctx, result);
            }
        }

        void IILVisitor<Context>.Branch(BranchInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.UnaryBranch(UnaryBranchInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.BinaryBranch(BinaryBranchInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Switch(SwitchInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.BinaryLogic(BinaryLogicInstruction instruction, Context ctx)
        {
            Type type;
            switch (instruction.Code)
            {
                case OpCode.And:
                    type = typeof(IR.LogicalAndInstruction);
                    break;

                case OpCode.Or:
                    type = typeof(IR.LogicalOrInstruction);
                    break;

                case OpCode.Xor:
                    type = typeof(IR.LogicalXorInstruction);
                    break;

                default:
                    throw new NotSupportedException();
            }

            Instruction result = _architecture.CreateInstruction(type, instruction.Results[0], instruction.First, instruction.Second);
            Replace(ctx, result);
        }

        void IILVisitor<Context>.Shift(ShiftInstruction instruction, Context ctx)
        {
            Type replType;
            switch (instruction.Code)
            {
                case OpCode.Shl:
                    replType = typeof(IR.ShiftLeftInstruction);
                    break;

                case OpCode.Shr:
                    replType = typeof(IR.ArithmeticShiftRightInstruction);
                    break;

                case OpCode.Shr_un:
                    replType = typeof(IR.ShiftRightInstruction);
                    break;

                default:
                    throw new NotSupportedException();
            }

            Instruction result = _architecture.CreateInstruction(replType, instruction.Results[0], instruction.First, instruction.Second);
            Replace(ctx, result);
        }

        void IILVisitor<Context>.Neg(NegInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Not(NotInstruction instruction, Context ctx)
        {
            Replace(ctx, _architecture.CreateInstruction(typeof(IR.LogicalNotInstruction), instruction.Results[0], instruction.Operands[0]));
        }

        void IILVisitor<Context>.Conversion(ConversionInstruction instruction, Context ctx)
        {
            ProcessConversionInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Callvirt(CallvirtInstruction instruction, Context ctx)
        {
            ProcessInvokeInstruction(instruction, ctx);
        }

        void IILVisitor<Context>.Cpobj(CpobjInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Newobj(NewobjInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Castclass(CastclassInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Isinst(IsInstInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Unbox(UnboxInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Throw(ThrowInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Box(BoxInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Newarr(NewarrInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldlen(LdlenInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldelema(LdelemaInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Ldelem(LdelemInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Stelem(StelemInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.UnboxAny(UnboxAnyInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Refanyval(RefanyvalInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.UnaryArithmetic(UnaryArithmeticInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Mkrefany(MkrefanyInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.ArithmeticOverflow(ArithmeticOverflowInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Endfinally(EndfinallyInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Leave(LeaveInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Arglist(ArglistInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.BinaryComparison(BinaryComparisonInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Localalloc(LocalallocInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Endfilter(EndfilterInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.InitObj(InitObjInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Cpblk(CpblkInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Initblk(InitblkInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Prefix(PrefixInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Rethrow(RethrowInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Sizeof(SizeofInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Refanytype(RefanytypeInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Add(AddInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Sub(SubInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Mul(MulInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Div(DivInstruction instruction, Context ctx)
        {
        }

        void IILVisitor<Context>.Rem(RemInstruction instruction, Context ctx)
        {
        }

        #endregion // IILVisitor<Context> Members

        #region Internals

        /// <summary>
        /// Determines if a store is silently truncating the value.
        /// </summary>
        /// <param name="dest">The destination operand.</param>
        /// <param name="source">The source operand.</param>
        /// <returns>True if the store is truncating, otherwise false.</returns>
        private bool IsTruncating(Operand dest, Operand source)
        {
            return false;
        }

        /// <summary>
        /// Determines if the load should sign extend the given source operand.
        /// </summary>
        /// <param name="source">The source operand to determine sign extension for.</param>
        /// <returns>True if the given operand should be loaded with its sign extended.</returns>
        private bool IsSignExtending(Operand source)
        {
            bool result = false;
            CilElementType cet = source.Type.Type;
            if (cet == CilElementType.I1 || cet == CilElementType.I2)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Determines if the load should zero extend the given source operand.
        /// </summary>
        /// <param name="source">The source operand to determine zero extension for.</param>
        /// <returns>True if the given operand should be loaded zero extended.</returns>
        private bool IsZeroExtending(Operand source)
        {
            bool result = false;
            CilElementType cet = source.Type.Type;
            if (cet == CilElementType.U1 || cet == CilElementType.U2)
            {
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

            switch (sigType.Type)
            {
                case CilElementType.I1: goto case CilElementType.I2;
                case CilElementType.I2:
                    result = new SigType(CilElementType.I4);
                    break;

                case CilElementType.U1: goto case CilElementType.U2;
                case CilElementType.U2:
                    result = new SigType(CilElementType.U4);
                    break;

                default:
                    result = sigType;
                    break;
            }

            return result;
        }

        private static readonly Type[][] s_convTable = new Type[][] {
            /* Unknown */ new Type[] { null, null, null, null, null, null, null },
            /* Int32 */   new Type[] { null, typeof(IR.LogicalAndInstruction), null, null, typeof(IR.FloatingPointToIntegerConversionInstruction), null, null },
            /* Int64 */   new Type[] { null, null, null, null, null, null, null },
            /* Native  */ new Type[] { null, null, null, null, null, null, null },
            /* F */       new Type[] { null, typeof(IR.IntegerToFloatingPointConversionInstruction), null, null, null, null, null },
            /* Ptr */     new Type[] { null, null, null, null, null, null, null },
            /* Object */  new Type[] { null, null, null, null, null, null, null },
        };

        /// <summary>
        /// Selects the appropriate IR conversion instruction.
        /// </summary>
        /// <param name="instruction">The IL conversion instruction.</param>
        /// <param name="ctx">The transformation context.</param>
        private void ProcessConversionInstruction(ConversionInstruction instruction, Context ctx)
        {
            Operand dest = instruction.Results[0];
            Operand src = instruction.Operands[0];
            int mask = 0;

            Type type = s_convTable[(int)dest.StackType][(int)src.StackType];
            if (null == type)
                throw new NotSupportedException();

            switch (dest.Type.Type)
            {
                case CilElementType.I1:
                    mask = 0xFF;
                    break;

                case CilElementType.I2:
                    mask = 0xFFFF;
                    break;

                case CilElementType.I4:
                    type = typeof(MoveInstruction);
                    break;

                case CilElementType.U1:
                    mask = 0xFF;
                    break;

                case CilElementType.U2:
                    mask = 0xFFFF;
                    break;
                
                case CilElementType.U4:
                    type = typeof(MoveInstruction);
                    break;

                case CilElementType.R4:
                    break;

                case CilElementType.R8:
                    break;

                default:
                    Debug.Assert(false);
                    throw new NotSupportedException();
            }

            if (0 != mask)
            {
                // We need to AND the result after conversion
                Replace(ctx, new Instruction[] {
                    _architecture.CreateInstruction(type, instruction.Results[0], instruction.Operands[0]),
                    _architecture.CreateInstruction(typeof(LogicalAndInstruction), instruction.Results[0], instruction.Results[0], new ConstantOperand(new SigType(CilElementType.I4), mask))
                });
            }
            else
            {
                Replace(ctx, _architecture.CreateInstruction(type, instruction.Results[0], instruction.Operands[0]));
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
            RuntimeMethod rm = instruction.InvokeTarget;
            Debug.Assert(null != rm, @"Call doesn't have a target.");
            // Retrieve the runtime type
            RuntimeType rt = RuntimeBase.Instance.TypeLoader.GetType(@"Mosa.Runtime.CompilerFramework.IntrinsicAttribute, Mosa.Runtime");
            // The replacement instruction
            Instruction replacement = null;

            if (true == rm.IsDefined(rt))
            {
                foreach (RuntimeAttribute ra in rm.CustomAttributes)
                {
                    if (ra.Type == rt)
                    {

                    }
                }
            }

            if (null == replacement)
            {
                ProcessInvokeInstruction(instruction, ctx);
            }
            else
            {
                Replace(ctx, replacement);
            }
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
        private void ProcessLoadInstruction(IL.ILoadInstruction load, Context ctx)
        {
            // We don't need to rewire the source/destination yet, its already there. :(
            Remove(ctx);

/* FIXME: This is only valid with reg alloc!
            Type type = null;

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
                if (1 == load.Destination.Definitions.Count && 1 == load.Destination.Uses.Count)
                {
                    load.Destination.Replace(load.Source);
                    Remove(ctx);
                }
                return;
            }
            else
            {
                Replace(ctx, _architecture.CreateInstruction(type, load.Destination, load.Source));
            }
*/
        }

        /// <summary>
        /// Replaces the IL store instruction by an appropriate IR move instruction.
        /// </summary>
        /// <param name="store">The IL store instruction to replace.</param>
        /// <param name="ctx">Provides the transformation context.</param>
        private void ProcessStoreInstruction(IStoreInstruction store, Context ctx)
        {
            // Do we need to truncate the value?
            if (true == IsTruncating(store.Destination, store.Source))
            {
                // Replace it with a truncating move...
                // FIXME: Implement proper truncation as specified in the CIL spec
                Debug.Assert(false);
            }
            else if (1 == store.Source.Definitions.Count && 1 == store.Source.Uses.Count)
            {
                store.Source.Replace(store.Destination);
                Remove(ctx);
                return;
            }

            Replace(ctx, _architecture.CreateInstruction(typeof(IR.MoveInstruction), store.Destination, store.Source));
        }

        /// <summary>
        /// Removes the current instruction from the instruction stream.
        /// </summary>
        /// <param name="ctx">The context of the instruction to remove.</param>
        private void Remove(Context ctx)
        {
            Remove(ctx.Block.Instructions[ctx.Index]);
            ctx.Block.Instructions.RemoveAt(ctx.Index--);
        }

        /// <summary>
        /// Removes the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction to remove.</param>
        private void Remove(Instruction instruction)
        {
            // FIXME: Remove this sequence, once we have a smart instruction collection which does this.
            for (int i = 0; i < instruction.Operands.Length; i++)
                instruction.SetOperand(i, null);
            for (int i = 0; i < instruction.Results.Length; i++)
                instruction.SetResult(i, null);
        }

        /// <summary>
        /// Replaces the currently processed instruction with another instruction.
        /// </summary>
        /// <param name="ctx">The transformation context.</param>
        /// <param name="instruction">The instruction to replace with.</param>
        private void Replace(Context ctx, Instruction instruction)
        {
            Instruction old = ctx.Block.Instructions[ctx.Index];
            ctx.Block.Instructions[ctx.Index] = instruction;
            Remove(old);
        }

        /// <summary>
        /// Replaces the currently processed instruction with a set of instruction.
        /// </summary>
        /// <param name="ctx">The transformation context.</param>
        /// <param name="instructions">The instructions to replace with.</param>
        private void Replace(Context ctx, IEnumerable<Instruction> instructions)
        {
            Remove(ctx);
            int count = ctx.Block.Instructions.Count;
            ctx.Block.Instructions.InsertRange(++ctx.Index, instructions);
            ctx.Index += (ctx.Block.Instructions.Count - count);
        }

        #endregion // Internals
    }
}
