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
using Mosa.Runtime.CompilerFramework.Ir;

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

		string IMethodCompilerStage.Name
		{
			get { return @"Logger"; }
		}

		void IMethodCompilerStage.Run(MethodCompilerBase compiler)
		{
            // Block provider
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            // Previous stage
            IMethodCompilerStage prevStage = compiler.GetPreviousStage(typeof(IMethodCompilerStage));
            // Line number
            int line = 0, index = 1;

            Debug.WriteLine(String.Format("IR representation of method {0} after stage {1}", compiler.Method, prevStage.Name));

            foreach (BasicBlock block in blockProvider)
            {
                Debug.WriteLine(String.Format("Block #{0} - label L_{1:X4}", index, block.Label));
                Debug.Indent();
                line = block.Label;
                foreach (Instruction inst in block.Instructions)
                {
                    Debug.WriteLine(String.Format("L_{0:X4}: {1}", inst.Offset, inst));
                }
                Debug.Unindent();
                index++;
            }
		}

		#endregion // IMethodCompilerStage Members
	}
}
