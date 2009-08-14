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

namespace Mosa.Runtime.CompilerFramework.IL
{

	/// <summary>
	/// Specifies opcode processing flags. These influence the processing performed
	/// by the ILDecodingStage.
	/// </summary>
	[Flags]
	public enum OpCodeFlags {
		/// <summary>
		/// No flags.
		/// </summary>
		None = 0x00,

		/// <summary>
		/// Marks an opcode slot as invalid.
		/// </summary>
		InvalidOpCode = 0x01,

		/// <summary>
		/// Causes the ILDecodingStage to break to the debugger, when processing the opcode.
		/// </summary>
		Debug = 0x02,

        /// <summary>
        /// Used to mark operations, which can safely be removed From the code stream (they're replaced by nops in the first pass and
        /// dropped in later stages, if they're not a branch target.)
        /// </summary>
        Drop = 0x04,
	}
}
