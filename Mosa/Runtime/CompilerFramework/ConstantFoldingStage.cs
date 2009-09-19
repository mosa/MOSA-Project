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
    public sealed class ConstantFoldingStage : CodeTransformationStage, IMethodCompilerStage, IL.IILVisitor<Context>, IR.IIRVisitor<Context>, IInstructionVisitor<Context>
    {
        /// <summary>
        /// Folds multiplication with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void IL.IILVisitor<Context>.Mul(IL.MulInstruction instruction, Context ctx)
        {
            if (instruction.First is ConstantOperand && instruction.Second is ConstantOperand)
            {
                int result = 0;
                float fresult = 0.0f; ;
                double dresult = 0.0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.U2;
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
        void IL.IILVisitor<Context>.Div(IL.DivInstruction instruction, Context ctx)
        {
            if (instruction.First is ConstantOperand && instruction.Second is ConstantOperand)
            {
                int result = 0;
                float fresult = 0.0f; ;
                double dresult = 0.0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.U2;
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

		/// <summary>
		/// Folds the remainder of 2 constants
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="ctx">The CTX.</param>
        void IL.IILVisitor<Context>.Rem(IL.RemInstruction instruction, Context ctx)
        {
            if (instruction.First is ConstantOperand && instruction.Second is ConstantOperand)
            {
                int result = 0;
                float fresult = 0.0f; ;
                double dresult = 0.0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.U2;
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

        void IInstructionVisitor<Context>.Visit(Instruction instruction, Context ctx)
        {
        }

        /// <summary>
        /// Folds additions with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void IL.IILVisitor<Context>.Add(IL.AddInstruction instruction, Context ctx)
        {
            if (instruction.First is ConstantOperand && instruction.Second is ConstantOperand)
            {
                int result = 0;
                float fresult = 0.0f; ;
                double dresult = 0.0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.U2;
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
        void IL.IILVisitor<Context>.Sub(IL.SubInstruction instruction, Context ctx)
        {
            if (instruction.First is ConstantOperand && instruction.Second is ConstantOperand)
            {
                int result = 0;
                float fresult = 0.0f; ;
                double dresult = 0.0;
                switch (instruction.Results[0].Type.Type)
                {
                    case Mosa.Runtime.Metadata.CilElementType.Char:
                        goto case Mosa.Runtime.Metadata.CilElementType.U2;
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


        #region Non-Folding
        void IL.IILVisitor<Context>.Nop(IL.NopInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Break(IL.BreakInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldarg(IL.LdargInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldarga(IL.LdargaInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldloc(IL.LdlocInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldloca(IL.LdlocaInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldc(IL.LdcInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldobj(IL.LdobjInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldstr(IL.LdstrInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldfld(IL.LdfldInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldflda(IL.LdfldaInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldsfld(IL.LdsfldInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldsflda(IL.LdsfldaInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldftn(IL.LdftnInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldvirtftn(IL.LdvirtftnInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldtoken(IL.LdtokenInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Stloc(IL.StlocInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Starg(IL.StargInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Stobj(IL.StobjInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Stfld(IL.StfldInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Stsfld(IL.StsfldInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Dup(IL.DupInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Pop(IL.PopInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Jmp(IL.JumpInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Call(IL.CallInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Calli(IL.CalliInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ret(IL.ReturnInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Branch(IL.BranchInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.UnaryBranch(IL.UnaryBranchInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.BinaryBranch(IL.BinaryBranchInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Switch(IL.SwitchInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.BinaryLogic(IL.BinaryLogicInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Shift(IL.ShiftInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Neg(IL.NegInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Not(IL.NotInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Conversion(IL.ConversionInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Callvirt(IL.CallvirtInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Cpobj(IL.CpobjInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Newobj(IL.NewobjInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Castclass(IL.CastclassInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Isinst(IL.IsInstInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Unbox(IL.UnboxInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Throw(IL.ThrowInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Box(IL.BoxInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Newarr(IL.NewarrInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldlen(IL.LdlenInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldelema(IL.LdelemaInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldelem(IL.LdelemInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Stelem(IL.StelemInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.UnboxAny(IL.UnboxAnyInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Refanyval(IL.RefanyvalInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.UnaryArithmetic(IL.UnaryArithmeticInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Mkrefany(IL.MkrefanyInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.ArithmeticOverflow(IL.ArithmeticOverflowInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Endfinally(IL.EndfinallyInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Leave(IL.LeaveInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Arglist(IL.ArglistInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.BinaryComparison(IL.BinaryComparisonInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Localalloc(IL.LocalallocInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Endfilter(IL.EndfilterInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.InitObj(IL.InitObjInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Cpblk(IL.CpblkInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Initblk(IL.InitblkInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Prefix(IL.PrefixInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Rethrow(IL.RethrowInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Sizeof(IL.SizeofInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Refanytype(IL.RefanytypeInstruction instruction, Context ctx)
        {
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
