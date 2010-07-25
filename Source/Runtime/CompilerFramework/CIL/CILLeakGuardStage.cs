/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (aka Michael Ruck, grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.CIL
{
    /// <summary>
    /// Logs CIL instruction, which leak past IR transformation.
    /// </summary>
    public class CILLeakGuardStage : BaseMethodCompilerStage, IMethodCompilerStage
    {
        public CILLeakGuardStage()
        {
            this.MustThrowCompilationException = false;
        }

        /// <summary>
        /// Determines if this stage throws a compilation exception, if a CIL instruction is detected.
        /// </summary>
        public bool MustThrowCompilationException
        {
            get; 
            set;
        }

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get
            {
                return @"CILLeakGuardStage";
            }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        public void Run()
        {
            for (int index = 0; index < this.BasicBlocks.Count; index++)
            {
                for (Context ctx = new Context(InstructionSet, BasicBlocks[index]); !ctx.EndOfInstruction; ctx.GotoNext())
                {
                    IInstruction instruction = ctx.Instruction;
                    if (instruction is ICILInstruction)
                    {
                        this.ThrowCompilationException(ctx);
                    }
                }
            }
        }

        /// <summary>
        /// Logs and optionally throws a compilation exception for the given context.
        /// </summary>
        /// <param name="context">The context to log and throw.</param>
        private void ThrowCompilationException(Context context)
        {
            string message = @"Leaking CIL instruction to late stages. Instruction " + context.Instruction.ToString(context) + @" at " + context.Label + @" in method " + this.MethodCompiler.Method;
            Trace.WriteLine(message);

            if (this.MustThrowCompilationException == true)
            {
                throw new CompilationException(message);
            }
        }
    }
}
