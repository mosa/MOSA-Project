/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Transforms the intermediate representation out of SSA form.
    /// </summary>
    /// <remarks>
    /// This transformation simplifies and expands all PHI functions and
    /// unifies variable version.
    /// </remarks>
    public sealed class LeaveSSA : IMethodCompilerStage
    {
        #region IMethodCompilerStage Members

        string IMethodCompilerStage.Name
        {
            get { return @"LeaveSSA"; }
        }

        void IMethodCompilerStage.Run(MethodCompilerBase compiler)
        {
            // Retrieve the basic block provider
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null == blockProvider)
                throw new InvalidOperationException(@"SSA Conversion requires basic blocks.");
            IArchitecture arch = compiler.Architecture;

            foreach (BasicBlock block in blockProvider)
            {
                if (block.Instructions.Count > 0)
                {
                    foreach (Instruction instruction in block.Instructions)
                    {
                        IR.PhiInstruction phi = instruction as IR.PhiInstruction;
                        if (null == phi)
                            break;

                        Operand res = phi.Result;
                        for (int i = 0; i < phi.Blocks.Count; i++)
                        {
                            Operand op = phi.Operands[i];

                            // HACK: Remove phi from the operand use list
                            op.Uses.Remove(phi);

                            if (false == Object.ReferenceEquals(res, op))
                            {
                                if (1 == op.Definitions.Count && 0 == op.Uses.Count)
                                {
                                    // Replace the operand, as it is only defined but never used again
                                    op.Replace(res);
                                }
                                else
                                {
                                    List<Instruction> insts = phi.Blocks[i].Instructions;
                                    int insIdx = insts.Count - 1;

                                    /* If there's a use, insert the move right after the last use
                                     * this really helps the register allocator as it keeps the lifetime
                                     * of the temporary short.
                                     */
                                    if (0 != op.Uses.Count)
                                    {
                                        // FIXME: Depends on sortable instruction offsets, we really need a custom collection here
                                        op.Uses.Sort(delegate(Instruction a, Instruction b)
                                        {
                                            return (a.Offset - b.Offset);
                                        });

                                        insIdx = insts.IndexOf(op.Uses[op.Uses.Count - 1]) + 1;
                                    }


                                    // Make sure we're inserting at a valid position
                                    if (insIdx == -1)
                                        insIdx = 0;

                                    Instruction move = arch.CreateInstruction(typeof(IR.MoveInstruction), res, op);
                                    insts.Insert(insIdx, move);
                                }
                            }
                        }

                        /* HACK: Hide the PHI instruction.
                         * 
                         * We're not removing the PHI instruction as it may still be valuable to calculate
                         * live ranges in later stages (e.g. register allocation) in those cases, the PHI
                         * function causes the live range to be virtually "extended".
                         * 
                         */
                        phi.Ignore = true;
                        //block.Instructions.RemoveAt(0);

                        // HACK: Remove phi from the operand def list
                        res.Definitions.Remove(phi);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertAfter<ConstantFoldingStage>(this);
        }

        #endregion // IMethodCompilerStage Members
    }
}
