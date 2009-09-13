/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Base interface for CIL branch instructions.
    /// </summary>
    public interface IBranch {
		
		#region Properties

        /// <summary>
        /// Retrieves the instruction offset.
        /// </summary>
        int Offset { get; }

		/// <summary>
		/// Returns the branch targets instruction index.
		/// </summary>
        int[] Targets { get; set; }

		#endregion // Properties
	}
}
