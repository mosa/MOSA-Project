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

        string IMethodCompilerStage.Name
        {
            get { return @"TrampolineBuilderStage"; }
        }

        void IMethodCompilerStage.Run(MethodCompilerBase compiler)
        {
            // Nothing to do here, the jit already gives us a list of instructions 
            // we need to compile so we don't have to do anything here.
        }

        #endregion // IMethodCompilerStage Members

        #region IInstructionsProvider Members

        List<Instruction> IInstructionsProvider.Instructions
        {
            get { return _instructions; }
        }

        #endregion // IInstructionsProvider Members

        #region IEnumerable<Instruction> Members

        IEnumerator<Instruction> IEnumerable<Instruction>.GetEnumerator()
        {
            return _instructions.GetEnumerator();
        }

        #endregion // IEnumerable<Instruction> Members

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _instructions.GetEnumerator();
        }

        #endregion // IEnumerable Members
    }
}
