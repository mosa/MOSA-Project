/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:simon_wollwage@yahoo.co.jp>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Performs IR constant folding of arithmetic instructions to optimize
    /// the code down to fewer calculations.
    /// </summary>
    public sealed class IRConstantFoldingStage : CodeTransformationStage, IMethodCompilerStage, IR.IIRVisitor<Context>, IInstructionVisitor<Context>
    {
       
        void IInstructionVisitor<Context>.Visit(Instruction instruction, Context ctx)
        {
        }

        /// <summary>
        /// Folds logical ANDs with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void IR.IIRVisitor<Context>.Visit(IR.LogicalAndInstruction instruction, Context ctx)
        {
            if (instruction.Operand1 is ConstantOperand && instruction.Operand2 is ConstantOperand)
            {
                int result = 0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Metadata.CilElementType.Char:
                        goto case Metadata.CilElementType.U2;
                    case Metadata.CilElementType.U1:
                        result = ((byte)(instruction.Operand1 as ConstantOperand).Value) & ((byte)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Metadata.CilElementType.U2:
                        result = ((ushort)(instruction.Operand1 as ConstantOperand).Value) & ((ushort)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Metadata.CilElementType.U4:
                        result = (int)(((uint)(instruction.Operand1 as ConstantOperand).Value) & ((uint)(instruction.Operand2 as ConstantOperand).Value));
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I1:
                        result = ((sbyte)(instruction.Operand1 as ConstantOperand).Value) & ((sbyte)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I2:
                        result = ((short)(instruction.Operand1 as ConstantOperand).Value) & ((short)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I4:
                        result = ((int)(instruction.Operand1 as ConstantOperand).Value) & ((int)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                    case Mosa.Runtime.Metadata.CilElementType.U:
                        goto case Mosa.Runtime.Metadata.CilElementType.U4;
                    default:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                }
                Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, result)));
            }
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalNotInstruction instruction, Context ctx)
        {
            
        }

        /// <summary>
        /// Folds logical ORs with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void IR.IIRVisitor<Context>.Visit(IR.LogicalOrInstruction instruction, Context ctx)
        {
            if (instruction.Operand1 is ConstantOperand && instruction.Operand2 is ConstantOperand)
            {
                int result = 0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.U2;
                    case Mosa.Runtime.Metadata.CilElementType.U1:
                        result = ((byte)(instruction.Operand1 as ConstantOperand).Value) | ((byte)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U2:
                        result = ((ushort)(instruction.Operand1 as ConstantOperand).Value) | ((ushort)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U4:
                        result = (int)(((uint)(instruction.Operand1 as ConstantOperand).Value) | ((uint)(instruction.Operand2 as ConstantOperand).Value));
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I1:
                        result = (sbyte)(((uint)(sbyte)(instruction.Operand1 as ConstantOperand).Value) | ((uint)(sbyte)(instruction.Operand2 as ConstantOperand).Value));
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I2:
                        result = ((short)(instruction.Operand1 as ConstantOperand).Value) | ((short)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I4:
                        result = ((int)(instruction.Operand1 as ConstantOperand).Value) | ((int)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                    case Mosa.Runtime.Metadata.CilElementType.U:
                        goto case Mosa.Runtime.Metadata.CilElementType.U4;
                    default:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                }
                Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, result)));
            }
        }

        /// <summary>
        /// Folds logical XORs with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void IR.IIRVisitor<Context>.Visit(IR.LogicalXorInstruction instruction, Context ctx)
        {
            if (instruction.Operand1 is ConstantOperand && instruction.Operand2 is ConstantOperand)
            {
                int result = 0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.U2;
                    case Mosa.Runtime.Metadata.CilElementType.U1:
                        result = ((byte)(instruction.Operand1 as ConstantOperand).Value) ^ ((byte)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U2:
                        result = ((ushort)(instruction.Operand1 as ConstantOperand).Value) ^ ((ushort)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U4:
                        result = (int)(((uint)(instruction.Operand1 as ConstantOperand).Value) ^ ((uint)(instruction.Operand2 as ConstantOperand).Value));
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I1:
                        result = ((sbyte)(instruction.Operand1 as ConstantOperand).Value) ^ ((sbyte)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I2:
                        result = ((short)(instruction.Operand1 as ConstantOperand).Value) ^ ((short)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I4:
                        result = ((int)(instruction.Operand1 as ConstantOperand).Value) ^ ((int)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                    case Mosa.Runtime.Metadata.CilElementType.U:
                        goto case Mosa.Runtime.Metadata.CilElementType.U4;
                    default:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                }
                Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, result)));
            }
        }

        #region IMethodCompilerStage

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public override string Name
        {
            get { return @"Constant Folding"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        void IMethodCompilerStage.Run(IMethodCompiler compiler)
        {
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null == blockProvider)
                throw new InvalidOperationException(@"Instruction stream must have been split to basic Blocks.");


			foreach (BasicBlock block in blockProvider.Blocks)
            {
				Context ctx = new Context(block);
                for (ctx.Index = 0; ctx.Index < block.Instructions.Count; ctx.Index++)
                {
                    block.Instructions[ctx.Index].Visit(this, ctx);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="pipeline"></param>
        public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertBefore<IL.CilToIrTransformationStage>(this);
        }
        #endregion

        #region IR
        void IR.IIRVisitor<Context>.Visit(IR.AddressOfInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.ArithmeticShiftRightInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.BranchInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.CallInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.EpilogueInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.FloatingPointCompareInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.FloatingPointToIntegerConversionInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.IntegerCompareInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.IntegerToFloatingPointConversionInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.JmpInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.LiteralInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.LoadInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.PhiInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.PopInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.PrologueInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.PushInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.MoveInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.ReturnInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.ShiftLeftInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.ShiftRightInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.SignExtendedMoveInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.StoreInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.UDivInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.URemInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.ZeroExtendedMoveInstruction instruction, Context ctx)
        {
        }
        #endregion

    }
}
