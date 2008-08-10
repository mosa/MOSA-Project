/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Base interface for CIL branch instructions.
    /// </summary>
    public interface IBranchInstruction {
		#region Properties

        /// <summary>
        /// Determines if the branch is conditional.
        /// </summary>
        bool IsConditional { get; }

		/// <summary>
		/// Returns the branch targets instruction index.
		/// </summary>
        int[] BranchTargets { get; set; }

		#endregion // Properties
	}
}
