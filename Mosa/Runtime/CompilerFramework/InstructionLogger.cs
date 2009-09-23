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
using System.Diagnostics;

using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Logs all incoming instructions and forwards them to the next compiler stage.
	/// </summary>
	public sealed class InstructionLogger : IMethodCompilerStage
    {
		#region Data members

        /// <summary>
        /// Static instance of the instruction logger.
        /// </summary>
        public static readonly InstructionLogger Instance = new InstructionLogger();

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="InstructionLogger"/>.
		/// </summary>
		private InstructionLogger()
		{
		}

		#endregion // Construction

		#region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Logger"; }
		}

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(IMethodCompiler compiler)
		{
            // Previous stage
            IMethodCompilerStage prevStage = compiler.GetPreviousStage<IMethodCompilerStage>();
            // Line number
            int index = 1;

            Debug.WriteLine(String.Format("IR representation of method {0} after stage {1}", compiler.Method, prevStage.Name));

            // Block provider
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null != blockProvider)
            {
                foreach (BasicBlock block in blockProvider)
                {
                    Debug.WriteLine(String.Format("Block #{0} - label L_{1:X4}", index, block.Label));
                    Debug.Indent();
                    LogInstructions(block.Instructions);
                    Debug.Unindent();
                    index++;
                }
            }
            else
            {
                IInstructionsProvider id = (IInstructionsProvider)compiler.GetPreviousStage(typeof(IInstructionsProvider));
                if (null != id)
                {
                    LogInstructions(id.Instructions);
                }
            }
		}

        /// <summary>
        /// Adds the stage to the pipeline.
        /// </summary>
        /// <param name="pipeline">The pipeline to add to.</param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.InsertAfter<IMethodCompilerStage>(this);
        }

		#endregion // IMethodCompilerStage Members

        #region Internals

        /// <summary>
        /// Logs the instructions in the given enumerable to the trace.
        /// </summary>
        /// <param name="instructions">The enumerable container of instructions.</param>
        private void LogInstructions(IEnumerable<LegacyInstruction> instructions)
        {
            StringBuilder text = new StringBuilder();
            foreach (LegacyInstruction inst in instructions)
            {
                text.Length = 0;
                if (true == inst.Ignore)
                    text.Append("; ");
                text.AppendFormat("L_{0:X4}: {1}", inst.Offset, inst);
                Debug.WriteLine(text.ToString());
            }
        }

        #endregion // Internals
    }
}
