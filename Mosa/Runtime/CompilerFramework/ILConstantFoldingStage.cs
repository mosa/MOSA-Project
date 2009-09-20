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

using Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Performs IR constant folding of arithmetic instructions to optimize
    /// the code down to fewer calculations.
    /// </summary>
	public sealed class ILConstantFoldingStage : CILStage, IMethodCompilerStage
    {

		#region IMethodCompilerStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name
		{
			get { return @"IL Constant Folding"; }
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

			foreach (BasicBlock block in blockProvider.Blocks) {
				Context ctx = new Context(block);
				for (ctx.Index = 0; ctx.Index < block.Instructions.Count; ctx.Index++) {
					block.Instructions[ctx.Index].Visit(this, ctx);
				}
			}
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<IL.CilToIrTransformationStage>(this);
		}
		#endregion

		#region Methods

		/// <summary>
        /// Folds multiplication with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void Mul(IL.MulInstruction instruction, Context ctx)
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
        void Div(IL.DivInstruction instruction, Context ctx)
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
        void Rem(IL.RemInstruction instruction, Context ctx)
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

        /// <summary>
        /// Folds additions with 2 constants
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="ctx">The context.</param>
        void Add(IL.AddInstruction instruction, Context ctx)
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
        void Sub(IL.SubInstruction instruction, Context ctx)
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

		#endregion // Methods
	}
}
