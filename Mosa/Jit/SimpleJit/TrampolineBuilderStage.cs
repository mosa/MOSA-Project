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

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Jit.SimpleJit
{
    /// <summary>
    /// The first stage in a method compiler building the call trampoline.
    /// </summary>
    sealed class TrampolineBuilderStage : IMethodCompilerStage, IInstructionsProvider
    {
        #region Data members

        /// <summary>
        /// Holds the instructions of this trampoline.
        /// </summary>
        private List<Instruction> _instructions;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the trampoline builder stage.
        /// </summary>
        /// <param name="instructions">The instructions that make up the trampoline.</param>
        public TrampolineBuilderStage(List<Instruction> instructions)
        {
            if (null == instructions)
                throw new ArgumentNullException(@"instructions");

            _instructions = instructions;
        }

        #endregion // Construction

        #region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"TrampolineBuilderStage"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(IMethodCompiler compiler)
        {
            // Nothing to do here, the jit already gives us a list of instructions 
            // we need to compile so we don't have to do anything here.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipeline"></param>
        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.Add(this);
        }

        #endregion // IMethodCompilerStage Members

        #region IInstructionsProvider Members

        List<Instruction> IInstructionsProvider.Instructions
        {
            get { return _instructions; }
        }

		/// <summary>
		/// Gets a list of instructions in intermediate representation.
		/// </summary>
		/// <value></value>
		public InstructionSet Instructions2
		{
			get { return null; } // FIXME
		}

        #endregion // IInstructionsProvider Members

        #region IEnumerable<Instruction> Members


        #endregion // IEnumerable<Instruction> Members

        #region IEnumerable Members


        #endregion // IEnumerable Members
    }
}
