/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

using IR = Mosa.Runtime.CompilerFramework.IR;
using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SimplePeepholeOptimizationStage : BaseTransformationStage, IMethodCompilerStage, IPlatformTransformationStage, IPipelineStage, IBlockOptimizationStage
    {

        #region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        string IPipelineStage.Name { get { return @"X86.SimplePeepholeOptimizationStage"; } }

        private static PipelineStageOrder[] _pipelineOrder = new PipelineStageOrder[] {
				new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(IBlockReorderStage)),
				new PipelineStageOrder(PipelineStageOrder.Location.After, typeof(MemToMemConversionStage)),
				new PipelineStageOrder(PipelineStageOrder.Location.Before, typeof(CodeGenerationStage))
			};

        /// <summary>
        /// Gets the pipeline stage order.
        /// </summary>
        /// <value>The pipeline stage order.</value>
        PipelineStageOrder[] IPipelineStage.PipelineStageOrder { get { return _pipelineOrder; } }

        #endregion // IMethodCompilerStage Members

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        public override void Run()
        {
            Context prev = null;
            foreach (BasicBlock block in BasicBlocks)
            {
                for (Context ctx = CreateContext(block); !ctx.EndOfInstruction; ctx.GotoNext())
                {
                    if (ctx.Instruction != null)
                    {
                        if (!ctx.Ignore)
                        {
                            if (prev != null)
                            {
                                if (RemoveMultipleStores(ctx, prev)) continue;
                                else if (RemoveSingleLineJump(ctx, prev)) { prev = ctx.Clone(); continue; }
                            }

                            prev = ctx.Clone();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Remove multiple occuring stores, for e.g. before:
        /// <code>
        /// mov eax, operand
        /// mov operand, eax
        /// </code>
        /// after:
        /// <code>
        /// mov eax, operand
        /// </code>
        /// </summary>
        /// <param name="current">The current context</param>
        /// <param name="previous">The previous context</param>
        /// <returns>True if an instruction has been removed</returns>
        private bool RemoveMultipleStores(Context current, Context previous)
        {
            if (current.BasicBlock == previous.BasicBlock)
                if (current.Instruction is CPUx86.MovInstruction && previous.Instruction is CPUx86.MovInstruction)
                {
                    if (previous.Result == current.Operand1 && previous.Operand1 == current.Result)
                    {
                        current.Remove();
                        return true;
                    }
                }

            return false;
        }

        /// <summary>
        /// Removes the single line jump.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="previous">The previous.</param>
        /// <returns></returns>
        private bool RemoveSingleLineJump(Context current, Context previous)
        {
            if (previous.Instruction is CPUx86.JmpInstruction)
                if (current.BasicBlock != previous.BasicBlock)	// should always be true
                    if (previous.Branch.Targets[0] == current.BasicBlock.Label)
                    {
                        previous.Remove();
                        return true;
                    }

            return false;
        }

        /// <summary>
        /// Removes the single line jump.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="previous">The previous.</param>
        /// <returns></returns>
        private bool ImproveBranchAndJump(Context current, Context previous)
        {
            if (current.Instruction is CPUx86.JmpInstruction)
                if (previous.Instruction is CPUx86.BranchInstruction)
                {
                    // TODO
                    // Swap branch target of jump and branch
                    // Negate branch condition
                }

            return false;
        }
    }
}
