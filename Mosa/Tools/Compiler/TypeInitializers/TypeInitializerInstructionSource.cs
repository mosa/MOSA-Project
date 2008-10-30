/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler.TypeInitializers
{
    /// <summary>
    /// Provides the instructions to invoke all type initializers.
    /// </summary>
    public sealed class TypeInitializerInstructionSource : IMethodCompilerStage, IInstructionsProvider
    {
        #region Data Members

        /// <summary>
        /// Holds the instructions to execute in the type initializer.
        /// </summary>
        private List<Instruction> instructions;

        #endregion // Data Members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeInitializerInstructionSource"/> class.
        /// </summary>
        public TypeInitializerInstructionSource()
        {
            this.instructions = new List<Instruction>();
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Schedules the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        public void Schedule(RuntimeMethod method)
        {
            this.instructions.Add(new CallInstruction(method));
        }

        #endregion // Methods

        #region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"Type Initializer Instruction Source"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(MethodCompilerBase compiler)
        {
            // Nothing to do
        }

        #endregion // IMethodCompilerStage Members

        #region IInstructionsProvider Members

        /// <summary>
        /// Gets a list of instructions in intermediate representation.
        /// </summary>
        /// <value></value>
        public List<Instruction> Instructions
        {
            get { return this.instructions; }
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

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.instructions.GetEnumerator();            
        }

        #endregion // IEnumerable Members
    }
}
