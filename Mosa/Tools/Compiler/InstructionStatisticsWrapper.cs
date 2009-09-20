/*
 * Created by Kai P. Reisert using SharpDevelop
 * Date: 11.12.2008 
 */

using System;

using Mosa.Runtime.CompilerFramework;

using NDesk.Options;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Wraps the constant folding optimization stage and adds an option to disable it.
    /// 
    /// TODO: put this wrapper stage somewhere in the actual pipeline.
    /// </summary>
    public class InstructionStatisticsWrapper : MethodCompilerStageWrapper<ILConstantFoldingStage>
    {
        /// <summary>
        /// Initializes a new instance of the ConstantFoldingWrapper class.
        /// </summary>
        public InstructionStatisticsWrapper()
        {
        }

        /// <summary>
        /// Adds the additional options for the parsing process to the given OptionSet.
        /// </summary>
        /// <param name="optionSet">A given OptionSet to add the options to.</param>
        public override void AddOptions(OptionSet optionSet)
        {
            optionSet.Add(
                "opt-stats",
                "Enable instruction statistics.",
                delegate(string v)
                {
                    if (v != null)
                    {
                        this.Enabled = true;
                    }
                });
        }
    }
}
