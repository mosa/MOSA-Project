/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;

using Mosa.Runtime.CompilerFramework;
using CIL = Mosa.Runtime.CompilerFramework.CIL;

using NDesk.Options;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Wraps the InstructionSchedulingWrapper optimization stage and adds an option to disable it.
    /// 
    /// TODO: put this wrapper stage somewhere in the actual pipeline.
    /// </summary>
    public class InstructionSchedulingWrapper : MethodCompilerStageWrapper<CIL.ConstantFoldingStage>
    {
        /// <summary>
        /// Initializes a new instance of the InstructionSchedulingWrapper class.
        /// </summary>
        public InstructionSchedulingWrapper()
        {
        }

        /// <summary>
        /// Adds the additional options for the parsing process to the given OptionSet.
        /// </summary>
        /// <param name="optionSet">A given OptionSet to add the options to.</param>
        public override void AddOptions(OptionSet optionSet)
        {
            optionSet.Add(
                "opt-no-is",
                "Disable instruction scheduling.",
                delegate(string v)
                {
                    if (v != null)
                    {
                        this.Enabled = false;
                    }
                });
        }
    }
}
