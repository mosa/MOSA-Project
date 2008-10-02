/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Platforms.x86.Instructions;
using Mosa.Platforms.x86.Instructions.Intrinsics;


namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Transforms 64-bit arithmetic to 32-bit operations.
    /// </summary>
    /// <remarks>
    /// This stage translates all 64-bit operations to appropriate 32-bit operations on
    /// architectures without appropriate 64-bit integral operations.
    /// </remarks>
    public sealed class LongOperandTransformationStage : 
        CodeTransformationStage,
        IR.IIRVisitor<CodeTransformationStage.Context>,
        IL.IILVisitor<CodeTransformationStage.Context>
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LongOperandTransformationStage"/> class.
        /// </summary>
        public LongOperandTransformationStage()
        {
        }

        #endregion // Construction

        #region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public sealed override string Name
        {
            get { return @"LongArithmeticTransformationStage"; }
        }

        #endregion // IMethodCompilerStage Members

        #region Utility Methods

        /// <summary>
        /// Expands the add instruction for 64-bit operands.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandAdd(Context ctx, IL.AddInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the sub instruction for 64-bit operands.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandSub(Context ctx, IL.SubInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the mul instruction for 64-bit operands.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandMul(Context ctx, IL.MulInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the div.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandDiv(Context ctx, IL.DivInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the rem.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandRem(Context ctx, IL.RemInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the arithmetic shift right instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandArithmeticShiftRight(Context ctx, IR.ArithmeticShiftRightInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the shift left instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandShiftLeft(Context ctx, IR.ShiftLeftInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the shift right instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandShiftRight(Context ctx, IR.ShiftRightInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the neg instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandNeg(Context ctx, IL.NegInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the not instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandNot(Context ctx, IR.LogicalNotInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the and instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandAnd(Context ctx, IR.LogicalAndInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the or instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandOr(Context ctx, IR.LogicalOrInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the neg instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandXor(Context ctx, IR.LogicalXorInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the neg instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandMove(Context ctx, IR.MoveInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the neg instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandUnsignedMove(Context ctx, IR.ZeroExtendedMoveInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the neg instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandSignedMove(Context ctx, IR.SignExtendedMoveInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the neg instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandStore(Context ctx, IR.StoreInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the pop instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandPop(Context ctx, IR.PopInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the push instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandPush(Context ctx, IR.PushInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the unary branch instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandUnaryBranch(Context ctx, IL.UnaryBranchInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the binary branch instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandBinaryBranch(Context ctx, IL.BinaryBranchInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the binary comparison instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandBinaryComparison(Context ctx, IL.BinaryComparisonInstruction instruction)
        {
            throw new NotSupportedException();
        }

        #endregion // Utility Methods

        #region IIRVisitor<Context> Members

        void IR.IIRVisitor<Context>.Visit(IR.AddressOfInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.ArithmeticShiftRightInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandArithmeticShiftRight(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.EpilogueInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.FloatingPointToIntegerConversionInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.IntegerToFloatingPointConversionInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.LiteralInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.LoadInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalAndInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandAnd(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalOrInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandOr(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalXorInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandXor(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalNotInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandNot(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.MoveInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandMove(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.PhiInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.PopInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandPop(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.PrologueInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.PushInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandPush(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.ReturnInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.ShiftLeftInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandShiftLeft(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.ShiftRightInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandShiftRight(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.SignExtendedMoveInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandSignedMove(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.StoreInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandStore(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.ZeroExtendedMoveInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandUnsignedMove(arg, instruction);
        }

        #endregion // IIRVisitor<Context> Members
    
        #region IILVisitor<Context> Members

        void IL.IILVisitor<Context>.Nop(IL.NopInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Break(IL.BreakInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldarg(IL.LdargInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldarga(IL.LdargaInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldloc(IL.LdlocInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldloca(IL.LdlocaInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldc(IL.LdcInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldobj(IL.LdobjInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldstr(IL.LdstrInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldfld(IL.LdfldInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldflda(IL.LdfldaInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldsfld(IL.LdsfldInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldsflda(IL.LdsfldaInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldftn(IL.LdftnInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldvirtftn(IL.LdvirtftnInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldtoken(IL.LdtokenInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Stloc(IL.StlocInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Starg(IL.StargInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Stobj(IL.StobjInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Stfld(IL.StfldInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Stsfld(IL.StsfldInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Dup(IL.DupInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Pop(IL.PopInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Jmp(IL.JumpInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Call(IL.CallInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Calli(IL.CalliInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ret(IL.ReturnInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Branch(IL.BranchInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.UnaryBranch(IL.UnaryBranchInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandUnaryBranch(arg, instruction);
        }

        void IL.IILVisitor<Context>.BinaryBranch(IL.BinaryBranchInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandBinaryBranch(arg, instruction);
        }

        void IL.IILVisitor<Context>.Switch(IL.SwitchInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.BinaryLogic(IL.BinaryLogicInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Shift(IL.ShiftInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Neg(IL.NegInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandNeg(arg, instruction);
        }

        void IL.IILVisitor<Context>.Not(IL.NotInstruction instruction, Context arg)
        {
            throw new NotSupportedException();
        }

        void IL.IILVisitor<Context>.Conversion(IL.ConversionInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Callvirt(IL.CallvirtInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Cpobj(IL.CpobjInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Newobj(IL.NewobjInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Castclass(IL.CastclassInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Isinst(IL.IsInstInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Unbox(IL.UnboxInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Throw(IL.ThrowInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Box(IL.BoxInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Newarr(IL.NewarrInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldlen(IL.LdlenInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldelema(IL.LdelemaInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldelem(IL.LdelemInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Stelem(IL.StelemInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.UnboxAny(IL.UnboxAnyInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Refanyval(IL.RefanyvalInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.UnaryArithmetic(IL.UnaryArithmeticInstruction instruction, Context arg)
        {
            throw new NotSupportedException();
        }

        void IL.IILVisitor<Context>.Mkrefany(IL.MkrefanyInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.ArithmeticOverflow(IL.ArithmeticOverflowInstruction instruction, Context arg)
        {
            throw new NotSupportedException();
        }

        void IL.IILVisitor<Context>.Endfinally(IL.EndfinallyInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Leave(IL.LeaveInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Arglist(IL.ArglistInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.BinaryComparison(IL.BinaryComparisonInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandBinaryComparison(arg, instruction);
        }

        void IL.IILVisitor<Context>.Localalloc(IL.LocalallocInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Endfilter(IL.EndfilterInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.InitObj(IL.InitObjInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Cpblk(IL.CpblkInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Initblk(IL.InitblkInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Prefix(IL.PrefixInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Rethrow(IL.RethrowInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Sizeof(IL.SizeofInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Refanytype(IL.RefanytypeInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Add(IL.AddInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
            {
                ExpandAdd(arg, instruction);
            }
        }

        void IL.IILVisitor<Context>.Sub(IL.SubInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
            {
                ExpandSub(arg, instruction);
            }
        }

        void IL.IILVisitor<Context>.Mul(IL.MulInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
            {
                ExpandMul(arg, instruction);
            }
        }

        void IL.IILVisitor<Context>.Div(IL.DivInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
            {
                ExpandDiv(arg, instruction);
            }
        }

        void IL.IILVisitor<Context>.Rem(IL.RemInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
            {
                ExpandRem(arg, instruction);
            }
        }

        #endregion // IILVisitor<Context> Members
    }
}
