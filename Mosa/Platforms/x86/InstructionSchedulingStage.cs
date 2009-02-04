/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:kintaro@mosa-project.org>)
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertBefore<LinearRegisterAllocator>(this);
        }
    }
}
