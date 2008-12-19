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
    public sealed class ConstantFoldingStage : CodeTransformationStage, IMethodCompilerStage, IL.IILVisitor<CodeTransformationStage.Context>, IR.IIRVisitor<CodeTransformationStage.Context>, IInstructionVisitor<CodeTransformationStage.Context>
    {
        /// <summary>
        /// Folds multiplication with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void IL.IILVisitor<CodeTransformationStage.Context>.Mul(IL.MulInstruction instruction, CodeTransformationStage.Context ctx)
        {
            if (instruction.First is ConstantOperand && instruction.Second is ConstantOperand)
            {
                int result = 0;
                float fresult = 0.0f; ;
                double dresult = 0.0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.I1;
                    case Mosa.Runtime.Metadata.CilElementType.U1:
                        result = ((byte)(instruction.First as ConstantOperand).Value) * ((byte)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U2:
                        result = ((ushort)(instruction.First as ConstantOperand).Value) * ((ushort)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U4:
                        result = (int)(((uint)(instruction.First as ConstantOperand).Value) * ((uint)(instruction.Second as ConstantOperand).Value));
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I1:
                        result = ((sbyte)(instruction.First as ConstantOperand).Value) * ((sbyte)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I2:
                        result = ((short)(instruction.First as ConstantOperand).Value) * ((short)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I4:
                        result = ((int)(instruction.First as ConstantOperand).Value) * ((int)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.R4:
                        fresult = ((float)(instruction.First as ConstantOperand).Value) * ((float)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.R8:
                        dresult = ((double)(instruction.First as ConstantOperand).Value) * ((double)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                    case Mosa.Runtime.Metadata.CilElementType.U:
                        goto case Mosa.Runtime.Metadata.CilElementType.U4;
                    default:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                }
                if (instruction.Results[0].Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, fresult)));
                else if (instruction.Results[0].Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, dresult)));
                else
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, result)));
            }
        }

        /// <summary>
        /// Folds divisions with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void IL.IILVisitor<CodeTransformationStage.Context>.Div(IL.DivInstruction instruction, CodeTransformationStage.Context ctx)
        {
            if (instruction.First is ConstantOperand && instruction.Second is ConstantOperand)
            {
                int result = 0;
                float fresult = 0.0f; ;
                double dresult = 0.0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.I1;
                    case Mosa.Runtime.Metadata.CilElementType.U1:
                        result = ((byte)(instruction.First as ConstantOperand).Value) / ((byte)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U2:
                        result = ((ushort)(instruction.First as ConstantOperand).Value) / ((ushort)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U4:
                        result = (int)(((uint)(instruction.First as ConstantOperand).Value) / ((uint)(instruction.Second as ConstantOperand).Value));
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I1:
                        result = ((sbyte)(instruction.First as ConstantOperand).Value) / ((sbyte)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I2:
                        result = ((short)(instruction.First as ConstantOperand).Value) / ((short)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I4:
                        result = ((int)(instruction.First as ConstantOperand).Value) / ((int)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.R4:
                        fresult = ((float)(instruction.First as ConstantOperand).Value) / ((float)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.R8:
                        dresult = ((double)(instruction.First as ConstantOperand).Value) / ((double)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                    case Mosa.Runtime.Metadata.CilElementType.U:
                        goto case Mosa.Runtime.Metadata.CilElementType.U4;
                    default:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                }
                if (instruction.Results[0].Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, fresult)));
                else if (instruction.Results[0].Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, dresult)));
                else
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, result)));
            }
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Rem(IL.RemInstruction instruction, CodeTransformationStage.Context ctx)
        {
            if (instruction.First is ConstantOperand && instruction.Second is ConstantOperand)
            {
                int result = 0;
                float fresult = 0.0f; ;
                double dresult = 0.0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.I1;
                    case Mosa.Runtime.Metadata.CilElementType.U1:
                        result = ((byte)(instruction.First as ConstantOperand).Value) % ((byte)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U2:
                        result = ((ushort)(instruction.First as ConstantOperand).Value) % ((ushort)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U4:
                        result = (int)(((uint)(instruction.First as ConstantOperand).Value) % ((uint)(instruction.Second as ConstantOperand).Value));
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I1:
                        result = ((sbyte)(instruction.First as ConstantOperand).Value) % ((sbyte)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I2:
                        result = ((short)(instruction.First as ConstantOperand).Value) % ((short)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I4:
                        result = ((int)(instruction.First as ConstantOperand).Value) % ((int)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.R4:
                        fresult = ((float)(instruction.First as ConstantOperand).Value) % ((float)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.R8:
                        dresult = ((double)(instruction.First as ConstantOperand).Value) % ((double)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                    case Mosa.Runtime.Metadata.CilElementType.U:
                        goto case Mosa.Runtime.Metadata.CilElementType.U4;
                    default:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                }
                if (instruction.Results[0].Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, fresult)));
                else if (instruction.Results[0].Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, dresult)));
                else
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, result)));
            }
        }

        void IInstructionVisitor<CodeTransformationStage.Context>.Visit(Instruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        /// <summary>
        /// Folds additions with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void IL.IILVisitor<CodeTransformationStage.Context>.Add(IL.AddInstruction instruction, CodeTransformationStage.Context ctx)
        {
            if (instruction.First is ConstantOperand && instruction.Second is ConstantOperand)
            {
                int result = 0;
                float fresult = 0.0f; ;
                double dresult = 0.0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.I1;
                    case Mosa.Runtime.Metadata.CilElementType.U1:
                        result = ((byte)(instruction.First as ConstantOperand).Value) + ((byte)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U2:
                        result = ((ushort)(instruction.First as ConstantOperand).Value) + ((ushort)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U4:
                        result = (int)(((uint)(instruction.First as ConstantOperand).Value) + ((uint)(instruction.Second as ConstantOperand).Value));
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I1:
                        result = ((sbyte)(instruction.First as ConstantOperand).Value) + ((sbyte)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I2:
                        result = ((short)(instruction.First as ConstantOperand).Value) + ((short)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I4:
                        result = ((int)(instruction.First as ConstantOperand).Value) + ((int)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.R4:
                        fresult = ((float)(instruction.First as ConstantOperand).Value) + ((float)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.R8:
                        dresult = ((double)(instruction.First as ConstantOperand).Value) + ((double)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                    case Mosa.Runtime.Metadata.CilElementType.U:
                        goto case Mosa.Runtime.Metadata.CilElementType.U4;
                    default:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                }
                if (instruction.Results[0].Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, fresult)));
                else if (instruction.Results[0].Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, dresult)));
                else
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, result)));
            }
        }

        /// <summary>
        /// Folds substractions with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void IL.IILVisitor<CodeTransformationStage.Context>.Sub(IL.SubInstruction instruction, CodeTransformationStage.Context ctx)
        {
            if (instruction.First is ConstantOperand && instruction.Second is ConstantOperand)
            {
                int result = 0;
                float fresult = 0.0f; ;
                double dresult = 0.0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.I1;
                    case Mosa.Runtime.Metadata.CilElementType.U1:
                        result = ((byte)(instruction.First as ConstantOperand).Value) - ((byte)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U2:
                        result = ((ushort)(instruction.First as ConstantOperand).Value) - ((ushort)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U4:
                        result = (int)(((uint)(instruction.First as ConstantOperand).Value) - ((uint)(instruction.Second as ConstantOperand).Value));
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I1:
                        result = ((sbyte)(instruction.First as ConstantOperand).Value) - ((sbyte)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I2:
                        result = ((short)(instruction.First as ConstantOperand).Value) - ((short)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I4:
                        result = ((int)(instruction.First as ConstantOperand).Value) - ((int)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.R4:
                        fresult = ((float)(instruction.First as ConstantOperand).Value) - ((float)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.R8:
                        dresult = ((double)(instruction.First as ConstantOperand).Value) - ((double)(instruction.Second as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.I:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                    case Mosa.Runtime.Metadata.CilElementType.U:
                        goto case Mosa.Runtime.Metadata.CilElementType.U4;
                    default:
                        goto case Mosa.Runtime.Metadata.CilElementType.I4;
                }
                if (instruction.Results[0].Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, fresult)));
                else if (instruction.Results[0].Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, dresult)));
                else
                    Replace(ctx, new IR.MoveInstruction(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, result)));
            }
        }

        /// <summary>
        /// Folds logical ANDs with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.LogicalAndInstruction instruction, Context ctx)
        {
            if (instruction.Operand1 is ConstantOperand && instruction.Operand2 is ConstantOperand)
            {
                int result = 0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.I1;
                    case Mosa.Runtime.Metadata.CilElementType.U1:
                        result = ((byte)(instruction.Operand1 as ConstantOperand).Value) & ((byte)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U2:
                        result = ((ushort)(instruction.Operand1 as ConstantOperand).Value) & ((ushort)(instruction.Operand2 as ConstantOperand).Value);
                        break;
                    case Mosa.Runtime.Metadata.CilElementType.U4:
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

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.LogicalNotInstruction instruction, Context ctx)
        {
            
        }

        /// <summary>
        /// Folds logical ORs with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.LogicalOrInstruction instruction, Context ctx)
        {
            if (instruction.Operand1 is ConstantOperand && instruction.Operand2 is ConstantOperand)
            {
                int result = 0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.I1;
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
                        result = ((sbyte)(instruction.Operand1 as ConstantOperand).Value) | ((sbyte)(instruction.Operand2 as ConstantOperand).Value);
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
        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.LogicalXorInstruction instruction, Context ctx)
        {
            if (instruction.Operand1 is ConstantOperand && instruction.Operand2 is ConstantOperand)
            {
                int result = 0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.I1;
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
                throw new InvalidOperationException(@"Instruction stream must have been split to basic blocks.");

            CodeTransformationStage.Context ctx = new CodeTransformationStage.Context();
            for (int currentBlock = 0; currentBlock < blockProvider.Blocks.Count; currentBlock++)
            {
                BasicBlock block = blockProvider.Blocks[currentBlock];
                ctx.Block = block;
                for (ctx.Index = 0; ctx.Index < block.Instructions.Count; ctx.Index++)
                {
                    block.Instructions[ctx.Index].Visit(this, ctx);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="pipeline"></param>
        new void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertBefore<IL.CilToIrTransformationStage>(this);
        }
        #endregion


        #region Non-Folding
        void IL.IILVisitor<CodeTransformationStage.Context>.Nop(IL.NopInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Break(IL.BreakInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldarg(IL.LdargInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldarga(IL.LdargaInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldloc(IL.LdlocInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldloca(IL.LdlocaInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldc(IL.LdcInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldobj(IL.LdobjInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldstr(IL.LdstrInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldfld(IL.LdfldInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldflda(IL.LdfldaInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldsfld(IL.LdsfldInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldsflda(IL.LdsfldaInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldftn(IL.LdftnInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldvirtftn(IL.LdvirtftnInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldtoken(IL.LdtokenInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Stloc(IL.StlocInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Starg(IL.StargInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Stobj(IL.StobjInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Stfld(IL.StfldInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Stsfld(IL.StsfldInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Dup(IL.DupInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Pop(IL.PopInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Jmp(IL.JumpInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Call(IL.CallInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Calli(IL.CalliInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ret(IL.ReturnInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Branch(IL.BranchInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.UnaryBranch(IL.UnaryBranchInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.BinaryBranch(IL.BinaryBranchInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Switch(IL.SwitchInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.BinaryLogic(IL.BinaryLogicInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Shift(IL.ShiftInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Neg(IL.NegInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Not(IL.NotInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Conversion(IL.ConversionInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Callvirt(IL.CallvirtInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Cpobj(IL.CpobjInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Newobj(IL.NewobjInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Castclass(IL.CastclassInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Isinst(IL.IsInstInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Unbox(IL.UnboxInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Throw(IL.ThrowInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Box(IL.BoxInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Newarr(IL.NewarrInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldlen(IL.LdlenInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldelema(IL.LdelemaInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Ldelem(IL.LdelemInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Stelem(IL.StelemInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.UnboxAny(IL.UnboxAnyInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Refanyval(IL.RefanyvalInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.UnaryArithmetic(IL.UnaryArithmeticInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Mkrefany(IL.MkrefanyInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.ArithmeticOverflow(IL.ArithmeticOverflowInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Endfinally(IL.EndfinallyInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Leave(IL.LeaveInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Arglist(IL.ArglistInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.BinaryComparison(IL.BinaryComparisonInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Localalloc(IL.LocalallocInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Endfilter(IL.EndfilterInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.InitObj(IL.InitObjInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Cpblk(IL.CpblkInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Initblk(IL.InitblkInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Prefix(IL.PrefixInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Rethrow(IL.RethrowInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Sizeof(IL.SizeofInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }

        void IL.IILVisitor<CodeTransformationStage.Context>.Refanytype(IL.RefanytypeInstruction instruction, CodeTransformationStage.Context ctx)
        {
        }
        #endregion

        #region IR
        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.AddressOfInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.ArithmeticShiftRightInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.BranchInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.CallInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.EpilogueInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.FloatingPointCompareInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.FloatingPointToIntegerConversionInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.IntegerCompareInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.IntegerToFloatingPointConversionInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.JmpInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.LiteralInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.LoadInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.PhiInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.PopInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.PrologueInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.PushInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.MoveInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.ReturnInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.ShiftLeftInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.ShiftRightInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.SignExtendedMoveInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.StoreInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.UDivInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.URemInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<CodeTransformationStage.Context>.Visit(IR.ZeroExtendedMoveInstruction instruction, Context ctx)
        {
        }
        #endregion

    }
}
