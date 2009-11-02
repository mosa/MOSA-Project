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
	public sealed class SimplePeepholdOptimizationStage :
		BaseTransformationStage,
		IMethodCompilerStage,
		IPlatformTransformationStage
	{

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name
		{
			get { return @"X86.SimplePeepholdOptimizationStage"; }
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertAfter<TweakTransformationStage>(this);
		}

		#endregion // IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			foreach (BasicBlock block in BasicBlocks) 
            {
				Context prev = null;
				for (Context ctx = new Context(InstructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext()) 
                {
                    if (ctx.Instruction != null)
                    {
                        if (!ctx.Ignore)
                        {
                            if (prev != null)
                            {
                                if (RemoveMultipleStores(ctx, prev)) continue;
                                if (RemoveSingleLineJump(ctx, prev)) continue;
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
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        private bool RemoveSingleLineJump(Context current, Context previous)
        {
            if (previous.Instruction is CPUx86.JmpInstruction)
            {
                if (previous.Branch.Targets[0] == current.Index)
                {
                    previous.Remove();
                    return true;
                }
            }

            return false;
        }
	}
}
