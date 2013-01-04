/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework
{
    /// <summary>
    /// Base class for code transformation stages.
    /// </summary>
    public abstract class BaseCodeTransformationStage : BaseMethodCompilerStage, IMethodCompilerStage, IVisitor
    {

        #region IMethodCompilerStage Members

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        public virtual void Run()
        {
            for (int index = 0; index < basicBlocks.Count; index++)
                for (Context ctx = new Context(instructionSet, basicBlocks[index]); !ctx.IsLastInstruction; ctx.GotoNext())
                    if (!ctx.IsEmpty)
                        ctx.Clone().Visit(this);
        }

        #endregion // IMethodCompilerStage Members


    }
}
