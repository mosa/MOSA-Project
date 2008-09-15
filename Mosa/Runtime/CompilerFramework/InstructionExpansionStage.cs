/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

#if OLD

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Performs IR expansion of instructions to more machine specific representations of
    /// individual operations.
    /// </summary>
    public sealed class InstructionExpansionStage : IMethodCompilerStage
    {
        #region IMethodCompilerStage Members

        string IMethodCompilerStage.Name
        {
            get { return @"IR Expansion"; }
        }

        void IMethodCompilerStage.Run(MethodCompilerBase compiler)
        {
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null == blockProvider)
                throw new InvalidOperationException(@"InstructionExpansionStage requires basic block provider.");

            foreach (BasicBlock block in blockProvider)
                ExpandInstructionList(compiler, block.Instructions);
        }

        #endregion // IMethodCompilerStage Members

        #region Internals

        /// <summary>
        /// Expands all instructions in the given list.
        /// </summary>
        /// <param name="methodCompiler">The executing method compiler.</param>
        /// <param name="instructionList">The list of instructions to expand.</param>
        private void ExpandInstructionList(MethodCompilerBase methodCompiler, List<Instruction> instructionList)
        {
            // Flag, if we've made a replacement...
            bool replaced;
            // Expansion result
            object expanded = null;
            // List used to hold temporary work results
            List<Instruction> instructions = null;
            // Enumerable of instructions
            IEnumerable<Instruction> oldInstructions = instructionList;

            do
            {
                // Reset the replaced flag
                replaced = false;

                // Create a new instructions list
                instructions = new List<Instruction>();

                // Iterate all instructions in the original list
                foreach (Instruction i in oldInstructions)
                {
                    // Expand this instruction
                    expanded = i.Expand(methodCompiler);
                    if (null != expanded)
                    {
                        Instruction exp = expanded as Instruction;
                        if (null != exp)
                        {
                            instructions.Add(exp);
                            replaced = (false == Object.ReferenceEquals(exp, i));
                        }
                        else
                        {
                            IEnumerable<Instruction> range = expanded as IEnumerable<Instruction>;
                            if (null == range)
                                throw new InvalidOperationException(@"Instruction.Expand returned invalid result object.");

                            instructions.AddRange(range);
                            replaced = true;
                        }
                    }
                }

                // Save the enumerator
                oldInstructions = instructions;
            }
            while (true == replaced);

            // Clear the old list
            instructionList.Clear();
            instructionList.AddRange(instructions);
        }

        #endregion // Internals
    }
}

#endif // #if OLD