/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InstructionSchedulingStage : IMethodCompilerStage
    {
        /// <summary>
        /// 
        /// </summary>
        private static int latencySum = 0;
        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get
            {
                return @"InstructionSchedulingStage";
            }
        }

        /// <summary>
        /// Runs the specified method compiler.
        /// </summary>
        /// <param name="methodCompiler">The method compiler.</param>
        public void Run(IMethodCompiler methodCompiler)
        {
            // Retrieve the latest basic block decoder
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)methodCompiler.GetPreviousStage(typeof(IBasicBlockProvider));

            if (null == blockProvider)
                throw new InvalidOperationException(@"Code generation requires a basic block provider.");

            foreach (BasicBlock block in blockProvider)
            {
                ScheduleBlock(block);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertBefore<LinearRegisterAllocator>(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        private void ScheduleBlock(BasicBlock block)
        {
            foreach (Instruction instruction in block.Instructions)
            {
                try
                {
                    byte latency = InstructionLatency.GetLatency(instruction);
                }
                catch (NotSupportedException)
                {
                }
            }
        }
    }
}
