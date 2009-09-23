/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System.Collections.Generic;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Implemented by compiler stages, which produce a list of instructions in
    /// intermediate representation From a source.
    /// </summary>
    public interface IInstructionsProvider // : IEnumerable<Instruction>
    {
        /// <summary>
        /// Gets a list of instructions in intermediate representation.
        /// </summary>
        List<LegacyInstruction> Instructions { get; }

		/// <summary>
		/// Gets a list of instructions in intermediate representation.
		/// </summary>
		/// <value>The instruction set.</value>
		InstructionSet InstructionSet { get; }

    }
}
