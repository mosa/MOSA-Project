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
                    for (Instruction instruction = block.Instructions[0]; instruction is IR.PhiInstruction; instruction = block.Instructions[0])
                    {
                        IR.PhiInstruction phi = instruction as IR.PhiInstruction;
                        Operand res = phi.Result;
                        for (int i = 0; i < phi.Blocks.Count; i++)
                        {
                            Operand op = phi.Operands[i];
                            Instruction move = arch.CreateInstruction(typeof(IR.MoveInstruction), res, op);
                            // HACK: Remove phi from the operand use list
                            op.Uses.Remove(phi);

                            List<Instruction> insts = phi.Blocks[i].Instructions;

							if (0 < insts.Count && insts[insts.Count - 1] is IBranchInstruction)
								insts.Insert(insts.Count - 1, move);
							else {								
								//insts.Add(move);
							}
                        }

                        // Remove the PHI instruction
                        block.Instructions.RemoveAt(0);
                        // HACK: Remove phi from the operand def list
                        res.Definitions.Remove(phi);
                    }
                }
            }
        }

        #endregion // IMethodCompilerStage Members
    }
}
