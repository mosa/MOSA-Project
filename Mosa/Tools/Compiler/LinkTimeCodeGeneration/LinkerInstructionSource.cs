/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *
 */

using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Tools.Compiler.LinkTimeCodeGeneration
{
    /// <summary>
    /// This class provides a source of an instruction list for the method compiler.
    /// </summary>
    /// <remarks>
    /// This instruction source is used during link time code generation in order to 
    /// have a source of instructions to compile. This source acts on a previously built
    /// list of instructions to pass through the following compilation stages.
    /// </remarks>
    sealed class LinkerInstructionSource : IMethodCompilerStage, IInstructionsProvider
    {
        #region Data Members

        /// <summary>
        /// Holds the instructions to emit during the linker process.
        /// </summary>
        private readonly List<Instruction> instructions;

        #endregion // Data Members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkerInstructionSource"/> class.
        /// </summary>
        /// <param name="instructions">The instructions to emit.</param>
        public LinkerInstructionSource(List<Instruction> instructions)
        {
            this.instructions = new List<Instruction>(instructions);
        }

        #endregion // Construction

        #region IInstructionsProvider Members

        /// <summary>
        /// Gets a list of instructions in intermediate representation.
        /// </summary>
        /// <value></value>
        public List<Instruction> Instructions
        {
            get { return this.instructions; }
        }

		/// <summary>
		/// Gets a list of instructions in intermediate representation.
		/// </summary>
		/// <value></value>
		public InstructionSet InstructionSet
		{
			get { return null; } // FIXME
		}

        #endregion // IInstructionsProvider Members

        #region IEnumerable<Instruction> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Instruction> GetEnumerator()
        {
            return this.instructions.GetEnumerator();
        }

        #endregion // IEnumerable<Instruction> Members

        #region IEnumerable Members

        #endregion // IEnumerable Members

        #region IMethodCompilerStage Members

        public string Name
        {
            get { return @"Link Time Code Generation Instruction Source"; }
        }

        public void Run(IMethodCompiler compiler)
        {
            // Nothing to do here, normally an instruction source would parse some source code
            // or intermediate form of it. We've already got the instructions in the ctor.
        }

        public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.Add(this);
        }

        #endregion // IMethodCompilerStage Members
    }
}
